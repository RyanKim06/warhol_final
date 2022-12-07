using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using n_O2Gauge;

namespace n_UiManager
{
    public class UiManager : MonoBehaviour
    {
        WaitForSeconds delay1 = new WaitForSeconds(1f);

        public List<Sprite> cutImages;
        public Image ImagePanel;
        
        public static GameObject gameEndPanel;
        public Text gameEndText;

        float time = 0f;
        float F_time = 2f;

        public Dialogue dialoge;
        public GameObject CutPanel;
        public GameData gamedata;

        [Header("BOSS")]
        public bool bossStage;
        public Text bossHealthText;
        public BossHealth bossHealth;

        private void Awake()
        {
            gamedata = GameObject.Find("GameData").GetComponent<GameData>();
            gameEndPanel = GameObject.Find("GameEnd");

            CutPanel.SetActive(true);   
            gameEndPanel.SetActive(false);
        }

        public void Start()
        {
            Debug.Log("UI MANAGER START");
            StartCoroutine(Fade());
        }

        public void Update()
        {
            if(O2.O2Gaugebar.value <= 0)
            {
                Time.timeScale = 0f; //일시정지
                gameEndPanel.SetActive(true);
                gameEndText.text = "실패";
            }

            if(bossStage)
            {
                if(bossHealth.currentHealth <= 0)
                {
                    Time.timeScale = 0f; //일시정지
                    gamedata.gameClear = true;
                }
            }

            if (gamedata.gameClear)
            {
                gameEndPanel.SetActive(true);
                gameEndText.text = "클리어";
                gamedata.unlockedStage[3] = true;
            }
        }


        IEnumerator Fade()
        {
            for(int i = 0; i < cutImages.Count; i++)
            {
                ImagePanel.sprite = cutImages[i];
                yield return StartCoroutine(Fadein());
                yield return StartCoroutine(FadeOut());
            }
            yield return StartCoroutine(PlayerDialog());
        }

        IEnumerator PlayerDialog()
        {
            CutPanel.SetActive(false);
            dialoge.UiSetActive(true);
            for(int i = 0; i < dialoge.playerDialogue.Length; ++i)
            {
                yield return StartCoroutine(dialoge.pTyping());
                yield return delay1;
            }
            dialoge.UiSetActive(false);
        }

        IEnumerator Fadein()
        {
            time = 0f;
            Color alpha = ImagePanel.color;
            while(alpha.a < 1f)
            {
                time += Time.deltaTime / F_time * 1.5f;
                alpha.a = Mathf.Lerp(0, 1, time);
                ImagePanel.color = alpha;
                yield return null;
            }

            yield return StartCoroutine(dialoge.typing());
            yield return delay1;

            yield return null;
        }

        IEnumerator FadeOut()
        {
            time = 0f;
            Color alpha = ImagePanel.color;
            while (alpha.a > 0f)
            {
                time += Time.deltaTime / F_time * 1.8f;
                alpha.a = Mathf.Lerp(1, 0, time);
                ImagePanel.color = alpha;
                yield return null;
            }

            yield return null;
        }

        IEnumerator pTypingCorutine()
        {
            CutPanel.SetActive(false);
            dialoge.UiSetActive(true);
            for (int i = 0; i < dialoge.playerDialogue.Length; ++i)
            {
                yield return StartCoroutine(dialoge.pTyping());
                yield return delay1;
            }
            dialoge.UiSetActive(false);
        }

        public void StopCorutine()
        {
            StopAllCoroutines();
            StartCoroutine(pTypingCorutine());
        }

        public static void ResetUi()
        {
            gameEndPanel.SetActive(false);
        }
    }
}
