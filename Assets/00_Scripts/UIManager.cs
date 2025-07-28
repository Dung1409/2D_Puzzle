using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : Singleton<UIManager>
{
    public GameObject popup;
    public TextMeshProUGUI bestScoreText;
    public override void Awake()
    {
        base.Awake();
        popup.SetActive(false);
    }

    public void Popup()
    {
        popup.SetActive(!popup.activeSelf);
        Time.timeScale = popup.activeSelf ? 0 : 1;
        if (!popup.activeSelf)
        {
            Observer.Notify("Timer");
            return;
        }
        bestScoreText.text = ScoreManager.intance.bestScore.ToString();
        ScoreManager.intance.SetupBestScore();
    }

    public void TryAgain()
    {
        Time.timeScale = 1;
        Scene activeScene = SceneManager.GetActiveScene();
        int bestScore = PlayerPrefs.GetInt("BestScore", 100);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("BestScore", bestScore);
        SceneManager.LoadScene(activeScene.name);
    }

    public  void BackToHome()
    {
        if (!GameManager.intance.isGameOver)
        {
            BoardData boardData = new BoardData();
            boardData.SaveDataBoard();
            string json = JsonUtility.ToJson(boardData, true);
            PlayerPrefs.SetString(Contant.BoardData, json);
            Observer.Notify(Contant.SaveData);
            PlayerPrefs.Save();
        }
        SceneManager.LoadScene(0);
    }
}