using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data curr;
    private int highscore;
    private int score;
    private bool pc = true;

    private void Awake()
    {
        if (curr != null & curr != this)
            Destroy(gameObject);

        curr = this;
        DontDestroyOnLoad(gameObject);
        //Cursor.visible = false;
    }
    public bool Pc { get { return pc; } }
    public Vector2 Score{get { return new Vector2(highscore, score); }}
    public void SetScore(int s)
    {
        score = s;
        if (score > highscore)
            highscore = score;
    }
}
