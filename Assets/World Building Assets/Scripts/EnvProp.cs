using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// This is the base class for all props in the game's environment
/// </summary>
public class EnvProp : MonoBehaviour
{
    [field: SerializeField]
    public PropScriptableObj propTemplate { get; private set; }
    public Health _Health { get; protected set; }
    public Rigidbody _Rigidbody { get; protected set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _Health = transform.AddComponent<Health>();
        _Health.InitiateHealth(propTemplate.health);
        _Rigidbody = transform.AddComponent<Rigidbody>();
        _Rigidbody.isKinematic = true;
    }
}
