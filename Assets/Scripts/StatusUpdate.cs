using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusUpdate : MonoBehaviour
{
    private Gamemanager gM;
    private Image power;
    private Image blade;
    private TextMeshProUGUI score;

    private void Awake()
    {   
        gM = GameObject.Find("GameManager").GetComponent<Gamemanager>();
        power = transform.Find("Power").GetComponent<Image>();
        blade = transform.Find("BladeTime").GetComponent<Image>();
        score = transform.Find("Score").GetComponent<TextMeshProUGUI>();

    }

    //Note: GetComponentsInChildren includes parent object
    private void FixedUpdate()
    {
        Vector3 currStats = gM.GetStatus();
        power.fillAmount = currStats[0];
        blade.fillAmount = currStats[1];
        score.SetText(currStats[2] + " pt");
    }
}
