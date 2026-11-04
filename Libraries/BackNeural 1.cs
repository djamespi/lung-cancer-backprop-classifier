using System;
using System.IO;
using System.Globalization;
using System.Linq;

namespace Backprop
{
	public class NeuralNet
	{
		private INeuron [] ineuron;//approximatelty 3072
		private HNeuron [] hneuron;// approx 64
		private ONeuron [] oneuron;//approx 10
        //private const double LRPOUT =0.2;//learning rate for the the ouptput layer
        //private const double LRPIN = 0.15;// learning rate for the input layer
        public double LRPOUT = 0.2; // learning rate for the output layer
        public double LRPIN = 0.15; // learning rate for the input layer
        private double [] errorComponent; // approx 10;
		private double [] errorDerivative;
		private double [] desiredout;

		public NeuralNet()
		{
			ineuron=new INeuron[3072];
			hneuron=new HNeuron[64];
			oneuron=new ONeuron[10];
			desiredout=new double[10];
			errorComponent=new double[10];
			errorDerivative=new double[10];
			createNeurons(ineuron.Length,hneuron.Length,oneuron.Length);
		}
		public NeuralNet(int input,int hidden,int output, Random random = null)
		{
			ineuron=new INeuron[input];
			hneuron=new HNeuron[hidden];
			oneuron=new ONeuron[output];
			errorComponent=new double[output];
			errorDerivative=new double[output];
			desiredout=new double[output];
			createNeurons(ineuron.Length,hneuron.Length,oneuron.Length,random);
		}
		public void createNeurons(int i,int h,int o, Random random = null)
		{
			for (int x=0;x<i;x++)
			{
				ineuron[x]=new INeuron(x,h,random);
			}
			for (int x=0;x<h;x++)
			{
				hneuron[x]=new HNeuron(x,o,random);
			}
			for (int x=0;x<o;x++)
			{
				oneuron[x]=new ONeuron(x);
			}

		}
		public double getOutputData(int pos)
		{
			return oneuron[pos].getOActivation();
		}
		public void setInputs(int pos,double data)
		{
			ineuron[pos].setInput(data);
		}
		public void setDesiredOutput(int pos,double data)
		{
			desiredout[pos]=data;
		}
		public double sigmoid(double dat)
		{
			if (dat>=20.00)
			{
				dat=32;
			}
			return (double)(1.0/(1.0 + System.Math.Exp(-dat)));
		}
		public void calculateHA()
		{
			// calculate the diferent activations on the 2 neurons
			for(int x=0;x<hneuron.Length;x++)
			{
				double summation=0.0;
				for(int y=0;y<ineuron.Length;y++)
				{
					summation+=ineuron[y].getInput()*ineuron[y].getWeight(x);
				}
				hneuron[x].setHactivation(sigmoid(summation+hneuron[x].getBias()));
				//hneuron[x].setHactivation(sigmoid(summation));
			}
		}
		public void calculateOA()
		{
			// calculate the differnent activations for the output layers
			for(int x=0;x<oneuron.Length;x++)
			{
				double summation=0.0;
				for(int y=0;y<hneuron.Length;y++)
				{
					summation+=hneuron[y].getHactivation()*hneuron[y].getWeight(x);
				}
				oneuron[x].setOActivation(sigmoid(summation+oneuron[x].getBias()));
				//oneuron[x].setOActivation(sigmoid(summation));
			}
		}
		public void calculateEC()
		{
			//calculate the different EC on the the output layer
			for (int x=0;x<oneuron.Length;x++)
			{
				errorComponent[x]=(desiredout[x])-(oneuron[x].getOActivation());
			}
		}
		public void calculateDER()
		{
			//calclulate the different derivatives on the output layer
			for (int x=0;x<oneuron.Length;x++)
			{
				errorDerivative[x]=oneuron[x].getOActivation()*(1-(oneuron[x].getOActivation()))*errorComponent[x];
			}
		}
		public void learn()
		{// trainning session
			this.run();
			this.calculateEC();
			this.calculateDER();
			for(int x=0;x<hneuron.Length;x++)//calculates the errors of every neuron in the 2 layer
				hneuron[x].calculateErr(errorDerivative);
			for(int x=0;x<hneuron.Length;x++)//change in the weights in the 2 to ouput
				hneuron[x].setWeight(LRPOUT,errorDerivative);
			for(int x=0;x<ineuron.Length;x++)//change the weights in the input to 2
			{
				for (int y=0;y<hneuron.Length;y++)
				{
					ineuron[x].setWeight(y,hneuron[y].getErr(),LRPIN);
				
				}
			}
			for (int x=0;x<oneuron.Length;x++)//change in output neuron bias
				oneuron[x].changeBias(LRPOUT,errorDerivative);
			for (int x=0;x<hneuron.Length;x++)//change in 2 neuron bias
				hneuron[x].changeBias(LRPIN);

		}
		public void run()
		{
			this.calculateHA();
			this.calculateOA();
			//application phase
		}
		public bool countgood()
		{
			// session terminator
			bool result=true;
			for(int x=0;x<oneuron.Length;x++)
			{
				if((errorComponent[x]-errorDerivative[x])!=0)
					result=false;
			}
			return result;
		}
		public void saveWeights(String path)
		{

			using (StreamWriter sw = new StreamWriter(path)) 
			{
				for (int x=0;x<ineuron.Length;x++)//saving the weights of the input layer
				{
					for(int y=0;y<hneuron.Length;y++)
					sw.WriteLine(ineuron[x].getWeight(y).ToString("R", CultureInfo.InvariantCulture));
				}
				for(int x=0;x<hneuron.Length;x++)//saving the wieghts of the hidden layer
				{
					for (int y=0;y<oneuron.Length;y++)
					{
						sw.WriteLine(hneuron[x].getWeight(y).ToString("R", CultureInfo.InvariantCulture));
					}
				}
				for (int x=0;x<hneuron.Length;x++)// saving hidden layer bias
				{
					sw.WriteLine(hneuron[x].getBias().ToString("R", CultureInfo.InvariantCulture));
				}
				for (int x=0;x<oneuron.Length;x++)// saving output layer bias
				{
					sw.WriteLine(oneuron[x].getBias().ToString("R", CultureInfo.InvariantCulture));
				}
			}// end of streamwriter
		}

		public void loadWeights(string path) => LoadWeightValues(ReadWeightValues(path));

		public static double[] ReadWeightValues(string path)
		{
			return File.ReadAllLines(path).Select(line =>
			{
				double value;
				if ((!double.TryParse(line, NumberStyles.Float, CultureInfo.InvariantCulture, out value) &&
				     !double.TryParse(line, NumberStyles.Float, CultureInfo.CurrentCulture, out value)) ||
				     double.IsNaN(value) || double.IsInfinity(value))
					throw new InvalidDataException("The weights file contains an invalid number.");
				return value;
			}).ToArray();
		}

		public void LoadWeightValues(double[] values)
		{
			int expected = (ineuron.Length + oneuron.Length + 1) * hneuron.Length + oneuron.Length;
			if (values.Length != expected)
				throw new InvalidDataException("The weights file does not match the network dimensions.");

			int index = 0;
			foreach (INeuron input in ineuron)
				for (int hidden = 0; hidden < hneuron.Length; hidden++)
					input.setWeight(hidden, values[index++]);
			foreach (HNeuron hidden in hneuron)
				for (int output = 0; output < oneuron.Length; output++)
					hidden.setWeight(output, values[index++]);
			foreach (HNeuron hidden in hneuron) hidden.setBias(values[index++]);
			foreach (ONeuron output in oneuron) output.setBias(values[index++]);
		}
	}
}
