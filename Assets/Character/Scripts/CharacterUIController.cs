using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIController : MonoBehaviour
{
    private Canvas _ResourceCanvas;
    [SerializeField, Header("Health")]private Health _HealthController;
    [SerializeField] private Slider _HealthBar;
    [SerializeField] private float _TargetHealth;
    [SerializeField] private bool canChangeHealth = false;
    [SerializeField] private float _UIChangeRate = 0.015f;

	[SerializeField, Header("Aether")] private Aether _AetherController;
    [SerializeField] private Slider _AetherBar;
	[SerializeField] private float _TargetAether;
	[SerializeField] private bool canChangeAether = false;

	private bool isPlayer;

    private void OnEnable()
    {
        if (_HealthController != null) _HealthController.OnHealthChanged += UpdateHealthUI;
    }

    public void InitializeUI(bool isEntityPlayer, Character character)
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
            else
            {
                _AetherBar = _ResourceCanvas.transform.Find("AetherBar").GetComponent<Slider>();

				canChangeAether = false;
				_AetherController = GetComponent<Aether>();
			    _AetherBar.maxValue = _AetherController._MaxAether;
                _AetherBar.value = _AetherController._MaxAether;
			    _TargetAether = _AetherController._MaxAether;

                _AetherController.OnAetherChanged += UpdateAetherUI;
			}
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
        ControlAetherUI();
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

    private void ControlAetherUI()
    {
        if (canChangeAether)
        {
            if((int)_AetherBar.value != _TargetAether)
            {
                _AetherBar.value = Mathf.Lerp(_AetherBar.value, _TargetAether, _UIChangeRate);
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

    private void UpdateAetherUI(int aether)
    {
        _TargetAether = aether;
        canChangeAether = true;
    }

    private IEnumerator CloseResourceCanvas()
    {
        yield return new WaitForSeconds(3.35f);
        _ResourceCanvas.gameObject.SetActive(false);
        
    }
}