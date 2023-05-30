using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveNextLevel : MonoBehaviour
{
    public GameObject enemies;
    private int HowManyEnemies;
    public int currentLevelIndex = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            if (SoulManager.soulCount == HowManyEnemies)
            {
                currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
                if (currentLevelIndex < SceneManager.GetActiveScene().buildIndex + 1)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else
                {
                    Debug.LogError("Next level does not exist!");
                }
                // buat level dulu baru gw settingin
            }
            else
            {
                Debug.Log("souls kurang");
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