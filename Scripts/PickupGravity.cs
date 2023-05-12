using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGravity : MonoBehaviour
{

    [field: SerializeField] public float GravitySpeed {get; private set;} = 0.2f;
    private List<ResourcePickup> _nearbyResources = new();

    private void FixedUpdate()
    {
        _nearbyResources.RemoveAll(pickup => pickup == null);

        foreach (ResourcePickup pickup in _nearbyResources)
        {
            Vector2 directionToCenter = (transform.position - pickup.transform.position).normalized;

            pickup.transform.Translate(directionToCenter * GravitySpeed * Time.fixedDeltaTime);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ResourcePickup pickup = collision.gameObject.GetComponent<ResourcePickup>();

        if(pickup)
        {
            _nearbyResources.Add(pickup);
        }

   }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ResourcePickup pickup = collision.gameObject.GetComponent<ResourcePickup>();

        if(pickup)
        {
            _nearbyResources.Remove(pickup);
        }

   }
}
