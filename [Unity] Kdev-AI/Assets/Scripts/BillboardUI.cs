using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardUI : MonoBehaviour {
    [SerializeField]
    private Camera uiCamera;

    void Update(){
        transform.LookAt(transform.position + uiCamera.transform.rotation * Vector3.forward, uiCamera.transform.rotation * Vector3.up);
    }
}
