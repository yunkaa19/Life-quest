using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeedSelector : MonoBehaviour
{


    public GameObject text2;
    public GameObject text3;
    public GameObject text4;
    public GameObject text5;
    public GameObject text6;

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
            text2.SetActive(false);
            text3.SetActive(true);
        }
        if (CheckSeeds(saltySeeds))
        {
            Seed.EnableDrop(Seed.SeedFlavor.Savory); // Allow savory seeds to drop
            text3.SetActive(false);
            text4.SetActive(true);
        }
        if (CheckSeeds(savorySeeds))
        {
            Seed.EnableDrop(Seed.SeedFlavor.Sour); // Allow sour seeds to drop
            text4.SetActive(false);
            text5.SetActive(true);
        }
        if (CheckSeeds(sourSeeds))
        {
            Seed.EnableDrop(Seed.SeedFlavor.Sweet); // Allow sweet seeds to drop
            text5.SetActive(false);
            text6.SetActive(true);
        }
        if (CheckSeeds(sweetSeeds))
        {
            Debug.Log("Completed"); // All seeds have been processed

            int minigamesPlayed = PlayerPrefs.GetInt("MinigamesPlayed",0);
            minigamesPlayed++;
            PlayerPrefs.SetInt("MinigamesPlayer",minigamesPlayed);
            SceneManager.LoadScene("CompletionScreen");

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
