using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public Refs refs;
    public MainCamera mainCam;

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

    void Awake()
    {
        refs.Initialize();
        player = GameObject.Instantiate(refs.player, new Vector3(10.0f,Consts.player_y,5.0f), Quaternion.identity);

        train = new List<GameObject>();
        train.Add(GameObject.Instantiate(refs.cartEngine, new Vector3(0.0f,Consts.train_y,0.0f), Quaternion.identity));
        train.Add(GameObject.Instantiate(refs.cartCraft,  new Vector3(0.0f,Consts.train_y,0.0f), Quaternion.identity));

        tiles = new GameObject[Consts.world_w,Consts.world_h];
        for (int x = 0; x < Consts.world_w; x++) {
            for (int z = 0; z < Consts.world_h; z++) {
                int tile_i = Random.Range(0, refs.tiles.Length);
                GameObject newTile = GameObject.Instantiate(refs.tiles[tile_i], new Vector3(x,Consts.tile_y,z), Quaternion.identity);
                tiles[x,z] = newTile;
            }
        } 

        int nStructures = Random.Range(100,200);
        for(int i = 0; i < nStructures; i++)
        {
            int structure_i = Random.Range(0,refs.structures.Length);
            Vector2 p = new Vector2(Random.Range(0,Consts.world_w),Random.Range(0,Consts.world_h));
            //for(int j = 0; j < i; j++)
                //if(structures[j].)
            GameObject newStructure = GameObject.Instantiate(refs.structures[structure_i], new Vector3(p.x,Consts.structure_y,p.y), Quaternion.identity);
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
