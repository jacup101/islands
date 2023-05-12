using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ParticleTrigger : MonoBehaviour
{
    [field: SerializeField] private ParticleSystem _ps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        TriggerParticleServerRpc();
    }

    [ServerRpc(RequireOwnership=false)]
    private void TriggerParticleServerRpc() {
        TriggerParticleClientRpc();
    }

    [ClientRpc]
    private void TriggerParticleClientRpc() {
        _ps.Play();
    }
}
