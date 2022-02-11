using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    void Start()
    {
        SetAllHpBarsParent();
    }
    void SetAllHpBarsParent()
    {
        foreach(GameObject healthBar in GameObject.FindGameObjectsWithTag("HealthBar"))
        {
            healthBar.transform.SetParent(transform);
            healthBar.GetComponent<HealthBar>().SetPosition();
        }
    }

}
