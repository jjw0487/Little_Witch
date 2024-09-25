using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StoreDecisionMaker : MonoBehaviour
{
    [SerializeField] private Image img_Item;
    [SerializeField] private Text txt_ItemName;
    [SerializeField] private Text txt_Price;
    [SerializeField] private Text txt_Decision;

    [SerializeField] private Button btn_Yes;
    [SerializeField] private Button btn_No;
    [SerializeField] private Button btn_Plus;
    [SerializeField] private Button btn_Minus;

    [SerializeField] private InputField inputField_Amount;

    private StorePopup popup;
    private ItemSlot slot;
    private int curValue;
    private bool isPurchasing;

    private int currency;

    public void InitializeStoreDecisionMaker(StorePopup _popup)
    {
        popup = _popup;

        this.transform.localScale = Vector3.zero;

        btn_Yes.onClick.AddListener(YesButton);

        btn_No.onClick.AddListener(CloseStoreDecisionMaker);

        btn_Plus.onClick.AddListener(() =>
        {
            OnAmountValueChanged(curValue + 1);
        });

        btn_Minus.onClick.AddListener(() =>
        {
            OnAmountValueChanged(curValue - 1);
        });

        inputField_Amount.onEndEdit.AddListener((s) =>
        {
            int value = int.Parse(s);
            OnAmountValueChanged(value);
        });
    }

    private void YesButton()
    {
        SoundManager.sInst.Play("Store");

        if (isPurchasing) popup.BuyItem(slot.GetItemData(), curValue);
        else popup.SellItem(slot, curValue);

        CloseStoreDecisionMaker();
    }

    public void ShowStoreDecisionMaker(ItemSlot _slot, int _curValue, bool _isPurchasing)
    {
        slot = _slot;
        curValue = _curValue;
        isPurchasing = _isPurchasing;
        img_Item.sprite = slot.GetItemData().Sprite;
        txt_ItemName.text = slot.GetItemData().ItemName;

        currency = isPurchasing ? slot.GetItemData().GetCurrencyWhenBuy() : slot.GetItemData().GetCurrencyWhenSell();

        txt_Price.text = (currency * curValue).ToString(); 

        inputField_Amount.text = curValue.ToString();

        txt_Decision.text = isPurchasing ? "에 구매합니다." : "에 판매합니다.";

        this.transform.DOScale(1f, 0.2f);
    }

    public void CloseStoreDecisionMaker()
    {
        SoundManager.sInst.Play("ButtonClick");

        this.transform.DOScale(0f, 0.2f);
    }

    private void OnAmountValueChanged(int _value)
    {
        if (!IsAvailableAmount(_value)) return;
        
        curValue = _value;

        txt_Price.text = (currency * curValue).ToString();

        inputField_Amount.text = curValue.ToString();
    }

    private bool IsAvailableAmount(int _value)
    {
        if (_value <= 0) return false;

        if (isPurchasing)
        {
            int price = currency * _value;
            return popup.GetPlayerGold() >= price;
        }
        else
        {
            return _value <= slot.GetItemValue();
        }
    }

}
