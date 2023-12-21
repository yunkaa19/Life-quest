using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public enum SeedFlavor { Bitter, Salty, Savory, Sour, Sweet }

    private SpriteRenderer spriteRenderer;
    public SeedFlavor CurrentFlavor { get; private set; }

    public bool IsBitter => CurrentFlavor == SeedFlavor.Bitter;
    private Rigidbody2D rb;

    // Assign the sprites for each flavor in the Unity Editor
    public Sprite bitterSprite;
    public Sprite saltySprite;
    public Sprite savorySprite;
    public Sprite sourSprite;
    public Sprite sweetSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.isKinematic = true; // Disable physics interactions
        }

        AssignRandomFlavor(); // Assign a random flavor when the seed awakes

        spriteRenderer.enabled = false; // Disable the sprite renderer initially
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().enabled = false; // Disable the collider initially
        }
    }
    
    public void EnableSeed()
    {
        spriteRenderer.enabled = true; // Enable the sprite renderer
        var collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true; // Enable the collider
        }
    }

    

    public void AssignRandomFlavor()
    {
        CurrentFlavor = (SeedFlavor)Random.Range(0, 5); // Set the flavor first

        switch (CurrentFlavor) // Use the CurrentFlavor for the switch
        {
            case SeedFlavor.Bitter:
                spriteRenderer.sprite = bitterSprite;
                break;
            case SeedFlavor.Salty:
                spriteRenderer.sprite = saltySprite;
                break;
            case SeedFlavor.Savory:
                spriteRenderer.sprite = savorySprite;
                break;
            case SeedFlavor.Sour:
                spriteRenderer.sprite = sourSprite;
                break;
            case SeedFlavor.Sweet:
                spriteRenderer.sprite = sweetSprite;
                break;

        }
    }

    public void CheckAndDropIfBitter()
    {
        if (IsBitter)
        {
            if (rb != null)
            {
                rb.isKinematic = false; // Enable physics interactions
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }



}
