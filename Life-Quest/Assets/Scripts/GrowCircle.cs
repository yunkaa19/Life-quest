using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowCircle : MonoBehaviour
{
    public Vector3 scaleChange;

    void Start()
    {
        scaleChange = new Vector3(1.3f, 1.3f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale += scaleChange * Time.deltaTime;

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
            Debug.Log("Play hit");
            Handheld.Vibrate();
        }
    }
}
