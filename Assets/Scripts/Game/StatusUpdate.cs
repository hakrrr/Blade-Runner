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

    private float lastScore;
    private void Awake()
    {   
        gM = GameObject.Find("GameManager").GetComponent<Gamemanager>();
        power = transform.Find("Power").GetComponent<Image>();
        blade = transform.Find("BladeTime").GetComponent<Image>();
        powerPer = power.transform.Find("PowerPercent").GetComponent<TextMeshProUGUI>();
        score = transform.Find("Score").GetComponent<TextMeshProUGUI>();
        lastScore = 0;
    }
    //Note: GetComponentsInChildren includes parent object
    private void FixedUpdate()
    {
        Vector3 currStats = gM.GetStatus();
        blade.fillAmount = currStats[1];
        DOVirtual.Float(power.fillAmount, currStats[0], 0.2f, UpdatePower);
        DOVirtual.Float(lastScore, currStats[2], 0.3f, UpdateScore);
        Color temp = blade.color;
        if (blade.fillAmount > 0.6f)
            temp.a = .8f;
        else
            temp.a = .35f;
        blade.color = temp;
    }
    private void UpdatePower(float x)
    {
        power.fillAmount = x;
        powerPer.SetText((x * 100f).ToString("F1") + "%");
    }
    private void UpdateScore(float x)
    {
        score.SetText(Mathf.FloorToInt(x) + " pt");
        lastScore = x;
    }
}
