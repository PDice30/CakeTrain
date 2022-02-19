using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    // public GameObject player;
    // public Player playerScript;
    // public GameObject train;
    // public GameObject[][] tiles;
    public GameObject[] tileLUT;
    // public GameObject[] structures;
    // public GameObject[] objects;


    // Start is called before the first frame update
    void Start()
    {
        // playerScript = player.GetComponent<Player>();
        for (int x = 0; x < 100; x++) {
            for (int z = 0; z < 10; z++) {
                int rand = Random.Range(0, tileLUT.Length);
                GameObject newTile = GameObject.Instantiate(tileLUT[rand], new Vector3(x, 0, z), Quaternion.Euler(90, 0, 0));
            }
        } 


        // map = new GameObject[10][10];
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
