using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DestroyResource : NetworkBehaviour
{

    public void DestroyObject() {
        DestroyObjectServerRpc();
    }


    [ServerRpc(RequireOwnership = false)]
    public void DestroyObjectServerRpc() {
        Destroy(this.gameObject);
    }

}
