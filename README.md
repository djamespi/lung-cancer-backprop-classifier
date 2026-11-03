# Lung Cancer Risk Predictor (Custom C# Neural Network)

A Windows Forms application that predicts the likelihood of lung cancer based on lifestyle and symptomatic factors. This project features a custom-built Multi-Layer Perceptron (MLP) neural network engine written entirely from scratch in C#, complete with an integrated data preprocessing pipeline and a wizard-style user interface.

## 🚀 Features

*   **Custom Neural Network Engine:** Implements a 3-layer architecture (Input, Hidden, Output) with feedforward and backpropagation algorithms, built without external machine learning libraries.
*   **Intuitive Survey UI:** A user-friendly, one-question-at-a-time wizard interface with dynamic inputs (sliders for age, buttons for yes/no questions) and a progress bar.
*   **Hidden Developer Mode:** A discrete configuration panel (accessed via the ⚙ icon) that allows developers to run training loops, tune hyperparameters (Learning Rate, Epochs, Hidden Nodes), and monitor output logs.
*   **Integrated Data Pipeline:** Automatically handles CSV parsing, stratified 80/20 train/test splitting, minority class oversampling, and MinMax feature scaling.
*   **Weight Management:** Save trained neural network weights to a local text file and load them on startup to bypass "Demo Mode".

## 📊 Dataset Description

The model is trained on the [Survey Lung Cancer Dataset](https://www.kaggle.com/datasets/mysarahmadbhat/lung-cancer) from Kaggle. The data is processed via `Data/survey lung cancer.csv` and includes 15 input features:
*   **Demographics:** Gender, Age
*   **Lifestyle:** Smoking, Alcohol Consumption, Peer Pressure
*   **Symptoms & Health:** Yellow Fingers, Anxiety, Chronic Disease, Fatigue, Allergies, Wheezing, Coughing, Shortness of Breath, Difficulty Swallowing, Chest Pain

## 🛠️ Tech Stack & Architecture

*   **Language & Framework:** C#, .NET Framework 4.8, Windows Forms (WinForms)
*   **Architecture Separation:** 
    *   `Libraries/`: Contains the core mathematical neural network classes (`NeuralNet`, `INeuron`, `HNeuron`, `ONeuron`).
    *   `RiskModel.cs` & `ModelConfig.cs`: Acts as the bridge between the raw neural network and the application, managing configurations, thresholds, and data routing.
    *   `DataPreprocessor.cs`: Handles all dataset cleaning, scaling, and manipulation.
    *   `Form1.cs`: Manages state for the UI wizard and hosts the background training execution.

## ⚙️ How to Run & Train

1.  **Compile & Launch:** Open `BackProp.sln` in Visual Studio, ensure the target is set to `.NET Framework 4.8`, and click Start.
2.  **Access Developer Mode:** Click the small gear icon (⚙) in the top left corner of the application to open the Developer & Tune Mode panel.
3.  **Run Training:** 
    *   Ensure `survey lung cancer.csv` is in the `Data/` folder and set to *Copy if newer* in its file properties.
    *   Adjust the hyperparameters (H-Nodes, Epochs, LR) and click **Run Training**.
    *   Check your Visual Studio **Output Window** (or console) to view epoch-by-epoch Mean Squared Error (MSE) reductions and the final evaluation metrics (Accuracy, Precision, Recall).
4.  **Save Weights:** Once satisfied with the model's accuracy, click **Save Weights**. Name the file `lung_cancer_weights.txt` and place it in the project root directory (set to *Copy if newer*) so the app automatically loads it on future startups.

## 👥 Authors

*   **Dexter James D. Benitez**
*   **Vince Mathew L. Silva**