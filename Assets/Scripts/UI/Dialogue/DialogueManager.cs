using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �÷��̾�� ��ȭ�� �ʿ��� Ư�� �̺�Ʈ�� �߻������� �� ȭ�鿡 ��ȭ UI�� ����, �÷��̾��� �ൿ ����
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Button btn_Next; // Next ��ư
    [SerializeField] private Text txt_partnerName; // ��ȭ ��� �̸�
    [SerializeField] private Text txt_Dialogue; // ��ȭ ����
    [SerializeField] private float typeLatency = 0.05f; // ���ڿ� ���� �ӵ�

    private DialogueData dialogue; // ���� ��ȭ
    private Action callback; // ��ȭ �� callback�Լ�
    private int progress; // ��ȭ ���൵

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

        // �� �߰��� ���� ��ư�� ������ �� ���� ���� ����Ǳ� ����
        this.StopAllCoroutines();

        StartCoroutine(TypeSentence(dialogue.Contents[progress]));

        progress++;
    }

    private void EndConversation()
    {
        callback();
    }

    private IEnumerator TypeSentence(string sentence) // ��ȭ ������ �� ���ھ� �����ϵ��� ǥ��
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
