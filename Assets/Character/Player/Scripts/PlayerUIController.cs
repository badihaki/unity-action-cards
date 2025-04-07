using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : CharacterUIController
{
	private PlayerCharacter _Player;
	private Camera _Cam;
	[SerializeField, Header("Aether")] private Aether _AetherController;
	[SerializeField] private Slider _AetherBar;
	[SerializeField] private float _TargetAether;
	[SerializeField] private bool canChangeAether = false;

	[SerializeField, Header("Deck")] private Image _DeckIcon;
	[SerializeField] private Sprite[] deckStatusIcons;
	private TextMeshProUGUI deckCountUI;

	private Canvas _WeaponCanvas;
	private Slider _WeaponMeter;

	[field: SerializeField, Header("Spells")]
	private Image _SpellIconBasePrefab;
	private Canvas _SpellSlingCanvas;
	private RectTransform _SpellContainer;
	private GameObject _Crosshair;
	[Serializable]
	public struct StoredSpellStruct
	{
		public SpellCardScriptableObj spell;
		public int chargesLeft;
		public Image spellIcon;
		public RectTransform spellIconTransform;
		public TextMeshProUGUI spellChargeText;
	}
	[field: SerializeField] public List<StoredSpellStruct> _ActiveSpellList { get; private set; }
	[SerializeField] private int _MaxSpellCount = 6;
	[field: SerializeField] public int _CurrentSpellIndex { get; private set; }
	private Canvas _InteractionCanvas;



	#region Initialize
	public override void InitializeUI(bool isEntityPlayer, Character character)
	{
		base.InitializeUI(isEntityPlayer, character);
		_Player = character as PlayerCharacter;
		_Cam = Camera.main;

		InitAetherUI();
		InitSpellUI();
		InitWeaponMeterUI();
		InitDeckUI();
		InitInteractionUI();
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
		_SpellContainer = _SpellSlingCanvas.transform.Find("Container").GetComponent<RectTransform>();
		_Crosshair = _SpellSlingCanvas.transform.Find("Crosshair").gameObject;
		SetShowCrossHair(false);
		_ActiveSpellList = new List<StoredSpellStruct>();

		AddSpellToUI(_Player._PlayerSpells._baseSpell);
		_Player._PlayerSpells.SetHowMuchTimeToAddToSpellTimer(_ActiveSpellList[0].spell._SpellAddonTime);
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

	private void InitInteractionUI()
	{
		_InteractionCanvas = transform.Find("InteractionCanvas").GetComponent<Canvas>();
		_InteractionCanvas.gameObject.SetActive(false);
		SetShowInteractionCanvas(false);
		_Player._InteractionController.onCanInteract += SetShowInteractionCanvas;
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
			_Player._InteractionController.onCanInteract += SetShowInteractionCanvas;
		}
	}
	protected override void OnDisable()
	{
		base.OnDisable();

		_AetherController.OnAetherChanged -= UpdateAetherUI;
		_Player._WeaponController.OnDurabilityChanged -= UpdateWeaponUI;
		_Player._PlayerCards.onDeckCountChanged -= UpdateDeckCount;
		_Player._PlayerCards.onDeckIsActiveChanged -= ChangeDeckIcon;
		_Player._InteractionController.onCanInteract -= SetShowInteractionCanvas;
	}

	protected override void Update()
	{
		base.Update();
		ControlAetherUI();
	}

	#region Aether UI Methods
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
	#endregion

	#region WeaponUI
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

	public void DisableWeaponUI() => _WeaponMeter.gameObject.SetActive(false);
	#endregion

	#region Deck UI
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
	#endregion

	#region Spell Sling UI
	public void SetShowCrossHair(bool isVisible) => _Crosshair.SetActive(isVisible);

	public void UpdateCrosshairPos(Vector3 pos)
	{
		if (pos == Vector3.zero)
		{
			Vector3 screenCenterPoint = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
			_Crosshair.transform.position = screenCenterPoint;
		}
		else
		{
			Vector3 screenPos = _Cam.WorldToScreenPoint(pos);
			_Crosshair.transform.position = screenPos;
		}
	}

	public void AddSpellToUI(SpellCardScriptableObj spellCard)
	{
		if (_ActiveSpellList.Count < _MaxSpellCount)
		{
			if (_ActiveSpellList.Count == 1) _ActiveSpellList[_CurrentSpellIndex].spellIconTransform.localScale = new Vector3(1.50f, 1.50f, 0.00f);
			// create a new data structure for the stored spell
			StoredSpellStruct newSpell = new StoredSpellStruct();
			
			// set spell settings
			newSpell.spell = spellCard;
			newSpell.chargesLeft = spellCard._SpellCharges;
			
			// instantiate the icon
			Image icon = Instantiate(_SpellIconBasePrefab, _SpellContainer);
			// set icon reference
			newSpell.spellIcon = icon; // get a reference to the icon
			newSpell.spellIcon.sprite = spellCard._CardImage; // set the icon image

			// set the rest of the parameters
			newSpell.spellChargeText = newSpell.spellIcon.GetComponentInChildren<TextMeshProUGUI>(); // get the spell icon
			newSpell.spellChargeText.text = newSpell.chargesLeft.ToString();
			newSpell.spellIconTransform = newSpell.spellIcon.GetComponent<RectTransform>();

			// finally add the spell to the list
			_ActiveSpellList.Add(newSpell);
		}
	}

	public void ChangeSpell(int dir)
	{
		if (_ActiveSpellList.Count > 1)
		{
			int maxIndex = _ActiveSpellList.Count - 1;
			if (dir > 0)
			{
				print("up");
				// change current spell index size to 1x1
				_ActiveSpellList[_CurrentSpellIndex].spellIconTransform.localScale = Vector3.one;
				_CurrentSpellIndex++;
				if (_CurrentSpellIndex > maxIndex)
				{
					_CurrentSpellIndex = 0;
				}
			}
			else
			{
				print("down");
				// change current spell index size to 1x1
				_ActiveSpellList[_CurrentSpellIndex].spellIconTransform.localScale = Vector3.one;
				_CurrentSpellIndex--;
				if (_CurrentSpellIndex < 0)
				{
					_CurrentSpellIndex = _ActiveSpellList.Count - 1;
				}
			}
			_Player._PlayerSpells.ResetSpellTimer();
			_Player._PlayerSpells.SetHowMuchTimeToAddToSpellTimer(_ActiveSpellList[_CurrentSpellIndex].spell._SpellAddonTime);
			// change current spell index size to 1.5x1.5
			_ActiveSpellList[_CurrentSpellIndex].spellIconTransform.localScale = new Vector3(1.50f, 1.50f, 0.00f);
		}
	}

	public void RemoveSpellCharge()
	{
		if (_CurrentSpellIndex != 0)
		{
			StoredSpellStruct modifiedSpell = _ActiveSpellList[_CurrentSpellIndex];
			modifiedSpell.chargesLeft -= 1;
			modifiedSpell.spellChargeText.text = modifiedSpell.chargesLeft.ToString();

			// yo, if there's no charges left, lets delete this
			if (modifiedSpell.chargesLeft <= 0)
			{
				int oldSpellIndexNumber = _CurrentSpellIndex;
				ChangeSpell(-1);
				Destroy(_ActiveSpellList[oldSpellIndexNumber].spellIcon.gameObject);
				_ActiveSpellList.Remove(_ActiveSpellList[oldSpellIndexNumber]);
			}
			else _ActiveSpellList[_CurrentSpellIndex] = modifiedSpell;
		}
	}
	#endregion

	#region Interaction UI
	public void SetShowInteractionCanvas(bool showCanvas) => _InteractionCanvas.gameObject.SetActive(showCanvas);
	#endregion

	// end
}
