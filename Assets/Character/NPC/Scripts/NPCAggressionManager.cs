using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAggressionManager : MonoBehaviour
{
    [field: SerializeField, Header("Aggression State")]
    public int _Aggression { get; private set; }
    [field: SerializeField]
    public List<Transform> _LastAggressors {  get; private set; }
    [field: SerializeField]
    private bool isAggressive;

    // waits
    private WaitForSeconds slowWait = new WaitForSeconds(1.35f);
    private WaitForSeconds fastWait = new WaitForSeconds(0.35f);

    // events
    public delegate void OnAggressed();
    public event OnAggressed IsAggressed;

	public void Initialize(NonPlayerCharacter npc)
    {
        _Aggression = 0;
        _LastAggressors = new List<Transform>();
    }

    public void AddAggression(int aggression, Transform aggressor)
    {
        _Aggression += aggression;
        if (_Aggression >= 50 && !isAggressive)
        {
            isAggressive = true;
            IsAggressed();
			StartCoroutine(SlowlyLowerAggression());
        }
        if (_Aggression > 100) _Aggression = 100;

        if(!_LastAggressors.Contains(aggressor) && _LastAggressors.Count < 4)
        {
            _LastAggressors.Add(aggressor);
        }
    }

    private IEnumerator SlowlyLowerAggression()
    {
        while(_Aggression >= 35)
        {
            yield return slowWait;
            _Aggression -= 1;
            print("slowly lose aggression");
            yield return null;
        }
        StartCoroutine(QuicklyLowerAggression());
    }
    private IEnumerator QuicklyLowerAggression()
    {
        while (isAggressive)
        {
            yield return fastWait;
            _Aggression -= 1;
			print("quickly lose aggression");
			if (_Aggression == 0)
            {
                _LastAggressors.Clear();
                isAggressive = false;
            }
            else
                yield return null;
        }
    }

    //
}