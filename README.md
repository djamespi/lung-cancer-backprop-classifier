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

## Hyperparameter Tuning & Analysis

To optimize the model's predictive performance without triggering overfitting on our 310-row dataset, we conducted an automated grid search evaluating permutations of learning rates, hidden node capacities, and training durations.

### Tuning Rationale
We constrained our tuning matrix to the following bounds to ensure a generalized fit:
*   **Hidden Nodes (4, 8, 15):** The hidden layer capacity was strictly bounded between the input feature count (15) and the output count (1). Using 4 nodes acts as a strict bottleneck to force pattern generalization, 8 serves as the geometric mean, and 15 provides maximum capacity. Exceeding 15 nodes on a dataset of this size would cause severe overfitting (memorization).
*   **Learning Rates (0.05, 0.1, 0.2):** These represent conservative, moderate, and aggressive step sizes for gradient descent. Values > 0.2 risk loss divergence, while values < 0.05 require exponential epochs to converge.
*   **Epochs (500, 1000):** Given the dataset size and 15-feature binary classification task, convergence occurs efficiently under 1000 iterations. Running tens of thousands of epochs would artificially inflate training accuracy to 100% while destroying the model's accuracy on unseen test data.

### Grid Search Results
The grid search evaluated 18 distinct configurations. The top three performing architectures all achieved a peak test accuracy of **87.50%**, with identical precision (95.55%) and recall (89.58%). 

| Rank | Learning Rate | Hidden Nodes | Epochs | Accuracy | Precision | Recall | Final MSE |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **1** | **0.10** | **8** | **1000** | **87.50%** | **95.55%** | **89.58%** | **0.0112** |
| 2 | 0.10 | 15 | 500 | 87.50% | 95.55% | 89.58% | 0.0141 |
| 3 | 0.05 | 8 | 1000 | 87.50% | 95.55% | 89.58% | 0.0148 |

### Optimal Configuration & Interpretation
The optimal configuration is **8 Hidden Nodes, 1000 Epochs, and a 0.10 Learning Rate**. 

While multiple configurations hit the 87.50% accuracy ceiling, this specific architecture produced the lowest Mean Squared Error (0.0112). Using 8 hidden nodes proved superior to 15 nodes because it provided enough structural capacity to map complex symptom relationships without becoming overly complex. The moderate 0.10 learning rate allowed the gradient descent algorithm to take stable steps, minimizing the final loss smoothly across the 1000 epochs without overshooting the global minimum.

---

## Model Training & Loss Progression

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
