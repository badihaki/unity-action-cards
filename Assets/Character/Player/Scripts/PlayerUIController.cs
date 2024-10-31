using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : CharacterUIController
{
	[SerializeField, Header("Aether")] private Aether _AetherController;
	[SerializeField] private Slider _AetherBar;
	[SerializeField] private float _TargetAether;
	[SerializeField] private bool canChangeAether = false;

	public override void InitializeUI(bool isEntityPlayer, Character character)
	{
		base.InitializeUI(isEntityPlayer, character);

		_AetherBar = _ResourceCanvas.transform.Find("AetherBar").GetComponent<Slider>();

		canChangeAether = false;
		_AetherController = GetComponent<Aether>();
		_AetherBar.maxValue = _AetherController._MaxAether;
		_AetherBar.value = _AetherController._MaxAether;
		_TargetAether = _AetherController._MaxAether;

		_AetherController.OnAetherChanged += UpdateAetherUI;
	}

	protected override void Update()
	{
		base.Update();
		ControlAetherUI();
	}

	private void ControlAetherUI()
	{
		if (canChangeAether)
		{
			if ((int)_AetherBar.value != _TargetAether)
			{
				_AetherBar.value = Mathf.Lerp(_AetherBar.value, _TargetAether, _UIChangeRate);
			}
		}
	}

	private void UpdateAetherUI(int aether)
	{
		_TargetAether = aether;
		canChangeAether = true;
	}
}
