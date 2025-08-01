using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ChangeInVelocity : MonoBehaviour
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
            text.text = "Vf - Vi";
            state++;
            return;
        }
        if (state == 1)
        {
            text.text = "VelocityFinal - VelocityInitial";
            text.color = Color.white;
            state++;
            return;
        }
        if (state == 2)
        {
            text.text = "Δv";
            text.color = myOrange;
            state = 0;
            return;
        }

    }
}
