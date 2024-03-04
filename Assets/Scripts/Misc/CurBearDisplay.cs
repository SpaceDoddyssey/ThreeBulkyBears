using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurBearDisplay : MonoBehaviour
{
    [SerializeField] private Image baby, mama, papa;

    public void UpdateDisplay(BearStats bearStats)
    {
        if (bearStats.bearName == "Baby")
        {
            ActivateBear(baby);
            DeactivateBear(mama);
            DeactivateBear(papa);
        }
        else if (bearStats.bearName == "Mama")
        {
            DeactivateBear(baby);
            ActivateBear(mama);
            DeactivateBear(papa);
        }
        else if (bearStats.bearName == "Papa")
        {
            DeactivateBear(mama);
            DeactivateBear(baby);
            ActivateBear(papa);
        }
    }

    private void DeactivateBear(Image bear)
    {
        bear.color = new Color(0.3f, 0.3f, 0.3f, 1f);
    }

    private void ActivateBear(Image bear)
    {
        bear.color = new Color(1f, 1f, 1f, 1f);
    }
}
