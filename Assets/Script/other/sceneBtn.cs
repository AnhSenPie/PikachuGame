
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuControll : MonoBehaviour
{
    public void MoveScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void quitGame()
    {
        Application.Quit();
    }

}
