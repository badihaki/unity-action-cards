using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour
{
    private Character _Character;
    private AudioSource _AudioController;

    [SerializeField] private List<AudioClip> InteractionClips;
    [SerializeField] private List<AudioClip> HurtClips;
    [SerializeField] private List<AudioClip> HardHitClips;

    private void OnEnable()
    {
        if (_Character != null) _Character._Health.OnHit += PlayHurtClip;
    }
    private void OnDisable()
    {
        _Character._Health.OnHit -= PlayHurtClip;
    }

    public void InitializeSoundManager(Character character)
    {
        // for the character
        _Character = character;
        _Character._Health.OnHit += PlayHurtClip;

        InteractionClips = new List<AudioClip>();
        _Character._CharacterSheet._HurtClips.ForEach(clip => HurtClips.Add(clip));
        _Character._CharacterSheet._HardHitClips.ForEach(clip => HardHitClips.Add(clip));

        // for the audiosource
        _AudioController = gameObject.AddComponent<AudioSource>();
        // TODO: we'd set up the volume here by asking the Game Manager how loud voices should be.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHurtClip(string hitType)
    {
        if (_AudioController)
        {
            switch (hitType)
            {
                case "hit":
                    if (HurtClips.Count == 0) break;
                    int hitClipIndex = UnityEngine.Random.Range(0, HurtClips.Count);
                    print("play hit sound: sound-" + hitClipIndex);
                    _AudioController.PlayOneShot(HurtClips[hitClipIndex]);
                    break;
                case "knockBack":
                    if (HardHitClips.Count == 0) break;
                    int knockBackClipIndex = UnityEngine.Random.Range(0, HurtClips.Count);
                    _AudioController.PlayOneShot(HardHitClips[knockBackClipIndex]);
                    print("play knockback sound-" + knockBackClipIndex);
                    break;
            }
        }
    }
}
