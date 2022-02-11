using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationsManager : MonoBehaviour
{
    Character character;
    public bool running;
    void Start()
    {
        //load the components
        character = GetComponent<Character>();
        character.animator.runtimeAnimatorController = character.runtimeAnimatorController;
    }
    public void Jump()
    {
        character.animator.SetBool("jumping", true);
    }
    public void UpdateJumpingVariable()
    {
        character.animator.SetBool("jumping", character.stateManager.IsJumping());
    }
    public void UpdateFallingVariable()
    {
        bool isFalling = character.stateManager.IsFalling(); //create variable to not call the method twice
        if (isFalling)
        {
            character.animator.SetBool("jumping", false); //to set jumping to false when character starts falling
        }
        character.animator.SetBool("falling", isFalling); //update the variable

    }
    public void SetGroundedVariable(bool grounded)
    {
        character.animator.SetBool("grounded", grounded);
    }
    public void SetRunningVariable(bool isRunning)
    {
        character.animator.SetBool("running", isRunning);
    }
    public void SetRunningVariable(float moveValue)
    {
        if (moveValue != 0)
        {
            running = true;
        }
        else
        {
            running = false;
        }
        character.animator.SetBool("running", running);
    }
    public void SetCrouching(bool isCrouching)
    {
        character.animator.SetBool("crouching", isCrouching);
    }
    public void PlayAttackAnimation(Attack attack)
    {
        // 1/ changes the runtimeAnimatorController
        //save all animator variables
        bool running = character.animator.GetBool("running");
        bool crouching = character.animator.GetBool("crouching");
        //change animator controller
        character.animator.runtimeAnimatorController = attack.animatorOverrideController;
        //restore animator variables (I have to do this because the variables are reset when changing overridecontroller)
        character.animator.SetBool("running", running);
        character.animator.SetBool("crouching", crouching);

        // 2/ plays the attack animation
        StartCoroutine(SetAttacking(attack.GetTotalFrames()));
    }
    IEnumerator SetAttacking(int attackLength)
    {
        character.animator.SetBool("attacking", true);
        for(int i = 0; i < attackLength; i++)
        {
            yield return new WaitForSeconds(1f / 60);
        }
        character.animator.SetBool("attacking", false);
    }

    public void PlayParryAnimation()
    {
        StartCoroutine(PlayParryAnimation(character.GetParryDuration()));
    }
    // might make a single method that changes a variable for a set amount of frames
    IEnumerator PlayParryAnimation(int parryLength)
    {
        character.animator.SetBool("parrying", true);
        for (int i = 0; i < parryLength; i++)
        {
            yield return new WaitForSeconds(1f / 60);
        }
        character.animator.SetBool("parrying", false);
    }

    public IEnumerator TakeHit(int stunDuration)
    {
        character.animator.SetBool("hit", true);
/*        StartCoroutine(character.movementManager.Shake());*/
        yield return new WaitForSeconds(GameManager.slowMotionDuration / 60);
        character.animator.SetBool("stunned", true);
        character.animator.SetBool("hit", false);
        yield return new WaitForSeconds((stunDuration - GameManager.slowMotionDuration) / 60);
        character.animator.SetBool("stunned", false);
    }




}
