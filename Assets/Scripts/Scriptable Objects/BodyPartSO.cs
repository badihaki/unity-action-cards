using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Body Part", menuName ="Create Body Part")]
public class BodyPartSO : ScriptableObject
{
    public Mesh mesh;
    public Material material;
}
