using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public GameObject bossHealthUI;

    public int maxHealth;
    public int currentHealth;

    public Sprite filledHeart;
    public Sprite emptyHeart;

    private void Awake()
    {
        //healthBar.text = (currentHealth / maxHealth).ToString();
    }

    private void Start()
    {
        maxHealth = bossHealthUI.transform.childCount;
        currentHealth = maxHealth;
    }

    public void Damage(int delta)
    {
        //Debug.Log($"BOSS DMG, currenthealth {currentHealth}");
        //Debug.Log($"BOSS DMG, dmg scale {delta}");
        currentHealth -= delta; ;//5 -5 = 4

        if (currentHealth >= 0) // 4 > 0
        {
            for (int i = currentHealth; i > currentHealth - delta; i--) //4, 4 >= 3, 4--
            {
                Debug.Log($"decreased health obj num: {i}");
                //bossHealthUI.transform.GetChild(i).gameObject.SetActive(false);
                bossHealthUI.transform.GetChild(i).GetComponent<Image>().sprite = emptyHeart;
            }
        }
        //Debug.Log($"BOSS DMG: after dmg {currentHealth}");
        //healthBar.text = (currentHealth / maxHealth).ToString();
    }

    public void BossReset()
    {
        currentHealth = maxHealth;

        for(int i = 0; i < maxHealth; i++)
        {
            bossHealthUI.transform.GetChild(i).GetComponent<Image>().sprite = filledHeart;
        }
    }
}
