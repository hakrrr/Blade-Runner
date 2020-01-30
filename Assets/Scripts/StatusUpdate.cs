using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUpdate : MonoBehaviour
{
    private Gamemanager gM;
    private Image[] bars;

    private void Awake()
    {
        bars = GetComponentsInChildren<Image>();
        gM = GameObject.Find("GameManager").GetComponent<Gamemanager>();
    }

    //Note: GetComponentsInChildren includes parent object
    private void FixedUpdate()
    {
        Vector2 currStats = gM.GetStatus();
        bars[1].fillAmount = currStats[0];
        bars[2].fillAmount = currStats[1];
    }
}
