# Lung Cancer Risk Predictor (Custom C# Neural Network)

A Windows Forms desktop application developed in C# that predicts the likelihood of lung cancer based on lifestyle and clinical risk factors. Built using a custom 3-layer Multi-Layer Perceptron (MLP) neural network adhering strictly to custom backpropagation classes without third-party ML frameworks.

---

## Project Overview
* **Track:** Binary Classification (High Risk vs. Low Risk)
* **Architecture:** 15 Input Neurons, 8 Hidden Neurons, 1 Output Neuron
* **Activation Function:** Sigmoid
* **Loss Function:** Mean Squared Error (MSE)
* **Framework:** .NET Framework 4.8 (Windows Forms)

---

## Dataset & Preprocessing
* **Source:** [Kaggle - Survey Lung Cancer Dataset](https://www.kaggle.com/datasets/mysarahmadbhat/lung-cancer)
* **Total Records:** 309 raw rows (33 duplicates removed, 276 final rows)
* **Features (15 Inputs):**
  * Demographics: Gender, Age
  * Habits & Environment: Smoking, Alcohol Consumption, Peer Pressure
  * Symptoms & Health: Yellow Fingers, Anxiety, Chronic Disease, Fatigue, Allergies, Wheezing, Coughing, Shortness of Breath, Difficulty Swallowing, Chest Pain
* **Target (1 Output):** Lung Cancer (1 = Yes / High Risk, 0 = No / Low Risk)
* **Pipeline (`DataPreprocessor.cs`):**
  * Stratified 80/20 Train/Test split to preserve class distribution.
  * Minority class oversampling to prevent imbalance bias.
  * Min-Max normalization fitted on training data to scale features to the `[0, 1]` range.

---

## ⚙️ Model Training & Loss Progression

### Hyperparameters (Optimal Configuration)
* **Hidden Nodes:** 8
* **Epochs:** 1000
* **Learning Rate (LR):** 0.1

### Performance Metrics (Test Set)
* **Accuracy:** 87.5%
* **Precision:** 95.6%
* **Recall:** 89.6%

### Visual Studio Output Log
```text
[Training] Initialization started...
[Training] Network structured with 15 In, 8 Hidden, 1 Out.
[Training] Hyperparameters - LR: 0.1, Epochs: 1000
[Training] Parsing CSV data...
[Training] Dataset loaded. Final valid rows: 276
[Training] Performing stratified 80/20 train-test split...
[Training] Oversampling minority class to balance dataset...
[Training] Applying MinMaxScaler to normalize features...
[Training] Entering epoch loop. Tracking Mean Squared Error (MSE)...
[Epoch    1 / 1000] Loss (MSE): 0.250304
[Epoch  100 / 1000] Loss (MSE): 0.037976
[Epoch  200 / 1000] Loss (MSE): 0.025609
[Epoch  300 / 1000] Loss (MSE): 0.019426
[Epoch  400 / 1000] Loss (MSE): 0.017320
[Epoch  500 / 1000] Loss (MSE): 0.016015
[Epoch  600 / 1000] Loss (MSE): 0.014716
[Epoch  700 / 1000] Loss (MSE): 0.014252
[Epoch  800 / 1000] Loss (MSE): 0.013970
[Epoch  900 / 1000] Loss (MSE): 0.013840
[Epoch 1000 / 1000] Loss (MSE): 0.013562
[Training] Running test set evaluation...
[Training] Sequence Complete!
[Training Metrics] Accuracy: 87.5% | Precision: 95.6% | Recall: 89.6%
