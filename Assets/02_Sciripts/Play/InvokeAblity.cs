using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using n_O2Gauge;

public enum EAblity
{
    O2,
    Jump,
    Shield,
    DeleteRock,
    DeleteHoles
}

public class InvokeAblity : MonoBehaviour
{
    public EAblity selected;
    public GameObject player;
    public Dialogue dialogue;

    public WaitForSeconds delay1 = new WaitForSeconds(0.05f);

    private void Awake()
    {
        dialogue = GameObject.Find("Canvas").GetComponent<Dialogue>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void CheckAblity()
    {
        switch(selected)
        {
            case EAblity.O2:
                StartCoroutine(Increse());
                StartCoroutine(dialogue.EffectDialogue(0, this.gameObject));
                break;

            case EAblity.Shield:
                O2.hasShield = true;
                StartCoroutine(dialogue.EffectDialogue(1, this.gameObject));
                break;

            case EAblity.Jump:
                //jump x2
                StartCoroutine(dialogue.EffectDialogue(2, this.gameObject));
                break;

            case EAblity.DeleteRock:
                //鞠籍力芭
                StartCoroutine(dialogue.EffectDialogue(3, this.gameObject));
                break;

            case EAblity.DeleteHoles:
                //备港力芭
                StartCoroutine(dialogue.EffectDialogue(4, this.gameObject));
                break;
        }
    }

    IEnumerator Increse()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        for (int i = 0; i < 50; i++)
        {
            O2.IncreseO2(1);
            yield return delay1;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //StartCoroutine(CheckAblity());
            CheckAblity();
            Debug.Log("CheckAblity()");
            
        }
    }

}
