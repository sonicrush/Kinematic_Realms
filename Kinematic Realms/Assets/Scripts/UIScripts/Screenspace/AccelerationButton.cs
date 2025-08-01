using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Button button;
    public GameObject formulaUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(revealFormula);
    }

   void revealFormula()
    {
        formulaUI.SetActive(true);
    }
}
