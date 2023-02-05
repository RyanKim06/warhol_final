using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartsItem : MonoBehaviour
{
    public FlyLittlePlayer player;
    public Slider progressBar;
    public int partsNum;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        player.partsNum += 1;
        progressBar.value += (float)(1f/partsNum);

        Debug.Log($"{progressBar.value}, {1 / partsNum}");
        Debug.Log("player.partsNum: " + player.partsNum);

        gameObject.SetActive(false);
    }
}
