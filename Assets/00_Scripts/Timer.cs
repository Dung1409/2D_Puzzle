using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timer;
    private float maxTimer;
    private Slider slider;
    public TextMeshProUGUI timerText;

    void Start()
    {
        this.gameObject.SetActive(false);
        maxTimer = 3;
        timer = maxTimer;
        slider = GetComponent<Slider>();
        Observer.AddListener("Timer", RegisterTimer);
    }

    public IEnumerator TimerRoutine()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            slider.value = timer / maxTimer;
            timerText.text = Mathf.Ceil(timer).ToString();
            yield return null;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void RegisterTimer()
    {
        this.gameObject.SetActive(true);
        PlayerPrefs.SetInt("BestScore", ScoreManager.intance.bestScore);
        StartCoroutine(TimerRoutine());
    }
}
