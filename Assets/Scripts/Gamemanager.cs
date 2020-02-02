using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Gamemanager : MonoBehaviour
{

    static Gamemanager current;
    [SerializeField] private GameObject[] smallObst;
    [SerializeField] private GameObject[] bigObst;
    [SerializeField] private GameObject[] airObstacle;
    [SerializeField] private Transform[] terrains;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject hand;
    [SerializeField] private float maxSpawnTime;
    public bool spawn;

    public float powerDrain = 0.06f;
    public float bladeCharge = 0.5f;
    public float bladeDrain = 0.5f;
    private const float groundSpeed = 17.5f;

    private float power;
    private float bladeTime;
    private int score;

    private void Awake()
    {
        if (current != null && current != this)
            Destroy(gameObject);

        current = this;
        DontDestroyOnLoad(gameObject);
        power = 1; bladeTime = .3f; score = 0;
        player.OnUpdatePower += OnUpdateStats;
    }
    private void Start()
    {
        if (spawn)
        {
            StartCoroutine(SmallSpawner());
            StartCoroutine(BigSpawner());
            StartCoroutine(AirSpawner());
        }
    }
    private void FixedUpdate()
    {
        foreach (Transform t in terrains)
        {
            Vector3 delta = Vector3.back * Time.deltaTime * player.Velocity * groundSpeed;
            t.position += Vector3.back * Time.deltaTime * player.Velocity * groundSpeed;
        }
        ///If in RunMode
        if (!hand.activeSelf)
        {
            power = Mathf.Clamp(power - Time.deltaTime * powerDrain, 0, 1);
            bladeTime = Mathf.Clamp(bladeTime + Time.deltaTime * bladeCharge, 0, 1);
            score = Mathf.Clamp(Mathf.FloorToInt(Time.deltaTime * 100) + score, 0, 999999);
        }
        else
        {
            bladeTime = Mathf.Clamp(bladeTime - Time.deltaTime * bladeDrain, 0, 1);
        }
    }
    private IEnumerator SmallSpawner()
    {
        int counter = 0;
        while (true)
        {
            if (counter == 30)
                maxSpawnTime /= 2;
            yield return new WaitWhile(() => hand.activeSelf || !spawn);
            yield return new WaitForSeconds(Random.Range(0.3f, maxSpawnTime));
            Vector3 spawnPos = new Vector3(Random.Range(-2.75f, 4f), -4.7f, 100f);
            Quaternion spawnRot = Quaternion.Euler(0, Random.Range(0, 360f), 0);
            Instantiate(smallObst[Random.Range(0, smallObst.Length)], spawnPos, spawnRot);
            ++counter;
        }
    }    
    private IEnumerator BigSpawner()
    {
        while (true)
        {
            yield return new WaitWhile(() => hand.activeSelf || !spawn);
            yield return new WaitForSeconds(Random.Range(2f, maxSpawnTime * 1.5f));
            Vector3 spawnPos = new Vector3(Random.Range(-2.75f, 4f), -4.7f, 100f);
            Quaternion spawnRot = Quaternion.Euler(0, Random.Range(0, 360f), 0);
            Instantiate(bigObst[Random.Range(0, bigObst.Length)], spawnPos, spawnRot);
        }
    }
    private IEnumerator AirSpawner()
    {
        while (true)
        {
            yield return new WaitWhile(() => hand.activeSelf || !spawn);
            yield return new WaitForSeconds(Random.Range(0f, maxSpawnTime));
            Vector3 spawnPos = new Vector3(Random.Range(-3.3f, 4.4f), Random.Range(-1.7f, -1), 100f);
            Quaternion spawnRot = Quaternion.Euler(Random.Range(-30f, 30f), Random.Range(0, 360f), Random.Range(-30f, 30));
            Instantiate(airObstacle[Random.Range(0, airObstacle.Length)], spawnPos, spawnRot);
        }
    }
    public Vector3 GetStatus()
    {
        return new Vector3(power, bladeTime, score);
    }
    private void OnUpdateStats(float p, int s)
    {
        power += p;
        score += s;
    }

}
