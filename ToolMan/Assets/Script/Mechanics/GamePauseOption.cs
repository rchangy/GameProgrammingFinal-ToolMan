using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GamePauseOption : MonoBehaviour
{
    private float _currentTimeScale;

    public void Pause()
    {
        _currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = _currentTimeScale;
    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
