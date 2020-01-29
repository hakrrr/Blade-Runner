using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{

    static Gamemanager current;
    [SerializeField] private GameObject[] groundObstacle;
    [SerializeField] private GameObject[] airObstacle;
    [SerializeField] private Transform[] terrains;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject hand;
    [SerializeField] private float maxSpawnTime;
    public bool spawn;

    private const float powerDrain = 2f;
    private const float bladeCharge = 2f;
    private const float groundSpeed = 20f;

    [HideInInspector] public float power;
    [HideInInspector] public float bladeTime;

    private void Awake()
    {
        if (current != null && current != this)
            Destroy(gameObject);

        current = this;
        DontDestroyOnLoad(gameObject);
        power = 100;
        bladeTime = 0;
    }
    private void Start()
    {
        if (spawn)
        {
            StartCoroutine(GroundSpawner());
            StartCoroutine(AirSpawner());
        }
    }
    private void FixedUpdate()
    {
        foreach (Transform t in terrains)
        {
            t.position += Vector3.back * Time.deltaTime * player.Velocity * groundSpeed;
        }
        ///If in RunMode
        if (!hand.activeSelf)
        {
            power -= Time.deltaTime * powerDrain;
            bladeTime += Time.deltaTime * bladeCharge;
            //Debug.Log("Power: " + power + " BladeTime: " + bladeTime);
        }
    }
    private IEnumerator GroundSpawner()
    {
        int counter = 0;
        while (true)
        {
            if (counter == 30)
                maxSpawnTime /= 2;
            yield return new WaitWhile(() => hand.activeSelf);
            yield return new WaitForSeconds(Random.Range(0f, maxSpawnTime));
            Vector3 spawnPos = new Vector3(Random.Range(-3.3f, 4.4f), -4.7f, 100f);
            Quaternion spawnRot = Quaternion.Euler(0, Random.Range(0, 360f), 0);
            Instantiate(groundObstacle[Random.Range(0, groundObstacle.Length)], spawnPos, spawnRot);
            ++counter;
        }
    }
    private IEnumerator AirSpawner()
    {
        while (true)
        {
            yield return new WaitWhile(() => hand.activeSelf);
            yield return new WaitForSeconds(Random.Range(0f, maxSpawnTime));
            Vector3 spawnPos = new Vector3(Random.Range(-3.3f, 4.4f), Random.Range(-1.7f, -1), 100f);
            Quaternion spawnRot = Quaternion.Euler(Random.Range(-30f, 30f), Random.Range(0, 360f), Random.Range(-30f, 30));
            Instantiate(airObstacle[Random.Range(0, airObstacle.Length)], spawnPos, spawnRot);
        }
    }
}
