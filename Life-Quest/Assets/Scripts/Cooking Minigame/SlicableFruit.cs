using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicableFruit : MonoBehaviour
{
    [SerializeField] private GameObject unslicedObject;
    private GameObject slicedObject;
    private Rigidbody2D m_rigibody;
    private Collider2D m_collider;

    private void Awake(){
        m_rigibody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();
    }

    public void SetSlicedObject(GameObject slicedObject){
        this.slicedObject = slicedObject;
    }
    public GameObject GetSlicedObject(){
        return slicedObject;
    }
    public void Slice(){
        Debug.Log("Sliced");
        unslicedObject.SetActive(false);
        slicedObject.SetActive(true);

        m_collider.enabled = false;
        m_rigibody.bodyType = RigidbodyType2D.Dynamic;
    }


}
