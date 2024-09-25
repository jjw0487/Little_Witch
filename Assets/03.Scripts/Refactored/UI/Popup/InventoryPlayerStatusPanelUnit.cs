using UnityEngine;
using UnityEngine.UI;

public class InventoryPlayerStatusPanelUnit : MonoBehaviour
{
    [SerializeField] private Text txt_Value;
    [SerializeField] private Text txt_AdditionalValue;

    public void UIUpdate(string mainValue, string additionalValue = "")
    {
        txt_Value.text = mainValue;

        if(txt_AdditionalValue != null)
        {
            txt_AdditionalValue.text = additionalValue;
        }
    }
}
