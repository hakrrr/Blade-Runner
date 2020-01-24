using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameloop : MonoBehaviour
{

    [SerializeField] private GameObject m_Obstacle;
    [SerializeField] private float m_SpawnTime;

    private float m_PlayerVel;
    private const float m_SpawnMax = 20f;

    private IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_SpawnTime);
            Vector3 spawnPos = new Vector3(Random.Range(-3.3f, 4.4f), Random.Range(-3.5f, -1f), 100f);
            Instantiate(m_Obstacle, spawnPos, Quaternion.identity);
        }
    }

    private void Start()
    {
        StartCoroutine(Spawner());
    }

}
