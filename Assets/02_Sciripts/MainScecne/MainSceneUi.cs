using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUi : MonoBehaviour
{
    public GameObject StartPanel;
    public GameData gamedata;

    public GameObject[] stageIcon;
    public GameObject[] stageLockIcon;

    void Start()
    {
        //Debug.Log("MainMenu Start() called.");

        StartPanel.SetActive(true);
        gamedata = GameObject.Find("GameData").GetComponent<GameData>();
        if (gamedata.isPlayGame)
        {
            StartPanel.SetActive(false);
        }
        Application.targetFrameRate = 60;

        for(int i = 0; i < gamedata.unlockedStage.Length; i++)
        {
            //Debug.Log(gamedata.unlockedStage.Length);
            if (gamedata.unlockedStage[i] == true && stageLockIcon[i] != null) //��� ������ ���������̸�
            {
                //Debug.Log("unloacked stage icon setting called");
                stageIcon[i].GetComponent<Image>().color = new Color(255f, 255f, 255f);
                stageLockIcon[i].gameObject.SetActive(false);
            }
            else if(gamedata.unlockedStage[i] == false && stageLockIcon[i] != null) //���������� ��� �����̸�
            {
                //Debug.Log("loacked stage icon setting called");
                stageLockIcon[i].gameObject.SetActive(true);

                if (i == 2) //���� ���������� ���
                {
                    stageIcon[i].GetComponent<Image>().color = new Color(0.05f, 0.05f, 0.05f);
                }
                else
                {
                    stageIcon[i].GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
                }
            }
        }
    }

    public void GoToStage(int n)
    {
        if (gamedata.unlockedStage[n] == true) //unlocked == true(������� ����)�� ���������� Load
        {
            LoadingSceneController.LoadScene(gamedata.stagenames[n]);
        }
    }
}
