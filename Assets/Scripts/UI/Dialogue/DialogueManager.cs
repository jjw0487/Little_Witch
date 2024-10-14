using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어에서 대화가 필요한 특정 이벤트를 발생시켰을 때 화면에 대화 UI를 노출, 플레이어의 행동 제한
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Button btn_Next; // Next 버튼
    [SerializeField] private Text txt_partnerName; // 대화 상대 이름
    [SerializeField] private Text txt_Dialogue; // 대화 내용
    [SerializeField] private float typeLatency = 0.05f; // 문자열 노출 속도

    private DialogueData dialogue; // 현재 대화
    private Action callback; // 대화 후 callback함수
    private int progress; // 대화 진행도

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

    private IEnumerator TypeSentence(string sentence) // 대화 내용이 한 글자씩 증가하도록 표현
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
