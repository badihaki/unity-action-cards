using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpell : MonoBehaviour
{
    private PlayerCharacter player;
    [SerializeField] private int maxSpellList = 6;
    [Serializable] public struct storedSpell
    {
        public SpellCardScriptableObj spell;
        public int chargesLeft;
    }
    public List<storedSpell> activeSpellList;
    [SerializeField] private SpellCardScriptableObj baseSpell;

    public void Initialize(PlayerCharacter pl)
    {
        player = pl;
        activeSpellList = new List<storedSpell>();
        AddSpellToList(baseSpell);
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

    // end
}
