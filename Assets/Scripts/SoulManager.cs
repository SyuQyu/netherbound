using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoulManager : MonoBehaviour
{
    public static int soulCount;
    public TextMeshProUGUI soulText;
    private static SoulManager instance;

    public static SoulManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    
    // Metode statis untuk menambah jumlah soul
    public static void AddSoul()
    {
        soulCount++;
    }
    
    // Update is called once per frame
    void Update()
    {
        soulText.text = soulCount.ToString();
    }
}