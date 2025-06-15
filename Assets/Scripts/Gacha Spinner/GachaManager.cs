using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    [Header("Gacha Config")]
    [Tooltip("Assign Gacha Pool asset to the wheel.")]
    [SerializeField] private GachaPool activeGachaPool;

    [Header("Wheel Objects")]
    [SerializeField] private Transform wheelTransform;
    [SerializeField] private Button spinButton;

    [Header("Spin Anim")]
    [SerializeField] private float spinSpeed = 1080f;
    [SerializeField] private float spinTime = 2.0f;
    [SerializeField] private float decelerationTime = 2.0f;

    private bool isSpinning = false;
    private List<GachaDrop> poolItems;

    void Start()
    {
        SetupWheel();

        if (spinButton != null)
        {
            spinButton.onClick.AddListener(StartSpin);
        }
    }

    void SetupWheel()
    {
        if (activeGachaPool == null) return;

        poolItems = activeGachaPool.possibleDrops;
        float segmentAngle = 360f / poolItems.Count;

        Debug.Log("Wheel setup complete. Ready to spin.");
    }

    public void StartSpin()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinTheWheelCoroutine());
        }
    }

    private IEnumerator SpinTheWheelCoroutine()
    {
        isSpinning = true;
        spinButton.interactable = false;

        int totalWeight = 0;
        foreach (var drop in poolItems) totalWeight += drop.weight;
        int randomNumber = Random.Range(1, totalWeight + 1);

        int winningIndex = -1;
        for (int i = 0; i < poolItems.Count; i++)
        {
            randomNumber -= poolItems[i].weight;
            if (randomNumber <= 0)
            {
                winningIndex = i;
                break;
            }
        }

        GachaItem winningItem = poolItems[winningIndex].item;
        float segmentAngle = 360f / poolItems.Count;
        float targetAngle = winningIndex * segmentAngle;

        float currentAngle = wheelTransform.eulerAngles.z;
        float targetRotation = -targetAngle;

        float elapsed = 0f;
        while (elapsed < spinTime)
        {
            float deltaAngle = spinSpeed * Time.deltaTime;
            wheelTransform.Rotate(0, 0, -deltaAngle);
            elapsed += Time.deltaTime;
            yield return null;
        }

        currentAngle = wheelTransform.eulerAngles.z;

        float finalZ = 360f * Mathf.Ceil(currentAngle / 360f) + targetRotation; 
        Quaternion startRot = wheelTransform.rotation;
        Quaternion endRot = Quaternion.Euler(0, 0, finalZ);

        elapsed = 0f;
        while (elapsed < decelerationTime)
        {
            elapsed += Time.deltaTime;
            float t = EaseOutCubic(elapsed / decelerationTime);
            wheelTransform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        wheelTransform.rotation = endRot;

        Debug.Log($"Spin finished! You won: {winningItem.itemName} ({winningItem.starRating} stars)!");
        isSpinning = false;
        spinButton.interactable = true;
    }


    //private IEnumerator SpinTheWheelCoroutine()
    //{
    //    isSpinning = true;
    //    spinButton.interactable = false;

    //    int totalWeight = 0;
    //    foreach (var drop in poolItems) { totalWeight += drop.weight; }
    //    int randomNumber = Random.Range(1, totalWeight + 1);

    //    int winningIndex = -1;
    //    for (int i = 0; i < poolItems.Count; i++)
    //    {
    //        randomNumber -= poolItems[i].weight;
    //        if (randomNumber <= 0)
    //        {
    //            winningIndex = i;
    //            break;
    //        }
    //    }

    //    GachaItem winningItem = poolItems[winningIndex].item;
    //    Debug.Log($"Prize determined: {winningItem.itemName} at index {winningIndex}");

    //    float segmentAngle = 360f / poolItems.Count;
    //    float targetAngle = winningIndex * segmentAngle;

    //    float finalAngle = -((spinRotations * 360f) + targetAngle);

    //    Quaternion startRotation = wheelTransform.rotation;
    //    Quaternion finalRotation = Quaternion.Euler(0, 0, finalAngle);

    //    float elapsedTime = 0f;
    //    while (elapsedTime < spinDuration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        float t = EaseOutCubic(elapsedTime / spinDuration);
    //        wheelTransform.rotation = Quaternion.Slerp(startRotation, finalRotation, t);
    //        yield return null;
    //    }

    //    wheelTransform.rotation = finalRotation;

    //    Debug.Log($"Spin finished! You won: {winningItem.itemName} ({winningItem.starRating} stars)!");

    //    isSpinning = false;
    //    spinButton.interactable = true;
    //}

    private float EaseOutCubic(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }
}

