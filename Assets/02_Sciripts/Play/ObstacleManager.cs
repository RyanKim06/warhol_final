using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObstacleManager : MonoBehaviour
{
    //�̱��� ����
    private static ObstacleManager instance = null;

    //������Ʈ Ǯ
    private IObjectPool<MeteorB> meteorPool;
    private IObjectPool<Waterdrop> waterdropPool;

    //������ ����
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private GameObject waterdropPrefab;

    private void Awake()
    {
        //�̱��� ����
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            //Awake�� �ʿ��� �ڵ�� �ۼ���
            meteorPool = new ObjectPool<MeteorB>(CreateMeteor, OnGetMeteor, OnReleaseMeteor, OnDestoryMeteor, maxSize: 100);
            waterdropPool = new ObjectPool<Waterdrop>(CreateWaterdrop, OnGetWaterdrop, OnReleaseWaterdrop, OnDestoryWaterdrop, maxSize: 40);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static ObstacleManager Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    /* Meteor Pool */
    public IObjectPool<MeteorB> GetMeteorPool()
    {
        return meteorPool;
    }

    private MeteorB CreateMeteor()
    {
        MeteorB meteor = Instantiate(meteorPrefab).GetComponent<MeteorB>();
        meteor.SetMeteorPool(meteorPool);
        return meteor;
    }

    //���׿� �������� ��� Ǯ���� ��������
    private void OnGetMeteor(MeteorB meteor)
    {
        meteor.gameObject.SetActive(true);
    }

    //���׿� destory ��� pool�� ������
    private void OnReleaseMeteor(MeteorB meteor)
    {
        meteor.gameObject.SetActive(false);
    }

    private void OnDestoryMeteor(MeteorB meteor)
    {
        Destroy(meteor.gameObject);
    }

    /* Waterdrop Pool */
    public IObjectPool<Waterdrop> GetWaterdropPool()
    {
        return waterdropPool;
    }

    private Waterdrop CreateWaterdrop()
    {
        Waterdrop waterdrop = Instantiate(waterdropPrefab).GetComponent<Waterdrop>();
        waterdrop.SetWaterdropPool(waterdropPool);
        return waterdrop;
    }

    private void OnGetWaterdrop(Waterdrop waterdrop)
    {
        waterdrop.gameObject.SetActive(true);
    }

    private void OnReleaseWaterdrop(Waterdrop waterdrop)
    {
        waterdrop.gameObject.SetActive(false);
    }

    private void OnDestoryWaterdrop(Waterdrop waterdrop)
    {
        Destroy(waterdrop.gameObject);
    }
}
