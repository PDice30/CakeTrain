using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [HideInInspector]
    public WorldManager worldManager;
    public float transitionTime;

    // Start is called before the first frame update
    float z_maxBounds, z_minBounds, x_minBounds, x_maxBounds;

    public void TransitionCamStart() {
        // X bounds determined by aspect ratio
        z_maxBounds = 5;
        z_minBounds = 5;
        x_minBounds = (float)gameObject.GetComponent<Camera>().aspect * 5.02811217517f; // Biggest determinier to be changed for aspect ratio
        x_maxBounds = Consts.world_w - 10; // TODO: determined be level length and passed from worldmanager, then - 9?
        transform.position = new Vector3(Consts.world_w - 10, Consts.camera_y, transform.position.z);
        transitionTime = 2.0f;
    }

    public void TransitionCamUpdate() {
        Vector3 prev = new Vector3(transform.position.x, Consts.camera_y, transform.position.z);
        Vector3 target = new Vector3(worldManager.player.transform.position.x, Consts.camera_y, worldManager.player.transform.position.z);
        Vector3 cur = Vector3.Lerp(prev, target, 0.01f);
        transform.position = cur;
        transitionTime -= Time.deltaTime;
        if(transitionTime <= 0.0f || (cur-target).sqrMagnitude < 0.0001)
        {
            transitionTime = 0.0f;
            worldManager.cameraIsReady = true;
        }
        clampCam();
    }

    public void CamStart()
    {
    }

    // Update is called once per frame
    public void CamUpdate()
    {
        transform.position = worldManager.player.transform.position;
        clampCam();
    }

    public void clampCam()
    {
        if (transform.position.x < x_minBounds) transform.position = new Vector3(x_minBounds,          Consts.camera_y, transform.position.z);
        if (transform.position.x > x_maxBounds) transform.position = new Vector3(x_maxBounds,          Consts.camera_y, transform.position.z);
        if (transform.position.z < z_minBounds) transform.position = new Vector3(transform.position.x, Consts.camera_y, z_minBounds);
        if (transform.position.z > z_maxBounds) transform.position = new Vector3(transform.position.x, Consts.camera_y, z_maxBounds);
    }
}
