using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameObjectEmitter : NetworkBehaviour
{
    [field: SerializeField] public GameObject ObjectPrefab {get; private set;}
    [field: SerializeField] private ParticleSystem _ps;

    private List<ParticleSystem.Particle> exitParticles = new();

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        _ps = GetComponent<ParticleSystem>();
    }    

    private void OnParticleTrigger()
    {
        // Exit Code
        _ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exitParticles);

        foreach (ParticleSystem.Particle p in exitParticles)
        {
            if(!IsServer) {
                return;
            }
            SpawnObjectServerRpc(p.position);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnObjectServerRpc(Vector3 position) {
        

        GameObject spawnedObject = Instantiate(ObjectPrefab, position, Quaternion.identity);
        spawnedObject.GetComponent<NetworkObject>().Spawn(true);
    }


}
