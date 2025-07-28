using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    public GameObject square;
    public Dictionary<Vector2Int, GameObject> index = new Dictionary<Vector2Int, GameObject>();
    [Range(5, 10)] public int row;
    [Range(5, 10)] public int column;
    public bool isGameOver = false;
    public int shapeCount;

    [HideInInspector]
    public List<ShapeData> currentShapeData = new List<ShapeData>();

    [Header("Square Colors")]
    public Dictionary<string, Sprite> squareColors = new Dictionary<string, Sprite>();
    public Sprite errorShape;



    public override void Awake()
    {
        base.Awake();
        shapeCount = 0;
        
        squareColors = Resources.LoadAll<Sprite>("SquaresColor").
                    ToDictionary(s => s.name.ToLower(), s => s);
        squareColors.TryGetValue("red", out errorShape);

        Observer.checkGameOver = (object[] _currentShapeData) =>
        {
            if (_currentShapeData == null || _currentShapeData.Length == 0)
                return false;

            foreach (var data in _currentShapeData)
            {
                if (CheckCanDrop(data as ShapeData))
                {
                    return false;
                }
            }
            return true;
        };
    }

    void Update()
    {
        if (shapeCount == 0)
        {
            Observer.Notify(Contant.GenerateShape);
        }
        Observer.Notify(Contant.CheckGameOver);
    }

    public bool CheckAvailableSpace(Vector2Int startIdx, ShapeData shapeData)
    {
        foreach (var o in shapeData.Offsets())
        {
            Vector2Int idx = startIdx + o;
            if(!index.ContainsKey(idx) || index[idx].GetComponent<SelectTile>().available == 1)
            return false;
        }
        return true;
    }

    public bool CheckCanDrop(ShapeData shapeData)
    {
        for (int r = 0; r <= row - shapeData.row; r++)
        {
            for (int c = 0; c <= column - shapeData.column; c++)
            {
                Vector2Int idx = new Vector2Int(r, c);
                if (CheckAvailableSpace(idx, shapeData))
                {
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator WaitCheckGameOver()
    {
        yield return new WaitForSeconds(0.5f);
        if ((bool)Observer.checkGameOver?.Invoke(currentShapeData.ToArray()))
        {
            isGameOver = true;
            yield return new WaitForSeconds(3f);
            int bestScore = PlayerPrefs.GetInt("BestScore", 100);
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("BestScore", bestScore);
            UIManager.intance.Popup();
            Time.timeScale = 0;
        }
    }

    public void GameOver()
    {
        if(isGameOver) return;
        StartCoroutine(WaitCheckGameOver());
    }

    public Sprite GetSquareColor()
    {
        int idx = UnityEngine.Random.Range(0, squareColors.Count);
        string key = squareColors.Keys.ElementAt(idx);
        if (key == "red")
        {
            idx = UnityEngine.Random.Range(0, idx - 1);
            key = squareColors.Keys.ElementAt(idx);
        }
        return squareColors[key];
    }

}

#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
public class PlayModeExitHandler
{
    static PlayModeExitHandler()
    {
        UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
    {
        if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode && SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (GameManager.intance.isGameOver)
            { 
                int bestScore = PlayerPrefs.GetInt("BestScore");
                PlayerPrefs.SetInt("BestScore", bestScore);
                return;
            }
                
            BoardData boardData = new BoardData();
            boardData.SaveDataBoard();
            string json = JsonUtility.ToJson(boardData, true);
            PlayerPrefs.SetString(Contant.BoardData, json);
            Observer.Notify(Contant.SaveData);
            PlayerPrefs.Save();
        }
    }
    
}  
#endif