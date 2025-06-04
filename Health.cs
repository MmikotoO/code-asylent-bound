using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float HP =100;
    public Slider HealthBar;

    void Update()
    {
        HealthBar.value = HP;
    }
}
