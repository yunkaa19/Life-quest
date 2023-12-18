using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public enum SeedFlavor { Bitter, Salty, Savory, Sour, Sweet }

    private SpriteRenderer spriteRenderer;
    public SeedFlavor CurrentFlavor { get; private set; }

    // Assign the sprites for each flavor in the Unity Editor
    public Sprite bitterSprite;
    public Sprite saltySprite;
    public Sprite savorySprite;
    public Sprite sourSprite;
    public Sprite sweetSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

}
