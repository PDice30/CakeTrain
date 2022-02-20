using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  float z_maxBounds, z_minBounds, x_minBounds, x_maxBounds;
  void Start()
  {
    z_maxBounds = 9; // Max Z bound of field view - 1 to account for size of player
    z_minBounds = 0;
    x_minBounds = 0;
    x_maxBounds = 49; // TODO: determined be level length and passed from worldmanager.
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

    if (transform.position.x < x_minBounds) transform.position = new Vector3(x_minBounds, 1, transform.position.z);
    if (transform.position.x > x_maxBounds) transform.position = new Vector3(x_maxBounds, 1, transform.position.z);
    if (transform.position.z < z_minBounds) transform.position = new Vector3(transform.position.x, 1, z_minBounds);
    if (transform.position.z > z_maxBounds) transform.position = new Vector3(transform.position.x, 1, z_maxBounds);
      
  }
}
