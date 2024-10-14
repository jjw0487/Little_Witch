using UnityEngine;

/// <summary>
/// 스킬 트리 팝업에 선행 스킬 유무, 레벨 제한을 확인하여 스킬의 사용 가능 유무를 UI로 표현
/// </summary>
public class SkillNode : MonoBehaviour
{
    [SerializeField] private SkillPanelSlot panel;

    [SerializeField] private int restrictedLevel; // 스킬의 레벨 제한

    [SerializeField] private SkillNode[] followingNode; // 해당 스킬 뒤에 스킬 레벨을 올릴 자격이 생기는 스킬

    /// <param name="playerLevel"></param> => restrictedLevel 레벨과 비교할 플레이어 레벨
    /// <param name="isSkillPointAvailable"></param> => 스킬 포인트 > 0 인지
    /// <param name="isPrerequisiteSkillOn"></param> => 다음 노드에 전달할 현재 노드 스킬 On/Off 여부
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
