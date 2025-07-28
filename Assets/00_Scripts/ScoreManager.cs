using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    public int score;
    public int bestScore;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;

    public Image bestScoreFront;
    public int bonus;
    public int lines;
    public Image newBestScore;
    public override void Awake()
    {
        base.Awake();
        score = PlayerPrefs.GetInt("Score", 0);
        bestScore = PlayerPrefs.GetInt("BestScore", 100);
        bonus = 0;
        scoreText.text = score.ToString();
        bestScoreText.text = bestScore.ToString();
        bestScoreFront.fillAmount = 1f * score / bestScore;
        Observer.AddListener(Contant.SaveData, SaveData);
    }
    public void UpdateScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();
        if (score > bestScore)
        {
            bestScore = score;
            bestScoreText.text = bestScore.ToString();
        }
        bestScoreFront.fillAmount = 1f * score / bestScore;
    }

    void OnDisable()
    {
        Observer.RemoveListener(Contant.SaveData, SaveData);
    }

    public void LinesCleared(int lines)
    {
        this.lines += lines;
        if (lines > 1)
        {
            // bonus += lines * 10; // Example bonus calculation
            // UpdateScore(bonus);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Score", score);
        if (PlayerPrefs.GetInt("Score") >= bestScore)
        {
            PlayerPrefs.SetInt("BestScore", bestScore);
        }
        PlayerPrefs.Save();
    }

    public void SetupBestScore()
    {
        if (score > PlayerPrefs.GetInt("BestScore", 0))
        {
            newBestScore.fillAmount = 1f;
            newBestScore.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
    }
}
