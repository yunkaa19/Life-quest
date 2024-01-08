using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTreasure : MonoBehaviour
{
    public Transform target;
    public float distanceToTarget;
    public int distanceState = 0;
    public int distanceAsInt;
    float cooldown;
    int isPreviousInt;


    IEnumerator vibrateTwice()
    {
        Handheld.Vibrate();
        yield return new WaitForSeconds(1);
        Handheld.Vibrate();
    }
    void Update()
    {
        if(cooldown < 4)
        {
            cooldown += Time.deltaTime;
        }
        
        
        
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        distanceAsInt = (int) distanceToTarget;
        if(isPreviousInt != distanceAsInt && cooldown > 3)
        {
            if(isPreviousInt > distanceAsInt)
            {
                StartCoroutine(vibrateTwice());
            }

            if(isPreviousInt < distanceAsInt)
            {
                Handheld.Vibrate();
            }
            isPreviousInt = distanceAsInt;
            cooldown = 0;
        }








    }
}
