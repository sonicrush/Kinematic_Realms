using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeInTime : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Button button;
    private int state;
    private Color myOrange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(expandEquation);
        ColorUtility.TryParseHtmlString("#FF7600", out myOrange);
    }
    void expandEquation()
    {
        if (state == 0)
        {
            text.text = "Tf - Ti";
            state++;
            return;
        }
        if (state == 1)
        {
            text.text = "TimeFinal - TimeInitial";
            text.color = Color.white;
            state++;
            return;
        }
        if (state == 2)
        {
            text.text = "Δt";
            text.color = myOrange;
            state = 0;
            return;
        }

    }
}