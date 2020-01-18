using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameloop : MonoBehaviour
{

    private readonly float []Lanes = new float [2] {-10f, 10f};

    [SerializeField] private GameObject m_Obstacle;
    [SerializeField] private float m_SpawnTime;
    [SerializeField] private float m_FloorSpeed = 5f;

    private float m_PlayerVel;
    private const float m_SpawnMax = 20f;

    private IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_SpawnTime);
            Instantiate(m_Obstacle, new Vector3(Lanes[Random.Range(0,2)], .3f, 100), Quaternion.identity);
        }
    }

    private void Start()
    {
        StartCoroutine(Spawner());
    }

}
