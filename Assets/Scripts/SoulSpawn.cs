using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSpawn : MonoBehaviour
{
    [SerializeField] private GameObject prefab = null;
    [SerializeField] public float SpawnY = 2;

    public GameObject Prefab
    {
        get { return this.prefab; }
        set { this.prefab = value; }
    }

    public void Reward()
    {
        Instantiate(this.prefab, new Vector3(this.transform.position.x, this.transform.position.y - SpawnY, this.transform.position.z), Quaternion.identity);
    }
}
