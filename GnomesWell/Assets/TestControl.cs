using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControl : MonoBehaviour
{
    public Rigidbody2D gnome;
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputForce = Input.GetAxis("Horizontal") * force;

        gnome.AddForce(new Vector2(inputForce, 0));


    }
}
