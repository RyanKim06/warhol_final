using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadTester : MonoBehaviour
{
    public GameData gamedata;
    void Start()
    {
        gamedata = GameObject.Find("GameData").GetComponent<GameData>();
    }

    public void sceneLoadTester(int Scennum)
    {
        gamedata.sceneNum = Scennum;
        LoadingSceneController.LoadScene(gamedata.stagenames[Scennum]);
    }

}
