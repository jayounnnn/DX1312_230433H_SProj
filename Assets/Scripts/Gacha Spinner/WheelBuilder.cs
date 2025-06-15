using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections.Generic;

public class WheelBuilder : MonoBehaviour
{
    [Header("Wheel Cinfuig")]
    [SerializeField] private Transform wheelParent;
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private GachaPool gachaPool;

    [Header("Item References (within the prefab)")]
    //The strings must match the names of the GOs inside the segment prefab
    [SerializeField] private string iconObjectName = "ItemIcon";
    [SerializeField] private string nameObjectName = "ItemName";


    void Start()
    {
        BuildWheel();
    }

    private void BuildWheel()
    {
        if (gachaPool == null || segmentPrefab == null || wheelParent == null)
        {
            Debug.LogError("WheelBuilder is not configured properly. Assign all fields in the Inspector.");
            return;
        }

        List<GachaDrop> drops = gachaPool.possibleDrops;
        float segmentAngle = 360f / drops.Count;

        //set a distance between the segments
        float radius = 300f; 

        //spawn the segments and then rotate them for each item
        for (int i = 0; i < drops.Count; i++)
        {
            GameObject newSegment = Instantiate(segmentPrefab, wheelParent);

            float angle = segmentAngle * i;
            newSegment.transform.localRotation = Quaternion.Euler(0, 0, -angle);

            Vector3 offset = Quaternion.Euler(0, 0, -angle) * Vector3.up * radius;
            newSegment.transform.localPosition = offset;

            Transform iconTransform = newSegment.transform.Find(iconObjectName);
            Transform nameTransform = newSegment.transform.Find(nameObjectName);

            if (iconTransform == null || nameTransform == null)
            {
                Debug.LogError($"Could not find '{iconObjectName}' or '{nameObjectName}' inside the segment prefab. Check the names.");
                continue;
            }

            Image itemIcon = iconTransform.GetComponent<Image>();
            TextMeshProUGUI itemName = nameTransform.GetComponent<TextMeshProUGUI>();

            GachaItem currentItem = drops[i].item;
            if (itemIcon != null) itemIcon.sprite = currentItem.itemIcon;
            if (itemName != null) itemName.text = currentItem.itemName;
        }
    }


    //private void BuildWheel()
    //{
    //    if (gachaPool == null || segmentPrefab == null || wheelParent == null)
    //    {
    //        Debug.LogError("WheelBuilder is not configured properly. Assign all fields in the Inspector.");
    //        return;
    //    }

    //    List<GachaDrop> drops = gachaPool.possibleDrops;
    //    float segmentAngle = 360f / drops.Count;

    //    for (int i = 0; i < drops.Count; i++)
    //    {
    //        GameObject newSegment = Instantiate(segmentPrefab, wheelParent);

    //        newSegment.transform.rotation = Quaternion.Euler(0, 0, segmentAngle * i);

    //        Transform iconTransform = newSegment.transform.Find(iconObjectName);
    //        Transform nameTransform = newSegment.transform.Find(nameObjectName);

    //        if (iconTransform == null || nameTransform == null)
    //        {
    //            Debug.LogError($"Could not find '{iconObjectName}' or '{nameObjectName}' inside the segment prefab. Check the names.");
    //            continue;
    //        }

    //        Image itemIcon = iconTransform.GetComponent<Image>();
    //        TextMeshProUGUI itemName = nameTransform.GetComponent<TextMeshProUGUI>();

    //        GachaItem currentItem = drops[i].item;
    //        if (itemIcon != null)
    //        {
    //            itemIcon.sprite = currentItem.itemIcon;
    //        }
    //        if (itemName != null)
    //        {
    //            itemName.text = currentItem.itemName;
    //        }
    //    }
    //}
}
