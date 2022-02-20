using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called before the first frame update
    float z_maxBounds, z_minBounds, x_minBounds, x_maxBounds;
    public void CamStart(GameObject player)
    {
        z_maxBounds = 5;
        z_minBounds = 5;
        x_minBounds = 10;
        x_maxBounds = 40; // TODO: determined be level length and passed from worldmanager, then - 10
    }

    // Update is called once per frame
    public void CamUpdate(Vector3 playerPos)
    {
        transform.position = new Vector3(playerPos.x, 10, playerPos.z);
        if (transform.position.x < x_minBounds) transform.position = new Vector3(x_minBounds, 10, transform.position.z);
        if (transform.position.x > x_maxBounds) transform.position = new Vector3(x_maxBounds, 10, transform.position.z);
        if (transform.position.z < z_minBounds) transform.position = new Vector3(transform.position.x, 10, z_minBounds);
        if (transform.position.z > z_maxBounds) transform.position = new Vector3(transform.position.x, 10, z_maxBounds);
    }
}
