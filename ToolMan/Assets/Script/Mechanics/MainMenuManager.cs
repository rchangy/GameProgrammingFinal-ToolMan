using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void ToLevel(int i)
    {
        SceneManager.LoadScene("Level" + i);
    }

    public void StoryMode()
    {

    }

    public void ArenaMode()
    {

    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }

}
