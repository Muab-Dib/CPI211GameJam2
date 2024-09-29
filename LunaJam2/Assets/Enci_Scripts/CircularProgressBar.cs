using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class CircularProgressBar : MonoBehaviour
{
    private bool isAcive=false;
    private float indicatorTimer;
    private float maxIndicatorTimer;

    private UnityEngine.UI.Image radialProgressBar;

    private void Awake()
    {
        radialProgressBar=GetComponent<UnityEngine.UI.Image>();
    }

    private void Update()
    {
        if(isAcive)
        {
            indicatorTimer-=Time.deltaTime;
            radialProgressBar.fillAmount= (indicatorTimer / maxIndicatorTimer);

            if(indicatorTimer<=0)
            {
                StopCountdown();
            }
            
        }

    }

    public void ActivateCountDown(float countdownTime)
    {
        isAcive=true;
        maxIndicatorTimer=countdownTime;
        indicatorTimer=maxIndicatorTimer;
    }

    public void StopCountdown()
    {
        isAcive=false;
    }

}
