using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 프레임 변경을 위한 드랍다운, 세팅 기능 제공
/// </summary>
public class FPSManager : MonoBehaviour
{
    public TMP_Dropdown fpsDropdown;

    [SerializeField] private int[] targetFrame;

    private void Start()
    {
        int curFrame = PLoad.Load("TargetFrameRate", 144);

        fpsDropdown.ClearOptions();

        HashSet<string> options = new HashSet<string>();

        int currentTargetFrameIndex = 0;

        for (int i = 0; i < targetFrame.Length; i++)
        {
            string option = "fps : " + targetFrame[i];

            options.Add(option);

            if (curFrame == targetFrame[i])
            {
                currentTargetFrameIndex = i;
            }
        }

        fpsDropdown.AddOptions(new List<string>(options));
        fpsDropdown.value = currentTargetFrameIndex;
        fpsDropdown.RefreshShownValue();
    }



    public void SetFrameRate(int targetFrameIdx)
    {
        Application.targetFrameRate = targetFrame[targetFrameIdx];

        PSave.Save("TargetFrameRate", targetFrame[targetFrameIdx]);
    }
}
