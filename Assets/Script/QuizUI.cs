using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizUI : MonoBehaviour
{
    [Serializable]
    public class Question
    {
        [TextArea] public string questionText;
        public string answerA;
        public string answerB;
        public string answerC;
        public string answerD;
        [Tooltip("0=A, 1=B, 2=C, 3=D")]
        public int correctIndex;
    }

    [Header("Questions (4 tane ekle)")]
    public List<Question> questions = new List<Question>();

    [Header("UI Refs")]
    public TMP_Text questionText;
    public Button btnA;
    public Button btnB;
    public Button btnC;
    public Button btnD;

    public TMP_Text txtA;
    public TMP_Text txtB;
    public TMP_Text txtC;
    public TMP_Text txtD;

    public TMP_Text feedbackText; // opsiyonel

    [Header("Settings")]
    public float feedbackSeconds = 0.6f;
    public bool hideOnFinish = true;

    private int _idx;
    private int _score;
    private bool _locked;
    private bool _started;

    private void Awake()
    {
        // Panel sahnede kapalı başlarsa Awake, StartQuiz içindeki SetActive(true) sırasında çalışır;
        // o durumda paneli tekrar kapatmamak için _started kontrol ediliyor.
        if (!_started)
            gameObject.SetActive(false);

        if (btnA) btnA.onClick.AddListener(() => Choose(0));
        if (btnB) btnB.onClick.AddListener(() => Choose(1));
        if (btnC) btnC.onClick.AddListener(() => Choose(2));
        if (btnD) btnD.onClick.AddListener(() => Choose(3));
    }

    public void StartQuiz()
    {
        if (questions == null || questions.Count == 0)
        {
            Debug.LogWarning("QuizUI: questions boş.");
            return;
        }

        _idx = 0;
        _score = 0;
        _locked = false;
        _started = true;

        gameObject.SetActive(true);
        Show(_idx);

        if (feedbackText) feedbackText.text = "";
    }

    private void Show(int i)
    {
        var q = questions[i];

        if (questionText) questionText.text = q.questionText;
        if (txtA) txtA.text = q.answerA;
        if (txtB) txtB.text = q.answerB;
        if (txtC) txtC.text = q.answerC;
        if (txtD) txtD.text = q.answerD;
    }

    private void Choose(int choice)
    {
        if (_locked) return;
        _locked = true;

        var q = questions[_idx];
        bool correct = (choice == q.correctIndex);
        if (correct) _score++;

        if (feedbackText) feedbackText.text = correct ? "Doğru ✅" : "Yanlış ❌";
        Invoke(nameof(Next), feedbackSeconds);
    }

    private void Next()
    {
        _locked = false;
        if (feedbackText) feedbackText.text = "";

        _idx++;
        if (_idx >= questions.Count)
        {
            Finish();
            return;
        }

        Show(_idx);
    }

    private void Finish()
    {
        if (feedbackText) feedbackText.text = $"Bitti! Skor: {_score}/{questions.Count}";

        if (hideOnFinish)
            Invoke(nameof(Hide), 1.0f);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
