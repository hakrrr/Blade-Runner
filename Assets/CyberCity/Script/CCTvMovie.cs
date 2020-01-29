using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTvMovie : MonoBehaviour 
{
    [ColorUsageAttribute(true, true)]
    public Color MaxBright = Color.black;
    public float Speed;
    public int MatId;
    public Texture[] Frames;
    int i;
    Material _Material;

    void Awake()
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks * 1000);
    }

	// Use this for initialization
	void Start () 
    {
        _Material = GetComponent<Renderer>().materials[MatId];
        //bright
        float random = Random.Range(1.0f, 1.5f);
        //color variations
        float random2 = Random.Range(0.2f, 0.8f);
        float random3 = Random.Range(0.2f, 0.8f);
        float random4 = Random.Range(0.2f, 0.8f);
        _Material.SetColor("_EmissionColor", new Color(random + random2, random + random3, random + random4));
        //emission level
        if (MaxBright != Color.black) GetComponent<Renderer>().materials[MatId].SetColor("_EmissionColor", MaxBright);
	}
	
	// Update is called once per frame
	void Update () 
    {
        i = (int)(Time.time * Speed);
        i = i % Frames.Length;
        _Material.SetTexture("_EmissionMap", Frames[i]);
	}
}
