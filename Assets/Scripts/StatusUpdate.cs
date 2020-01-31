using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StatusUpdate : MonoBehaviour
{
    private Gamemanager gM;
    private Image power;
    private Image blade;
    private TextMeshProUGUI powerPer;
    private TextMeshProUGUI score;

    private float lastPowerPer;
    private float lastScore;
    private void Awake()
    {   
        gM = GameObject.Find("GameManager").GetComponent<Gamemanager>();
        power = transform.Find("Power").GetComponent<Image>();
        blade = transform.Find("BladeTime").GetComponent<Image>();
        powerPer = power.transform.Find("PowerPercent").GetComponent<TextMeshProUGUI>();
        score = transform.Find("Score").GetComponent<TextMeshProUGUI>();
    }

    //Note: GetComponentsInChildren includes parent object
    private void FixedUpdate()
    {
        Vector3 currStats = gM.GetStatus();
        //power.fillAmount = currStats[0];
        DOVirtual.Float(power.fillAmount, currStats[0], 0.2f, UpdatePower);
        blade.fillAmount = currStats[1];
        score.SetText(currStats[2] + " pt");
    }

    private void UpdatePower(float x)
    {
        power.fillAmount = x;
        powerPer.SetText((x * 100f).ToString("F1") + "%");
    }
}
