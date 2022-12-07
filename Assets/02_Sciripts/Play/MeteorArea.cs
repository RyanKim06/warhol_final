using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MeteorArea : MonoBehaviour
{ 
    public GameObject []meteors;
    public float spawnLengthOffset; //Area ������������ �󸶳� ����߷��� ������ ���ΰ�
    public bool meteorMoveHorizontal; //���׿� �������� ����(��->�Ʒ�==false or ����->����==true)

    private IObjectPool<MeteorB> meteorPool;

    //boss
    public bool boss = false;
    public float meteorDestoryLength;
    public float meteorSpeed;
    public float firstDelay;
    public float delay;
    public int bossMeteorLineCnt;
    public int bossMeteorSpawnCnt;

    private void Start()
    {
        meteorPool = ObstacleManager.Instance.GetMeteorPool();

        //StartCoroutine(SpawnMeteor());
    }

    private void SpawnMeteor(float offsetY)
    {
        if(meteorMoveHorizontal) //���׿� ����->����
        {
            //Instantiate(meteor[Random.Range(0, meteor.Length)],
            //    transform.position + new Vector3(spawnLengthOffset, offsetY),
            //    transform.rotation);

            var meteor = meteorPool.Get();
            meteor.transform.position = transform.position;
            meteor.transform.rotation = transform.rotation;
            meteor.transform.position += new Vector3(spawnLengthOffset, offsetY);
            meteor.GetComponent<ObstacleMove>().destoryLength = meteorDestoryLength;
            meteor.GetComponent<ObstacleMove>().speed = meteorSpeed;
        }
        else //���׿� ��->�Ʒ�
        {
            //Instantiate(meteor[Random.Range(0, meteor.Length)],
            //    transform.position + new Vector3(offsetY, spawnLengthOffset),
            //    transform.rotation);

            var meteor = meteorPool.Get();
            //Debug.Log("After Get()");
            meteor.transform.position = transform.position;
            meteor.transform.rotation = transform.rotation;
            meteor.transform.position += new Vector3(offsetY + 0.3f, spawnLengthOffset);
            //meteor.GetComponent<MeteorB>().destoryLength = meteorDestoryLength;
            //meteor.GetComponent<MeteorB>().speed = meteorSpeed;
        }
    }

    IEnumerator SpawnLevelMeteor()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);

            SpawnMeteor(4f);
            yield return new WaitForSeconds(2f);

            SpawnMeteor(-4f);
            yield return new WaitForSeconds(1f);

            SpawnMeteor(0f);
            yield return null;
        }
    }

    IEnumerator SpawnBossMeteor()
    {
        yield return new WaitForSeconds(firstDelay);

        for(int i = 0; i < bossMeteorLineCnt; i++)
        {
            for (int j = 0; j <= bossMeteorSpawnCnt; j++)
            {
                int offset = Random.Range(-bossMeteorSpawnCnt, bossMeteorSpawnCnt + 1);
                SpawnMeteor(offset);
                yield return new WaitForSeconds(0.04f);
            }
            yield return new WaitForSeconds(delay);
            //Debug.Log("SpawnBossMeteor");
        }
        yield return null;
        SetActive(false);
    }

    private void SetActive(bool b)
    {
        gameObject.SetActive(b);
    }

    private void OnEnable()
    {
        if (boss) //���� Mode �϶���
        {
            StartCoroutine(SpawnBossMeteor());
            //Debug.Log("OnEnable, SpawnBossMeteor() start coroutine");
        }
        else
        {
            StartCoroutine(SpawnLevelMeteor());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<MeteorB>().DestroyMeteor();
    }
}
