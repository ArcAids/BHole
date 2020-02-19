using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recoil
{
    private RecoilData recoilData;
    Transform gunTransform;
    float recoil;
    float recoilY;
    public Recoil(Transform gunTransform, RecoilData data)
    {
        this.gunTransform = gunTransform;
        recoilData = data;
    }

    public void StartRecoil()
    {
        // in seconds
        recoil = recoilData.recoil;

        recoilY = Random.Range(-recoilData.maxRecoil_y, recoilData.maxRecoil_y);
    }

    public void Recoiling()
    {
        if (recoil > 0f)
        {
            Quaternion maxRecoil = Quaternion.Euler(recoilData.maxRecoil_x, recoilY, 0f);
            // Dampen towards the target rotation
            gunTransform.localRotation = Quaternion.Lerp(gunTransform.localRotation, maxRecoil, Time.deltaTime * recoilData.recoilSpeed);
            recoil -= Time.deltaTime;
        }
        else
        {
            recoil = 0f;
            // Dampen towards the target rotation
            gunTransform.localRotation = Quaternion.Slerp(gunTransform.localRotation, Quaternion.identity, Time.deltaTime * recoilData.recoilSpeed );
        }
    }
}

[System.Serializable]
public class RecoilData
{
    public float recoil = 0.0f;
    public float maxRecoil_x = -20f;
    public float maxRecoil_y = 20f;
    public float recoilSpeed = 2f;
}
