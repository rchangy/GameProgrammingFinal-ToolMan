using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GamePauseOption : MonoBehaviour
{
    private float _currentTimeScale;
    [SerializeField] GameManager gameManager;

    private bool _originalHintControlEnable;

    public void Pause()
    {
        _currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        // Cut scene
        Objective o = gameManager.currentObjective;
        if (o)
        {
            Cutscene c = o.GetComponent<Cutscene>();
            if (c) c._skipEnable = false;
        }
        // Hint UI
        _originalHintControlEnable = gameManager.uIController.controlEnalbe;
        gameManager.uIController.SetControlEnable(false);
    }

    public void Resume()
    {
        Time.timeScale = _currentTimeScale;

        // Cut scene
        Objective o = gameManager.currentObjective;
        if (o)
        {
            Cutscene c = o.GetComponent<Cutscene>();
            if (c) c._skipEnable = true;
        }
        // Hint UI
        gameManager.uIController.SetControlEnable(_originalHintControlEnable);
    }
    public void ToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}
