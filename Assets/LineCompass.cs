using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LineCompass : MonoBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    Gauge gauge;
    [SerializeField]
    Transform destination;
    [SerializeField]
    RectTransform marker;
    [SerializeField]
    List<RectTransform> compassLines;

    bool active=true;

    float width;
    float ratio;
    float playerDirection;
    float visibleRange;
    InfiniteScroller compassScroller;
    float markerAngle;
    List<RectTransform> markers;
    public Vector3 North { get => Vector3.forward; }

    private void Start()
    {
        compassScroller = new InfiniteScroller(compassLines);
        width = GetComponent<RectTransform>().rect.width;
        ratio=width/compassScroller.TotalLength;
        visibleRange = ratio / 2;

        if (destination == null)
            marker.gameObject.SetActive(false);

        //Debug.Log(FindDifference(0.1f, 0.5f));
        //Debug.Log(FindDifference(0.5f, 0.1f));
        //Debug.Log(FindDifference(0.9f, 0.1f));
        //Debug.Log(FindDifference(0.1f, 0.9f));
        //Debug.Log(FindDifference(0.7f, 0.9f));
        //Debug.Log(FindDifference(0.9f, 0.7f));
        //Debug.Log(FindDifference(0.1f, 0.7f));
        //Debug.Log(FindDifference(0.95f, 0.05f));
    }

    public void SetMarker(Transform destination)
    {
        this.destination = destination;
        marker.gameObject.SetActive(true);
    }

    public void HideMarker()
    {
        this.destination = null;
        marker.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        float playerAngle=PointTowards(player.forward);
        if (destination != null)
        {
            markerAngle = AngleFromNorth((destination.position - player.transform.position).normalized);
            UpdateMarkerNormalized(markerAngle);
        }
    }

    void UpdateMarkerNormalized(float angleFromNorthNormalized)
    {
        Vector3 markerPosition=marker.transform.localPosition;
        float difference=FindDifference(playerDirection,angleFromNorthNormalized);
        //Debug.Log(difference);
        if (Mathf.Abs(difference)<visibleRange)
        {
            markerPosition.x = (difference / visibleRange) * (width / 2);
            marker.transform.localPosition=markerPosition;
        }
    }

    void UpdateMarker(float angleFromNorth)
    {
        UpdateMarkerNormalized(angleFromNorth/360);
    }

    float PointTowards(Vector3 direction)
    {
        playerDirection = AngleFromNorth(direction);
        gauge?.SetValue(playerDirection);
        compassScroller?.Scroll(playerDirection);
        //Debug.Log(playerDirection);
        return playerDirection;
    }

    float AngleFromNorth(Vector3 direction)
    {
        Vector3 playerWithNoHeight = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
        float angle = Vector3.Angle(North, playerWithNoHeight);
        if (Vector3.Cross(North, playerWithNoHeight).y > 0)
        {
            angle = 360 - angle;
        }
        angle = (angle / 360);
        return angle;
    }

    float FindDifference(float first, float second)
    {
        float difference=first-second;
        if(difference<-0.5f )
        {
            difference = (1+difference);
        }
        else if(difference > 0.5f)
        {
            difference = -(1 - difference);
        }
        return difference;
    }
}

[System.Serializable]
public class TempClass
{
    public string temp;
    public float what;
    public string exam;
}