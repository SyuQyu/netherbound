using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSpawn : MonoBehaviour
{
    [SerializeField] private GameObject prefab = null;

    public GameObject Prefab
    {
        get { return this.prefab; }
        set { this.prefab = value; }
    }

    public void Reward()
    {
        Instantiate(this.prefab, this.transform.position, Quaternion.identity);
    }
}
