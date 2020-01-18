using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private GameObject m_Player;
    [SerializeField] private float m_FloorSpeed = 5f;
    
    private Material m_PlaneMaterial;
    private void Start()
    {
        m_PlaneMaterial = GetComponent<Renderer>().material;
    }
    private void FixedUpdate()
    {
        float currVel = m_Player.GetComponent<PlayerController>().Velocity;
        Vector2 currPos = m_PlaneMaterial.GetTextureOffset("_BaseMap");
        Vector2 newPos = new Vector2(0, (currPos.y - Time.deltaTime * m_FloorSpeed * currVel) % 100);
        m_PlaneMaterial.SetTextureOffset("_BaseMap", newPos);
    }


}
