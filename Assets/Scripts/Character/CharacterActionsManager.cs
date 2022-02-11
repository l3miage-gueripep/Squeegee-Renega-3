using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionsManager : MonoBehaviour
{
    Character character;
    AttackDebugger attackDebugger;
    [SerializeField] bool doShowHitboxes;

    private void Start()
    {
        character = GetComponent<Character>();
        if (doShowHitboxes)
        {
            attackDebugger = gameObject.AddComponent<AttackDebugger>();
        }
    }
    public void AttackIfPossible()
    {
        if (character.stateManager.CanAct())
        {
            // character only stops if he does a grounded attack
            if (character.stateManager.isGrounded)
            {
                character.movementManager.Stop();
            }
            Attack(GetAttack());
        }
    }
    void Attack(Attack attack)
    {
        //things to do: play the animation, detect hitbox
        character.animationsManager.PlayAttackAnimation(attack);
        StartCoroutine(DoAttack(attack));
    }
    public void Parry()
    {
        if (character.stateManager.CanParry())
        {
            character.movementManager.Stop();
            character.animationsManager.PlayParryAnimation();
        }
    }
    Attack GetAttack()
    {
        if (!character.stateManager.isGrounded || character.stateManager.justJumped)
        {
            return character.attacks["airAttack"];
        }
        else if (character.stateManager.IsCrouching())
        {
            return character.attacks["downAttack"];
        }
        else if (character.stateManager.IsWatchingUpward())
        {
            return character.attacks["upwardAttack"];
        }
        else
        {
            return character.attacks["jab"];
        }
    }

    IEnumerator DoAttack(Attack attack)
    {

        List<Collider2D> alreadyHitEnemies = new List<Collider2D>();
        for (int currentFrame = 0; currentFrame < attack.startupFrames + attack.activeFrames; currentFrame++)
        {
            if (currentFrame > attack.startupFrames)
            {
                Hit(GetHitEnemies(attack), alreadyHitEnemies, attack);
                if(attackDebugger != null)
                {
                    attackDebugger.ShowAttackHitbox(attack);
                }
            }
            currentFrame++;
            yield return new WaitForSeconds(1f / 60);
        }
    }
    Collider2D[] GetHitEnemies(Attack attack)
    {
        LayerMask layers = LayerMask.GetMask(new string[] { "Character" , "Airborn Character" });
        return Physics2D.OverlapCircleAll(attack.hurtboxCenter.position, attack.range / 2, layers); // radius is 5/2 the size of the scale for the same number
    }
    void Hit(Collider2D[] hitEnemies, List<Collider2D> alreadyHitEnemies, Attack attack)
    {
        foreach (Collider2D hitEnemy in hitEnemies)
        {
            //prevents player from hitting himself
            if (hitEnemy.attachedRigidbody != character.rb2d)
            {
                //prevents attack for hitting one enemy multiple times
                if (!alreadyHitEnemies.Contains(hitEnemy))
                {
                    StartCoroutine(hitEnemy.GetComponent<Character>().TakeHit(attack, character));
                    alreadyHitEnemies.Add(hitEnemy);
                }
            }
        }
    }







}
