using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipParts : MonoBehaviour
{
    public GameData gamedata;
    public int stageToUnlock;
    bool isPartCheck;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isPartCheck = true;

        gamedata = GameObject.Find("GameData").GetComponent<GameData>();
        foreach(string one in gamedata.parts)
        {
            if(one == gamedata.name)
            {
                isPartCheck = false;
            }
        }

        if (isPartCheck)
        {
            gamedata.parts.Add(gameObject.name);
            gamedata.gameClear = true;
            gamedata.unlockedStage[stageToUnlock] = true;
        }
    }
}
