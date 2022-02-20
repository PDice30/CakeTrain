using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public WorldManager worldManager;
  int x;
  int z;
  float z_maxBounds, z_minBounds, x_minBounds, x_maxBounds;
  public Vector2 interactionOffset;
  public Vector2 netOffset;
  public GameObject collidingStructure;
  public GameObject collidingCart;
  public GameObject interactingStructure;
  float interactingDamage;
  public GameObject interactingCart;
  public GameObject interactingObject;
  public GameObject holdingObject;

  void Start()
  {
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

    if(moving)
    {
      off.Normalize();
      interactionOffset = off*Consts.player_reach;
      netOffset         = off*Consts.net_reach;
      off               = off*Consts.player_speed*Time.deltaTime;

      Vector2 proposed = p+off;

      //bounds
      if (proposed.x                      < x_minBounds) proposed.x = x_minBounds;
      if (proposed.x+Consts.player_size.x > x_maxBounds) proposed.x = x_maxBounds-Consts.player_size.x;
      if (proposed.y                      < z_minBounds) proposed.y = z_minBounds;
      if (proposed.y+Consts.player_size.y > z_maxBounds) proposed.y = z_maxBounds-Consts.player_size.y;
      Vector3 proposed3 = new Vector3(proposed.x,transform.position.y,proposed.y);

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


      Vector2 interactingPt = new Vector2(proposed3.x+Consts.player_half_size.x+interactionOffset.x,proposed3.z+Consts.player_half_size.y+interactionOffset.y);
      interactingStructure = null;
      interactingCart = null;
      interactingObject = null;
      if(!holdingObject)
      {
        Vector2 netPt = new Vector2(proposed3.x+Consts.player_half_size.x+netOffset.x,proposed3.z+Consts.player_half_size.y+netOffset.y);
        Vector2 netBasePt = netPt-Consts.net_half_size;

        for(int i = 0; i < worldManager.objects.Count; i++)
        {
          Vector3 op3 = worldManager.objects[i].GetComponent<Transform>().position;
          Vector2 op2 = new Vector2(op3.x,op3.z);
          if(Utils.quadCollide(netBasePt,Consts.net_size,op2,Consts.unit_size))
            interactingObject = worldManager.objects[i];
        }
        if(!interactingObject)
        {
          for(int i = 0; i < worldManager.structures.Count; i++)
          {
            if(Utils.quadCollidePt3(worldManager.structures[i].GetComponent<Transform>().position,Consts.unit_size,interactingPt))
              interactingStructure = worldManager.structures[i];
          }
        }
      }

      if(Utils.quadCollidePt3(worldManager.cartCrafting.GetComponent<Transform>().position,Consts.cart_size,interactingPt))
        interactingCart = worldManager.cartCrafting;

      transform.position = proposed3;
    }

    worldManager.tileHighlight_Passive.transform.position = Consts.hiddenTilePosition;
    worldManager.tileHighlight_Active.transform.position  = Consts.hiddenTilePosition;

    // Interacting Checks
    if(holdingObject)
    {
      if(Input.GetKeyDown(KeyCode.Space))
      { //drop
        if(interactingCart)
        {
          // TODO: Display cart preview 

          if (interactingCart.GetComponent<CraftingCart>().objectsInCrafter.Count == Consts.maximumCraftObjects) {
            // cart is full
            // TODO: Vibrate feedback and don't insert
          } else {
            // cart is not full, add to crafting cart
            interactingCart.GetComponent<CraftingCart>().objectsInCrafter.Add(holdingObject.GetComponent<Object>().type);
            worldManager.objects.Remove(holdingObject);
            Destroy(holdingObject);
            holdingObject = null;
          }
        }
        else
        {
          holdingObject.transform.position = new Vector3(holdingObject.transform.position.x,Consts.object_y,holdingObject.transform.position.z);
          holdingObject = null;
        }
      }
      else
      { //hold
        holdingObject.transform.position = new Vector3(transform.position.x+Consts.player_hand_offset.x,Consts.held_object_y,transform.position.z+Consts.player_hand_offset.y);
      }
    }
    else if(interactingObject)
    {
      if(Input.GetKeyDown(KeyCode.Space))
      { //pickup
        holdingObject = interactingObject;
      }
    }
    else if(interactingCart)
    {
      // TODO: Display Preview based on if craft is valid

      if(Input.GetKeyDown(KeyCode.Space))
      { // submit
        if (interactingCart.GetComponent<CraftingCart>().craftIsValid) {
          interactingCart.GetComponent<CraftingCart>().objectsInCrafter.Clear();
          // TODO : Spit out crafted object
        } else { 
          //eject and instantiate objects that were in crafter
          float xPos = -0.5f;
          foreach(ObjectId objectId in interactingCart.GetComponent<CraftingCart>().objectsInCrafter) {

            GameObject ejectedObj = GameObject.Instantiate(worldManager.refs.objects[(int)objectId], new Vector3(interactingCart.transform.position.x + xPos, Consts.object_y, interactingCart.transform.position.z - 0.5f), Quaternion.identity);
            Utils.resizePrefab(ejectedObj, Consts.object_s);
            worldManager.objects.Add(ejectedObj);
            xPos += 0.5f;
          }
          interactingCart.GetComponent<CraftingCart>().objectsInCrafter.Clear();
        }
      }
    }
    else if(interactingStructure)
    {
      Structure s = interactingStructure.GetComponent<Structure>();
      if(Input.GetKey(KeyCode.Space))
      {
        if(s.type != StructureId.BEDROCK)
        {
          worldManager.tileHighlight_Active.transform.position = new Vector3(s.transform.position.x, Consts.object_y + 2f, s.transform.position.z);
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
          worldManager.tileHighlight_Passive.transform.position = new Vector3(interactingStructure.transform.position.x, Consts.object_y + 1f, interactingStructure.transform.position.z);
        }
      }
    }

    if(!interactingStructure)
    {
      interactingDamage = 0.0f;
    }
      
  }
}
