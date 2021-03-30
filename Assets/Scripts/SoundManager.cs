using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string mouseOverEvent;

    [FMODUnity.EventRef]
    public string clickEvent;

    [FMODUnity.EventRef]
    public string reloadEvent;

    [FMODUnity.EventRef]
    public string switchWeaponEvent;

    public void PlayMouseOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot(mouseOverEvent); 
    }

    public void PlayClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(clickEvent);
    }

    public void PlayShoot(string shootEvent)
    {
        FMODUnity.RuntimeManager.PlayOneShot(shootEvent);
    }

    public void PlayReload()
    {
        FMODUnity.RuntimeManager.PlayOneShot(reloadEvent);
    }

    public void PlaySwitchWeapon()
    {
        FMODUnity.RuntimeManager.PlayOneShot(switchWeaponEvent);
    }
}
