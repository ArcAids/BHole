using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] WeaponSystem.WeaponBehaviour GGun;
    [SerializeField] WeaponSystem.WeaponBehaviour WGun;
    [SerializeField] WeaponSystem.WeaponBehaviour PGun;
    [SerializeField] Transform player;

    private void Start()
    {
        if (GameManager.Instance.PGunUnlocked)
        {
            PGun.GetComponent<Fungus.Flowchart>().enabled = false;
            PGun.transform.position = player.position;
        }
        if (GameManager.Instance.WGunUnlocked)
        {
            WGun.GetComponent<Fungus.Flowchart>().enabled = false;
            WGun.transform.position = player.position;
        }
        if (GameManager.Instance.GGunUnlocked)
        {
            GGun.GetComponent<Fungus.Flowchart>().enabled=false;
            GGun.transform.position = player.position;
        }
    }

    public void ActivatePGun()
    {
        GameManager.Instance.PGunUnlocked = true;
    }
    public void ActivateGGun()
    {
        GameManager.Instance.GGunUnlocked = true;
    }
    public void ActivateWGun()
    {
        GameManager.Instance.WGunUnlocked = true;
    }
}
