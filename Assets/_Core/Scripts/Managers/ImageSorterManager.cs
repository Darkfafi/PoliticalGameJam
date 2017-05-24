using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageSorterManager : MonoBehaviour
{
    private void Start ()
    {
        List<RectTransform> items = new List<RectTransform>();
        foreach (RectTransform item in gameObject.GetComponentsInChildren<RectTransform>())
        {
            items.Add(item);
        }
        List<RectTransform> newItems = items.OrderBy(y => y.position.y).ToList();
        foreach (RectTransform item in newItems)
        {
            item.SetAsFirstSibling();
        }
    }
}
