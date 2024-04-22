using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] GameObject mask, bomb;
    [SerializeField] TextMeshPro text; // hien thi so bomb 
    public bool isOpen;

    public void SetActiveMask(bool isActive)
    {
        mask.SetActive(isActive);
    }
    public void SetActiveBomb(bool isActive)
    {
        bomb.SetActive(isActive);
    }
    // up date so bom
    public void Reset()
    {
        SetActiveMask(true);
        SetActiveBomb(false);
        isOpen = false;
        SetText("");
    }

    public void SetText(string numberBomb)
    {
        text.SetText(numberBomb);
    }
}
