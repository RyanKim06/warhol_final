using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject boss;
    public int vaildHitsToCombo; //ÄÞº¸¸¦ ÃæÁ·ÇÏ´Â ÄÞº¸°ø°Ý È½¼ö

    private GameObject bossSpawnPos;
    private int comboCount = 0;

    private void Awake()
    {
        bossSpawnPos = GameObject.Find("BossSpawnPos");
    }

    public void SpawnBoss()
    {
        Instantiate(boss, bossSpawnPos.transform.position, bossSpawnPos.transform.rotation);
    }

    public void KillBoss()
    {
        Debug.Log($"Boss Died, comboCount: {comboCount}, vaildHitsToCombo: {vaildHitsToCombo}");
    }

    public void AttackCombo()
    {
        comboCount++;

        if(comboCount <= vaildHitsToCombo)
            KillBoss();

    }
}
