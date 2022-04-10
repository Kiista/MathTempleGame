using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

[Serializable()]
public struct UIManagerParametres
{
    [Header("Answers Options")]
    [SerializeField] float margins;
    public float Margins { get { return margins; } }

    [Header("Resolution Screen Options")]
    [SerializeField] Color correctBGColor;
    public Color CorrectBGColor { get { return correctBGColor; } }
    [SerializeField] Color incorrectBGColor;
    public Color IncorrectBGColor { get { return incorrectBGColor; } }
    [SerializeField] Color finalBGColor;
    public Color FinalBGColor { get { return finalBGColor; } }
}
[Serializable()]
public struct UIElements
{
    [Header("answersContentArea")]
    [SerializeField] RectTransform answersContentArea;
    public RectTransform AnswersContentArea { get { return answersContentArea; } }

    [Header("questionInfoTextObject")]
    [SerializeField] TextMeshProUGUI questionInfoTextObject;
    public TextMeshProUGUI QuestionInfoTextObject { get { return questionInfoTextObject; } }

    [Header("P1scoreText")]
    [SerializeField] TextMeshProUGUI P1scoreText;
    public TextMeshProUGUI P1ScoreText { get { return P1scoreText; } }

    [Header("P2scoreText")]
    [SerializeField] TextMeshProUGUI P2scoreText;
    public TextMeshProUGUI P2ScoreText { get { return P2scoreText; } }

    [Space]

    [SerializeField] Animator resolutionScreenAnimator;
    public Animator ResolutionScreenAnimator { get { return resolutionScreenAnimator; } }

    [Header("resolutionBackground")]
    [SerializeField] Image resolutionBackground;
    public Image ResolutionBG { get { return resolutionBackground; } }

    [Header("resolutionStateInfoText")]
    [SerializeField] TextMeshProUGUI resolutionStateInfoText;
    public TextMeshProUGUI ResolutionStateInfoText { get { return resolutionStateInfoText; } }

    [Header("resolutionScoreText")]
    [SerializeField] TextMeshProUGUI resolutionScoreText;
    public TextMeshProUGUI ResolutionScoreText { get { return resolutionScoreText; } }

    [Space]
    [Header("highScoreText")]
    [SerializeField] TextMeshProUGUI highScoreText;
    public TextMeshProUGUI HighScoreText { get { return highScoreText; } }

    [Header("mainCanvasGroup")]
    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup MainCanvasGroup { get { return mainCanvasGroup; } }

    [Header("finishUIElements")]
    [SerializeField] RectTransform finishUIElements;
    public RectTransform FinishUIElements { get { return finishUIElements; } }

}
public class UIManager : MonoBehaviour
{
    public enum ResolutionScreenType { Correct, Incorrect, Finish }


    [Header("References")]
    [SerializeField] GameEvents events;

    [Header("Answer Prefabs")]
    [SerializeField] AnswerData answerPrefab;

    [Header("Game Manager")]
    [SerializeField] GameManager masterManager;


    [Space]
    [SerializeField] UIElements uIElements;

    [Space]
    [SerializeField] UIManagerParametres parameters;

    private PlayerData currentPlayerData;
    private GameManager gameManager;

    List<AnswerData> currentAnswers = new List<AnswerData>();
    private int resStateParaHash = 0;

    private IEnumerator IE_DisplayTimedResolution;

