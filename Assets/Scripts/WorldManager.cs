using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public Refs refs;
    public Camera mainCam;

    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public List<GameObject> train;
    [HideInInspector]
    public GameObject[,] tiles;
    [HideInInspector]
    public List<GameObject> structures;
    [HideInInspector]
    public List<GameObject> objects;


    const int world_h = 10;
    const int world_w = 100;

    const float tile_y = 1.0f;
    const float structure_y = 2.0f;
    const float player_y = 3.0f;
    const float train_y = 4.0f;

    void Awake()
    {
        refs.Initialize();
        player = GameObject.Instantiate(refs.player, new Vector3(0.0f,player_y,0.0f), Quaternion.identity);

        train = new List<GameObject>();
        train.Add(GameObject.Instantiate(refs.cartEngine, new Vector3(0.0f,train_y,0.0f), Quaternion.identity));
        train.Add(GameObject.Instantiate(refs.cartCraft,  new Vector3(0.0f,train_y,0.0f), Quaternion.identity));

        tiles = new GameObject[world_w,world_h];
        for (int x = 0; x < world_w; x++) {
            for (int z = 0; z < world_h; z++) {
                int tile_i = Random.Range(0, refs.tiles.Length);
                GameObject newTile = GameObject.Instantiate(refs.tiles[tile_i], new Vector3(x,tile_y,z), Quaternion.identity);
                tiles[x,z] = newTile;
            }
        } 

        int nStructures = Random.Range(100,200);
        for(int i = 0; i < nStructures; i++)
        {
            int structure_i = Random.Range(0,refs.structures.Length);
            Vector2 p = new Vector2(Random.Range(0,world_w),Random.Range(0,world_h));
            //for(int j = 0; j < i; j++)
                //if(structures[j].)
            GameObject newStructure = GameObject.Instantiate(refs.structures[structure_i], new Vector3(p.x,structure_y,p.y), Quaternion.identity);
            structures.Add(newStructure);
        }

        mainCam.CamStart(player);
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mainCam.CamUpdate(player.transform.position);
    }
}
