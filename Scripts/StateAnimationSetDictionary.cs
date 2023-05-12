using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateAnimationSetDictionary : SerializableDictionary<PlayerState, DirectionalAnimationSet>
{
    public AnimationClip GetFacingClipFromState(PlayerState playerState, Vector2 facingDirection)
    {
        if(TryGetValue(playerState, out DirectionalAnimationSet animationSet))
        {
            return animationSet.GetFacingClip(facingDirection);
        } else
        {
            Debug.LogError($"Player state {playerState.name} is not found in the animation dictionary.");
        }

        //Failed to find animation set
        return null;
    }
}
