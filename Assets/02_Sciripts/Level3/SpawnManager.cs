using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public float maxTime = 1.6f;
    private float timer = 0;

    public float maxendTime = 1;
    private float endTime = 0;
    
    public float height;

    public GameObject pipe;
    public GameObject body;

    public float pipeSpawnInterval;
    public float partsSpawnInterval;

    public bool isgamestart = false;
    public Slider progressbar;

    private int pipeSpawnCnt = 0;

    void Update()
    {
        if (isgamestart)
        {
            movepipe();
        }
    }

    private void movepipe()
    {
        if (timer > maxTime) //timer 끝날때마다
        {
            GameObject newpipe = Instantiate(pipe);
            newpipe.transform.position = transform.position + new Vector3(0, Random.Range(-1f, 3.5f), 0);
            pipeSpawnCnt += 1;
            timer = 0;

            if(pipeSpawnCnt >= 3)
            {
                Invoke("SpawnParts", 0.4f);
                pipeSpawnCnt = 0;
            }
        }
        timer += Time.deltaTime;
        //progressbar.value = (float)endTime / 18f;
    }

    private void SpawnParts()
    {
        body.SetActive(true);
        body.transform.position = transform.position + new Vector3(0, Random.Range(-0.2f, 2.7f), 0);
    }
}
