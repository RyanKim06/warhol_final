using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using n_Player;


public class Bow : MonoBehaviour
{
    public Transform shotPoint; // 어디서 부터 표시할 것인지
    static bool isBallThrown;
    public GameObject point; // 점 오브젝트
    GameObject[] points; // 여러개의 점 오브젝트를 담을것
    public int numberOfPoints; // 점 갯수

    PlayerController player;
    public Rigidbody2D _rigidbody;

    void Start()
    {
        player = GetComponent<PlayerController>();

        // 점 생성
        points = new GameObject[numberOfPoints];
        isBallThrown = false;
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(point, shotPoint.position, Quaternion.identity); // Instantiate(생성할 게임 오브젝트, 생성할 장소, 방향(Quaternion.identitiy <= 기본값))
        }
        isBallThrowChange(isBallThrown);
    }

    void FixedUpdate()
    {
        if (!isBallThrown)
            return;

        for (int i = 0; i < numberOfPoints; i++)
            points[i].GetComponent<SpriteRenderer>().color = Color.white;

        //Vector3 vel = GetForceFrom(firstPoint, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Vector3 vel = GetForceFrom(transform.position, transform.position - player.jumpDir);
        float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
        //transform.eulerAngles = Vector3.forward * angle;
        setTrajectoryPoints(transform.position, vel / _rigidbody.mass);

    }

    private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        if (player.isFacingRight)
            return (Vector2.right * (fromPos.x - toPos.x) + Vector2.up * (fromPos.y - toPos.y)) * player.jumpTimeCounter;
        else
            return (Vector2.left * (fromPos.x - toPos.x) + Vector2.up * (fromPos.y - toPos.y)) * player.jumpTimeCounter;
    }

    void setTrajectoryPoints(Vector3 pStartPosition, Vector3 pVelocity)
    {
        float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));
        float fTime = 0;

        fTime += 0.1f;
        for (int i = 0; i < numberOfPoints; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * _rigidbody.gravityScale * fTime * fTime / 2.0f);
            Vector3 pos = Vector3.right * (pStartPosition.x + dx) + Vector3.up * (pStartPosition.y + dy) + Vector3.forward * 2;
            points[i].transform.position = pos;

            points[i].transform.eulerAngles = Vector3.forward * (Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude) * fTime, pVelocity.x) * Mathf.Rad2Deg);
            fTime += 0.1f;
        }
    }

    public void isBallThrowChange(bool Throw)
    {
        isBallThrown = Throw;
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i].SetActive(isBallThrown);
        }

    }
    public void Jump(Rigidbody2D rb)
    {
        rb.AddForce(GetForceFrom(transform.position, transform.position - player.jumpDir), ForceMode2D.Impulse);
    }

}

