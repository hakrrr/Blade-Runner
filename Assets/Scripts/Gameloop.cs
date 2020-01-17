using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameloop : MonoBehaviour
{

    private readonly float []Lanes = new float [2] {-10f, 10f};

    [SerializeField] private GameObject m_Obstacle;
    [SerializeField] private GameObject m_Player;
    [SerializeField] private GameObject m_Ground;
    [SerializeField] private float m_SpawnTime;

    private Material m_PlaneMaterial;
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
        m_PlaneMaterial = m_Ground.GetComponent<Renderer>().material;
        StartCoroutine(Spawner());
    }

    private void FixedUpdate()
    {
        float currVel = m_Player.GetComponent<PlayerController>().Velocity;
        Vector2 currPos = m_PlaneMaterial.GetTextureOffset("_BaseMap");
        Vector2 newPos = new Vector2(0, (currPos.y - Time.deltaTime * 5 * currVel)%100);
        m_PlaneMaterial.SetTextureOffset("_BaseMap", newPos);
    }

}
