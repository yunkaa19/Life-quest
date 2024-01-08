using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSelector : MonoBehaviour
{

    private List<Seed> bitterSeeds = new List<Seed>();
    private List<Seed> saltySeeds = new List<Seed>();
    private List<Seed> savorySeeds = new List<Seed>();
    private List<Seed> sourSeeds = new List<Seed>();
    private List<Seed> sweetSeeds = new List<Seed>();



private void Start()
{

    
        Seed[] seeds = FindObjectsOfType<Seed>();

        foreach (var seed in seeds)
        {
            if (seed.IsBitter) bitterSeeds.Add(seed);
            if (seed.IsSalty) saltySeeds.Add(seed);
            if (seed.IsSavory) savorySeeds.Add(seed);
            if (seed.IsSour) sourSeeds.Add(seed);
            if (seed.IsSweet) sweetSeeds.Add(seed);
        }

}

    private void Update()
    {


        if (CheckSeeds(bitterSeeds))
        {
            Seed.EnableDrop(Seed.SeedFlavor.Salty); // Allow salty seeds to drop
        }
        if (CheckSeeds(saltySeeds))
        {
            Seed.EnableDrop(Seed.SeedFlavor.Savory); // Allow savory seeds to drop
        }
        if (CheckSeeds(savorySeeds))
        {
            Seed.EnableDrop(Seed.SeedFlavor.Sour); // Allow sour seeds to drop
        }
        if (CheckSeeds(sourSeeds))
        {
            Seed.EnableDrop(Seed.SeedFlavor.Sweet); // Allow sweet seeds to drop
        }
        if (CheckSeeds(sweetSeeds))
        {
            Debug.Log("Completed"); // All seeds have been processed
        }



    }

        private bool CheckSeeds(List<Seed> seeds)
    {
        int seedsClicked = 0;

        foreach (var seed in seeds)
        {
            if (seed.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
            {
                seedsClicked++;
            }
        }

        return seedsClicked >= seeds.Count;
    }




}
