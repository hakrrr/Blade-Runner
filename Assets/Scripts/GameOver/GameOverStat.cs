using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverStat : MonoBehaviour
{
    private Data data;

    private void Awake()
    {
        data = GameObject.Find("Data").GetComponent<Data>();
    }

    private void Start()
    {
        transform.Find("Highscore").GetComponent<TextMeshProUGUI>().
            SetText("HighScore: " + data.Score.x + " pt");
        transform.Find("YourScore").GetComponent<TextMeshProUGUI>().
            SetText("Your Score: " + data.Score.y + " pt");
    }

}
