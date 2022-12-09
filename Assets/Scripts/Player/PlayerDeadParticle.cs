using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadParticle : MonoBehaviour
{
    public void AnimationFinishTrigger()
    {
        ObjectPool<PlayerDeadParticle>.Instance.Recycle(this);
    }
}
