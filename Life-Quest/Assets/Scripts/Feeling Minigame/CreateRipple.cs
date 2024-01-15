using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRipple : MonoBehaviour
{
    
    
    float timerSpawnRipple = 2f;
    public GameObject Ripple;
    void Update()
    {
        timerSpawnRipple += Time.deltaTime;

        if(timerSpawnRipple >= 3.5f)
        {
            timerSpawnRipple = 0;
            Instantiate(Ripple, this.transform.position, Quaternion.identity);
        }
    }
}
