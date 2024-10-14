using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObject/DialogueData", order = 6)]

public class DialogueData : ScriptableObject
{
    [SerializeField] private string partnerName;
    public string PartnerName => partnerName;

    [TextArea(3, 10)]
    [SerializeField] private string[] contents;
    public string[] Contents => contents;
}
