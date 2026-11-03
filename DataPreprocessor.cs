using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Backprop
{
	/// <summary>
	/// Holds one row per sample. X = input features, Y = desired output(s).
	/// For this project Y has a single value: 1 = lung cancer (YES), 0 = no lung cancer (NO).
	/// </summary>
	public class Dataset
	{
		public string[] FeatureNames;
		public double[][] X;   // [sample][feature]
		public double[][] Y;   // [sample][output]

		public int Count { get { return X.Length; } }
		public int FeatureCount { get { return FeatureNames.Length; } }

		public Dataset Subset(int[] indices)
		{
			Dataset d = new Dataset();
			d.FeatureNames = FeatureNames;
			d.X = indices.Select(i => X[i]).ToArray();
			d.Y = indices.Select(i => Y[i]).ToArray();
			return d;
		}

		public int CountPositive()
		{
			return Y.Count(y => y[0] >= 0.5);
		}
	}

	/// <summary>Summary of what the loader did, for the training log / data description.</summary>
	public class LoadReport
	{
		public int RawRows;
		public int MalformedSkipped;
		public int DuplicatesRemoved;
		public int FinalRows;
		public int Positive;
		public int Negative;

		public override string ToString()
		{
			return string.Format(
				"Raw rows read      : {0}\n" +
				"Malformed skipped  : {1}\n" +
				"Duplicates removed : {2}\n" +
				"Final rows         : {3}\n" +
				"Cancer (YES)       : {4} ({5:P1})\n" +
				"No cancer (NO)     : {6} ({7:P1})",
				RawRows, MalformedSkipped, DuplicatesRemoved, FinalRows,
				Positive, FinalRows == 0 ? 0.0 : (double)Positive / FinalRows,
				Negative, FinalRows == 0 ? 0.0 : (double)Negative / FinalRows);
		}
	}

	/// <summary>
	/// MinMax scaler: x' = (x - min) / (max - min), giving values in [0,1].
	/// Call Fit() on the TRAINING set only, then Transform() on both train and test,
	/// so no information from the test set leaks into training.
	/// </summary>
	public class MinMaxScaler
	{
		private double[] min;
		private double[] max;

		public bool IsFitted { get { return min != null; } }
		public double[] Min { get { return min; } }
		public double[] Max { get { return max; } }

		public void Fit(double[][] X)
		{
			int n = X[0].Length;
			min = new double[n];
			max = new double[n];
			for (int j = 0; j < n; j++)
			{
				min[j] = double.MaxValue;
				max[j] = double.MinValue;
			}
			for (int i = 0; i < X.Length; i++)
			{
				for (int j = 0; j < n; j++)
				{
					if (X[i][j] < min[j]) min[j] = X[i][j];
					if (X[i][j] > max[j]) max[j] = X[i][j];
				}
			}
		}

		public double[] TransformRow(double[] row)
		{
			if (!IsFitted) throw new InvalidOperationException("Call Fit() first.");
			double[] r = new double[row.Length];
			for (int j = 0; j < row.Length; j++)
			{
				double range = max[j] - min[j];
				double v = range == 0.0 ? 0.0 : (row[j] - min[j]) / range;
				// unseen test/user values can fall outside the training range; clamp to [0,1]
				if (v < 0.0) v = 0.0;
				if (v > 1.0) v = 1.0;
				r[j] = v;
			}
			return r;
		}

		public double[][] Transform(double[][] X)
		{
			return X.Select(row => TransformRow(row)).ToArray();
		}
	}

	public static class DataPreprocessor
	{
		/// <summary>
		/// Reads survey_lung_cancer.csv, cleans it and encodes it numerically.
		/// Cleaning: trims header/field whitespace (e.g. "FATIGUE " and "ALLERGY " in the header),
		/// skips malformed rows, and removes exact duplicate rows.
		/// Encoding: GENDER M=1/F=0, AGE kept as a number, every other symptom column
		/// 1 -> 0 (No) and 2 -> 1 (Yes), LUNG_CANCER YES=1/NO=0.
		/// Features are NOT scaled here; use MinMaxScaler after splitting.
		/// </summary>
		public static Dataset Load(string path, out LoadReport report)
		{
			report = new LoadReport();
			string[] lines = File.ReadAllLines(path);
			if (lines.Length < 2) throw new InvalidDataException("CSV has no data rows.");

			string[] header = lines[0].Split(',').Select(h => h.Trim().ToUpperInvariant()).ToArray();
			int labelCol = Array.IndexOf(header, "LUNG_CANCER");
			if (labelCol < 0) throw new InvalidDataException("LUNG_CANCER column not found.");

			// every column except the label is a feature, in file order
			List<int> featureCols = new List<int>();
			for (int c = 0; c < header.Length; c++)
				if (c != labelCol) featureCols.Add(c);

			List<double[]> xs = new List<double[]>();
			List<double[]> ys = new List<double[]>();
			HashSet<string> seen = new HashSet<string>();

			for (int r = 1; r < lines.Length; r++)
			{
				if (string.IsNullOrWhiteSpace(lines[r])) continue;
				report.RawRows++;

				string[] f = lines[r].Split(',').Select(s => s.Trim().ToUpperInvariant()).ToArray();
				if (f.Length != header.Length) { report.MalformedSkipped++; continue; }

				double[] x = new double[featureCols.Count];
				bool ok = true;
				for (int k = 0; k < featureCols.Count && ok; k++)
				{
					int c = featureCols[k];
					double v;
					ok = EncodeFeature(header[c], f[c], out v);
					x[k] = v;
				}
				double label;
				if (ok) ok = EncodeLabel(f[labelCol], out label); else label = 0;
				if (!ok) { report.MalformedSkipped++; continue; }

				// exact duplicate (all features + label) -> keep first occurrence only.
				// Removing duplicates BEFORE the split also stops identical rows from
				// landing in both the training and the test set.
				string key = string.Join("|", f);
				if (!seen.Add(key)) { report.DuplicatesRemoved++; continue; }

				xs.Add(x);
				ys.Add(new double[] { label });
			}

			Dataset d = new Dataset();
			d.FeatureNames = featureCols.Select(c => header[c]).ToArray();
			d.X = xs.ToArray();
			d.Y = ys.ToArray();

			report.FinalRows = d.Count;
			report.Positive = d.CountPositive();
			report.Negative = d.Count - report.Positive;
			return d;
		}

		private static bool EncodeFeature(string column, string value, out double result)
		{
			result = 0.0;
			if (column == "GENDER")
			{
				if (value == "M") { result = 1.0; return true; }
				if (value == "F") { result = 0.0; return true; }
				return false;
			}
			if (column == "AGE")
			{
				double age;
				if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out age)) return false;
				if (age <= 0 || age > 120) return false;   // impossible age -> treat as bad row
				result = age;
				return true;
			}
			// survey answers: 1 = No, 2 = Yes
			if (value == "1") { result = 0.0; return true; }
			if (value == "2") { result = 1.0; return true; }
			return false;
		}

		private static bool EncodeLabel(string value, out double result)
		{
			result = 0.0;
			if (value == "YES") { result = 1.0; return true; }
			if (value == "NO") { result = 0.0; return true; }
			return false;
		}

		/// <summary>
		/// Stratified split: train and test keep (almost) the same YES/NO ratio.
		/// Important here because the dataset is heavily imbalanced.
		/// A fixed seed makes the split reproducible.
		/// </summary>
		public static void StratifiedSplit(Dataset data, double testRatio, int seed,
			out Dataset train, out Dataset test)
		{
			Random rng = new Random(seed);
			List<int> trainIdx = new List<int>();
			List<int> testIdx = new List<int>();

			foreach (double cls in new double[] { 0.0, 1.0 })
			{
				int[] idx = Enumerable.Range(0, data.Count)
					.Where(i => data.Y[i][0] == cls).ToArray();
				Shuffle(idx, rng);
				int nTest = (int)Math.Round(idx.Length * testRatio);
				for (int i = 0; i < idx.Length; i++)
				{
					if (i < nTest) testIdx.Add(idx[i]); else trainIdx.Add(idx[i]);
				}
			}

			int[] tr = trainIdx.ToArray();
			int[] te = testIdx.ToArray();
			Shuffle(tr, rng);
			Shuffle(te, rng);
			train = data.Subset(tr);
			test = data.Subset(te);
		}

		/// <summary>
		/// Randomly duplicates minority-class (NO) rows in the TRAINING set until both classes
		/// have the same count. Optional: use it only on the training set, never on the test set.
		/// </summary>
		public static Dataset OversampleMinority(Dataset train, int seed)
		{
			Random rng = new Random(seed);
			int[] pos = Enumerable.Range(0, train.Count).Where(i => train.Y[i][0] >= 0.5).ToArray();
			int[] neg = Enumerable.Range(0, train.Count).Where(i => train.Y[i][0] < 0.5).ToArray();
			int[] minor = pos.Length < neg.Length ? pos : neg;
			int[] major = pos.Length < neg.Length ? neg : pos;

			List<int> all = new List<int>(Enumerable.Range(0, train.Count));
			for (int k = minor.Length; k < major.Length; k++)
				all.Add(minor[rng.Next(minor.Length)]);

			int[] result = all.ToArray();
			Shuffle(result, rng);
			return train.Subset(result);
		}

		/// <summary>Fisher-Yates shuffle in place. Use a new order every epoch (online training).</summary>
		public static void Shuffle(int[] array, Random rng)
		{
			for (int i = array.Length - 1; i > 0; i--)
			{
				int j = rng.Next(i + 1);
				int tmp = array[i];
				array[i] = array[j];
				array[j] = tmp;
			}
		}

		public static int[] ShuffledIndices(int n, Random rng)
		{
			int[] idx = Enumerable.Range(0, n).ToArray();
			Shuffle(idx, rng);
			return idx;
		}
	}
}
