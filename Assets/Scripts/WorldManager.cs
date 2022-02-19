using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject playerRef;
    private GameObject player;
    // public Player playerScript;
    // public GameObject train;
    // public GameObject[][] tiles;
    public GameObject[] tileRef;
    public Camera mainCam = Camera.main;
    // public GameObject[] structures;
    // public GameObject[] objects;


    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Instantiate(playerRef, new Vector3(0, 1, 0), Quaternion.identity);
        
        for (int x = 0; x < 100; x++) {
            for (int z = 0; z < 10; z++) {
                int rand = Random.Range(0, tileRef.Length);
                GameObject newTile = GameObject.Instantiate(tileRef[rand], new Vector3(x, 0, z), Quaternion.identity);
            }
        } 


        // map = new GameObject[10][10];
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
