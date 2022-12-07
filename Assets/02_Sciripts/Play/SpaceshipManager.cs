using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipManager : MonoBehaviour
{
    public List<string> partsName;
    public List<GameObject> partsGameObject;

    private void Awake()
    {
        partsName = GameObject.Find("GameData").GetComponent<GameData>().parts;
        foreach(string one in partsName)
        {
            foreach(GameObject obj in partsGameObject)
            {
                if(obj.name == one)
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}
