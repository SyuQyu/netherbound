using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveNextLevel : MonoBehaviour
{
    public GameObject enemies;
    private int HowManyEnemies;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            if (SoulManager.soulCount == HowManyEnemies)
            {
                // buat level dulu baru gw settingin
            }
        }
    }

    public int getChildren(GameObject obj)
    {
        int count = 0;

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            GameObject childObj = obj.transform.GetChild(i).gameObject;
            if (childObj.GetComponent<Damageable>() != null)
            {
                count++;
            }
        }
        return count;
    }

    // Start is called before the first frame update
    void Start()
    {
        HowManyEnemies = getChildren(enemies);
        Debug.Log("Child Count Custom: " + HowManyEnemies);
    }

    // Update is called once per frame
    void Update()
    {
        if (SoulManager.soulCount == HowManyEnemies)
        {
            Debug.Log("soul sudah tercukupi");
        }
        else
        {
            Debug.Log("soul tidak tercukupi, expected: " + HowManyEnemies + ", " + SoulManager.soulCount);
        }
    }
}