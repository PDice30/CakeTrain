using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called before the first frame update
    float upperBounds, lowerBounds, leftBounds, rigthBounds;
    public void CamStart(GameObject player)
    {
        upperBounds = 10;
        lowerBounds = -10;
        leftBounds = 0;
        rigthBounds = 10;
    }

    // Update is called once per frame
    public void CamUpdate(Vector3 playerPos)
    {
        transform.position = new Vector3(playerPos.x, 10, playerPos.z);
        if (transform.position.x < leftBounds) transform.position = new Vector3(leftBounds, 10, transform.position.z);
        if (transform.position.x > rigthBounds) transform.position = new Vector3(rigthBounds, 10, transform.position.z);
        if (transform.position.z < lowerBounds) transform.position = new Vector3(transform.position.x, 10, lowerBounds);
        if (transform.position.z > upperBounds) transform.position = new Vector3(transform.position.x, 10, upperBounds);
    }
}
