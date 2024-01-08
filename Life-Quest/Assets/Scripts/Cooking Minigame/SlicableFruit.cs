using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicableFruit : MonoBehaviour
{
    [SerializeField] private GameObject unslicedObject;
    private GameObject slicedObject;
    private Rigidbody2D m_rigibody;
    private Collider2D m_collider;

    public delegate void TomatoSliced();
    public event TomatoSliced OnTomatoSliced;

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
        unslicedObject.SetActive(false);
        slicedObject.SetActive(true);

        m_collider.enabled = false;
        m_rigibody.bodyType = RigidbodyType2D.Dynamic;

        // Notify GameManager that the tomato is sliced
        OnTomatoSliced?.Invoke();
    }


}
