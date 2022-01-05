using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public Transform Cam;
    public float height;
    public float speed;



    public void ToLevel(int i)
    {
        CheckpointManager.UpdateCheckpoint(i);
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
    
}
