using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using n_O2Gauge;
using Cinemachine;
using n_UiManager;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;

namespace n_Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speed; // 플레이어 이동 속도
        public Vector3 jumpDir; // 점프 힘, 앞으로가는 힘
        public bool velocityCtrl = true;

        private Rigidbody2D rb; // 중력 컴포넌트
        private float moveInput; // 이동 값 받기 -1(<-, a) , 0, 1(->, d)
        private Animator animator;

        //public float moveDrag;
        //public float stopDrag;

        private bool isGrounded; // 땅에 붙어있나 체크
        private bool isStageOut;

        public float jumpTimeCounter; // 점프 시간
        private bool isJumping; // 점프를 했는지 체크
        private bool isJimageIn = false;

        [SerializeField]    // 유니티 화면에 Gaugebar 변수 보이게 하기 위해
        private Slider Gaugebar; // 게이지 바

        private float maxGauge = 100; // 게이지 최대
        private float curGauge = 0; // 게이지 최소

        public GameObject reSpone;
        bool jumpKeyDown;
        public bool isFacingRight = true;
        public bool isCamerSizeDown = false;

        Bow bow;

        // 시네머신 카메라
        public Cinemachine.CinemachineVirtualCamera cinevirtual;

        public GameData gamedata;

        [Header("Waterdrop")]
        public bool useWaterdrop;
        public int maxWaterdrop;
        private int leftWaterdrop;
        [HideInInspector] public bool hasWaterdrop = false;
        public Text leftWaterdropText;
        private IObjectPool<Waterdrop> waterdropPool;

        [Header("LightExp")]
        public GameObject lightExpPrefab;
        public float lightThrowCounter = 0f;
        bool lightKeyDown;
        bool lightKeyClick = true;
        bool lightThrowing = false;
        GameObject lightExp;
        int lightKeyClickCnt = 0;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>(); // 중력 컴포넌트 가져옴
            Gaugebar.value = (float)curGauge / (float)maxGauge; // 초기 설정
            animator = GetComponent<Animator>(); // 애니메이션
            bow = GetComponent<Bow>(); // 점프 할때 예상 강도 보여줄려고 만든 스크립트 가져옴
            cinevirtual.m_Lens.OrthographicSize = 10; // 카메라 크기 초기설정
        }

        private void Start()
        {
            gamedata = GameObject.Find("GameData").GetComponent<GameData>(); // game data 가져옴
            gamedata.isPlayGame = true; // 게임을 한 번 플레이 했다고 알려줌

            if(useWaterdrop)
            {
                waterdropPool = ObstacleManager.Instance.GetWaterdropPool();
                leftWaterdrop = maxWaterdrop;
                leftWaterdropText.text = leftWaterdrop.ToString();
            }
        }

        private void FixedUpdate()
        {
            if (jumpKeyDown || isJumping)
            { 
                return;
            }

            if (moveInput != 0f && velocityCtrl)
            {
                rb.velocity = Vector2.right * moveInput * speed;
                O2.MoveO2GaugeDown(); //걸을때 산소 게이지 감소
            }

            Flip();
            ChangeAnim();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) //DEBUG
            {
                O2.DamageO2(80);
            }

            if (jumpKeyDown && cinevirtual.m_Lens.OrthographicSize < 15)
            {
                cinevirtual.m_Lens.OrthographicSize += 0.17f;
            }
            else if(isGrounded && cinevirtual.m_Lens.OrthographicSize > 10 && !jumpKeyDown)
            {
                cinevirtual.m_Lens.OrthographicSize -= 0.25f;
            }

            if (isStageOut) // 사망시 처음으로 이동
            {
                O2.DamageO2(100);
                isStageOut = false;
            }

            if (isGrounded == true && jumpKeyDown) // 땅에 있고 점프키를 누르고 있을때
            {
                isJumping = true; // 점프를 하고있다고 설정
                jumpTimeCounter += Time.deltaTime; // 누른시간 만큼 더하기
                if (jumpTimeCounter >= 1) // 만약 누른시간이 1보다 크다면 1로 고정
                {
                    jumpTimeCounter = 1;
                }
                animator.SetBool("isJumpCharge", true);
            }
            else if (isJumping == true) // 점프를 하고 있다면
            {
                if (jumpTimeCounter > 0) // 점프시간이 0보다 크다면
                {
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    // 점프시간이 0보다 작다면 낙하함
                    if (isGrounded)
                    {
                        animator.SetBool("isJump", false);
                        isJumping = false;
                        hasWaterdrop = false;
                    }
                }
                O2.MoveO2GaugeDown();
            }

            if(isGrounded && !jumpKeyDown) //땅에 있고 점프 버튼 안누르고 있을때
            {
                lightThrowing = true;
                lightThrowCounter += Time.deltaTime;
                if (lightThrowCounter >= 1)
                    lightThrowCounter = 1;
            }
            else if(lightThrowing == true && lightKeyDown == false)
            {
                if(lightThrowCounter > 0)
                    lightThrowCounter -= Time.deltaTime;

                lightThrowing = false;
            }

            HandleGauge();
            if(O2.O2Gaugebar.value <= 0)
            {
                animator.SetBool("isDeath", true);
            }
             
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // exit로 조져볼것
            if(collision.gameObject.tag == "round")
            {
                isGrounded = true;
            }
            if(collision.gameObject.tag == "stageout")
            {
                isStageOut = true;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "round")
            {
                isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "round")
            {
                isGrounded = false;
            }
        }

        void Jump()
        {
            //rb.velocity = Vector2.right * transform.localScale.x * speed;
            //rb.velocity = Vector2.up * jumpForce * jumpTimeCounter;
            //rb.velocity = new Vector2(transform.localScale.x * jumpDir.x, jumpDir.y * jumpTimeCounter);
            //Debug.Log(rb.velocity);
            //rb.velocity = jumpDir * jumpTimeCounter;
            if (!isJimageIn) { jumpTimeCounter = 0; }
            if (!isGrounded)
            {
                if (jumpTimeCounter != 0)
                {
                    bow.Jump(rb);
                }
                else
                {
                    rb.AddForce(new Vector2(0, 1),ForceMode2D.Impulse);
                }
            }
            animator.SetBool("isJumpCharge", false);
            animator.SetBool("isJump", true);
        }

        private void ExplodeLight()
        {
            lightExp.gameObject.GetComponent<LightExp>().TurnOn(5f);
        }

        private void ChangeAnim()
        {
            if (rb.velocity.normalized.x == 0)
            {
                animator.SetBool("IsWalking", false);
            }
            else
            {
                animator.SetBool("IsWalking", true);
            }
        }

        private void HandleGauge()
        {
            Gaugebar.value = Mathf.Lerp(Gaugebar.value, jumpTimeCounter, Time.deltaTime * 10);
        }

        public void SpawnWaterdrop()
        {
            Debug.Log("Click BTN");
            Debug.Log($"{leftWaterdrop > 0}, {!isGrounded}");
            if ((leftWaterdrop > 0) && !isGrounded)
            {
                Debug.Log("SpawnWaterdrop(), Pass if");
                var waterdrop = waterdropPool.Get();
                waterdrop.transform.position = transform.position;
                SetLeftWaterdrop(leftWaterdrop - 1);
            }
        }

        private void SetLeftWaterdrop(int i)
        {
            leftWaterdrop = i;
            leftWaterdropText.text = leftWaterdrop.ToString();

            if (leftWaterdrop <= 0)
                leftWaterdropText.color = Color.red;
        }

        public void JunpKeyDown() // 점프를 눌렀다고
        {
            jumpKeyDown = true;
            
            if (isGrounded)
            {
                bow.isBallThrowChange(isGrounded);
            }

            Debug.Log($"isgrounded keydown: {isGrounded}");
        }

        public void JunpKeyUp()
        {
            isGrounded = false;
            Jump();
            jumpKeyDown = false;
            bow.isBallThrowChange(isGrounded);
            Debug.Log($"isgrounded keyup: {isGrounded}");
        }

        public void LightKeyClick()
        {
            //if(lightKeyClick)
            //{
            //    lightExp = Instantiate(lightExpPrefab, transform.position, transform.rotation);
            //    lightExp.SetActive(true);
            //    lightKeyClick = !lightKeyClick;
            //}
            //else
            //{
            //    ExplodeLight();
            //    lightKeyClick = !lightKeyClick;
            //}

            //if (lightExp().isDelay) return;

            switch(lightKeyClickCnt)
            {
                case 0:
                    lightExp.SetActive(true);
                    break;
                case 1:
                    lightExp.GetComponent<LightExp>().TurnOn(5f);
                    break;
                default:
                    Debug.Log($"lightKeyClickCnt: out of range ({lightKeyClickCnt})");
                    break;
            }

            lightKeyClickCnt++;
            if(lightKeyClickCnt > 2)
            {
                lightKeyClickCnt = 0;
            }
        }

        public void ImageIn()
        {
            isJimageIn = true;
        }

        public void ImageOut()
        {
            isJimageIn = false;
        }

        public void MoveKeyDown(float MoveValue)
        {
            moveInput = MoveValue;
        }

        public void MoveKeyUp()
        {
            moveInput = 0;
        }

        private void Flip()
        {
            if(isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }

        public void CharReset()
        {
            Time.timeScale = 1f; //일시정지 해제

            if(gamedata.gameClear) //게임 클리어 시
            {
                gamedata.gameClear = false;
                gamedata.sceneToLoadNum = 0;
                LoadingSceneController.LoadScene("MainScene");
            }
            else
            {
                GetComponent<Transform>().position = reSpone.GetComponent<Transform>().position;
                isStageOut = false;
                animator.SetBool("isDeath", false);
                O2.IncreseO2(100);
                
                if(useWaterdrop)
                    SetLeftWaterdrop(maxWaterdrop);

                UiManager.ResetUi();
            }
            
        }

        public void ForceWithDamage(Vector2 force, float damage)
        {
            velocityCtrl = false;
            rb.AddForce(force, ForceMode2D.Impulse);
            O2.DamageO2(damage);
            velocityCtrl = true;
        }

        public void ChargeWaterdrop(int i)
        {
            leftWaterdrop = i;
            leftWaterdropText.text = leftWaterdrop.ToString();
            /*Debug.Log($"after charge: {leftWaterdrop}");*/
        }
    }
}
