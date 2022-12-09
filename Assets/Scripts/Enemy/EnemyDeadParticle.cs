using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadParticle : MonoBehaviour
{
    public void AnimationFinishTrigger()
    {
        ObjectPool<EnemyDeadParticle>.Instance.Recycle(this);
    }
}