    void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
        events.DisplayResolutionScreen += DisplayResolution;
        events.ScoreUpdated += UpdateScoreUI;

    }
    void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
        events.DisplayResolutionScreen -= DisplayResolution;
        events.ScoreUpdated -= UpdateScoreUI;
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        currentPlayerData = gameManager.player1;

        uIElements.P1ScoreText.text = "Player 1 Score: " + gameManager.player1.numberOfCorrectAnswers;
        uIElements.P2ScoreText.text = "Player 2 Score: " + gameManager.player2.numberOfCorrectAnswers;

        UpdateScoreUI(currentPlayerData.numberOfCorrectAnswers);
        resStateParaHash = Animator.StringToHash("ScreenState");
    }
    void UpdateQuestionUI(Question question)
    {
        uIElements.QuestionInfoTextObject.text = question.info;
        CreateAnswers(question);

    }

    void DisplayResolution(ResolutionScreenType type, int score)
    {
        UpdateResolutionUI(type, score);
        uIElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 2);
        uIElements.MainCanvasGroup.blocksRaycasts = false;

        if (type != ResolutionScreenType.Finish)
        {
            if (IE_DisplayTimedResolution != null)
            {
                StopCoroutine(IE_DisplayTimedResolution);
            }
            IE_DisplayTimedResolution = DisplayTimeResolution();
            StartCoroutine(IE_DisplayTimedResolution);
        }
    }

    IEnumerator DisplayTimeResolution()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        uIElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 1);
        uIElements.MainCanvasGroup.blocksRaycasts = true;
    }

    void UpdateResolutionUI(ResolutionScreenType type, int score)
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

        switch (type)
        {
            case ResolutionScreenType.Correct:
                uIElements.ResolutionBG.color = parameters.CorrectBGColor;
                uIElements.ResolutionStateInfoText.text = "CORRECT";
                uIElements.ResolutionScoreText.text = "+" + score;
                break;
            case ResolutionScreenType.Incorrect:
                uIElements.ResolutionBG.color = parameters.IncorrectBGColor;
                uIElements.ResolutionStateInfoText.text = "WRONG";
                uIElements.ResolutionScoreText.text = "-" + score;
                break;
            case ResolutionScreenType.Finish:
                uIElements.ResolutionBG.color = parameters.FinalBGColor;
                uIElements.ResolutionStateInfoText.text = "FINAL SCORE";

                StartCoroutine(CalculateScore());
                uIElements.FinishUIElements.gameObject.SetActive(true);
                uIElements.HighScoreText.gameObject.SetActive(true);
                uIElements.HighScoreText.text = ((highscore > events.StartupHighScore) ? "new " : String.Empty) + "Highscore:" + highscore;
                break;
        }
    }

    IEnumerator CalculateScore()
    {

        if (events.CurrentFinalScore == 0)
        {
            uIElements.ResolutionScoreText.text = 0.ToString() ;
            yield break;
        }
        var scoreValue = 0;

        var scoreMoreThanZero = events.CurrentFinalScore > 0;

        while ( scoreMoreThanZero ? scoreValue < events.CurrentFinalScore : scoreValue > events.CurrentFinalScore)
        {
            scoreValue += scoreMoreThanZero ? 1 : -1;
            uIElements.ResolutionScoreText.text = scoreValue.ToString();

            yield return null;
        }
    }

    void CreateAnswers(Question question)
    {
        EraseAnswers();

        float offset = 0 - parameters.Margins;
        for (int i = 0; i < question.Answers.Length; i++)
        {
            AnswerData newAnswer = (AnswerData)Instantiate(answerPrefab, uIElements.AnswersContentArea);
            newAnswer.UpdateData(question.Answers[i].Info, i );

            newAnswer.Rect.anchoredPosition = new Vector2(0, offset);

            offset -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);
            uIElements.AnswersContentArea.sizeDelta = new Vector2(uIElements.AnswersContentArea.sizeDelta.x, offset * -1);

            currentAnswers.Add(newAnswer);
        }
    }

    void EraseAnswers()
    {
        foreach (var answer in currentAnswers)
        {
            Destroy(answer.gameObject);
        }
        currentAnswers.Clear();
    }

    void UpdateScoreUI(int score)
    {
        if (currentPlayerData == gameManager.player1)
        {
            uIElements.P2ScoreText.text = "Player 2 Score: " + gameManager.player2.numberOfCorrectAnswers;
        }
        else
        {
            
            uIElements.P1ScoreText.text = "Player 1 Score: " + gameManager.player1.numberOfCorrectAnswers;
        }
        if (currentPlayerData == gameManager.player2)
        {
            uIElements.P2ScoreText.text = "Player 2 Score: " + gameManager.player2.numberOfCorrectAnswers;
        }
        else
        {
            
            uIElements.P1ScoreText.text = "Player 1 Score: " + gameManager.player1.numberOfCorrectAnswers;
        }
       
            
    }
}
