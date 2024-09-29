using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    public GameObject boostUpEffect;
    public bool isBostedUp;
    private float duration;

    private void Awake()
    {
        boostUpEffect.SetActive(false);
    }

    public void StartBoostEffect(float duration)
    {
        isBostedUp=true;
        boostUpEffect.SetActive(true);
        boostUpEffect.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>().ActivateCountDown(duration);

        StartCoroutine(EndBoostUpEffect(duration));
    }

    IEnumerator EndBoostUpEffect(float duration)
    {
        yield return new WaitForSeconds(duration);

        isBostedUp=false;
        boostUpEffect.SetActive(false);
    }
}
