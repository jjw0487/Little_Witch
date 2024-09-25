using Enums;
using System.Linq;
using UnityEngine;

public static class QuestEvent
{
    public delegate void HuntingQuestEvent(string monsterName);
    public static HuntingQuestEvent huntingQuestEvent;

    public delegate void CollectQuestEvent(ItemData targetItem);
    public static CollectQuestEvent collectQuestEvent;

    public delegate bool DeliveryQuestEvent(NpcType deliveryTarget, out DialogueData dialogue);
    public static DeliveryQuestEvent deliveryQuestEvent;

    public delegate void QuestNotificationEvent(QuestData quest, bool accepted);
    public static QuestNotificationEvent questNotificationEvent;
}

public static class EventManager
{
    public delegate void EventRemover();
    public static EventRemover eventRemover;

    public delegate void SceneUIUpdate(); // ! 저장된 이벤트 제거 !
    public static SceneUIUpdate uiUpdateEvent;

    public delegate void SaveDataEvent();
    public static SaveDataEvent saveDataEvent;

    public delegate MonsterHPSlider MonsterHPEvent();
    public static MonsterHPEvent monsterHPEvent;

    public delegate void FloatingDamageEvent(Vector3 pos, float dmg);
    public static FloatingDamageEvent floatingDamageEvent;

    public delegate void ItemSpawnEvent(Vector3 pos, int value, ItemObject item);
    public static ItemSpawnEvent itemSpawnEvent;

    public delegate void NormalAttackEvent();
    public static NormalAttackEvent normalAttackEvent;

    public delegate void ItemNotificationEvent(ItemData item);
    public static ItemNotificationEvent itemNotificationEvent;

    public delegate void GoldNotificationEvent(int value);
    public static GoldNotificationEvent goldNotificationEvent;

    public delegate void InteractionNotificationEvent(bool active);
    public static InteractionNotificationEvent interactionNotificationEvent;
}

public static class PSave
{
    public static void Save(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
    public static void Save(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
    public static void Save(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
    public static void Save(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

}

public static class PLoad
{
    public static int Load(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }
    public static bool Load(string key, bool defaultValue = false)
    {
        return PlayerPrefs.GetInt(key,
            defaultValue ? 1 : 0) == 1 ? true : false;
    }
    public static string Load(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }
    public static float Load(string key, float defaultValue = 0f)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }
}

public static class ConvertTo
{

    public static int[] StringToIntArr(string s)
    {
        int[] result = s.Split('_').Select(int.Parse).ToArray();
        return result;
    }

    public static string KeycodeToString(KeyCode code)
    {
        switch (code)
        {
            //A-B-C-D-E-F-G-H-I-J-K-L-M-N-O-P-Q-R-S-T-U-V-W-X-Y-Z
            case KeyCode.A: return "A";
            case KeyCode.B: return "B";
            case KeyCode.C: return "C";
            case KeyCode.D: return "D";
            case KeyCode.E: return "E";
            case KeyCode.F: return "F";
            case KeyCode.G: return "G";
            case KeyCode.H: return "H";
            case KeyCode.I: return "I";
            case KeyCode.J: return "J";
            case KeyCode.K: return "K";
            case KeyCode.L: return "L";
            case KeyCode.M: return "M";
            case KeyCode.N: return "N";
            case KeyCode.O: return "O";
            case KeyCode.P: return "P";
            case KeyCode.Q: return "Q";
            case KeyCode.R: return "R";
            case KeyCode.S: return "S";
            case KeyCode.T: return "T";
            case KeyCode.U: return "U";
            case KeyCode.V: return "V";
            case KeyCode.W: return "W";
            case KeyCode.X: return "X";
            case KeyCode.Y: return "Y";
            case KeyCode.Z: return "Z";

            // 0-1-2-3-4-5-6-7-8-9
            case KeyCode.Alpha0: return "0";
            case KeyCode.Alpha1: return "1";
            case KeyCode.Alpha2: return "2";
            case KeyCode.Alpha3: return "3";
            case KeyCode.Alpha4: return "4";
            case KeyCode.Alpha5: return "5";
            case KeyCode.Alpha6: return "6";
            case KeyCode.Alpha7: return "7";
            case KeyCode.Alpha8: return "8";
            case KeyCode.Alpha9: return "9";

            // F1-F2-F3-F4-F5-F6-F7-F8-F9-F10-F11-F12
            case KeyCode.F1: return "F1";
            case KeyCode.F2: return "F2";
            case KeyCode.F3: return "F3";
            case KeyCode.F4: return "F4";
            case KeyCode.F5: return "F5";
            case KeyCode.F6: return "F6";
            case KeyCode.F7: return "F7";
            case KeyCode.F8: return "F8";
            case KeyCode.F9: return "F9";
            case KeyCode.F10: return "F10";
            case KeyCode.F11: return "F11";
            case KeyCode.F12: return "F12";

            case KeyCode.UpArrow: return "Up";
            case KeyCode.DownArrow: return "Dn";
            case KeyCode.LeftArrow: return "Lft";
            case KeyCode.RightArrow: return "Rt";

            case KeyCode.Minus: return "-";
            case KeyCode.Equals: return "=";

            case KeyCode.RightAlt: return "Alt";
            case KeyCode.RightControl: return "Ctr";
            case KeyCode.RightShift: return "Sh";

            case KeyCode.LeftAlt: return "Alt";
            case KeyCode.LeftControl: return "Ctr";
            case KeyCode.LeftShift: return "Sh";

            case KeyCode.Tab: return "Tab";
            case KeyCode.Space: return "SB";

            case KeyCode.Backspace: return "Bsp";

            case KeyCode.Insert: return "Ins";
            case KeyCode.Delete: return "Del";
            case KeyCode.Home: return "Hm";
            case KeyCode.End: return "End";
            case KeyCode.PageDown: return "Pdn";
            case KeyCode.PageUp: return "Pup";

                default: return "?";
                
        }

    }
}