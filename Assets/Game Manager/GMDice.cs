using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMDice : MonoBehaviour
{
    public int RollD4() => Random.Range(1, 4);
    public int RollD6() => Random.Range(1, 6);
    public int RollD10() => Random.Range(1, 10);
    public int RollD20() => Random.Range(1, 20);
    public int RollD100()
    {
        float result = Random.Range(0.01f, 1.0f);
        return (int)result * 100;
    }
    public float RollRandomDice(float min, float max) => Random.Range(min, max);
}
