using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseButton : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void SetText(string txt)
    {
        text.text = txt;
    }
}
