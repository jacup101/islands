using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CombatHitbox : NetworkBehaviour
{
    [SerializeField] int direction;
    [SerializeField] List<GameObject> in_field;
    // "Down" - 0
    // "Up" - 1
    // "Left" - 2
    // "Right" - 3
    [SerializeField] List<Vector3> local_pos;
    // Start is called before the first frame update
    public void SetDirection(int dir) {
        this.direction = dir;
        this.transform.localPosition = this.local_pos[this.direction];
    }

    void Start() {
        in_field = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>() != null) {
            in_field.Add(other.gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController>() != null) {
            in_field.Remove(other.gameObject);
        }   
    }

    public void DoDamage() {
        if (IsOwner) {
            int damage = this.gameObject.transform.parent.GetComponent<PlayerNetwork>().GetDamageAmt();
            foreach (GameObject obj in in_field) {
                obj.GetComponent<PlayerNetwork>().Damage(damage);
            }
        }
    }
}
