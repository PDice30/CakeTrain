using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
    public Refs refs;
    public MainCamera mainCam;
    public Canvas canvas;
    public Text timeUntilNightText;
    public Text gameOverText;
    public Button restartButton;
    

    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public GameObject cartCrafting;
    [HideInInspector]
    public GameObject cartEngine;
    [HideInInspector]
    public List<GameObject> carts;
    [HideInInspector]
    public List<GameObject> tracks;
    public List<GameObject> track; //singular connected track
    [HideInInspector]
    public GameObject[,] tiles;
    [HideInInspector]
    public List<GameObject> structures;
    [HideInInspector]
    public List<GameObject> objects;
    [HideInInspector]
    public List<GameObject> products;
    [HideInInspector]
    public List<GameObject> enemies;
    [HideInInspector]
    public List<GameObject> turrets;
    [HideInInspector]
    public List<GameObject> bullets;

    [HideInInspector]
    public GameObject tileHighlight_Passive;
    [HideInInspector]
    public GameObject structureHighlight_Passive;
    [HideInInspector]
    public GameObject structureHighlight_Active;

    [HideInInspector]
    public GameObject objectHighlight_Passive;
    [HideInInspector]
    public GameObject productHighlight_Passive;
    [HideInInspector]
    public GameObject cartHighlight_Passive;
    [HideInInspector]
    public GameObject enemyHighlight_Passive;

    [HideInInspector]
    public List<GameObject> craftPreviews_BG;
    [HideInInspector]
    public List<GameObject> craftPreviews_Eject;
    [HideInInspector]
    public List<GameObject> craftPreviews_Submit;
    [HideInInspector]
    public GameObject craftPreviewTail;

    [HideInInspector]
    public List<GameObject> objectPreviews;
    [HideInInspector]
    public List<GameObject> objectPreviews_Wood;
    [HideInInspector]
    public List<GameObject> objectPreviews_Iron;

    public int nights;
    [HideInInspector]
    public bool isNight;
    [HideInInspector]
    public float timeUntilNight;
    [HideInInspector]
	public int trainProgress;
    [HideInInspector]
	public float trainIncProgress;
    [HideInInspector]
    public bool enemiesHaveBeenSpawned;
    [HideInInspector]
    public bool cameraIsReady;
    [HideInInspector]
    public bool startGameFlag;
    [HideInInspector]
    public float flipArtTimer;

    public int ore_seed;
    public int bedrock_seed;
    public int water_seed;

    void initTiles()
    {
        Noise.reseed();

        tiles = new GameObject[Consts.world_w,Consts.world_h];
        float mul = 0.05f;
        
        for (int x = 0; x < Consts.world_w; x++) {
            for (int z = 0; z < Consts.world_h; z++) {
                int tile_i =  (int)((Noise.snoise2D(Noise.perm_t,x*mul,z*mul)+1.0f)*50.0f);
                     if(tile_i < 80)  tile_i = (int)TileId.GRASS;
                else if(tile_i < 100) tile_i = (int)TileId.WATER;
                else tile_i = (int)TileId.WATER;
                GameObject newTile = GameObject.Instantiate(refs.tiles[tile_i], new Vector3(x,Consts.tile_y,z), Quaternion.identity);
                Tile nt = newTile.GetComponent<Tile>();
                nt.type = (TileId)tile_i;
                nt.x = x;
                nt.z = z;
                tiles[x,z] = newTile;
            }
        } 

    }

    void initIron()
    {
        Noise.reseed();

        //iron
        int structure_i = (int)StructureId.IRONDEPOSIT;
        float mul = 0.1f;
        for (int x = 0; x < Consts.world_w; x++) {
            for (int z = 0; z < Consts.world_h; z++) {
                if(tiles[x,z].GetComponent<Tile>().type == TileId.WATER)
                    continue;
                float v =  Noise.snoise2D(Noise.perm_t,x*mul,z*mul);
                if(v > 0.8f)
                {
                    bool collides;
                    collides = false;
                    for(int j = 0; j < structures.Count; j++) {
                        Structure ts = structures[j].GetComponent<Structure>();
                        if(ts.x == x && ts.z == z) {
                            collides = true;
                            break;
                        }
                    }
                    if(!collides)
                    {

                        GameObject newStructure = GameObject.Instantiate(refs.structures[structure_i], new Vector3(x,Consts.structure_y,z), Quaternion.identity);
                        Structure ns = newStructure.GetComponent<Structure>();
                        ns.type = (StructureId)structure_i;
                        ns.x = x;
                        ns.z = z;
                        structures.Add(newStructure);
                    }
                }
            }
        } 
    }

    void initStructures()
    {
        initIron();

        int px;
        int pz;
        int nStructures = Random.Range(100,200);
        for(int i = 0; i < nStructures; i++) {
            int structure_i = Random.Range(0, Consts.spawnableStructures);
            bool collides;
            do {
                px = Random.Range(0,Consts.world_w);
                pz = Random.Range(0,Consts.world_h);
                collides = false;
                if(tiles[px,pz].GetComponent<Tile>().type == TileId.WATER)
                {
                    collides = true;
                    break;
                }
                for(int j = 0; j < structures.Count; j++) {
                    Structure ts = structures[j].GetComponent<Structure>();
                    if(ts.x == px && ts.z == pz) {
                        collides = true;
                        break;
                    }
                }
            }
            while(collides);
            GameObject newStructure = GameObject.Instantiate(refs.structures[structure_i], new Vector3(px,Consts.structure_y,pz), Quaternion.identity);
            Structure ns = newStructure.GetComponent<Structure>();
            ns.type = (StructureId)structure_i;
            ns.x = px;
            ns.z = pz;
            structures.Add(newStructure);
        }

    }

    void initEngine(int px, int pz)
    {
        GameObject newEngine = GameObject.Instantiate(refs.cartEngine, new Vector3(px+Consts.cart_recenter_off.x, Consts.cart_y, pz+Consts.cart_recenter_off.y), Quaternion.identity);
        Utils.resizePrefab(newEngine,Consts.cart_s);
        Cart ne = newEngine.GetComponent<Cart>();
        ne.x = px;
        ne.z = pz;
        cartEngine = newEngine;
        carts.Add(newEngine);
        //convert underlying tile to grass
        GameObject newTile = GameObject.Instantiate(refs.tiles[(int)TileId.GRASS], new Vector3(px,Consts.tile_y,pz), Quaternion.identity);
        Tile nt = newTile.GetComponent<Tile>();
        nt.type = TileId.GRASS;
        nt.x = px;
        nt.z = pz;
        tiles[px,pz] = newTile;
        //remove underlying structure
        for(int i = 0; i < structures.Count; i++) {
            Structure ts = structures[i].GetComponent<Structure>();
            if(ts.x == px && ts.z == pz) {
                Destroy(structures[i]);
                structures.RemoveAt(i);
                break;
            }
        }
        //(force) add underlying track
        GameObject newTrack = GameObject.Instantiate(refs.track, new Vector3(px,Consts.track_y,pz), Quaternion.identity);
        Track ns = newTrack.GetComponent<Track>();
        ns.x = px;
        ns.z = pz;
        track.Add(newTrack);
    }

    void initCraft(int px, int pz)
    {
        GameObject newCraft = GameObject.Instantiate(refs.cartCraft, new Vector3(px+Consts.cart_recenter_off.x, Consts.cart_y, pz+Consts.cart_recenter_off.y), Quaternion.identity);
        Utils.resizePrefab(newCraft,Consts.cart_s);
        CraftingCart nc = newCraft.GetComponent<CraftingCart>();
        nc.objectsInCrafter = new List<ObjectId>();
        nc.x = px;
        nc.z = pz;
        nc.worldManager = this;
        cartCrafting = newCraft;
        carts.Add(newCraft);
        //convert underlying tile to grass
        GameObject newTile = GameObject.Instantiate(refs.tiles[(int)TileId.GRASS], new Vector3(px,Consts.tile_y,pz), Quaternion.identity);
        Tile nt = newTile.GetComponent<Tile>();
        nt.type = TileId.GRASS;
        nt.x = px;
        nt.z = pz;
        tiles[px,pz] = newTile;
        //remove underlying structure
        for(int i = 0; i < structures.Count; i++) {
            Structure ts = structures[i].GetComponent<Structure>();
            if(ts.x == px && ts.z == pz) {
                Destroy(structures[i]);
                structures.RemoveAt(i);
                break;
            }
        }
        //(force) add underlying track
        GameObject newTrack = GameObject.Instantiate(refs.track, new Vector3(px,Consts.track_y,pz), Quaternion.identity);
        Track ns = newTrack.GetComponent<Track>();
        ns.x = px;
        ns.z = pz;
        track.Insert(0,newTrack);
    }

    void initTrain()
    {
        carts = new List<GameObject>();
        tracks = new List<GameObject>();
        track = new List<GameObject>();
        int px = Random.Range(4,Consts.world_h);
        int pz = Random.Range(2,Consts.world_h-2);
        initEngine(px,pz);
        initCraft(px-1,pz);
        trainProgress = track.Count-1;
        trainIncProgress = 0.0f;
    }

    void initPlayer(int px, int pz)
    {
        player = GameObject.Instantiate(refs.player, new Vector3(px+Consts.player_recenter_off.x,Consts.player_y,pz+Consts.player_recenter_off.y), Quaternion.identity);
        Utils.resizePrefab(player,Consts.player_s);
        Player p = player.GetComponent<Player>();
        p.worldManager = this;
        //convert underlying tile to grass
        GameObject newTile = GameObject.Instantiate(refs.tiles[(int)TileId.GRASS], new Vector3(px,Consts.tile_y,pz), Quaternion.identity);
        Tile nt = newTile.GetComponent<Tile>();
        nt.type = TileId.GRASS;
        nt.x = px;
        nt.z = pz;
        tiles[px,pz] = newTile;
        //remove underlying structure
        for(int i = 0; i < structures.Count; i++) {
            Structure ts = structures[i].GetComponent<Structure>();
            if(ts.x == px && ts.z == pz) {
                Destroy(structures[i]);
                structures.RemoveAt(i);
                break;
            }
        }
    }

    void spawnEnemies()
    {
        int px;
        int pz;
        int nEnemies = Random.Range(nights, (int)(nights*Consts.IncreasingDifficultyValue));
        for (int i = 0; i < nEnemies; i++) {
            int enemies_i = Random.Range(0, refs.enemies.Length);
            px = player.GetComponent<Player>().x+15+Random.Range(0,10); //to the right of the screen (TODO: calc visible screen width)
            pz = Random.Range(0, Consts.world_h);
            GameObject newEnemy = GameObject.Instantiate(refs.enemies[enemies_i], new Vector3(px, Consts.enemy_y, pz), Quaternion.identity);
            newEnemy.GetComponent<Enemy>().worldManager = this;
            enemies.Add(newEnemy);
        }

        Debug.Log("Number of Enemies Spawned: " + enemies.Count);
    }

    public void spawnBulletTowardClosestEnemy(GameObject source)
    {
        if(enemies.Count == 0) return;

        int px;
        int pz;
        Structure s = source.GetComponent<Structure>();
        px = s.x;
        pz = s.z;
        Vector3 inipos = new Vector3(px, Consts.bullet_y, pz);
        GameObject closest_e = null;
        float closest_d = 99999999.9f;
        Vector2 bullet_pos = new Vector2(inipos.x,inipos.z);
        Vector2 enemy_pos;
        foreach(GameObject enemy in enemies) {
            enemy_pos = new Vector2(enemy.transform.position.x,enemy.transform.position.z);
            float d = Vector2.Distance(bullet_pos,enemy_pos);
            if(d < closest_d)
            {
                closest_e = enemy;
                closest_d = d;
            }
        }

        if(closest_d < Consts.turret_vision_dist)
        {
            GameObject newBullet = GameObject.Instantiate(refs.bullet, inipos, Quaternion.identity);
            Utils.resizePrefab(newBullet,Consts.bullet_s);
            Bullet b = newBullet.GetComponent<Bullet>();
            b.x = px;
            b.z = pz;

            enemy_pos = new Vector2(closest_e.transform.position.x,closest_e.transform.position.z);
            Vector2 dir = (enemy_pos-bullet_pos).normalized;
            b.dx = dir.x;
            b.dz = dir.y;

            bullets.Add(newBullet);
        }
        
    }

    void initTileHighlighters() {
        tileHighlight_Passive = GameObject.Instantiate(refs.tileHighlight_Passive, Consts.hiddenTilePosition, Quaternion.identity);
        structureHighlight_Passive = GameObject.Instantiate(refs.structureHighlight_Passive, Consts.hiddenTilePosition, Quaternion.identity);
        structureHighlight_Active = GameObject.Instantiate(refs.structureHighlight_Active, Consts.hiddenTilePosition, Quaternion.identity);
        objectHighlight_Passive = GameObject.Instantiate(refs.objectHighlight_Passive, Consts.hiddenTilePosition, Quaternion.identity);
        Utils.resizePrefab(objectHighlight_Passive,Consts.object_s);
        productHighlight_Passive = GameObject.Instantiate(refs.productHighlight_Passive, Consts.hiddenTilePosition, Quaternion.identity);
        Utils.resizePrefab(productHighlight_Passive,Consts.product_s);
        cartHighlight_Passive = GameObject.Instantiate(refs.cartHighlight_Passive, Consts.hiddenTilePosition, Quaternion.identity);
        Utils.resizePrefab(cartHighlight_Passive,Consts.cart_s);
        enemyHighlight_Passive = GameObject.Instantiate(refs.enemyHighlight_Passive, Consts.hiddenTilePosition, Quaternion.identity);
        Utils.resizePrefab(enemyHighlight_Passive,Consts.enemy_s);
    }

    void initCraftingPreviews() {
        craftPreviews_BG =      new List<GameObject>();
        craftPreviews_Eject =   new List<GameObject>();
        craftPreviews_Submit =  new List<GameObject>();
        objectPreviews =        new List<GameObject>();
        objectPreviews_Wood =   new List<GameObject>();
        objectPreviews_Iron =   new List<GameObject>();

        // Tile/Color Previews
        for (int i = 0; i < Consts.numCraftPreviewBGs; i++) {
            GameObject newCraftPreviewBG = GameObject.Instantiate(refs.craftPreviewBg, Consts.hiddenTilePosition, Quaternion.identity);
            craftPreviews_BG.Add(newCraftPreviewBG);
        } 
        for (int i = 0; i < Consts.numCraftPreviewEjects; i++) {
            GameObject newCraftPreviewEject = GameObject.Instantiate(refs.craftPreviewEject, Consts.hiddenTilePosition, Quaternion.identity);
            craftPreviews_Eject.Add(newCraftPreviewEject);
        } 
        for (int i = 0; i < Consts.numCraftPreviewSubmits; i++) {
            GameObject newCraftPreviewSubmit = GameObject.Instantiate(refs.craftPreviewSubmit, Consts.hiddenTilePosition, Quaternion.identity);
            craftPreviews_Submit.Add(newCraftPreviewSubmit);
        } 

        // Object Previews
        for (int i = 0; i < Consts.numCraftPreviewSubmits; i++) {
            GameObject newObjectPreviewSubmit = GameObject.Instantiate(refs.objectWood, Consts.hiddenTilePosition, Quaternion.identity);
            Utils.resizePrefab(newObjectPreviewSubmit, Consts.object_s);
            objectPreviews_Wood.Add(newObjectPreviewSubmit);
        } 
        for (int i = 0; i < Consts.numCraftPreviewSubmits; i++) {
            GameObject newObjectPreviewSubmit = GameObject.Instantiate(refs.objectIron, Consts.hiddenTilePosition, Quaternion.identity);
            Utils.resizePrefab(newObjectPreviewSubmit, Consts.object_s);
            objectPreviews_Iron.Add(newObjectPreviewSubmit);
        } 
        
        GameObject newCraftPreviewTail = GameObject.Instantiate(refs.craftPreviewTail, Consts.hiddenTilePosition, Quaternion.identity);
        craftPreviewTail = newCraftPreviewTail;
    }

    void initGameplay() 
    {
        nights = 0;
        isNight = false;
        enemiesHaveBeenSpawned = false;
        timeUntilNight = Consts.timeUntilNight;
    }   

    void initCanvas()
    {
        
    }

    public void getObjectPreviews() {
        CraftingCart cc = cartCrafting.GetComponent<CraftingCart>();
        objectPreviews.Clear();
        for (int i = 0; i < cc.objectsInCrafter.Count; i++) {
            switch (cc.objectsInCrafter[i]) {
                case ObjectId.WOOD:
                    objectPreviews.Add(objectPreviews_Wood[i]);
                    break;
                case ObjectId.IRON:
                    objectPreviews.Add(objectPreviews_Iron[i]);
                    break;
            }
        }
    }

    // void startCameraTransition()
    // {
    //     timeUntilNightText = canvas.GetComponentInChildren<Text>();
    // }

    public void resolveLatentTrack()
    {
        Track end = track[track.Count-1].GetComponent<Track>();
        for(int i = 0; i < tracks.Count; i++)
        {
            Track nt = tracks[i].GetComponent<Track>();
            if(
                (nt.x == end.x && (nt.z == end.z-1 || nt.z == end.z+1)) ||
                (nt.z == end.z && (nt.x == end.x-1 || nt.x == end.x+1))
            )
            {
                track.Add(tracks[i]);
                tracks.RemoveAt(i);
                resolveLatentTrack();
                return;
            }
        }
    }

    public void appendTrack(int x, int z)
    {
        GameObject newTrack = GameObject.Instantiate(refs.track, new Vector3(x,Consts.track_y,z), Quaternion.identity);
        Track nt = newTrack.GetComponent<Track>();
        nt.x = x;
        nt.z = z;
        Track end = track[track.Count-1].GetComponent<Track>();
        if(
            (nt.x == end.x && (nt.z == end.z-1 || nt.z == end.z+1)) ||
            (nt.z == end.z && (nt.x == end.x-1 || nt.x == end.x+1))
        )
        {
            track.Add(newTrack);
            resolveLatentTrack();
        }
        else tracks.Add(newTrack);
    }

    void trainUpdate()
    {
        if(trainProgress < track.Count-1)
        {
            trainIncProgress += Time.deltaTime;
            if(trainIncProgress > 1.0f)
            {
                trainProgress++;
                trainIncProgress -= 1.0f;
            }
            if(trainProgress == track.Count-1)
            {
                trainIncProgress = 0.0f;
                for(int i = 0; i < carts.Count; i++)
                {
                    Vector3 trackp = track[trainProgress-i].transform.position;
                    carts[i].transform.position = new Vector3(trackp.x+Consts.cart_recenter_off.x,Consts.cart_y,trackp.z+Consts.cart_recenter_off.y);
                }
            }
            else
            {
                for(int i = 0; i < carts.Count; i++)
                {
                    Vector3  trackp = track[trainProgress-i  ].transform.position;
                    Vector3 ntrackp = track[trainProgress-i+1].transform.position;
                    Vector3 ltrackp = Vector3.Lerp(trackp,ntrackp,trainIncProgress);
                    carts[i].transform.position = new Vector3(ltrackp.x+Consts.cart_recenter_off.x,Consts.cart_y,ltrackp.z+Consts.cart_recenter_off.y);
                }
            }
        }
    }

    void restartOnClick() {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Finish")) {
            Destroy(go);
        }
        Time.timeScale = 1;
        Awake();
    }

    void Awake()
    {
        Consts.initConsts();
        refs.Initialize();

        ore_seed = Random.Range(0,100);
        bedrock_seed = Random.Range(0,100);
        water_seed = Random.Range(0,100);

        initTiles();
        initStructures();
        initTrain();
        initPlayer(10,5);
        initGameplay();
        initCanvas();
        initTileHighlighters();
        initCraftingPreviews();

        mainCam.worldManager = this;
        cameraIsReady = false;
        startGameFlag = true;
        flipArtTimer = Consts.timeUntilArtFlip;
        // startCameraTransition();

    }
    
    // Start is called before the first frame update
    void Start()
    {
        gameOverText.text = "";
        mainCam.TransitionCamStart();
        restartButton.GetComponent<Button>().onClick.AddListener(restartOnClick);
        restartButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Player>().health <= 0) {
            Time.timeScale = 0;
            gameOverText.text = "You Died!";
            restartButton.gameObject.SetActive(true);
        }
        if (startGameFlag && cameraIsReady) {
            startGameFlag = false;
            initGameplay();
            mainCam.CamStart();
        }
        if (cameraIsReady) {
            player.GetComponent<Player>().PlayerUpdate();
            trainUpdate();

            mainCam.CamUpdate();
            foreach(GameObject enemy in enemies) {
                enemy.GetComponent<Enemy>().EnemyUpdate();
            }

            foreach(GameObject turret in turrets) {
                turret.GetComponent<Turret>().TurretUpdate();
            }

            for(int i = 0; i < bullets.Count; i++) {
                GameObject bullet = bullets[i];
                Bullet b = bullet.GetComponent<Bullet>();
                b.BulletUpdate();

                if(b.life > Consts.bullet_max_life)
                {
                    bullets.RemoveAt(i);
                    i--;
                    Destroy(bullet);
                    continue;
                }

                GameObject collidingEnemy = null;
                for(int j = 0; j < enemies.Count; j++)
                {
                    if(Utils.quadCollide3(bullet.transform.position,Consts.bullet_size,enemies[j].GetComponent<Transform>().position,Consts.enemy_size))
                    {
                        collidingEnemy = enemies[j];
                        enemies.RemoveAt(j);
                        Destroy(collidingEnemy);
                        bullets.RemoveAt(i);
                        i--;
                        Destroy(bullet);
                        break;
                    }
                }

            }

            timeUntilNight -= Time.deltaTime;
            if (timeUntilNight <= 0) {
                isNight = true; //isNight = !isNight; //don't bother flipping until we actually have day/night
                nights++;
                if (isNight) spawnEnemies();
                timeUntilNight = Consts.timeUntilNight;
            }

            //timeUntilNightText.text = timeUntilNight.ToString("0.0");
            timeUntilNightText.text = player.GetComponent<Player>().health.ToString();
        } else {
            mainCam.TransitionCamUpdate();
        } 


        // Material Flipping
        if (flipArtTimer <= 0) {
            player                      .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip();
            cartEngine                  .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip();
            cartCrafting                .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip();
            craftPreviewTail            .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip();
            tileHighlight_Passive       .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip();
            structureHighlight_Passive  .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip();
            structureHighlight_Active   .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip();
            objectHighlight_Passive     .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip();
            productHighlight_Passive    .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip();
            cartHighlight_Passive       .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip();

            //for(int i = 0; i < structures.Count; i++)               { structures[i]             .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip(); }
            for(int i = 0; i < objects.Count; i++)                  { objects[i]                .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip(); }
            for(int i = 0; i < products.Count; i++)                 { products[i]               .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip(); }
            for(int i = 0; i < enemies.Count; i++)                  { enemies[i]                .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip(); }
            for(int i = 0; i < track.Count; i++)                    { track[i]                  .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip(); }
            for(int i = 0; i < tracks.Count; i++)                   { tracks[i]                 .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip(); }
            for(int i = 0; i < craftPreviews_BG.Count; i++)         { craftPreviews_BG[i]       .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip(); }
            for(int i = 0; i < craftPreviews_Eject.Count; i++)      { craftPreviews_Eject[i]    .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip(); }
            for(int i = 0; i < craftPreviews_Submit.Count; i++)     { craftPreviews_Submit[i]   .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip(); }
            for(int i = 0; i < objectPreviews.Count; i++)           { objectPreviews[i]         .transform.GetChild(0).gameObject.GetComponent<Swapper>().flip(); }
            /*
            for(int i = 0; i < tiles.GetLength(0); i++) { 
                for (int j = 0; j < tiles.GetLength(1); j++) {
                    tiles[i,j].transform.GetChild(0).gameObject.GetComponent<Swapper>().flip();
                }  
            } 
            */
            flipArtTimer = Consts.timeUntilArtFlip;
        }

        flipArtTimer -= Time.deltaTime;
    }
}
