using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObstacleManager : MonoBehaviour
{
    //싱글톤 패턴
    private static ObstacleManager instance = null;

    //오브젝트 풀
    private IObjectPool<MeteorB> meteorPool;
    private IObjectPool<Waterdrop> waterdropPool;

    //프리팹 지정
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private GameObject waterdropPrefab;

    private void Awake()
    {
        //싱글톤 패턴
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            //Awake시 필요한 코드들 작성란
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

    //메테오 직접생성 대신 풀에서 가져오기
    private void OnGetMeteor(MeteorB meteor)
    {
        meteor.gameObject.SetActive(true);
    }

    //메테오 destory 대신 pool에 돌려줌
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
