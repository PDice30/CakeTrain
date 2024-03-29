using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileId : int
{
    DIRT = 0,
    GRASS,
    WATER,
    COUNT,
}

public enum StructureId: int
{
    TREE = 0,
    IRONDEPOSIT,
    BEDROCK,
    BARRICADE,
    TURRET,
    COUNT,
}

public enum ObjectId: int
{
    WOOD = 0,
    IRON,
    COUNT,
}

public enum ProductId: int
{
    TRACK = 0,
    BARRICADE,
    TURRET,
    COUNT,
}

public enum EnemyId : int
{
    BAT = 0,
    GOBLIN,
    COUNT,
}

public class Consts
{
    //world
    public const int world_h = 10;
    public const int world_w = 100;
    public const float timeUntilNight = 20f;
    public const float timeUntilArtFlip = 0.1f;
    public static Vector3 hiddenTilePosition;


    //render layers
    public const float tile_y              = 1.0f;
    public const float highlight_tile_y    = 2.0f;
    public const float track_y             = 3.0f;
    public const float structure_y         = 4.0f;
    public const float hilight_structure_y = 5.0f;
    public const float object_y            = 6.0f;
    public const float hilight_object_y    = 7.0f;
    public const float product_y           = 8.0f;
    public const float hilight_product_y   = 9.0f;
    public const float player_y            = 10.0f;
    public const float enemy_y             = 11.0f;
    public const float hilight_enemy_y     = 12.0f;
    public const float cart_y              = 13.0f;
    public const float hilight_cart_y      = 14.0f;
    public const float held_object_y       = 15.0f;
    public const float held_product_y      = 16.0f;
    public const float preview_status_y    = 17.0f;
    public const float preview_bg_y        = 18.0f;
    public const float preview_object_y    = 19.0f;
    public const float bullet_y            = 20.0f;
    public const float camera_y            = 100.0f;

    //sizes
    public static Vector2 unit_size;

    public const float cart_s = 0.8f;
    public static Vector2 cart_size;
    public static Vector2 cart_half_size;
    public static Vector2 cart_recenter_off;

    public const float player_s = 0.5f;
    public const float player_reach = player_s;
    public static Vector2 player_size;
    public static Vector2 player_half_size;
    public static Vector2 player_recenter_off;
    public const float player_speed = 5.0f;

    public const float net_s = player_s*2.0f;
    public const float net_reach = player_s/2.0f;
    public static Vector2 net_size;
    public static Vector2 net_half_size;

    public const float object_s = 0.6f;
    public static Vector2 object_size;
    public static Vector2 object_half_size;
    public static Vector2 object_recenter_off;

    public const float product_s = 0.3f;
    public static Vector2 product_size;
    public static Vector2 product_half_size;
    public static Vector2 product_recenter_off;

    public static Vector2 player_hand_offset;

    public const float enemy_s = 1.0f;
    public static Vector2 enemy_size;
    public static Vector2 enemy_half_size;
    public static Vector2 enemy_recenter_off;

    public static float bullet_s = 0.5f;
    public static Vector2 bullet_size;
    public static Vector2 bullet_half_size;
    public static Vector2 bullet_recenter_off;
    public const float bullet_speed = 5.0f;
    public const float bullet_max_life = 20.0f;

    public const float turret_vision_dist = 10.0f;

    public const int maximumCraftObjects = 3;
    public const int numCraftPreviewBGs = 3;
    public const int numCraftPreviewEjects = 3;
    public const int numCraftPreviewSubmits = 3;
    public const int spawnableStructures = 3;

    public static List<List<ObjectId>> recipes;
    public static List<ProductId> recipeProducts;
    public static List<ObjectId> trackRecipe;
    public static List<ObjectId> barricadeRecipe;
    public static List<ObjectId> turretRecipe;

    // Debug settings
    public const bool AllowMelee = false;
    public const float DefaultStructureHealth = 0.75f;
    public const float IncreasingDifficultyValue = 1.2f;

    public static void initConsts()
    {
        hiddenTilePosition = new Vector3(-100, -100, -100);

        unit_size = new Vector2(1.0f,1.0f);

        cart_size = new Vector2(cart_s,cart_s);
        cart_half_size = new Vector2(cart_s,cart_s)*0.5f;
        cart_recenter_off = (unit_size-cart_size)/2.0f;

        player_size = new Vector2(player_s,player_s);
        player_half_size = new Vector2(player_s,player_s)*0.5f;
        player_recenter_off = (unit_size-player_size)/2.0f;

        net_size = new Vector2(net_s,net_s);
        net_half_size = new Vector2(net_s,net_s)*0.5f;

        object_size = new Vector2(object_s,object_s);
        object_half_size = new Vector2(object_s,object_s)*0.5f;
        object_recenter_off = (unit_size-object_size)/2.0f;

        product_size = new Vector2(product_s,product_s);
        product_half_size = new Vector2(product_s,product_s)*0.5f;
        product_recenter_off = (unit_size-product_size)/2.0f;

        enemy_size = new Vector2(enemy_s,enemy_s);
        enemy_half_size = new Vector2(enemy_s,enemy_s)*0.5f;
        enemy_recenter_off = (unit_size-enemy_size)/2.0f;

        bullet_size = new Vector2(bullet_s,bullet_s);
        bullet_half_size = new Vector2(bullet_s,bullet_s)*0.5f;
        bullet_recenter_off = (unit_size-bullet_size)/2.0f;

        player_hand_offset = (player_size-object_size)/2.0f;

        recipes = new List<List<ObjectId>>();
        recipeProducts = new List<ProductId>();

        trackRecipe = new List<ObjectId>();
        trackRecipe.Add(ObjectId.WOOD);
        trackRecipe.Add(ObjectId.IRON);
        recipes.Add(trackRecipe);
        recipeProducts.Add(ProductId.TRACK);

        barricadeRecipe = new List<ObjectId>();
        barricadeRecipe.Add(ObjectId.WOOD);
        barricadeRecipe.Add(ObjectId.WOOD);
        recipes.Add(barricadeRecipe);
        recipeProducts.Add(ProductId.BARRICADE);

        turretRecipe = new List<ObjectId>();
        turretRecipe.Add(ObjectId.IRON);
        turretRecipe.Add(ObjectId.IRON);
        recipes.Add(turretRecipe);
        recipeProducts.Add(ProductId.TURRET);
    }
}
