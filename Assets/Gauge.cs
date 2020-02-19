using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    [SerializeField]
    GameObject pinPivot;

    float rotation;
    [SerializeField]
    float startAngle;
    [SerializeField]
    float endAngle;

    float difference;
    public void Start()
    {
        difference = endAngle - startAngle;
        SetValue(0);
    }

    public void SetValue(float value)
    {
        rotation = startAngle + (difference*value);
        pinPivot.transform.localRotation = Quaternion.Euler(0,0,rotation);

    }

}

[System.Serializable]
public class GaugeData
{
    public float startPoint;
    public float EndPoint;
    
}
