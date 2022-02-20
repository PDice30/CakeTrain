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
    COUNT,
}

public enum ObjectId: int
{
    WOOD = 0,
    IRON,
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
    public const float timeUntilNight = 5f;
    public static Vector3 hiddenTilePosition;


    //render layers
    public const float tile_y = 1.0f;
    public const float structure_y = 2.0f;
    public const float hilight_structure_y = 3.0f;
    public const float object_y = 4.0f;
    public const float hilight_object_y = 5.0f;
    public const float player_y = 6.0f;
    public const float enemy_y = 7.0f;
    public const float cart_y = 8.0f;
    public const float hilight_cart_y = 9.0f;
    public const float held_object_y = 10.0f;
    public const float camera_y = 100.0f;

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

    public const float object_s = 0.3f;
    public static Vector2 object_size;
    public static Vector2 object_half_size;
    public static Vector2 object_recenter_off;

    public static Vector2 player_hand_offset;

    public const int maximumCraftObjects = 3;
    public const int numCraftPreviewBGs = 3;
    public const int numCraftPreviewEjects = 3;
    public const int numCraftPreviewSubmits = 3;

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

        player_hand_offset = (player_size-object_size)/2.0f;
    }
}
