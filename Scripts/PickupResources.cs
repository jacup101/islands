using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PickupResources : NetworkBehaviour
{
    [field: SerializeField] public Inventory Inventory {get; private set;}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ResourcePickup pickup = collision.gameObject.GetComponent<ResourcePickup>();

        if(pickup)
        {
            Inventory.AddResources(pickup.ResourceType, 1);
            pickup.gameObject.GetComponent<DestroyResource>().DestroyObject();

        }
    }

    

}
