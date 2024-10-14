using UnityEngine;

/// <summary>
/// ��ų Ʈ�� �˾��� ���� ��ų ����, ���� ������ Ȯ���Ͽ� ��ų�� ��� ���� ������ UI�� ǥ��
/// </summary>
public class SkillNode : MonoBehaviour
{
    [SerializeField] private SkillPanelSlot panel;

    [SerializeField] private int restrictedLevel; // ��ų�� ���� ����

    [SerializeField] private SkillNode[] followingNode; // �ش� ��ų �ڿ� ��ų ������ �ø� �ڰ��� ����� ��ų

    /// <param name="playerLevel"></param> => restrictedLevel ������ ���� �÷��̾� ����
    /// <param name="isSkillPointAvailable"></param> => ��ų ����Ʈ > 0 ����
    /// <param name="isPrerequisiteSkillOn"></param> => ���� ��忡 ������ ���� ��� ��ų On/Off ����
    public void NodeUpdate(int playerLevel, bool isSkillPointAvailable, bool isPrerequisiteSkillOn)
    {
        panel.LockEvent(restrictedLevel <= playerLevel && isPrerequisiteSkillOn, isSkillPointAvailable);

        if(followingNode != null)
        {
            for (int i = 0; i < followingNode.Length; i++)
            {
                followingNode[i].NodeUpdate(playerLevel, isSkillPointAvailable, panel.IsSkillAvailable());
            }
        }
    }
}
