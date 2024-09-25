using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Button btn_Next;
    [SerializeField] private Text txt_partnerName;
    [SerializeField] private Text txt_Dialogue;
    [SerializeField] private float typeLatency;
    private DialogueData dialogue;
    private Action callback;
    private int progress;

    private void Start()
    {
        btn_Next.onClick.AddListener(DisplayNextSentence);
    }

    public void InitializeDialogue(DialogueData _dialogue, Action _callback)
    {
        progress = 0;
        dialogue = _dialogue;
        callback = _callback;

        txt_partnerName.text = dialogue.PartnerName;

        StartConversation();
    }

    public void StartConversation()
    {
        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if(progress >= dialogue.Contents.Length)
        {
            EndConversation();
            return;
        }

        // 글 중간에 다음 버튼을 눌렀을 시 다음 대사로 진행되기 위해
        this.StopAllCoroutines();

        StartCoroutine(TypeSentence(dialogue.Contents[progress]));

        progress++;
    }

    private void EndConversation()
    {
        callback();
    }

    private IEnumerator TypeSentence(string sentence)
    {
        SoundManager.sInst.Play("Dialogue");

        txt_Dialogue.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            txt_Dialogue.text += letter;

            yield return new WaitForSeconds(typeLatency);
        }
    }   

}
