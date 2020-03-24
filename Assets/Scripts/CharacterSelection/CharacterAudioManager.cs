using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClips[] clips;
    [SerializeField] PlayerBox[] playerBox;
    [SerializeField] CharacterBox[] characterBox;

    private void OnEnable()
    {
        playerBox[0].OnSelectAudio += OnSelect;
        playerBox[1].OnSelectAudio += OnSelect;
        playerBox[2].OnSelectAudio += OnSelect;
        playerBox[3].OnSelectAudio += OnSelect;
        characterBox[0].OnDeniedAudio += OnDeniedAudio;
        characterBox[1].OnDeniedAudio += OnDeniedAudio;
        characterBox[2].OnDeniedAudio += OnDeniedAudio;
        characterBox[3].OnDeniedAudio += OnDeniedAudio;
        characterBox[0].OnSelectAudio += OnSelect;
        characterBox[1].OnSelectAudio += OnSelect;
        characterBox[2].OnSelectAudio += OnSelect;
        characterBox[3].OnSelectAudio += OnSelect;
        characterBox[0].OnHoverAudio += OnHoverAudio;
        characterBox[1].OnHoverAudio += OnHoverAudio;
        characterBox[2].OnHoverAudio += OnHoverAudio;
        characterBox[3].OnHoverAudio += OnHoverAudio;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnSelect()
    {
        audioSource.PlayOneShot(clips[0].clip);
    }

    void OnDeniedAudio()
    {
        audioSource.PlayOneShot(clips[1].clip);
    }

    void OnHoverAudio()
    {
        audioSource.PlayOneShot(clips[2].clip);
    }
}
