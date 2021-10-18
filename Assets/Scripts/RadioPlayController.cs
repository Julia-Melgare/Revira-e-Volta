using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioPlayController : MonoBehaviour
{
    private void OnEnable()
    {
        GraberController.onGrabObject += PlayRadioSound;
    }

    private void OnDisable()
    {
        GraberController.onGrabObject -= PlayRadioSound;
    }

    void PlayRadioSound(GameObject selected)
    {
        var radioAudioSrc = gameObject.GetComponent<AudioSource>();
        if (selected == gameObject && !radioAudioSrc.isPlaying)
            gameObject.GetComponent<AudioSource>().Play();
    }
}
