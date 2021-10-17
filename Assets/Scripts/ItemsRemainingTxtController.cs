using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsRemainingTxtController : MonoBehaviour
{
    private int voltaTotalObjects;
    private int voltaCurrentObjects = 0;
    [SerializeField] private TMPro.TextMeshProUGUI itemsRemainingText;

    private void OnEnable()
    {
        LevelStateController.onVoltaTotalObjReady += GetVoltaTotalObjects;
        LevelStateController.onCurrentVoltaObjChange += UpdateCurrentVoltaObjects;
    }

    private void GetVoltaTotalObjects(int value)
    {
        voltaTotalObjects = value;
        itemsRemainingText.text = string.Format("{0}/{1}", voltaCurrentObjects, voltaTotalObjects);
    }

    private void UpdateCurrentVoltaObjects(int value)
    {
        voltaCurrentObjects = value;
        itemsRemainingText.text = string.Format("{0}/{1}", voltaCurrentObjects, voltaTotalObjects);
    }
}
