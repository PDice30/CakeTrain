using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [HideInInspector]
    public WorldManager worldManager;

    private float timeUntilReady;
    // Start is called before the first frame update
    float z_maxBounds, z_minBounds, x_minBounds, x_maxBounds;

    public void TransitionCamStart() {
        transform.position = new Vector3(Consts.world_w - 10, Consts.camera_y, transform.position.z);
        timeUntilReady = 3f;
    }

    public void TransitionCamUpdate(GameObject player) {
        transform.position = Vector3.Lerp(new Vector3(transform.position.x, Consts.camera_y, transform.position.z), new Vector3(player.transform.position.x, Consts.camera_y, player.transform.position.z), 0.005f);
        timeUntilReady -= Time.deltaTime;
        if (timeUntilReady <= 0) {
            worldManager.cameraIsReady = true;
        }
    }

    public void CamStart(GameObject player)
    {
        // X bounds determined by aspect ratio
        z_maxBounds = 5;
        z_minBounds = 5;
        x_minBounds = (float)gameObject.GetComponent<Camera>().aspect * 5.02811217517f; // Biggest determinier to be changed for aspect ratio
        x_maxBounds = Consts.world_w - 10; // TODO: determined be level length and passed from worldmanager, then - 9?
    }

    // Update is called once per frame
    public void CamUpdate(Vector3 playerPos)
    {
        transform.position = new Vector3(playerPos.x, Consts.camera_y, playerPos.z);
        if (transform.position.x < x_minBounds) transform.position = new Vector3(x_minBounds,          Consts.camera_y, transform.position.z);
        if (transform.position.x > x_maxBounds) transform.position = new Vector3(x_maxBounds,          Consts.camera_y, transform.position.z);
        if (transform.position.z < z_minBounds) transform.position = new Vector3(transform.position.x, Consts.camera_y, z_minBounds);
        if (transform.position.z > z_maxBounds) transform.position = new Vector3(transform.position.x, Consts.camera_y, z_maxBounds);
    }
}
