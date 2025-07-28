using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class PlayGame : MonoBehaviour
{
   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) )
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            SceneManager.LoadScene(1);
        }
    }
}
