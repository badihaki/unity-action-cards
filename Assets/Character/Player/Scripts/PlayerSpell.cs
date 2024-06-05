using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSpell : MonoBehaviour
{
    [Serializable] public struct storedSpell
    {
        public SpellCardScriptableObj spell;
        public int chargesLeft;
        public Image spellIcon;
        public RectTransform spellIconTransform;
        public TextMeshProUGUI spellChargeText;
    }

    private PlayerCharacter player;
    [SerializeField] private int _maxSpellList = 6;
    [SerializeField] private List<storedSpell> _activeSpellList;
    [SerializeField] private int _currentSpellIndex;
    [SerializeField] private SpellCardScriptableObj _baseSpell;

    [SerializeField] private LockSpellTargetRotation _spellTarget;

    private float timeToAddToTimer;
    [SerializeField] private float _spellTimer;

    [SerializeField] private GameObject _spellUIPrefab;
    [SerializeField] private GameObject _spellUI;
    [SerializeField] private GameObject _spellIconPrefab;

    public void Initialize(PlayerCharacter pl)
    {
        player = pl;
        DeployUI();

        _activeSpellList = new List<storedSpell>();
        AddSpellToList(_baseSpell);
        _spellTarget = GetComponentInChildren<LockSpellTargetRotation>();
        _spellTimer = 0.0f;
        timeToAddToTimer = _activeSpellList[_currentSpellIndex].spell._SpellAddonTime;
    }

    private void DeployUI()
    {
        _spellUI = Instantiate(_spellUIPrefab, Vector3.zero, Quaternion.identity).transform.Find("Organizer").gameObject;
    }

    private void Update()
    {
        RunSpellTimer();
    }

    private void RunSpellTimer()
    {
        if (_spellTimer <= 0) _spellTimer = 0.0f;
        else
        {
            _spellTimer -= Time.deltaTime;
        }
    }

    public void AddSpellToList(SpellCardScriptableObj spellCard)
    {
        if (_activeSpellList.Count < _maxSpellList)
        {
            if (_activeSpellList.Count == 1) _activeSpellList[_currentSpellIndex].spellIconTransform.localScale = new Vector3(1.50f, 1.50f, 0.00f);
            // create a new struct
            storedSpell spell = new storedSpell();
            
            // set spell settings
            spell.spell = spellCard;
            spell.chargesLeft = spellCard._SpellCharges;

            // instantiate the icon, set ref to icon in struct
            GameObject icon = Instantiate(_spellIconPrefab, _spellUI.GetComponent<RectTransform>());
            spell.spellIcon = icon.GetComponent<Image>();
            spell.spellIcon.sprite = spellCard._CardImage;
            spell.spellChargeText = spell.spellIcon.GetComponentInChildren<TextMeshProUGUI>();
            spell.spellChargeText.text = spell.chargesLeft.ToString();
            spell.spellIconTransform = spell.spellIcon.GetComponent<RectTransform>();

            // add spell to list
            _activeSpellList.Add(spell);
        }
    }

    public void ChangeSpellIndex()
    {
        if (_activeSpellList.Count > 1)
        {
            int maxIndex = _activeSpellList.Count - 1;

            // change current spell index size to 1x1
            _activeSpellList[_currentSpellIndex].spellIconTransform.localScale = Vector3.one;
            _currentSpellIndex++;
            if (_currentSpellIndex > maxIndex)
            {
                _currentSpellIndex = 0;
            }
            _spellTimer = 0.0f;
            timeToAddToTimer = _activeSpellList[_currentSpellIndex].spell._SpellAddonTime;
            // change current spell index size to 1.5x1.5
            _activeSpellList[_currentSpellIndex].spellIconTransform.localScale = new Vector3(1.50f, 1.50f, 0.00f);
        }
    }

    public void UseSpell()
    {
        if(_spellTimer <= 0)
        {
            player._AnimationController.SetTrigger(_activeSpellList[_currentSpellIndex].spell._SpellAnimationBool.ToString());
            Projectile conjuredSpell = Instantiate(_activeSpellList[_currentSpellIndex].spell._SpellProjectile, _spellTarget.transform.position, _spellTarget.targetRotation).GetComponent<Projectile>();

            conjuredSpell.name = _activeSpellList[_currentSpellIndex].spell._CardName;
            conjuredSpell.InitializeProjectile(player, _activeSpellList[_currentSpellIndex].spell._SpellDamage, _activeSpellList[_currentSpellIndex].spell._SpellProjectileSpeed, _activeSpellList[_currentSpellIndex].spell._SpellLifetime);
            _spellTimer = timeToAddToTimer;
        
            // remove a charge
            if (_currentSpellIndex != 0)
            {
                storedSpell modifiedSpell = _activeSpellList[_currentSpellIndex];
                modifiedSpell.chargesLeft -= 1;
                modifiedSpell.spellChargeText.text = modifiedSpell.chargesLeft.ToString();

                // yo, if there's no charges left, lets delete this
                if (modifiedSpell.chargesLeft <= 0)
                {
                    int oldSpellIndexNumber = _currentSpellIndex;
                    ChangeSpellIndex();
                    Destroy(_activeSpellList[oldSpellIndexNumber].spellIcon.gameObject);
                    _activeSpellList.Remove(_activeSpellList[oldSpellIndexNumber]);
                }
                else _activeSpellList[_currentSpellIndex] = modifiedSpell;
            }
        }
    }

    // end
}
