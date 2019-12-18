using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameloop : MonoBehaviour
{
    [SerializeField] private GameObject m_Obstacle;
    [SerializeField] private float m_SpawnTime;

    private const float m_SpawnMax = 20f;

    private IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_SpawnTime);
            Instantiate(m_Obstacle, new Vector3(Random.Range(-m_SpawnMax, m_SpawnMax), .3f, 200), Quaternion.identity);
        }
    }

    private void Start()
    {
        StartCoroutine(Spawner());
    }

}
