using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public Refs refs;

    private GameObject player;
    // public Player playerScript;
    // public GameObject train;
    // public GameObject[][] tiles;
    public Camera mainCam;
    // public GameObject[] structures;
    // public GameObject[] objects;


    void Awake()
    {
        refs.Initialize();
        player = GameObject.Instantiate(refs.player, new Vector3(10, 1, 5), Quaternion.identity);
        //  mainCam.player = player;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(refs.tiles.Length);
        mainCam.CamStart(player);
        for (int x = 0; x < 50; x++) {
            for (int z = 0; z < 10; z++) {
                int rand = Random.Range(0, refs.tiles.Length);
                GameObject newTile = GameObject.Instantiate(refs.tiles[rand], new Vector3(x, 0, z), Quaternion.identity);
            }
        } 


        // map = new GameObject[10][10];
    }

    // Update is called once per frame
    void Update()
    {
        mainCam.CamUpdate(player.transform.position);
    }
}
