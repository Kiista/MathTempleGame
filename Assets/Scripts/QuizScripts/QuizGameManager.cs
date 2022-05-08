using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizGameManager : MonoBehaviour {
    private Data data = new Data();

    [SerializeField] GameEvents events = null;

    [SerializeField] Animator timerAnimtor = null;
    [SerializeField] TextMeshProUGUI timerText = null;
    [SerializeField] Color timerHalfWayOutColor = Color.yellow;
    [SerializeField] Color timerAlmostOutColor = Color.red;

    private List<AnswerData> PickedAnswers = new List<AnswerData>();
    private List<int> FinishedQuestions = new List<int>();
    private int currentQuestion = 0;

    private int timerStateParaHash = 0;

    private IEnumerator IE_WaitTillNextRound = null;
    private IEnumerator IE_StartTimer;
    private Color timerDefaultColor = Color.white;

    private bool IsFinished {
        get {
            return (FinishedQuestions.Count < data.Questions.Length) ? false : true;
        }
    }

    private void OnEnable () {
        events.UpdateQuestionAnswer += UpdateAnswers;
    }

    private void OnDisable () {
        events.UpdateQuestionAnswer -= UpdateAnswers;
    }

    private void Awake () {
        events.CurrentFinalScore = 0;
    }
    private void Start () {

        events.StartupHighScore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

        timerDefaultColor = timerText.color;
        LoadData();


        timerStateParaHash = Animator.StringToHash("TimerState");

        var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);

        Display();
    }

    public void UpdateAnswers (AnswerData newAnswer) {
        if (data.Questions[currentQuestion].Type == AnswerType.Single) {
            foreach (var answer in PickedAnswers) {
                if (answer != newAnswer) {
                    answer.Reset();
                }
            }
            PickedAnswers.Clear();
            PickedAnswers.Add(newAnswer);
        }
        else {
            bool alreadyPicked = PickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked) {
                PickedAnswers.Remove(newAnswer);
            }
            else {
                PickedAnswers.Add(newAnswer);
            }
        }
    }
    public void EraseAnswers () {
        PickedAnswers = new List<AnswerData>();
    }


    void Display () {
        EraseAnswers();
        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null) {
            events.UpdateQuestionUI(question);
        }

        if (question.UseTimer) {
            UpdateTimer(question.UseTimer);
        }
    }

    public void Accept () {
        UpdateTimer(false);
        bool isCorrect = CheckAnswers();

        if (isCorrect) {
            CustomSceneManager.LoadScene(11);
        }
        else {
            CustomSceneManager.LoadScene(12);
        }
        FinishedQuestions.Add(currentQuestion);

        UpdateScore((isCorrect) ? data.Questions[currentQuestion].AddScore : -data.Questions[currentQuestion].AddScore);

        if (IsFinished) {
            SetHighScore();
        }

        var type = (IsFinished) ? UIManager.ResolutionScreenType.Finish : (isCorrect) ? UIManager.ResolutionScreenType.Correct : UIManager.ResolutionScreenType.Incorrect;

        if (events.DisplayResolutionScreen != null) {
            events.DisplayResolutionScreen(type, data.Questions[currentQuestion].AddScore);
        }

        //AudioManager.instance.PlaySound((isCorrect) ? "CorrectSFX" : "IncorrectSFX");

        if (type != UIManager.ResolutionScreenType.Finish) {

            if (IE_WaitTillNextRound != null) {
                StopCoroutine(IE_WaitTillNextRound);
            }
            IE_WaitTillNextRound = WaitTillNextRound();
            StartCoroutine(IE_WaitTillNextRound);
        }

    }

    private void UpdateTimer (bool state) {
        switch (state) {
            case true:
                IE_StartTimer = StartTimer();
                StartCoroutine(IE_StartTimer);

                timerAnimtor.SetInteger(timerStateParaHash, 2);
                break;
            case false:
                if (IE_StartTimer != null) {
                    StopCoroutine(IE_StartTimer);
                }

                timerAnimtor.SetInteger(timerStateParaHash, 1);
                break;
        }

    }

    IEnumerator StartTimer () {
        var totalTime = data.Questions[currentQuestion].Timer;
        var timeLeft = totalTime;

        timerText.color = timerDefaultColor;
        while (timeLeft > 0) {
            timeLeft--;

            //AudioManager.instance.PlaySound("CountdownSFX");

            if (timeLeft < totalTime / 2 && timeLeft > totalTime / 4) {
                timerText.color = timerHalfWayOutColor;
            }
            if (timeLeft < totalTime / 4) {
                timerText.color = timerAlmostOutColor;
            }

            timerText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        Accept();
    }

    IEnumerator WaitTillNextRound () {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
    }


    bool CheckAnswers () {
        if (!CompareAnswers()) {
            return false;
        }
        return true;
    }
    bool CompareAnswers () {
        if (PickedAnswers.Count > 0) {
            List<int> c = data.Questions[currentQuestion].GetCorrectAnswers();
            List<int> p = PickedAnswers.Select(x => x.AnswerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }
        return false;
    }

    void LoadData () {
        data = Data.Fetch();
    }

    public void RestartAGame () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame () {
        Application.Quit();
    }

    private void SetHighScore () {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        if (highscore < events.CurrentFinalScore) {
            PlayerPrefs.SetInt(GameUtility.SavePrefKey, events.CurrentFinalScore);
        }
    }

    private void UpdateScore (int add) {
        events.CurrentFinalScore += add;
        if (events.CurrentFinalScore < 0) {
            events.CurrentFinalScore = 0;
        }
        events.ScoreUpdated?.Invoke(events.CurrentFinalScore);
    }

    Question GetRandomQuestion () {
        var randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;

        return data.Questions[currentQuestion];
    }
    int GetRandomQuestionIndex () {
        var random = 0;
        if (FinishedQuestions.Count < data.Questions.Length) {
            do {
                random = UnityEngine.Random.Range(0, data.Questions.Length);
            } while (FinishedQuestions.Contains(random) || random == currentQuestion);
        }
        return random;
    }
}
