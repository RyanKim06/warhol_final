using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyLittlePlayer : MonoBehaviour
{
    public float velocity = 1;
    private Rigidbody2D rb;
    public GameObject collider1;
    public GameObject collider2;

    public GameObject statePanel;
    public Text gamestateText;
    public GameObject restartButton;
    public GameObject crearButton;
    public GameObject startButton;

    public PipeSpawner pipespawner;
    public bool isgamestart = false;
    public Animator ani;

    public GameObject gameendpanel;
    public GameData gamedata;

    public GameObject EndingImg;
    public GameObject cutImg;

    void Start()
    {
        gamedata = GameObject.Find("GameData").GetComponent<GameData>(); // game data 가져옴
        gamedata.isPlayGame = true; // 게임을 한 번 플레이 했다고 알려줌
        rb = GetComponent<Rigidbody2D>();
        restartButton.SetActive(false);
        crearButton.SetActive(false);
        startButton.SetActive(true);
    }

    void Update()
    {
        if (isgamestart)
        {
            GetMouse();
        }
    }

    private void GetMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rb.velocity = Vector2.up * velocity;
        }
    }

    public void GameStart()
    {
        isgamestart = true;
        pipespawner.isgamestart = true;
        statePanel.SetActive(false);
        collider1.SetActive(false);
        ani.SetBool("isPlayerPro", true);
        StartCoroutine(WaitForIt());

    }

    public void GameCler()
    {
        Time.timeScale = 1f;
        gamedata.gameClear = false;
        LoadingSceneController.LoadScene("MainScene");
        gamedata.unlockedStage[0] = true;
        gamedata.unlockedStage[1] = true;
        gamedata.unlockedStage[2] = true;
        gamedata.unlockedStage[3] = true;
    }

    public void hideCut()
    {
        cutImg.SetActive(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        LoadingSceneController.LoadScene("GamePlayScene3");
    }

    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(1.0f);
        ani.SetBool("isPlayerSwim", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "stage3object")
        {
            Time.timeScale = 0f; //일시정지
            gameendpanel.SetActive(true);
            crearButton.SetActive(true);
            startButton.SetActive(false);
            restartButton.SetActive(false);
        }

        if (collision.gameObject.tag == "stageout")
        {
            Time.timeScale = 0f; //일시정지
            gameendpanel.SetActive(true);
            restartButton.SetActive(true);
            crearButton.SetActive(false);
            startButton.SetActive(false);
        }
    }

}
