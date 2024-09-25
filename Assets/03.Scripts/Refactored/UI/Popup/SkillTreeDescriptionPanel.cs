using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeDescriptionPanel : MonoBehaviour
{
    [SerializeField] private Image img_Skill;
    [SerializeField] private Text txt_Name;
    [SerializeField] private Text txt_Type;
    [SerializeField] private Text txt_RestrictedLevel;
    [SerializeField] private Text txt_Desc;

    public void On(SkillData data, Vector3 pos)
    {
        img_Skill.sprite = data.Sprt;
        txt_Name.text = data.SkillName;
        txt_Type.text = data.TypeName;
        txt_RestrictedLevel.text = $"제한 레벨 : LV.{data.RestrictedLevel}";
        txt_Desc.text = data.Description;

        this.transform.position = pos;

        this.gameObject.SetActive(true);
    }

    public void Off()
    {
        this.gameObject.SetActive(false);
    }
}
