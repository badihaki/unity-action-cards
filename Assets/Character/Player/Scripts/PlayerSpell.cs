using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Experimental.GraphView.GraphView;
using Unity.Mathematics;

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
    private Camera cam;

    [SerializeField] private int _maxSpellList = 6;
    [SerializeField] private List<storedSpell> _activeSpellList;
    [SerializeField] private int _currentSpellIndex;
    [SerializeField] private SpellCardScriptableObj _baseSpell;

    [field: SerializeField] public Transform _spellOrigin { get; private set; }

    private float timeToAddToTimer;
    [SerializeField] private float _spellTimer;

    [Header("UI")]
    [Tooltip("The UI Prefab that controls spell stuff")]
    [SerializeField] private GameObject _spellUIPrefab;
    [SerializeField] private GameObject _spellContainerUI;
    [SerializeField] private GameObject _spellIconPrefab;

    [Header("UI, targeting stuff")]
    [SerializeField] private Image _crosshair;

    public void Initialize(PlayerCharacter pl)
    {
        player = pl;
        cam = Camera.main;
        DeployUI();

        _activeSpellList = new List<storedSpell>();
        AddSpellToList(_baseSpell);
        _spellOrigin = player._PlayerActor.transform.Find("SpellTarget");
        _spellTimer = 0.0f;
        timeToAddToTimer = _activeSpellList[_currentSpellIndex].spell._SpellAddonTime;
    }

    private void DeployUI()
    {
        GameObject ui = Instantiate(_spellUIPrefab, Vector3.zero, Quaternion.identity);
        _spellContainerUI = ui.transform.Find("Organizer").gameObject;
        _crosshair = ui.transform.Find("Crosshair").GetComponent<Image>();
        _crosshair.gameObject.SetActive(false);
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
    /*
    #region Crosshair Functions
    public void ShowCrosshair() => _crosshair.gameObject.SetActive(true);

    public void HideCrosshair() => _crosshair.gameObject.SetActive(false);

    public void UpdateCrosshair()
    {
        // update crosshair here
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        Ray ray = cam.ScreenPointToRay(screenCenterPoint);
        targetPos = ray.direction;
        Vector3 desiredCrosshairPos = _spellTarget.transform.position + transform.forward * 10;

        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
        {
            targetPos = hit.point;
            desiredCrosshairPos = cam.WorldToScreenPoint(hit.point);
        }
        _crosshair.transform.position = desiredCrosshairPos;
    }
    #endregion
    */
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
            GameObject icon = Instantiate(_spellIconPrefab, _spellContainerUI.GetComponent<RectTransform>());
            spell.spellIcon = icon.GetComponent<Image>();
            spell.spellIcon.sprite = spellCard._CardImage;
            spell.spellChargeText = spell.spellIcon.GetComponentInChildren<TextMeshProUGUI>();
            spell.spellChargeText.text = spell.chargesLeft.ToString();
            spell.spellIconTransform = spell.spellIcon.GetComponent<RectTransform>();

            // add spell to list
            _activeSpellList.Add(spell);
        }
    }

    public void ChangeSpell(int dir)
    {
        if (_activeSpellList.Count > 1)
        {
            int maxIndex = _activeSpellList.Count - 1;
            if(dir > 0)
            {
                print("up");
                // change current spell index size to 1x1
                _activeSpellList[_currentSpellIndex].spellIconTransform.localScale = Vector3.one;
                _currentSpellIndex++;
                if (_currentSpellIndex > maxIndex)
                {
                    _currentSpellIndex = 0;
                }
            }
            else
            {
                print("down");
                // change current spell index size to 1x1
                _activeSpellList[_currentSpellIndex].spellIconTransform.localScale = Vector3.one;
                _currentSpellIndex--;
                if (_currentSpellIndex < 0)
                {
                    _currentSpellIndex = _activeSpellList.Count -1;
                }
            }
            _spellTimer = 0.0f;
            timeToAddToTimer = _activeSpellList[_currentSpellIndex].spell._SpellAddonTime;
            // change current spell index size to 1.5x1.5
            _activeSpellList[_currentSpellIndex].spellIconTransform.localScale = new Vector3(1.50f, 1.50f, 0.00f);
        }
    }

    public void UseSpell(Vector3 targetPos)
    {
        if(_spellTimer <= 0)
        {
            ShootSpell(targetPos);
        }
    }

    private void ShootSpell(Vector3 targetPos)
    {
        player._AnimationController.SetTrigger(_activeSpellList[_currentSpellIndex].spell._SpellAnimationBool.ToString());
        
        Projectile conjuredSpell;
        if(targetPos !=  Vector3.zero)
        {
            //player._LocomotionController.RotateTowardsTarget(targetPos);
            conjuredSpell = Instantiate(_activeSpellList[_currentSpellIndex].spell._SpellProjectile, _spellOrigin.transform.position, Quaternion.identity).GetComponent<Projectile>();
            Quaternion targetDir = Quaternion.Euler(player._PlayerActor.transform.position - targetPos);
        }
        else
        {
            conjuredSpell = Instantiate(_activeSpellList[_currentSpellIndex].spell._SpellProjectile, _spellOrigin.transform.position, Quaternion.identity).GetComponent<Projectile>();
        }
        conjuredSpell.transform.rotation = player._PlayerActor.transform.rotation;
        conjuredSpell.name = _activeSpellList[_currentSpellIndex].spell._CardName;
        conjuredSpell.InitializeProjectile(player, _activeSpellList[_currentSpellIndex].spell._SpellDamage, _activeSpellList[_currentSpellIndex].spell._SpellProjectileSpeed, _activeSpellList[_currentSpellIndex].spell._SpellLifetime, _activeSpellList[_currentSpellIndex].spell._SpellKnockAndLaunchForces, _activeSpellList[_currentSpellIndex].spell._SpellImpactVFX);
        _spellTimer = timeToAddToTimer;
        RemoveSpellCharge();
    }

    public Vector3 DetectRangedTargets()
    {
        Vector3 targetPos = Vector3.zero;
        // print($"number of targets {player._LockOnTargeter.rangeTargets.Count}");
        /*
        if (player._Controls._MoveInput != Vector2.zero)
            player._LocomotionController.RotateInstantly(player._Controls._MoveInput);
         */
        if (player._LockOnTargeter.rangeTargets.Count > 0)
        {
            player._LockOnTargeter.rangeTargets.ForEach(t =>
            {
                print($">>>>>> targetable object is {t.name} with a position of {t.position} || playerSpell.DetectRangeTargets");
                if (targetPos == Vector3.zero) targetPos = t.position;
                else if (Vector3.Distance(transform.position, t.position) > Vector3.Distance(transform.position, targetPos))
                {
                    targetPos = t.position;
                }
            });
        }
        // print($"targetting {targetPos}");
        return targetPos;
    }

    private void RemoveSpellCharge()
    {
        if (_currentSpellIndex != 0)
        {
            storedSpell modifiedSpell = _activeSpellList[_currentSpellIndex];
            modifiedSpell.chargesLeft -= 1;
            modifiedSpell.spellChargeText.text = modifiedSpell.chargesLeft.ToString();

            // yo, if there's no charges left, lets delete this
            if (modifiedSpell.chargesLeft <= 0)
            {
                int oldSpellIndexNumber = _currentSpellIndex;
                ChangeSpell(-1);
                Destroy(_activeSpellList[oldSpellIndexNumber].spellIcon.gameObject);
                _activeSpellList.Remove(_activeSpellList[oldSpellIndexNumber]);
            }
            else _activeSpellList[_currentSpellIndex] = modifiedSpell;
        }
    }
    // end
}
