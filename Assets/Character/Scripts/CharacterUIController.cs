using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIController : MonoBehaviour
{
    private Canvas _ResourceCanvas;
    private Health _HealthController;
    [SerializeField] private Slider _HealthBar;
    [SerializeField] private float _TargetHealth;
    [SerializeField] private bool canChangeHealth = false;
    [SerializeField] private float _HealthChangeRate = 0.015f;
    private bool isPlayer;

    private void OnEnable()
    {
        if (_HealthController != null) _HealthController.OnHealthChanged += UpdateHealthUI;
    }

    public void InitializeUI(bool isEntityPlayer)
    {
        if(!_ResourceCanvas) _ResourceCanvas = transform.Find("ResourceCanvas").GetComponent<Canvas>();
        if (!_HealthBar) _HealthBar = _ResourceCanvas.transform.Find("HealthBar").GetComponent<Slider>();
        if (_HealthController == null)
        {
            _HealthController = GetComponent<Health>();
            _HealthBar.maxValue = _HealthController._MaxHealth;
            _TargetHealth = _HealthController._MaxHealth;
            canChangeHealth = false;
            _HealthBar.value = _HealthBar.maxValue;
            _HealthController.OnHealthChanged += UpdateHealthUI;
            isPlayer = isEntityPlayer;

            if (!isPlayer) _ResourceCanvas.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _HealthController.OnHealthChanged -= UpdateHealthUI;
    }

    // Update is called once per frame
    private void Update()
    {
        ControlHealthUI();
    }

    private void ControlHealthUI()
    {
        if(canChangeHealth)
        {
            if (!isPlayer) OpenResourceCanvas();
            if ((int)_HealthBar.value != _TargetHealth)
            {
                _HealthBar.value = Mathf.Lerp(_HealthBar.value, _TargetHealth, _HealthChangeRate);
            }
            else
            {
                canChangeHealth = false;
                if (!isPlayer) StartCoroutine(CloseResourceCanvas());
            }
        }
    }

    private void OpenResourceCanvas()
    {
        if (_ResourceCanvas.gameObject.activeSelf == false)
        {
            _ResourceCanvas.gameObject.SetActive(true);
        }
    }

    private void UpdateHealthUI(int health)
    {
        _TargetHealth = health;
        canChangeHealth = true;
    }

    private IEnumerator CloseResourceCanvas()
    {
        yield return new WaitForSeconds(3.35f);
        _ResourceCanvas.gameObject.SetActive(false);
        
    }
}