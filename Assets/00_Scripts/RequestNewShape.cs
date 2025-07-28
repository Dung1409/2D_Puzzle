using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestNewShape : MonoBehaviour
{
    private int requestCount;
    private Button _button;
    private TextMeshProUGUI text;
    void Start()
    {
        requestCount = PlayerPrefs.GetInt("RequestCount", 3);
        _button = GetComponent<Button>();
        ShowRequestCount();
        _button.onClick.AddListener(RequestShape);
        _button.interactable = requestCount > 0;
        Observer.AddListener("SaveData", SaveData);
    }

    void ShowRequestCount()
    {
        if (text == null)
        {
            text = _button.GetComponentInChildren<TextMeshProUGUI>();
        }
        text.text = requestCount.ToString();
    }

    public void RequestShape()
    {
        if (requestCount <= 0) return;
        requestCount--;
        ShowRequestCount();
        GameManager.intance.currentShapeData.Clear();
        Observer.Notify("GenerateShape");
        _button.interactable = requestCount > 0;
    }

    void SaveData()
    {
        PlayerPrefs.SetInt("RequestCount", requestCount);
        PlayerPrefs.Save();
    }
}
