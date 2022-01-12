using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public Transform Cam;
    public float height;
    public float speed;

    [SerializeField] GameObject[] levelButtonTexts;



    public void ToLevel(int i)
    {
        CheckpointManager.UpdateCheckpoint(i);
        SceneManager.LoadScene("Level" + i);
    }

    public void ArenaMode()
    {
        CheckpointManager.UpdateCheckpoint(5);
        SceneManager.LoadScene("Arena");
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }

    private void Start()
    {
        StartCoroutine(CamPos());
    }

    private IEnumerator CamPos()
    {
        float target = Cam.position.y - height;
        while (Cam.position.y > target)
        {
            Cam.position -= new Vector3(0, speed * Time.deltaTime, 0);
            if(Cam.position.y < target)
            {
                Cam.position = new Vector3(Cam.position.x, target, Cam.position.z);
            }
            yield return null;
        }
    }
    
    public void DisplayButtons()
    {
        if (levelButtonTexts.Length == 5)
        {
            CheckpointManager.LoadCheckpoint();
            int currentLevel = CheckpointManager.GetCheckpointInfo().level;
            for (int i = 0; i < 5; i++)
            {
                Text levelText = levelButtonTexts[i].GetComponent<Text>();
                if (i >= currentLevel)
                {
                    levelText.text = "???";
                }
                else
                {
                    levelText.text = "Level " + (i + 1).ToString();
                }
            }
        }
    }
}
