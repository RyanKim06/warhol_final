using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Boss1B : MonoBehaviour
{
    //Health
    private float health = 100f;

    //Attacks
    public float meteorAttackRange;
    public float spearAttackRange;
    public float rushAttackRange;
    public float attackDelay;

    //knockback(PushAttack)
    public float knockbackForce;
    private float forceDmg = 10f;

    //Meteor Attack
    public float meteorLineDelay;
    private IObjectPool<MeteorB> meteorPool;

    //Spear Attack
    public GameObject spear;
    public GameObject rayImg;
    [SerializeField] private LayerMask spearDetectLayer;
    private SpearController spearController;

    //Rush Attack
    public float rushSpeed;
    private bool rushAttack = false;

    [Header("UI")]
    public Image bossAttackPreviewImage;
    private float playerDistance;

    private GameObject player;
    private n_Player.PlayerController playerController;
    private bool attacking = false;
    private bool isFlipped = false;
    private float distance;
    private IEnumerator attackCo;
    private Rigidbody2D rb;
    private Animator animator;
    private BossHealth bossHealth;
    private IEnumerator attackPreviewFadeCo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bossHealth = GetComponent<BossHealth>();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<n_Player.PlayerController>();
        meteorPool = ObstacleManager.Instance.GetMeteorPool();
        spearController = spear.GetComponent<SpearController>();
        attackPreviewFadeCo = FadeInDeactive();
        Attack();
    }

    private void ChangeBossAttackText(string str)
    {
        //Debug.Log(bossAttackPreviewImage.transform.GetChild(0).GetComponent<Text>() != null);
        bossAttackPreviewImage.transform.GetChild(0).GetComponent<Text>().text = str;
    }

    private void Update()
    {
        if(attacking)
        {
            StartCoroutine(attackPreviewFadeCo);
        }
        else
        {
            StopCoroutine(attackPreviewFadeCo);
            bossAttackPreviewImage.color = Color.white;
            LookAtPlayer();
        }
        playerDistance = Vector2.Distance(PlayerPos(), transform.position);
        //Meteor Attack
        if (meteorAttackRange <= playerDistance && playerDistance < spearAttackRange) { ChangeBossAttackText("메테오"); }
        //Spear Attack
        else if (spearAttackRange <= playerDistance && playerDistance < rushAttackRange) { ChangeBossAttackText("창 투하"); }
        //Rush Attack
        else if (rushAttackRange <= playerDistance) { ChangeBossAttackText("돌진"); }

        //Debug.Log($"text update dist: {playerDistance}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) //�нú� �ɷ�: �÷��̾�� �浹 ��, �÷��̾�� �˹�ȿ��(Push)
        {
            Knockback(collision);
        }

        if(collision.gameObject.CompareTag("waterdrop"))
        {
            bossHealth.Damage(1);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //if(knockbackDelay == false) -> Knockback(collision);
            Knockback(collision); //+개선(넉백 딜레이 코루틴 추가, 코루틴에서 knockbackDelay 관리)
        }
    }

    private void Knockback(Collision2D collision)
    {
        //Vector2 dir = ((collision.transform.position - transform.position) + new Vector3(PlayerSide() * knockbackForce, knockbackForce)).normalized;
        //Vector2 force = dir * knockbackForce;
        //Debug.Log(force);

        //playerController.ForceWithDamage(force, forceDmg);
        Vector2 playerPos = PlayerPos();

        Vector2 target = new Vector2(playerPos.x + (10f * PlayerSide()), 15f);
        Vector2 dir = (target - playerPos).normalized;
        playerController.ForceWithDamage(dir * knockbackForce, forceDmg);
    }

    private void Attack()
    {
        Debug.Log($"Attack()");
        distance = Vector2.Distance(PlayerPos(), transform.position);

        //Meteor Attack
        if (meteorAttackRange <= distance && distance < spearAttackRange) { attackCo = MeteorAttack(); }
        //Spear Attack
        else if (spearAttackRange <= distance && distance < rushAttackRange) { attackCo = SpearAttack(); }
        //Rush Attack
        else if (rushAttackRange <= distance) { attackCo = RushAttack(); }

        StartCoroutine(attackCo);     
        //StartCoroutine(MeteorAttack());
    }

    IEnumerator MeteorAttack()
    {
        attacking = true;
        //alert red sign(coroutine)
        animator.SetBool("MeteorBool", true);
        Vector3 playerPos = PlayerPos();

        for (int i = 0; i < 6; i++) //line
        {
            for (int j = 0; j <= 6; j++) //meteors
            {
                float offsetX = Random.Range(-6f, 6f);
                MeteorB meteor = meteorPool.Get();
                ObstacleMove obstacleMove = meteor.GetComponent<ObstacleMove>();

                meteor.transform.position = player.transform.position; //���׿� ������ġ ������ġ��
                obstacleMove.SetSpawnPos(player.transform.position + new Vector3(offsetX + 0.3f, 15f));
                obstacleMove.moveDir = new Vector2(0f, -1f);
                obstacleMove.speed = Random.Range(6f, 14f);
                obstacleMove.destoryLength = 20f;

                meteor.transform.position += new Vector3(offsetX + 0.3f, 15f);
                meteor.GetComponentInChildren<SpriteRenderer>().transform.rotation = 
                    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));

                //Debug.Log($"{obstacleMove.transform.position}, {meteor.transform.position}");
                yield return new WaitForSeconds(0.04f);
            }
            yield return new WaitForSeconds(meteorLineDelay);
        }
        animator.SetBool("MeteorBool", false);
        yield return new WaitForSeconds(attackDelay);

        attacking = false;
        Attack();
        yield return null;
    }
    
    IEnumerator SpearAttack()
    {
        attacking = true;
        spear.SetActive(true);
        animator.SetTrigger("SpearTrigger");

        Vector2 playerPos = PlayerPos();
        float halfDist = (Vector2.Distance(playerPos, spear.transform.position) / 2);
        Vector3 throwTarget = new Vector2(halfDist * PlayerSide(), halfDist);

        //going up
        while(spear.transform.position.y <= throwTarget.y * 2 && spearController.collisionEntered == false)
        {
            spear.transform.position = Vector2.MoveTowards(spear.transform.position, spear.transform.position + throwTarget * 2, Time.deltaTime * 20f);
            spear.transform.rotation = Quaternion.RotateTowards(spear.transform.rotation, Quaternion.Euler(0f, 0f, 20f), Time.deltaTime * 4f);
            yield return new WaitForFixedUpdate();
        }
       
        //high rotate
        while(spear.transform.rotation.z < 0.999 && spearController.collisionEntered == false)
        {
            spear.transform.rotation = Quaternion.RotateTowards(spear.transform.rotation, Quaternion.Euler(0f, 0f, 178f), Time.deltaTime * 100f);
            Debug.Log(spear.transform.rotation.z);
            yield return new WaitForFixedUpdate();
        }

        Debug.Log("Start Draw Ray");    
        Debug.DrawRay(spear.transform.position, spear.transform.up * 50, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(spear.transform.position, spear.transform.up, 50f, spearDetectLayer);
        //Debug.Log($"{hit.collider.name}");
        if (hit) //Raycast hit이 유효하면
        {
            rayImg.transform.position = hit.point;
            rayImg.SetActive(true);
        }
        yield return new WaitForSeconds(0.8f);

        //going down(0.3f == margin)
        while (spear.transform.position.y > rayImg.transform.position.y + 0.3f && spearController.collisionEntered == false)
        {
            spear.transform.position = Vector2.MoveTowards(spear.transform.position, rayImg.transform.position, Time.deltaTime * 50f);
            yield return new WaitForFixedUpdate();
            Debug.Log("spear moves on");
        }
        Debug.Log("spear Finished");
        StartCoroutine(spearController.FadeInDeactive()); //FadeOut,SetActive(false)
        rayImg.SetActive(false);
        yield return new WaitForSeconds(attackDelay);

        attacking = false;
        Attack();
        yield return null;
    }
    
    IEnumerator RushAttack()
    {
        attacking = true;
        rushAttack = true;
        animator.SetBool("RushBool", true);
        //forceDmg *= 2; //RushAttack�߿��� �浹 ������ 2��
        //float dist;

        //Vector2 playerPos = PlayerPos();
        //Vector2 target = new Vector2(playerPos.x, rb.position.y);
        //while (rushAttack)
        //{
        //    //target���� �̵�
        //    Vector2 newPos = Vector2.MoveTowards(rb .position, target, rushSpeed * Time.deltaTime);
        //    rb.MovePosition(newPos);

        //    //distance margin
        //    dist = (playerPos.x - rb.position.x);
        //    if (dist < 0) { dist = dist * -1; }
        //    if (dist < 5f)
        //        rushAttack = false;

        //    yield return new WaitForFixedUpdate();
        //}

        Vector2 playerPos = PlayerPos();
        //Vector2 dir = playerPos - (Vector2)transform.position;
        float rushTime = 0f;
        Vector2 dir2 = Vector2.right * PlayerSide();
        float dist = Vector2.Distance(playerPos, rb.position);
        Debug.Log($"dist: {dist}");
        Debug.Log($"movelength: {dir2 * dist}");
        while(Vector2.Distance(playerPos, rb.position) > 5f)
        {
            rushTime += Time.deltaTime;
            if(rushTime > 15f) //n초 넘으면 rush 끝냄
                break;

            //rb.MovePosition(Vector2.MoveTowards(rb.position, dir * Vector2.Distance(playerPos, rb.position), rushSpeed * Time.deltaTime));
            Vector2 newPos = Vector2.MoveTowards(rb.position, dir2 * dist, rushSpeed * Time.deltaTime);
            rb.MovePosition(newPos);

            Debug.Log($"move exit > 5f: {Vector2.Distance(playerPos, rb.position)}");
            yield return new WaitForFixedUpdate();
        }

        Debug.Log("Rush FIN");
        rushAttack = false;
        animator.SetBool("MeteorBool", false);
        yield return new WaitForSeconds(attackDelay);

        attacking = false;
        Attack();
        yield return null;
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > PlayerPos().x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < PlayerPos().x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public float GetHealth() { return health; }

    private Vector3 PlayerPos() { return player.transform.position; }

    private float PlayerSide()
    {
        //left
        if (PlayerPos().x < rb.position.x)
            return -1f;
        else if (PlayerPos().x > rb.position.x) //right
            return 1f;
        else
            return 0f;
    }

    public IEnumerator FadeInDeactive()
    {
        //공걱 중일떄는, 알파 up, down and TO RED
        //
        bossAttackPreviewImage.color = Color.red;
        Color c = bossAttackPreviewImage.color;
        for (float f = 1f; f >= 0.2f; f -= 0.0001f)
        {
            c.g = f;
            c.b = f;
            bossAttackPreviewImage.color = c;
            yield return null;
        }
        
        for (float f = 0.2f; f <= 1; f += 0.0001f)
        {
            c.g = f;
            c.b = f;
            bossAttackPreviewImage.color = c;
            yield return null;
        }
    }
}
