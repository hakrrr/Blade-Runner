using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CleanUp : MonoBehaviour
{
    GameObject Terrain;
    void Start()
    {
        //Terrain = AssetDatabase.LoadAssetAtPath("Assets/Prefab/Cybercity/TerrainA.prefab",
        //    typeof(GameObject)) as GameObject;
        Terrain = PrefabUtility.LoadPrefabContents("Assets/Prefab/Cybercity/TerrainD.prefab");
        if (Terrain != null)
        {
            Debug.Log("Successfully loaded!");
            for (int i = 0; i < Terrain.transform.childCount; i++)
            {
                GameObject child = Terrain.transform.GetChild(i).gameObject;
                if (child.GetComponents<Component>().Length == 1)
                {
                    Debug.Log("Destroying " + child.name);
                    Destroy(child);
                }
                else if (child.GetComponent<Collider>() == null && child.layer != 8)
                {
                    Destroy(child.GetComponent<Collider>());
                }
            }
        }
        PrefabUtility.SaveAsPrefabAsset(Terrain, "Assets/Prefab/Cybercity/TerrainD.prefab");
        PrefabUtility.UnloadPrefabContents(Terrain);
    }
}
