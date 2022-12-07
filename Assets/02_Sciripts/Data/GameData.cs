using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public List<string> stagenames;
    public int sceneNum;
    public int sceneToLoadNum;

    public bool isPlayGame = false;

    public int dieCount = 0;
    public List<string> parts;
    public bool gameClear = false;
    public bool[] unlockedStage = new bool[4] {true, false, false, false};
}
