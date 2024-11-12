using System.Collections.Generic;
using UnityEngine;

public class NPCAggressionManager : MonoBehaviour
{
    [field: SerializeField, Header("Aggression State")]
    public int _Aggression { get; private set; }
    [field: SerializeField]
    public List<Transform> _LastAggressors {  get; private set; }

    public void Initialize(NonPlayerCharacter npc)
    {
        _Aggression = 0;
        _LastAggressors = new List<Transform>();
    }

    public void AddAggression(int aggression, Transform aggressor)
    {
        _Aggression += aggression;
        if(!_LastAggressors.Contains(aggressor) && _LastAggressors.Count < 4)
        {
            _LastAggressors.Add(aggressor);
        }
    }

    //
}
