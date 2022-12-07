using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using n_O2Gauge;

public class ObstacleMove : MonoBehaviour
{
    public Vector2 moveDir;
    public float speed;
    public float destoryLength;

    private Vector3 spawnPos;

    private void OnEnable()
    {
        spawnPos = transform.position;
    }

    private void Update()
    {
        transform.Translate(moveDir * Time.deltaTime * speed);

        //메테오 moveLength만큼 이동 시, 풀에 돌려줌
        if (transform.position.x < (spawnPos.x - destoryLength) || transform.position.y < (spawnPos.y - destoryLength))
        {
            MeteorB m = GetComponent<MeteorB>();
            Waterdrop w = GetComponent<Waterdrop>();

            if (m != null)
            {
                m.DestroyMeteor();
            }
            else if(w != null)
            {
                Debug.Log(w);
                w.DestroyWaterdrop();
            }
        }
    }

    public void SetSpawnPos(Vector3 pos)
    {
        spawnPos = pos;
    }
}
