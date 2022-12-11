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
        public float speed; // �÷��̾� �̵� �ӵ�
        public Vector3 jumpDir; // ���� ��, �����ΰ��� ��
        public bool velocityCtrl = true;

        private Rigidbody2D rb; // �߷� ������Ʈ
        private float moveInput; // �̵� �� �ޱ� -1(<-, a) , 0, 1(->, d)
        private Animator animator;

        //public float moveDrag;
        //public float stopDrag;

        private bool isGrounded; // ���� �پ��ֳ� üũ
        private bool isStageOut;

        public float jumpTimeCounter; // ���� �ð�
        private bool isJumping; // ������ �ߴ��� üũ
        private bool isJimageIn = false;

        [SerializeField]    // ����Ƽ ȭ�鿡 Gaugebar ���� ���̰� �ϱ� ����
        private Slider Gaugebar; // ������ ��

        private float maxGauge = 100; // ������ �ִ�
        private float curGauge = 0; // ������ �ּ�

        public GameObject reSpone;
        bool jumpKeyDown;
        public bool isFacingRight = true;
        public bool isCamerSizeDown = false;

        Bow bow;

        // �ó׸ӽ� ī�޶�
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
            rb = GetComponent<Rigidbody2D>(); // �߷� ������Ʈ ������
            Gaugebar.value = (float)curGauge / (float)maxGauge; // �ʱ� ����
            animator = GetComponent<Animator>(); // �ִϸ��̼�
            bow = GetComponent<Bow>(); // ���� �Ҷ� ���� ���� �����ٷ��� ���� ��ũ��Ʈ ������
            cinevirtual.m_Lens.OrthographicSize = 10; // ī�޶� ũ�� �ʱ⼳��
        }

        private void Start()
        {
            gamedata = GameObject.Find("GameData").GetComponent<GameData>(); // game data ������
            gamedata.isPlayGame = true; // ������ �� �� �÷��� �ߴٰ� �˷���

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
                O2.MoveO2GaugeDown(); //������ ��� ������ ����
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

            if (isStageOut) // ����� ó������ �̵�
            {
                O2.DamageO2(100);
                isStageOut = false;
            }

            if (isGrounded == true && jumpKeyDown) // ���� �ְ� ����Ű�� ������ ������
            {
                isJumping = true; // ������ �ϰ��ִٰ� ����
                jumpTimeCounter += Time.deltaTime; // �����ð� ��ŭ ���ϱ�
                if (jumpTimeCounter >= 1) // ���� �����ð��� 1���� ũ�ٸ� 1�� ����
                {
                    jumpTimeCounter = 1;
                }
                animator.SetBool("isJumpCharge", true);
            }
            else if (isJumping == true) // ������ �ϰ� �ִٸ�
            {
                if (jumpTimeCounter > 0) // �����ð��� 0���� ũ�ٸ�
                {
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    // �����ð��� 0���� �۴ٸ� ������
                    if (isGrounded)
                    {
                        animator.SetBool("isJump", false);
                        isJumping = false;
                        hasWaterdrop = false;
                    }
                }
                O2.MoveO2GaugeDown();
            }

            if(isGrounded && !jumpKeyDown) //���� �ְ� ���� ��ư �ȴ����� ������
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
            // exit�� ��������
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

        public void JunpKeyDown() // ������ �����ٰ�
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
            Time.timeScale = 1f; //�Ͻ����� ����

            if(gamedata.gameClear) //���� Ŭ���� ��
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
