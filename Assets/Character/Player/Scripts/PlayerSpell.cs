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

    [SerializeField] private Transform spellTarget;

    public void Initialize(PlayerCharacter pl)
    {
        player = pl;
        activeSpellList = new List<storedSpell>();
        AddSpellToList(baseSpell);
        spellTarget = player.transform.Find("SpellTarget");
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
    }

    public void UseSpell()
    {
        Projectile conjuredSpell = Instantiate(activeSpellList[currentSpellIndex].spell._SpellProjectile, spellTarget.position, transform.rotation).GetComponent<Projectile>();
        conjuredSpell.name = activeSpellList[currentSpellIndex].spell._CardName;
        conjuredSpell.InitializeProjectile(player, activeSpellList[currentSpellIndex].spell._SpellDamage, activeSpellList[currentSpellIndex].spell._SpellSpeed);
    }

    // end
}
