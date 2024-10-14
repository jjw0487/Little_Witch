using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class Resolution
{
    public int width;
    public int height;

    public override string ToString()
    {
        return width + "x" + height;
    }
}

/// <summary>
/// 해상도 변경을 위한 드랍다운, 세팅 기능 제공
/// </summary>
public class ResolutionManager : MonoBehaviour
{
    [SerializeField] private Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;

    private void Start()
    {
        //resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        HashSet<string> options = new HashSet<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].ToString();
            options.Add(option);


            // 현재 해상도 찾기
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(new List<string>(options));
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        // 변경을 희망하는 수치로 즉시 해상도를 변경

        PSave.Save("Resolution_Width", resolution.width);
        PSave.Save("Resolution_Height", resolution.height);
    }
}
