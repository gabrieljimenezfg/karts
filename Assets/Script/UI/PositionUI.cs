using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PositionUI : MonoBehaviour
{
    [SerializeField]
    private List<CarInfo> cars;
    [SerializeField] private TextMeshProUGUI positionPlayerText;

    private void Update()
    {
        SortCars();
        var playerIndex = 0;
        for (int i = 0; i < cars.Count; i++)
        {
            if (TryGetComponent<CarController>(out _))
            {
                playerIndex = i;
                break;
            }
        }

        positionPlayerText.text = (playerIndex + 1) + "º";
        Debug.Log(playerIndex);
    }

    private void SortCars()
    {
        cars.Sort((a, b) => b.OrderReference().CompareTo(a.OrderReference()));
    }
}