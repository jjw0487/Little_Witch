using System.Collections;
using UnityEngine;

public class Fox : Monster
{
    protected override void HuntingQuestEvent()
    {
        if (QuestEvent.huntingQuestEvent != null) QuestEvent.huntingQuestEvent(data.Name);
    }
}
