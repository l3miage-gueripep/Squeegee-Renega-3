using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //mandatory
    [HideInInspector] public Transform hurtboxCenter;
    public float range;
    public int startupFrames;
    public int activeFrames;
    public int recoveryFrames;
    public int damage;
    public AnimatorOverrideController animatorOverrideController;

    public float knockbackPower;
    public int hitStunDuration;
    public Vector2 knockBackDirection;

    private void Start()
    {
        hurtboxCenter = transform;
    }
    public int GetTotalFrames()
    {
        return startupFrames + activeFrames + recoveryFrames;
    }
}
