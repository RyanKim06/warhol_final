using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class dialogue_text
{
    [TextArea]
    public string dialogue;
}

public class Dialogue : MonoBehaviour
{

    [SerializeField] private Text txt_Dialogue;
    [SerializeField] private Text txt_PlayerDialogue;
    [SerializeField] private Text txt_EfectDialogue;

    [SerializeField] private GameObject playerDialogueImage;
    [SerializeField] private GameObject EfectDialogue;

    [SerializeField] public dialogue_text[] dialogue;
    [SerializeField] public dialogue_text[] playerDialogue;
    [SerializeField] public dialogue_text[] effectDialogue;

    public int count = 0;
    public int playerDCount = 0;
    public int efectCount = 0;

    private void Awake()
    {
        EfectDialogue.SetActive(false);
        UiSetActive(false);
    }

    public IEnumerator typing()
    {
        txt_Dialogue.text = "";
        yield return new WaitForSeconds(0.7f);
        for(int i = 0; i <= dialogue[count].dialogue.Length; ++i)
        {
            if (Input.GetMouseButtonDown(0))
            {
                txt_Dialogue.text = dialogue[count].dialogue;
                break;
            }
            txt_Dialogue.text = dialogue[count].dialogue.Substring(0, i);

            yield return new WaitForSeconds(0.07f);
        }
        count++;
    }
    public IEnumerator pTyping()
    {
        txt_PlayerDialogue.text = "";
        yield return new WaitForSeconds(0.9f);
        for (int i = 0; i <= playerDialogue[playerDCount].dialogue.Length; i++)
        {
            txt_PlayerDialogue.text = playerDialogue[playerDCount].dialogue.Substring(0, i);

            yield return new WaitForSeconds(0.09f);
        }
        playerDCount++;
    }

    public void UiSetActive(bool is_SetActive)
    {
        playerDialogueImage.SetActive(is_SetActive);
    }

    public IEnumerator EffectDialogue(int effectDialogueindex, GameObject obj)
    {
        EfectDialogue.SetActive(true);
        txt_EfectDialogue.text = effectDialogue[effectDialogueindex].dialogue;
        //obj.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        
        yield return new WaitForSeconds(1.5f);
        Destroy(obj);
        EfectDialogue.SetActive(false);
    }

}
