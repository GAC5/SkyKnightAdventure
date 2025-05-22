using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hitSFX;
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip hurtSFX;
    [SerializeField] AudioClip landSFX;
    [SerializeField] AudioClip powerupSFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip selectSFX;

    public void PlaySFX(string clipToPlay)
    {
        switch (clipToPlay)
        {
            case "Hit":
                audioSource.clip = hitSFX;
                break;
            case "Jump":
                audioSource.clip = jumpSFX;
                break;
            case "Hurt":
                audioSource.clip = hurtSFX;
                break;
            case "Land":
                audioSource.clip = landSFX;
                break;
            case "Powerup":
                audioSource.clip = powerupSFX;
                break;
            case "Death":
                audioSource.clip = deathSFX;
                break;
            case "Select":
                audioSource.clip = selectSFX;
                break;
        }
        audioSource.Play();
    }
}
