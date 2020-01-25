using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameloop : MonoBehaviour
{

    [SerializeField] private GameObject[] m_Obstacle;
    [SerializeField] private float m_SpawnTime;

    private IEnumerator CrystalSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0f, m_SpawnTime));
            Vector3 spawnPos = new Vector3(Random.Range(-3.3f, 4.4f), -4.7f, 100f);
            Quaternion spawnRot = Quaternion.Euler(Random.Range(-30f, 30f), Random.Range(0, 360f), Random.Range(-30f, 30));
            Instantiate(m_Obstacle[0], spawnPos, spawnRot);
        }
    }

    private IEnumerator MeteorSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0f, m_SpawnTime));
            Vector3 spawnPos = new Vector3(Random.Range(-3.3f, 4.4f), Random.Range(-1.7f, -1), 100f);
            Quaternion spawnRot = Quaternion.Euler(Random.Range(-30f, 30f), Random.Range(0, 360f), Random.Range(-30f, 30));
            Instantiate(m_Obstacle[1], spawnPos, spawnRot);
        }
    }

    private void Start()
    {
        StartCoroutine(CrystalSpawner());
        StartCoroutine(MeteorSpawner());
    }

}
