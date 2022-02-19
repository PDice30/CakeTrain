using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  
  void Start()
  {
    
      // map = new   GameObject[10][10];
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKey(KeyCode.W)) {
      transform.position = transform.position + new Vector3(0, 0, 0.05f);
    }
    if (Input.GetKey(KeyCode.S)) {
      transform.position = transform.position + new Vector3(0, 0, -0.05f);
    }
    if (Input.GetKey(KeyCode.A)) {
      transform.position = transform.position + new Vector3(-0.05f, 0, 0);
    }
    if (Input.GetKey(KeyCode.D)) {
      transform.position = transform.position + new Vector3(0.05f, 0, 0);
    }
      
  }
}
