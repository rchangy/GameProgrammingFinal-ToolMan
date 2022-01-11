using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public Transform Cam;
    public float height;
    public float speed;

    [SerializeField] GameObject[] levelButtons;



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
        if (levelButtons.Length == 5)
        {
            CheckpointManager.LoadCheckpoint();
            int currentLevel = CheckpointManager.GetCheckpointInfo().level;
            for (int i = 0; i < 5; i++)
            {
                if (i >= currentLevel)
                    levelButtons[i].gameObject.SetActive(false);
                else
                    levelButtons[i].gameObject.SetActive(true);
            }
        }
    }
}
