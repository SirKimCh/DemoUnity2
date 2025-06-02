using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeathBarController : MonoBehaviour
{
    [SerializeField] private PlayerHeath playerHeath;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        totalHealthBar.fillAmount = playerHeath.currentHeath / 10;
    }
    private void Update()
    {
        currentHealthBar.fillAmount = playerHeath.currentHeath / 10;
    }
}
