using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RDG;

public class GrowCircle : MonoBehaviour
{
    public Vector3 scaleChange;
    public float distanceToPlayer;
    float growthSpeed;

    void Start()
    {
        scaleChange = new Vector3(1.3f, 1.3f, 0f);
    }

    // Update is called once per frame
    void Update()
    {     
        distanceToPlayer = Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position);

        growthSpeed = 0.1f * (float)distanceToPlayer;

        this.transform.localScale += scaleChange * Time.deltaTime * growthSpeed;

        if(this.transform.localScale.x > 30f)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided");
        if(collision.gameObject.tag == "Player")
        {
            Handheld.Vibrate();
        }
    }
}
