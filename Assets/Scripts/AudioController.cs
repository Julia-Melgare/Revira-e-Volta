using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip pickupObjClip;
    [SerializeField] private AudioClip pickupGoalClip;
    [SerializeField] private AudioClip dropObjClip;
    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioSource BGMAudioSource;

    private void OnEnable()
    {
        GraberController.onGrabObject += PlayPickupObjClip;
        GraberController.onGrabGoal += PlayPickupGoalClip;
        SelectableObjectController.onObjectCollision += PlayDropObjClip;
    }

    private void OnDisable()
    {
        GraberController.onGrabObject -= PlayPickupObjClip;
        GraberController.onGrabGoal -= PlayPickupGoalClip;
        SelectableObjectController.onObjectCollision -= PlayDropObjClip;
    }

    void PlayPickupObjClip(GameObject selected)
    {
        SFXAudioSource.PlayOneShot(pickupObjClip);
    }

    void PlayPickupGoalClip()
    {
        SFXAudioSource.PlayOneShot(pickupGoalClip);
    }

    void PlayDropObjClip(Collision other)
    {
        //SFXAudioSource.PlayOneShot(dropObjClip);
    }
}
