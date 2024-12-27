
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class time : MonoBehaviour
{
    public Slider slider;
    public Color low;
    public Color high;
    public TextMeshProUGUI timeTxt;
    float timeCounter;
    public bool isPause = false;
    private void Start()
    {
        timeCounter = 300f;
    }
    private void Update()
    {

        if(timeCounter > 0 && !isPause && !Board.Instance.winning)
        {
            timeCounter -= Time.deltaTime;
            setTime();
        }
        if(timeCounter <= 0)
        {
            Board.Instance.defeat = true;
        } 
    }
    public void setTime()
    {
        slider.value = timeCounter;
        slider.maxValue = 100;
        slider.fillRect.GetComponent<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
        timeTxt.text = timeCounter.ToString("F2");
    }
}
