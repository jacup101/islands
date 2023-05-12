using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Harvestable : NetworkBehaviour
{
    [field: SerializeField] public ToolType HarvestingType {get; private set;}
    [field: SerializeField] public int ResourceCount {get; private set;}
    [field: SerializeField] public ParticleSystem ResourceEmitPS {get; private set;}
    [field: SerializeField] public GameObject EffectOnDestroyPrefab {get; private set;}
    [field: SerializeField] public AudioSource axe_sound;
    [field: SerializeField] public AudioSource pickaxe_sound;
    [field: SerializeField] bool is_tree;
    private int _amountHarvested = 0;


    public bool TryHarvest(ToolType harvestingType, int amount)
    {
        if(harvestingType == HarvestingType)
        {
            TriggerParticleServerRpc(amount);
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Harvest(int amount)
    {
        if (is_tree) {
            axe_sound.Play();
        } else {
            pickaxe_sound.Play();
        }

        int amountToSpawn = Mathf.Min(amount, ResourceCount - _amountHarvested);

        if(amountToSpawn > 0)
        {
            ResourceEmitPS.Emit(amount);
            _amountHarvested += amountToSpawn;
        }

        // The node is depleted
        if(_amountHarvested >= ResourceCount)
        {
            // Instead of destroying, play a little animation and set a timer for 
            if(EffectOnDestroyPrefab)
            {
                Instantiate(EffectOnDestroyPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void TriggerParticleServerRpc(int amount) {
        TriggerParticleClientRpc(amount);
    }

    [ClientRpc]
    private void TriggerParticleClientRpc(int amount) {
        Harvest(amount);
    }
}
