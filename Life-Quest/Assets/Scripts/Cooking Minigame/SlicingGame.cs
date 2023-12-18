using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicingGame : MonoBehaviour
{

    // [SerializeField] private SlicableFruit tomatoPrefab;
    // [SerializeField] private Transform spawnPoint;
    // private int tomatoesSliced = 0;
    // private bool canSpawn = true;

    // void Start()
    // {
    //     // Start the initial timer
    //     StartCoroutine(SpawnTomatoesRoutine());
    // }

    // IEnumerator SpawnTomatoesRoutine()
    // {
    //     while (tomatoesSliced < 4)
    //     {
    //         yield return new WaitForSeconds(2f);

    //         // Check if the previous tomato has been sliced
    //         if (!canSpawn) continue;

    //         SpawnTomato();
    //     }
    // }

    // void SpawnTomato()
    // {
    //     SlicableFruit newTomato = Instantiate(tomatoPrefab, spawnPoint.position, Quaternion.identity);
    //     newTomato.OnTomatoSliced += HandleTomatoSliced;
    //     canSpawn = false; // Set to false immediately to prevent immediate spawning
    // }

    // void HandleTomatoSliced()
    // {
    //     tomatoesSliced++;
    //     Debug.Log("Tomato sliced! Total sliced: " + tomatoesSliced);

    //     if (tomatoesSliced < 4)
    //     {
    //         // Set canSpawn to true so that the next tomato can be spawned
    //         canSpawn = true;
    //         // Start the timer for the next tomato spawn
    //         StartCoroutine(SpawnTomatoesRoutine());
    //     }
    //     else
    //     {
    //         Debug.Log("All tomatoes sliced!");
    //     }
    // }

    


}
