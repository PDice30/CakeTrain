using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public int health;
  public WorldManager worldManager;
  public int x;
  public int z;
  float z_maxBounds, z_minBounds, x_minBounds, x_maxBounds;
  public Vector2 interactionOffset;
  public Vector2 netOffset;
  public GameObject collidingStructure;
  public GameObject collidingCart;
  public GameObject collidingEnemy;
  public GameObject interactingStructure;
  float interactingDamage;
  public GameObject interactingCart;
  public GameObject interactingObject;
  public GameObject interactingProduct;
  public GameObject interactingEnemy;
  public GameObject holdingObject;
  public GameObject holdingProduct;

  void Start()
  {
    health = 5;
    x_minBounds = 0;
    x_maxBounds = Consts.world_w;
    z_minBounds = 0;
    z_maxBounds = Consts.world_h;
    interactionOffset = new Vector2(Consts.player_reach,0.0f);
    netOffset = new Vector2(Consts.net_reach,0.0f);
  }

  // Update is called once per frame
  public void PlayerUpdate()
  {
    Vector2 p = new Vector2(transform.position.x,transform.position.z);
    Vector2 off = new Vector2(0.0f,0.0f);
    bool moving = false;

         if(Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) { off.y += 1.0f; moving = true; }
    else if(Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) { off.y -= 1.0f; moving = true; }
         if(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) { off.x -= 1.0f; moving = true; }
    else if(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) { off.x += 1.0f; moving = true; }

    Vector2 proposed = p;
    if(moving)
    {
      off.Normalize();
      interactionOffset = off*Consts.player_reach;
      netOffset         = off*Consts.net_reach;
      off               = off*Consts.player_speed*Time.deltaTime;
      if(holdingObject || holdingProduct) off /= 2.0f;

      proposed += off;
    }

    //bounds
    if (proposed.x                      < x_minBounds) proposed.x = x_minBounds;
    if (proposed.x+Consts.player_size.x > x_maxBounds) proposed.x = x_maxBounds-Consts.player_size.x;
    if (proposed.y                      < z_minBounds) proposed.y = z_minBounds;
    if (proposed.y+Consts.player_size.y > z_maxBounds) proposed.y = z_maxBounds-Consts.player_size.y;
    Vector3 proposed3 = new Vector3(proposed.x,transform.position.y,proposed.y);
    x = (int)(proposed3.x+Consts.player_s/2.0f);
    z = (int)(proposed3.z+Consts.player_s/2.0f);

    collidingStructure = null;
    for(int i = 0; i < worldManager.structures.Count; i++)
    {
      if(Utils.quadCollide3Correct(proposed3,Consts.player_size,worldManager.structures[i].GetComponent<Transform>().position,Consts.unit_size,ref proposed3))
      {
        collidingStructure = worldManager.structures[i];
      }
    }

    collidingCart = null;
    for(int i = 0; i < worldManager.carts.Count; i++)
    {
      if(Utils.quadCollide3Correct(proposed3,Consts.player_size,worldManager.carts[i].GetComponent<Transform>().position,Consts.cart_size,ref proposed3))
      {
        collidingCart = worldManager.carts[i];
      }
    }

    collidingEnemy = null;
    for(int i = 0; i < worldManager.enemies.Count; i++)
    {
      if(Utils.quadCollide3Correct(proposed3,Consts.player_size,worldManager.enemies[i].GetComponent<Transform>().position,Consts.enemy_size,ref proposed3))
      {
        collidingEnemy = worldManager.enemies[i];
        worldManager.enemies.RemoveAt(i);
        Destroy(collidingEnemy);
        health--;
      }
    }

    Vector2 interactingPt = new Vector2(proposed3.x+Consts.player_half_size.x+interactionOffset.x,proposed3.z+Consts.player_half_size.y+interactionOffset.y);
    interactingStructure = null;
    interactingCart = null;
    interactingObject = null;
    interactingProduct = null;
    interactingEnemy = null;
    if(!holdingObject && !holdingProduct)
    {
      Vector2 netPt = new Vector2(proposed3.x+Consts.player_half_size.x+netOffset.x,proposed3.z+Consts.player_half_size.y+netOffset.y);
      Vector2 netBasePt = netPt-Consts.net_half_size;

      for(int i = 0; i < worldManager.enemies.Count; i++)
      {
        Vector3 op3 = worldManager.enemies[i].GetComponent<Transform>().position;
        Vector2 op2 = new Vector2(op3.x,op3.z);
        if(Utils.quadCollide(netBasePt,Consts.net_size,op2,Consts.enemy_size))
          interactingEnemy = worldManager.enemies[i];
      }
      if(!interactingEnemy)
      {
        for(int i = 0; i < worldManager.products.Count; i++)
        {
          Vector3 op3 = worldManager.products[i].GetComponent<Transform>().position;
          Vector2 op2 = new Vector2(op3.x,op3.z);
          if(Utils.quadCollide(netBasePt,Consts.net_size,op2,Consts.product_size))
            interactingProduct = worldManager.products[i];
        }
        if(!interactingProduct)
        {
          for(int i = 0; i < worldManager.objects.Count; i++)
          {
            Vector3 op3 = worldManager.objects[i].GetComponent<Transform>().position;
            Vector2 op2 = new Vector2(op3.x,op3.z);
            if(Utils.quadCollide(netBasePt,Consts.net_size,op2,Consts.object_size))
              interactingObject = worldManager.objects[i];
          }
          if(!interactingObject)
          {
            for(int i = 0; i < worldManager.structures.Count; i++)
            {
              if(Utils.quadCollidePt3(worldManager.structures[i].GetComponent<Transform>().position,Consts.unit_size,interactingPt))
                interactingStructure = worldManager.structures[i];
            }
            if(!interactingStructure)
            {
              //nothing within "interaction" net, so fall back to "colliding"
              if(collidingCart && collidingCart != worldManager.cartEngine && !holdingProduct) interactingCart = collidingCart;
              else if(collidingStructure) interactingStructure = collidingStructure;
            }
          }
        }
      }
    }

    if(!holdingProduct)
    {
      if(Utils.quadCollidePt3(worldManager.cartCrafting.GetComponent<Transform>().position,Consts.cart_size,interactingPt))
        interactingCart = worldManager.cartCrafting;
    }

    transform.position = proposed3;

    worldManager.tileHighlight_Passive.transform.position      = Consts.hiddenTilePosition;
    worldManager.structureHighlight_Passive.transform.position = Consts.hiddenTilePosition;
    worldManager.structureHighlight_Active.transform.position  = Consts.hiddenTilePosition;
    worldManager.objectHighlight_Passive.transform.position    = Consts.hiddenTilePosition;
    worldManager.productHighlight_Passive.transform.position   = Consts.hiddenTilePosition;
    worldManager.cartHighlight_Passive.transform.position      = Consts.hiddenTilePosition;
    worldManager.enemyHighlight_Passive.transform.position     = Consts.hiddenTilePosition;

    foreach(GameObject obj in worldManager.craftPreviews_BG) {
      obj.transform.position                                = Consts.hiddenTilePosition;
    }
    foreach(GameObject obj in worldManager.craftPreviews_Eject) {
      obj.transform.position                                = Consts.hiddenTilePosition;
    }
    foreach(GameObject obj in worldManager.craftPreviews_Submit) {
      obj.transform.position                                = Consts.hiddenTilePosition;
    }
    foreach(GameObject obj in worldManager.objectPreviews_Wood) {
      obj.transform.position                                = Consts.hiddenTilePosition;
    }
    foreach(GameObject obj in worldManager.objectPreviews_Iron) {
      obj.transform.position                                = Consts.hiddenTilePosition;
    }
    worldManager.craftPreviewTail.transform.position        = Consts.hiddenTilePosition;

    // Display Preview
    if (interactingCart) {
      CraftingCart cc = interactingCart.GetComponent<CraftingCart>();
      Vector3 cartPos = interactingCart.transform.position;
      
      if (cc.objectsInCrafter.Count != 0) {
        float xPos = cartPos.x+Consts.cart_s/2.0f; //center of cart
        float zPos = cartPos.z+Consts.cart_s*1.5f; //above cart
        worldManager.craftPreviewTail.transform.position = new Vector3(xPos-0.5f, Consts.preview_bg_y, zPos-0.5f);
        zPos += 0.1f;

        int numObjects = cc.objectsInCrafter.Count;
        float spacing = Consts.object_s;
        switch (numObjects) {
          case 1:                       break;
          case 2: xPos -= spacing/2.0f; break;
          case 3: xPos -= spacing;      break;
          default: xPos = 0f; break;
        }

        for (int i = 0; i < cc.objectsInCrafter.Count; i++) {
          worldManager.craftPreviews_BG[i].transform.position = new Vector3(xPos-0.5f, Consts.preview_bg_y, zPos-0.5f);
          if(!holdingObject)
          {
            if(cc.craftIsValid) worldManager.craftPreviews_Submit[i].transform.position = new Vector3(xPos-0.5f, Consts.preview_status_y, zPos-0.5f);
            else                worldManager.craftPreviews_Eject[ i].transform.position = new Vector3(xPos-0.5f, Consts.preview_status_y, zPos-0.5f);
          }
          worldManager.objectPreviews[i].transform.position = new Vector3(xPos-Consts.object_s/2.0f, Consts.preview_object_y, zPos-Consts.object_s/2.0f);
          xPos += spacing;
        }
      }
    }

    // Interacting Checks
    if(holdingProduct)
    {
      holdingProduct.transform.position = new Vector3(transform.position.x+Consts.player_hand_offset.x,Consts.held_product_y,transform.position.z+Consts.player_hand_offset.y);

      Product pr = holdingProduct.GetComponent<Product>();
      worldManager.tileHighlight_Passive.transform.position = new Vector3(x, Consts.highlight_tile_y, z);
      if(Input.GetKeyDown(KeyCode.Space))
      { //drop
        switch(pr.type)
        {
          case ProductId.TRACK:
            worldManager.appendTrack(x,z);
            break;
          case ProductId.BARRICADE:
            GameObject barricade = GameObject.Instantiate(worldManager.refs.structureBarricade, new Vector3(x, Consts.structure_y, z), Quaternion.identity);
            Structure bs = barricade.GetComponent<Structure>();
            bs.type = StructureId.BARRICADE;
            bs.x = x;
            bs.z = z;
            worldManager.structures.Add(barricade);
            break;
          case ProductId.TURRET:
            GameObject turret = GameObject.Instantiate(worldManager.refs.structureTurret, new Vector3(x, Consts.structure_y, z), Quaternion.identity);
            Structure ts = turret.GetComponent<Structure>();
            ts.type = StructureId.BARRICADE;
            ts.x = x;
            ts.z = z;
            worldManager.structures.Add(turret);
            break;
        }
        worldManager.products.Remove(holdingProduct);
        Destroy(holdingProduct);
        holdingProduct = null;
      }
    }
    else if(holdingObject)
    {
      holdingObject.transform.position = new Vector3(transform.position.x+Consts.player_hand_offset.x,Consts.held_object_y,transform.position.z+Consts.player_hand_offset.y);
      if(interactingCart) // add to cart
      {
        worldManager.cartHighlight_Passive.transform.position = new Vector3(interactingCart.transform.position.x, Consts.hilight_cart_y, interactingCart.transform.position.z);
        if(Input.GetKeyDown(KeyCode.Space))
        { 
          // Display Preview
          CraftingCart cc = interactingCart.GetComponent<CraftingCart>();
          Vector3 cartPos = interactingCart.transform.position;

          if (cc.objectsInCrafter.Count == Consts.maximumCraftObjects) {
            // cart is full
            // TODO: Vibrate feedback and don't insert
          } else {
            // cart is not full, add to crafting cart
            cc.objectsInCrafter.Add(holdingObject.GetComponent<Object>().type);
            worldManager.getObjectPreviews();
            cc.validateRecipe();
            worldManager.objects.Remove(holdingObject);
            Destroy(holdingObject);
            holdingObject = null;
          }
        }
      }
      else
      {
        if(Input.GetKeyDown(KeyCode.Space))
        { //drop
          holdingObject.transform.position = new Vector3(holdingObject.transform.position.x,Consts.object_y,holdingObject.transform.position.z);
          holdingObject = null;
        }
      }
    }
    else if(interactingEnemy)
    {
      worldManager.enemyHighlight_Passive.transform.position = new Vector3(interactingEnemy.transform.position.x, Consts.hilight_enemy_y, interactingEnemy.transform.position.z);
      if(Input.GetKeyDown(KeyCode.Space))
      {
        worldManager.enemies.Remove(interactingEnemy);
        Destroy(interactingEnemy);
        interactingEnemy = null;
      }
    }
    else if(interactingProduct)
    {
      worldManager.productHighlight_Passive.transform.position = new Vector3(interactingProduct.transform.position.x, Consts.hilight_product_y, interactingProduct.transform.position.z);
      if(Input.GetKeyDown(KeyCode.Space))
      { //pickup
        holdingProduct = interactingProduct;
      }
    }
    else if(interactingObject)
    {
      worldManager.objectHighlight_Passive.transform.position = new Vector3(interactingObject.transform.position.x, Consts.hilight_object_y, interactingObject.transform.position.z);
      if(Input.GetKeyDown(KeyCode.Space))
      { //pickup
        holdingObject = interactingObject;
      }
    }
    else if(interactingCart)
    {
      worldManager.cartHighlight_Passive.transform.position = new Vector3(interactingCart.transform.position.x, Consts.hilight_cart_y, interactingCart.transform.position.z);
      CraftingCart cc = interactingCart.GetComponent<CraftingCart>();
      Vector3 cartPos = interactingCart.transform.position;

      if(Input.GetKeyDown(KeyCode.Space))
      { // submit
        if (cc.craftIsValid) {
          GameObject craftedObj = GameObject.Instantiate(worldManager.refs.products[(int)cc.product], new Vector3(interactingCart.transform.position.x, Consts.product_y, interactingCart.transform.position.z - 0.5f), Quaternion.identity);
          craftedObj.GetComponent<Product>().type = cc.product;
          Utils.resizePrefab(craftedObj, Consts.product_s);
          worldManager.products.Add(craftedObj);
        } else { 
          //eject and instantiate objects that were in crafter
          float xPos = -0.5f;
          foreach(ObjectId objectId in cc.objectsInCrafter) {
            GameObject ejectedObj = GameObject.Instantiate(worldManager.refs.objects[(int)objectId], new Vector3(interactingCart.transform.position.x + xPos, Consts.object_y, interactingCart.transform.position.z - 0.5f), Quaternion.identity);
            ejectedObj.GetComponent<Object>().type = objectId;
            Utils.resizePrefab(ejectedObj, Consts.object_s);
            worldManager.objects.Add(ejectedObj);
            xPos += 0.5f;
          }
        }
        cc.craftIsValid = false;
        cc.objectsInCrafter.Clear();
        worldManager.objectPreviews.Clear();
      }
    }
    else if(interactingStructure)
    {
      Structure s = interactingStructure.GetComponent<Structure>();
      if(Input.GetKey(KeyCode.Space))
      {
        if(s.type != StructureId.BEDROCK)
        {
          worldManager.structureHighlight_Active.transform.position = new Vector3(interactingStructure.transform.position.x, Consts.hilight_structure_y, interactingStructure.transform.position.z);
          interactingDamage += Time.deltaTime;
          if(interactingDamage >= 1.0f)
          {
            GameObject newObject;
            Object o;
            switch(s.type)
            {
              case StructureId.BEDROCK: break; //impossible!
              case StructureId.TREE:
                newObject = GameObject.Instantiate(worldManager.refs.objects[(int)ObjectId.WOOD], new Vector3(s.x+Consts.object_recenter_off.x,Consts.object_y,s.z+Consts.object_recenter_off.y), Quaternion.identity);
                Utils.resizePrefab(newObject,Consts.object_s);
                o = newObject.GetComponent<Object>();
                o.type = ObjectId.WOOD;
                o.x = s.x;
                o.z = s.z;
                worldManager.objects.Add(newObject);
                break;
              case StructureId.IRONDEPOSIT:
              {
                newObject = GameObject.Instantiate(worldManager.refs.objects[(int)ObjectId.IRON], new Vector3(s.x+Consts.object_recenter_off.x,Consts.object_y,s.z+Consts.object_recenter_off.y), Quaternion.identity);
                Utils.resizePrefab(newObject,Consts.object_s);
                o = newObject.GetComponent<Object>();
                o.type = ObjectId.IRON;
                o.x = s.x;
                o.z = s.z;
                worldManager.objects.Add(newObject);
                break;
              }
            }
            for(int i = 0; i < worldManager.structures.Count; i++)
            {
              if(worldManager.structures[i] == interactingStructure)
              {
                Destroy(worldManager.structures[i]);
                worldManager.structures.RemoveAt(i);
                break;
              }
            }
            interactingStructure = null;
          }
        }
      }
      else
      {
        if(s.type != StructureId.BEDROCK) {
          worldManager.structureHighlight_Passive.transform.position = new Vector3(interactingStructure.transform.position.x, Consts.hilight_structure_y, interactingStructure.transform.position.z);
        }
      }
    }

    if(!interactingStructure || !Input.GetKey(KeyCode.Space))
    {
      interactingDamage = 0.0f;
    }
      
  }
}
