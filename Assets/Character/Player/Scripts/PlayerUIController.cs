using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : CharacterUIController
{
	private PlayerCharacter _Player;
	[SerializeField, Header("Aether")] private Aether _AetherController;
	[SerializeField] private Slider _AetherBar;
	[SerializeField] private float _TargetAether;
	[SerializeField] private bool canChangeAether = false;

	[SerializeField, Header("Deck")] private Image _DeckIcon;
	[SerializeField] private Sprite[] deckStatusIcons;
	private TextMeshProUGUI deckCountUI;

	private Canvas _WeaponCanvas;
	[SerializeField, Header("Weapon Meter")] private Slider _WeaponMeter;

	private Canvas _SpellSlingCanvas;

	#region Initialize
	public override void InitializeUI(bool isEntityPlayer, Character character)
	{
		base.InitializeUI(isEntityPlayer, character);
		_Player = character as PlayerCharacter;

		InitAetherUI();
		InitSpellUI();
		InitWeaponMeterUI();
		InitDeckUI();
	}

	private void InitAetherUI()
	{
		_AetherBar = _ResourceCanvas.transform.Find("AetherBar").GetComponent<Slider>();

		canChangeAether = false;
		_AetherController = GetComponent<Aether>();
		_AetherBar.maxValue = _AetherController._MaxAether;
		_AetherBar.value = _AetherController._MaxAether;
		_TargetAether = _AetherController._MaxAether;
		_AetherController.OnAetherChanged += UpdateAetherUI; // subscribe to the OnAetherChanged event
	}

	private void InitSpellUI()
	{
		_SpellSlingCanvas = transform.Find("SpellSlingCanvas").GetComponent<Canvas>();
	}
	
	private void InitWeaponMeterUI()
	{
		_WeaponCanvas = transform.Find("WeaponCanvas").GetComponent<Canvas>();
		_WeaponMeter = _WeaponCanvas.transform.Find("WeaponMeter").GetComponent<Slider>();
		_WeaponMeter.gameObject.SetActive(false);
		_Player._WeaponController.OnDurabilityChanged += UpdateWeaponUI; // sub to the OnDurabilityChanged event
	}

	private void InitDeckUI()
	{
		_DeckIcon = _ResourceCanvas.transform.Find("DeckIcon").GetComponent<Image>();
		_DeckIcon.sprite = deckStatusIcons[1];
		deckCountUI = _DeckIcon.transform.Find("Count").GetComponent<TextMeshProUGUI>();
		_Player._PlayerCards.onDeckCountChanged += UpdateDeckCount;
		UpdateDeckCount(_Player._PlayerCards._Deck.Count);
		_Player._PlayerCards.onDeckIsActiveChanged += ChangeDeckIcon;
		ChangeDeckIcon(true);
	}
	#endregion

	protected override void OnEnable()
	{
		base.OnEnable();

		if(_Player != null)
		{
			_AetherController.OnAetherChanged += UpdateAetherUI;
			_Player._WeaponController.OnDurabilityChanged += UpdateWeaponUI;
			_Player._PlayerCards.onDeckCountChanged += UpdateDeckCount;
			_Player._PlayerCards.onDeckIsActiveChanged += ChangeDeckIcon;
		}
	}
	protected override void OnDisable()
	{
		base.OnDisable();

		_AetherController.OnAetherChanged -= UpdateAetherUI;
		_Player._WeaponController.OnDurabilityChanged -= UpdateWeaponUI;
		_Player._PlayerCards.onDeckCountChanged -= UpdateDeckCount;
		_Player._PlayerCards.onDeckIsActiveChanged -= ChangeDeckIcon;
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

	public void ActivateWeaponUI(int durability)
	{
		_WeaponMeter.gameObject.SetActive(true);
		_WeaponMeter.value = durability;
		_WeaponMeter.maxValue = durability;
	}

	private void UpdateWeaponUI(int durability)
	{
		_WeaponMeter.value = durability;
	}

	public void ChangeDeckIcon(bool isActive)
	{
		if (isActive)
			_DeckIcon.sprite = deckStatusIcons[1];
		else
			_DeckIcon.sprite = deckStatusIcons[0];
	}

	public void UpdateDeckCount(int amount)
	{
		deckCountUI.text = amount.ToString();
	}

	public void DisableWeaponUI() => _WeaponMeter.gameObject.SetActive(false);
}
