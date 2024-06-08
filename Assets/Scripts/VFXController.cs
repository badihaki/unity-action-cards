using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [field: SerializeField] public float lifetime { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        if (lifetime <= 0) lifetime = 1.5f;
        StartCoroutine("Death");
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(lifetime);
        print("done waiting, deleting " + name + " now");
        Destroy(gameObject);
    }
}
