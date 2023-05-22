using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SoulManager : MonoBehaviour
{
    public int soulCount;

    public TextMeshProUGUI soulText;

    // Update is called once per frame
    void Update()
    {
        soulText.text = soulCount.ToString();
    }
}
