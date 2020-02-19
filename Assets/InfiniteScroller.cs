using System.Collections.Generic;
using UnityEngine;

public class InfiniteScroller
{
    List<RectTransform> transforms;
    public float TotalLength{ get; private set; }
    float hidingPoint;
    float startingPoint;
    float width;
    public InfiniteScroller(List<RectTransform> transforms)
    {
        if (transforms == null || transforms.Count == 0)
        { Debug.LogError("no transforms to scroll."); return; }

        this.transforms = transforms;
        foreach (var item in this.transforms)
        {
            TotalLength+=item.rect.width;
        }
        startingPoint = -(TotalLength * (1 - (1f / transforms.Count)))/2;
        hidingPoint = -startingPoint;
        width = transforms[0].rect.width;
    }

    public void Scroll(float point)
    {
        float startPosition = (point * TotalLength)-width/2 ;
        if (startPosition > hidingPoint)
            startPosition = startingPoint -(width-(startPosition-hidingPoint));
        Vector3 objectPosition=transforms[0].localPosition;

        foreach (var item in transforms)
        {
            objectPosition.x = startPosition +width/2;
            item.localPosition = objectPosition;
            startPosition += width;

            if(startPosition > hidingPoint)
                startPosition = startingPoint -(width - (startPosition - hidingPoint));
        }
    }

   
}
