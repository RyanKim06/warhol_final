using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ESphere
{
    O2,
    Jump,
    Shield,
    DeleteRock,
    DeleteHoles,
    EnableWaterdrop
}

public class Sphere : MonoBehaviour
{
    public ESphere sphere;
    public Text coolTimeText = null;
    public Text leftWaterdropText;
    public n_Player.PlayerController playerController;

    void InvokeSphere()
    {
        switch (sphere)
        {
            case ESphere.O2:
                break;
            case ESphere.Jump:
                break;
            case ESphere.Shield:
                break;
            case ESphere.DeleteRock:
                break;
            case ESphere.DeleteHoles:
                break;
            case ESphere.EnableWaterdrop:
                //playerController.GetLeftWaterdrop() = playerController.maxWaterdrop;
                playerController.ChargeWaterdrop(playerController.maxWaterdrop);
                leftWaterdropText.color = new Color(0f, 237f, 255f);
                StartCoroutine(RespawnCooltime(5f));
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            InvokeSphere();
        }
    }

    IEnumerator RespawnCooltime(float cooltime)
    {
        coolTimeText.enabled = true;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        float leftTime = cooltime;
        while(leftTime > 0f)
        {
            leftTime -= Time.deltaTime;
            coolTimeText.text = ((int)leftTime).ToString();
            yield return null;
        }
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        coolTimeText.enabled = false;
    }
}
