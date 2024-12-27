
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    Canvas canvas;
    public time sliderTime;
    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }
    public void gameContinue() 
    {
        canvas.enabled = false;
    }
    public void newGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void backHome()
    {
        SceneManager.LoadScene(0);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        sliderTime.isPause = canvas.enabled;
    }
    public void Pause()
    {
        canvas.enabled = !canvas.enabled;
    }
}
