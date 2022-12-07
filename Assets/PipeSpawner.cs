using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipeSpawner : MonoBehaviour
{
    public float maxTime = 1;
    private float timer = 0;
    public float maxendTiem = 1;
    private float endTiem = 0;
    public GameObject pipe;
    public float height;

    public bool isgamestart = false;
    public Slider progressbar;

    public GameObject body;
    void Start()
    {
    }

    void Update()
    {
        if (isgamestart)
        {
            movepipe();
        }
    }

    private void movepipe()
    {
        if (endTiem < maxendTiem)
        {
            if (timer > maxTime)
            {
                GameObject newpipe = Instantiate(pipe);
                newpipe.transform.position = transform.position + new Vector3(0, Random.Range(-height, height), 0);
                Destroy(newpipe, 15);
                timer = 0;
            }
            timer += Time.deltaTime;
        }
        else
        {
            body.SetActive(true);
        }
        endTiem += Time.deltaTime;
        progressbar.value = (float)endTiem / 18.0f;
    }
}
