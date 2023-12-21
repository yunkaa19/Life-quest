using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSelector : MonoBehaviour
{
    private List<Seed> bitterSeeds = new List<Seed>();
    private int totalBitterSeeds = 0;
    private int bitterSeedsClicked = 0;

    

private void Start()
{
    Seed[] seeds = FindObjectsOfType<Seed>();

    foreach (var seed in seeds)
    {
        Debug.Log("Seed flavor: " + seed.CurrentFlavor); // Debugging line
        if (seed.IsBitter)
        {
            bitterSeeds.Add(seed);
            totalBitterSeeds++;
        }
    }

    Debug.Log("Total bitter seeds: " + totalBitterSeeds); // Debugging line
}

    private void Update()
    {
        bitterSeedsClicked = 0; // Reset the count each frame

        foreach (var seed in bitterSeeds)
        {
            // Check if the bitter seed has a Dynamic Rigidbody
            if (seed.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
            {
                bitterSeedsClicked++;
                Debug.Log("Bitter seed clicked: " + bitterSeedsClicked);
            }
        }

        // Check if all bitter seeds have been pressed
        if (bitterSeedsClicked >= totalBitterSeeds)
        {
            Debug.Log("All bitter seeds have been pressed.");
            // Continue with the next seed type...
        }
    }


}
