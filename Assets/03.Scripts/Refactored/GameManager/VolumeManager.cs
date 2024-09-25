using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private Button btn_Sound; // 사운드 온 오프
    [SerializeField] private Image img_Sound;
    [SerializeField] private Image img_Sound_Button;
    [SerializeField] private Slider slider;
    [SerializeField] private Text txt_SliderValue;

    [SerializeField] private Color[] soundColors; // 0 : off, 1 : on
    [SerializeField] private Sprite[] soundSprites; // 0 : off, 1 : on

    private bool isOn = true;

    private void Start()
    {
        InitializeVolumeManager();
    }

    private void InitializeVolumeManager()
    {
        isOn = true;

        slider.value = PLoad.Load("AudioVolume", 100);

        if (slider.value > 0)
        {
            if (!isOn) UIUpdate(true);
        }
        else
        {
            if (isOn) UIUpdate(false);
        }

        btn_Sound.onClick.AddListener(OnButtonClicked);
    }

    private void UIUpdate(bool active)
    {
        isOn = active;
        img_Sound.sprite = soundSprites[active ? 1 : 0];
        img_Sound_Button.color = soundColors[active ? 1 : 0];
    }

    public void OnVolumeValueChanged()
    {
        int value = (int)slider.value;

        SoundManager.sInst.OnVolumeValueChanged(value);

        txt_SliderValue.text = value.ToString();

        if (value > 0)
        {
            if (!isOn) UIUpdate(true);
        }
        else
        {
            if (isOn) UIUpdate(false);
        }
    }

    public void OnButtonClicked()
    {
        if(isOn)
        {
            slider.value = 0;

            SoundManager.sInst.OnVolumeValueChanged(0);

            txt_SliderValue.text = "0";

            UIUpdate(false);
        }
        else
        {
            slider.value = 100;

            SoundManager.sInst.OnVolumeValueChanged(100);

            txt_SliderValue.text = "100";

            UIUpdate(true);
        }
    }



}
