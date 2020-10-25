using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeCubes : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool CollisionSet;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Cube" && !CollisionSet)
        {
            for(int i = collision.transform.childCount -1; i >= 0; i--)
            {
                Transform Child = collision.transform.GetChild(i);
                Child.gameObject.AddComponent<Rigidbody>();
                Child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(70f, Vector3.up, 5f);
                Child.SetParent(null);
            }
            Destroy(collision.gameObject);
            CollisionSet = true;
        }
    }
}
