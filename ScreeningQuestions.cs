using System.Collections.Generic;

namespace BackProp
{
    public enum QuestionType
    {
        YesNo,
        Gender,
        Age
    }

    /// <summary>One screen of the questionnaire.</summary>
    public sealed class Question
    {
        public string Text { get; }
        public string Hint { get; }
        public QuestionType Type { get; }

        public Question(string text, string hint, QuestionType type)
        {
            Text = text;
            Hint = hint;
            Type = type;
        }
    }

    /// <summary>
    /// The questions, in EXACTLY the same order as the network's input neurons
    /// (the column order of the lung cancer CSV, without the LUNG_CANCER target).
    /// Do not reorder without retraining.
    /// </summary>
    public static class ScreeningQuestions
    {
        public static readonly IList<Question> All = new List<Question>
        {
            new Question("What is your gender?",                          "Choose the option that applies to you.",        QuestionType.Gender), // 0 GENDER
            new Question("How old are you?",                              "Drag the slider to your age.",                  QuestionType.Age),    // 1 AGE
            new Question("Do you smoke?",                                 "",                                              QuestionType.YesNo),  // 2 SMOKING
            new Question("Do you have yellowish stains on your fingers?", "Often linked to heavy smoking.",                QuestionType.YesNo),  // 3 YELLOW_FINGERS
            new Question("Do you often feel anxious?",                    "",                                              QuestionType.YesNo),  // 4 ANXIETY
            new Question("Do you experience peer pressure?",              "From friends, family or people around you.",    QuestionType.YesNo),  // 5 PEER_PRESSURE
            new Question("Do you have a chronic disease?",                "A long-term medical condition.",                QuestionType.YesNo),  // 6 CHRONIC DISEASE
            new Question("Do you often feel unusually tired?",            "Fatigue that doesn't go away with rest.",       QuestionType.YesNo),  // 7 FATIGUE
            new Question("Do you have allergies?",                        "",                                              QuestionType.YesNo),  // 8 ALLERGY
            new Question("Do you experience wheezing?",                   "A whistling sound when you breathe.",           QuestionType.YesNo),  // 9 WHEEZING
            new Question("Do you consume alcohol regularly?",             "",                                              QuestionType.YesNo),  // 10 ALCOHOL CONSUMING
            new Question("Do you have a frequent cough?",                 "",                                              QuestionType.YesNo),  // 11 COUGHING
            new Question("Do you experience shortness of breath?",        "",                                              QuestionType.YesNo),  // 12 SHORTNESS OF BREATH
            new Question("Do you have difficulty swallowing?",            "",                                              QuestionType.YesNo),  // 13 SWALLOWING DIFFICULTY
            new Question("Do you experience chest pain?",                 "",                                              QuestionType.YesNo),  // 14 CHEST PAIN
        };
    }
}
