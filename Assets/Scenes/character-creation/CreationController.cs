using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationController : MonoBehaviour
{
    [field: SerializeField] private SkinnedMeshRenderer head;
    [field: SerializeField] private SkinnedMeshRenderer[] _head_parts;
    [field: SerializeField] private SkinnedMeshRenderer hair;
    [field: SerializeField] private SkinnedMeshRenderer[] _hair_parts;
    [field: SerializeField] private SkinnedMeshRenderer horns;
    [field: SerializeField] private SkinnedMeshRenderer[] _horns_parts;
    [field: SerializeField] private SkinnedMeshRenderer top;
    [field: SerializeField] private SkinnedMeshRenderer[] top_parts;
    [field: SerializeField] private SkinnedMeshRenderer bottom;
    [field: SerializeField] private SkinnedMeshRenderer[] bottom_parts;
    [field: SerializeField] private SkinnedMeshRenderer hands;
    [field: SerializeField] private SkinnedMeshRenderer[] _hands_parts;
    // Start is called before the first frame update
    void Start()
    {
        // head = _head_parts[0];
        // hair = _hair_parts[0];
        // horns = _horns_parts[0];
        // top = GameObject.Find("Character").transform.Find("Model").Find("Top").GetComponent<SkinnedMeshRenderer>();
        top.sharedMesh = top_parts[1].sharedMesh;
        // hands = _hands_parts[0];
        bottom.sharedMesh = bottom_parts[1].sharedMesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
