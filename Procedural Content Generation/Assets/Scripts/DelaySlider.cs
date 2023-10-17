using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelaySlider : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private Turtle turt;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateDelay()
    {
        turt.SetTimeDelay(slider.value);
    }
}
