using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public enum SeedFlavor { Bitter, Salty, Savory, Sour, Sweet }

    private SpriteRenderer spriteRenderer;
    public SeedFlavor CurrentFlavor { get; private set; }

    public bool IsBitter => CurrentFlavor == SeedFlavor.Bitter;
    public bool IsSalty => CurrentFlavor == SeedFlavor.Salty;
    public bool IsSavory => CurrentFlavor == SeedFlavor.Savory;
    public bool IsSour => CurrentFlavor == SeedFlavor.Sour;
    public bool IsSweet => CurrentFlavor == SeedFlavor.Sweet;


    private static Dictionary<SeedFlavor, bool> dropConditions = new Dictionary<SeedFlavor, bool>()
    {
        { SeedFlavor.Bitter, true },
        { SeedFlavor.Salty, false },
        { SeedFlavor.Savory, false },
        { SeedFlavor.Sour, false },
        { SeedFlavor.Sweet, false }
    };


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


    static public void EnableDrop(SeedFlavor flavor)
    {
        if (dropConditions.ContainsKey(flavor))
        {
            dropConditions[flavor] = true;
        }
    }

    // static public void SaltyDrop(){
    // canSaltyDrop = true;
    // }
    // static public void SavoryDrop(){
    // canSavoryDrop = true;
    // }
    // static public void SourDrop(){
    // canSourDrop = true;
    // }
    // static public void SweetDrop(){
    // canSweetDrop = true;
    // }

    public bool CanDrop()
    {
        return dropConditions.ContainsKey(CurrentFlavor) && dropConditions[CurrentFlavor];

        // if (IsBitter) return true; // Bitter seeds can always drop
        // if (IsSalty && canSaltyDrop) return true; // Salty seeds drop if flag is set
        // if (IsSavory && canSavoryDrop) return true; // Salty seeds drop if flag is set
        // if (IsSour && canSourDrop) return true; // Salty seeds drop if flag is set
        // if (IsSweet && canSweetDrop) return true; // Salty seeds drop if flag is set
        // return false; // Other seeds cannot drop
    }





    public void CheckAndDrop()
    {
        if (CanDrop() && rb != null)
        {
            rb.isKinematic = false; // Enable physics interactions
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }


}
