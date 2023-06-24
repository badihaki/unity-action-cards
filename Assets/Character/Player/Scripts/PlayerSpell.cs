using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpell : MonoBehaviour
{
    [Serializable] public struct storedSpell
    {
        public SpellCardScriptableObj spell;
        public int chargesLeft;
    }

    private PlayerCharacter player;
    [SerializeField] private int maxSpellList = 6;
    [SerializeField] private List<storedSpell> activeSpellList;
    [SerializeField] private int currentSpellIndex;
    [SerializeField] private SpellCardScriptableObj baseSpell;

    [SerializeField] private LockSpellTargetRotation spellTarget;

    private float timeToAddToTimer;
    [SerializeField] private float spellTimer;

    public void Initialize(PlayerCharacter pl)
    {
        player = pl;
        activeSpellList = new List<storedSpell>();
        AddSpellToList(baseSpell);
        spellTarget = GetComponentInChildren<LockSpellTargetRotation>();
        spellTimer = 0.0f;
        timeToAddToTimer = activeSpellList[currentSpellIndex].spell._SpellAddonTime;
    }

    private void Update()
    {
        RunSpellTimer();
    }

    private void RunSpellTimer()
    {
        if (spellTimer <= 0) spellTimer = 0.0f;
        else
        {
            spellTimer -= Time.deltaTime;
        }
    }

    public void AddSpellToList(SpellCardScriptableObj spellCard)
    {
        if (activeSpellList.Count < maxSpellList)
        {
            storedSpell spell = new storedSpell();
            spell.spell = spellCard;
            spell.chargesLeft = spellCard._SpellCharges;
            activeSpellList.Add(spell);
        }
    }

    public void ChangeSpellIndex()
    {
        int maxIndex = activeSpellList.Count;

        currentSpellIndex++;
        if (currentSpellIndex > maxIndex)
        {
            currentSpellIndex = 0;
        }
        spellTimer = 0.0f;
        timeToAddToTimer = activeSpellList[currentSpellIndex].spell._SpellAddonTime;
    }

    public void UseSpell()
    {
        if(spellTimer <= 0)
        {
            Projectile conjuredSpell = Instantiate(activeSpellList[currentSpellIndex].spell._SpellProjectile, spellTarget.transform.position, spellTarget.targetRotation).GetComponent<Projectile>();
            conjuredSpell.name = activeSpellList[currentSpellIndex].spell._CardName;
            conjuredSpell.InitializeProjectile(player, activeSpellList[currentSpellIndex].spell._SpellDamage, activeSpellList[currentSpellIndex].spell._SpellProjectileSpeed);
            spellTimer = timeToAddToTimer;
        
            // remove a charge
            if (currentSpellIndex != 0)
            {
                storedSpell modifiedSpell = activeSpellList[currentSpellIndex];
                modifiedSpell.chargesLeft -= 1;
                if (modifiedSpell.chargesLeft <= 0)
                {
                    int oldSpellIndexNumber = currentSpellIndex;
                    ChangeSpellIndex();
                    activeSpellList.Remove(activeSpellList[oldSpellIndexNumber]);
                }
                else activeSpellList[currentSpellIndex] = modifiedSpell;
            }
        }
    }

    // end
}
