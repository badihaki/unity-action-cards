using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAggressionManager : MonoBehaviour
{
    private NonPlayerCharacter _NPC;
    [field: SerializeField, Header("Aggression State")]
    public int _Aggression { get; private set; }
    [field: SerializeField]
    public List<Transform> _LastAggressors {  get; private set; }
    [field: SerializeField]
    public bool isAggressive { get; private set; }

    // waits
    private WaitForSeconds slowWait = new WaitForSeconds(1.35f);
    private WaitForSeconds fastWait = new WaitForSeconds(0.35f);

    // events
    public delegate void OnAggressed();
    public event OnAggressed IsAggressed;

	public void Initialize(NonPlayerCharacter npc)
    {
        _NPC = npc;
        _Aggression = 0;
        _LastAggressors = new List<Transform>();
    }

    public void AddAggression(int aggression, Transform aggressor)
    {
        _Aggression += aggression;
        
        if(!_LastAggressors.Contains(aggressor) && _LastAggressors.Count < 4)
        {
            if (GameManagerMaster.GameMaster.GMSettings.logExtraNPCData)
                print(">>>>>>> adding aggressor <<");
            _LastAggressors.Add(aggressor);
        }

        if (_Aggression >= 50 && !isAggressive)
        {
            isAggressive = true;
			_NPC._NPCActor.animationController.SetBool("aggressive", true);
            //print("aggressed in animator <<<<<<<<<<<<<<");
			IsAggressed();
			StartCoroutine(SlowlyLowerAggression());
        }
        if (_Aggression > 100) _Aggression = 100;
    }

    private IEnumerator SlowlyLowerAggression()
    {
        while(_Aggression >= 35)
        {
            yield return slowWait;
            _Aggression -= 1;
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
            if (GameManagerMaster.GameMaster.GMSettings.logExraPlayerData)
                print("quickly lose aggression");
			if (_Aggression == 0)
            {
                // lost all aggression here
                _LastAggressors.Clear();
                isAggressive = false;
                _NPC._NPCActor.animationController.SetBool("aggressive", false);
                _NPC._StateMachine.ChangeState(_NPC._StateMachine._IdleState);
            }
            else
                yield return null;
        }
    }

    //
}
