using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    [SerializeField]
    private float movespeed = 10f;

    void Update(){
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) {
            Movement();
        }
    }

    private void Movement() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3 (x, 0, z);

        transform.Translate (direction.normalized * movespeed * Time.deltaTime);
	}
}
