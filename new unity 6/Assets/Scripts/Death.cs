using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {
    
    Animator animator;
    AudioSource audioSource;
    [SerializeField] private AudioSource soundtrack;
    [SerializeField] private GameObject restartMenu;
    [SerializeField] private AudioClip clip;
    private Score scoreManager;  // Reference to the Score script

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        // Find the Score script in the scene and assign it
        scoreManager = FindObjectOfType<Score>();
    }

    public void Die() {
        animator.Play("Death");
        soundtrack.mute = true;
        audioSource.PlayOneShot(clip);
        restartMenu.SetActive(true);
       
        // Using constraints instead of stopping time, so the animation continues
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        // Stop the score from updating
        if (scoreManager != null) {
            scoreManager.StopScore();
        }
    }
}
