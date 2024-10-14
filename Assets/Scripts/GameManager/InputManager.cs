using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class InputManager : MonoBehaviour
{
    [SerializeField] private KeyEvent player_PickUpItem;

    [SerializeField] private KeyEvent player_Dash; // 대시 넣어주자

    [SerializeField] private KeyEvent quick_Item_First;
    [SerializeField] private KeyEvent quick_Item_Second;

    [SerializeField] private KeyEvent quick_Skill_First;
    [SerializeField] private KeyEvent quick_Skill_Second;
    [SerializeField] private KeyEvent quick_Skill_Third;
    [SerializeField] private KeyEvent quick_Skill_Fourth;

    [SerializeField] private KeyEvent quick_Inventory;
    [SerializeField] private KeyEvent quick_Quest;
    [SerializeField] private KeyEvent quick_SkillTree;
    [SerializeField] private KeyEvent quick_Shortcut;

    [SerializeField] private List<KeyEvent> keyEvents;

    public List<KeyEvent> GetKeyEvents() => keyEvents;

    private void Start()
    {
        InitializeInputManager();
    }
    private void InitializeInputManager()
    {
        player_PickUpItem.Initialize("Player_PickUpItem");

        quick_Item_First.Initialize("Quick_Item_First");
        quick_Item_Second.Initialize("Quick_Item_Second");

        quick_Skill_First.Initialize("Quick_Skill_First");
        quick_Skill_Second.Initialize("Quick_Skill_Second");
        quick_Skill_Third.Initialize("Quick_Skill_Third");
        quick_Skill_Fourth.Initialize("Quick_Skill_Fourth");

        quick_Inventory.Initialize("Quick_Inventory");
        quick_Quest.Initialize("Quick_Quest");
        quick_SkillTree.Initialize("Quick_SkillTree");
        quick_Shortcut.Initialize("Quick_Shortcut");
    }
    void Update()
    {
        player_PickUpItem.InputEvent();

        quick_Item_First.InputEvent();
        quick_Item_Second.InputEvent();

        quick_Skill_First.InputEvent();
        quick_Skill_Second.InputEvent();
        quick_Skill_Third.InputEvent();
        quick_Skill_Fourth.InputEvent();

        quick_Inventory.InputEvent();
        quick_Quest.InputEvent();
        quick_SkillTree.InputEvent();
        quick_Shortcut.InputEvent();
    }

    
}
