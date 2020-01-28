using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{

    static Gamemanager current;
    [SerializeField] private GameObject[] m_Obstacle;
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
            StartCoroutine(CrystalSpawner());
            StartCoroutine(MeteorSpawner());
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
    private IEnumerator CrystalSpawner()
    {
        while (true)
        {
            yield return new WaitWhile(() => hand.activeSelf);
            yield return new WaitForSeconds(Random.Range(0f, maxSpawnTime));
            Vector3 spawnPos = new Vector3(Random.Range(-3.3f, 4.4f), -4.7f, 100f);
            Quaternion spawnRot = Quaternion.Euler(Random.Range(-30f, 30f), Random.Range(0, 360f), Random.Range(-30f, 30));
            Instantiate(m_Obstacle[Random.Range(0, 2)], spawnPos, spawnRot);
        }
    }
    private IEnumerator MeteorSpawner()
    {
        while (true)
        {
            yield return new WaitWhile(() => hand.activeSelf);
            yield return new WaitForSeconds(Random.Range(0f, maxSpawnTime));
            Vector3 spawnPos = new Vector3(Random.Range(-3.3f, 4.4f), Random.Range(-1.7f, -1), 100f);
            Quaternion spawnRot = Quaternion.Euler(Random.Range(-30f, 30f), Random.Range(0, 360f), Random.Range(-30f, 30));
            Instantiate(m_Obstacle[2], spawnPos, spawnRot);
        }
    }
}
