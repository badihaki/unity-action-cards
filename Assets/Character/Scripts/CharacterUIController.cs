using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIController : MonoBehaviour
{
    protected Canvas _ResourceCanvas;
    [SerializeField, Header("Health")]private Health _HealthController;
    [SerializeField] private Slider _HealthBar;
    [SerializeField] private float _TargetHealth;
    [SerializeField] private bool canChangeHealth = false;
    [SerializeField] protected float _UIChangeRate = 0.015f;

	private bool isPlayer;

    protected virtual void OnEnable()
    {
        if (_HealthController != null) _HealthController.OnHealthChanged += UpdateHealthUI;
    }

    public virtual void InitializeUI(bool isEntityPlayer, Character character)
    {
        transform.Find("ResourceCanvas").GetComponent<Canvas>();
		if (!_ResourceCanvas) _ResourceCanvas = transform.Find("ResourceCanvas").GetComponent<Canvas>();
        if (!_HealthBar) _HealthBar = _ResourceCanvas.transform.Find("HealthBar").GetComponent<Slider>();
        if (_HealthController == null)
        {
            _HealthController = GetComponent<Health>();
            _HealthBar.maxValue = _HealthController._MaxHealth;
            _HealthBar.value = _HealthBar.maxValue;
            _TargetHealth = _HealthController._MaxHealth;
            
            canChangeHealth = false;
            _HealthController.OnHealthChanged += UpdateHealthUI;
            isPlayer = isEntityPlayer;

            if (!isPlayer)
            {
                UpdatePositionMatchActor posUpdater = _ResourceCanvas.transform.AddComponent<UpdatePositionMatchActor>();
                posUpdater.Initialize(character._Actor);
                _ResourceCanvas.gameObject.SetActive(false);
            }
        }
    }

    protected virtual void OnDisable()
    {
        _HealthController.OnHealthChanged -= UpdateHealthUI;
    }

    // Update is called once per frame
    protected virtual void Update()
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
                _HealthBar.value = Mathf.Lerp(_HealthBar.value, _TargetHealth, _UIChangeRate);
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