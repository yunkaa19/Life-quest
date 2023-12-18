using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicableFruit : MonoBehaviour
{
    [SerializeField] private GameObject unslicedObject;
    private GameObject slicedObject;
    private Rigidbody2D m_rigibody;
    private Collider2D m_collider;

    private Seed[] seeds;

    public delegate void TomatoSliced();
    public event TomatoSliced OnTomatoSliced;

    private void Awake(){
        m_rigibody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();

        // Assuming the seeds are direct children of a GameObject named "Seeds"
        seeds = transform.Find("Seeds").GetComponentsInChildren<Seed>(true);
    }

    public void SetSlicedObject(GameObject slicedObject){
        this.slicedObject = slicedObject;
    }
    public GameObject GetSlicedObject(){
        return slicedObject;
    }
    public void Slice(){
        unslicedObject.SetActive(false);
        slicedObject.SetActive(true);

        m_collider.enabled = false;
        // m_rigibody.bodyType = RigidbodyType2D.Dynamic;

        // Notify GameManager that the tomato is sliced
        OnTomatoSliced?.Invoke();


        foreach (var seed in seeds)
        {
            seed.AssignRandomFlavor();
            seed.gameObject.SetActive(true);
        }
        LogSeedFlavors();

    }

    private void LogSeedFlavors()
    {
        Dictionary<Seed.SeedFlavor, int> flavorCounts = new Dictionary<Seed.SeedFlavor, int>();

        foreach (var seed in seeds)
        {
            if (!flavorCounts.ContainsKey(seed.CurrentFlavor))
            {
                flavorCounts[seed.CurrentFlavor] = 0;
            }
            flavorCounts[seed.CurrentFlavor]++;
        }

        foreach (var flavor in flavorCounts)
        {
            Debug.Log($"{flavor.Key}: {flavor.Value}");
        }
    }

}
