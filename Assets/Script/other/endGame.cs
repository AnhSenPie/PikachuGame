
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endGame : MonoBehaviour
{
    Canvas canvas;
    public TextMeshProUGUI title;
    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }
    public void newGame()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }
    public void backHome()
    {
        SceneManager.LoadScene(0);
    }
    private void Update()
    {
        if(Board.Instance != null)
        {
            if (Board.Instance.winning)
            {
                title.text = "VICTORY";
                title.color = Color.white;
                canvas.enabled=true;
            }
            if (Board.Instance.defeat)
            {
                title.text = "DEFEAT";
                title.color = Color.red;
                canvas.enabled=true;
            }
        }

    }

}
