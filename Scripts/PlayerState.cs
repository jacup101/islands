using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "Islandz/PlayerState")]
public class PlayerState : ScriptableObject
{
    [field: SerializeField] public bool CanMove {get; set;} = true;
    [field: SerializeField] public bool CanExitWhilePlaying {get; set;} = true;

}
