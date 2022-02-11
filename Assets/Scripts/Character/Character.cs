using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //stats and informations
    public string label; //can't use name
    public int maxHP;
    public int weight;
    public int movementSpeed;
    public float scale;
    public float jumpForce;
    public Dictionary<string, Attack> attacks = new Dictionary<string, Attack>();

    //components
    public RuntimeAnimatorController runtimeAnimatorController;
    [HideInInspector] public Rigidbody2D rb2d;
    [HideInInspector] public BoxCollider2D boxCollider2D;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterAnimationsManager animationsManager;
    [HideInInspector] public CharacterMovementManager movementManager;
    [HideInInspector] public CharacterActionsManager actionsManager;
    [HideInInspector] public CharacterStateManager stateManager;
    

    //other
    HealthBar healthBar;
    public LayerMask characterLayer;
    public LayerMask airbornCharacterLayer;
    public GameManager gameManager;

    //variables that are the same for all characters
    int parryStartupFrames = 0;
    int parryActiveFrames = 5;
    int parryRecoveryFrames = 5;

    private void Start()
    {
        //get its components
        animationsManager = GetComponent<CharacterAnimationsManager>();
        movementManager = GetComponent<CharacterMovementManager>();
        actionsManager = GetComponent<CharacterActionsManager>();
        stateManager = GetComponent<CharacterStateManager>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>();
        characterLayer = LayerMask.NameToLayer("Character");
        airbornCharacterLayer = LayerMask.NameToLayer("Airborn Character");
        gameObject.layer = characterLayer;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //resize and relocate
        transform.localScale = new Vector3(scale, scale, 1);
        float spriteHeight = gameObject.GetComponent<SpriteRenderer>().bounds.size.y; //get the height of the character image
        transform.position = new Vector3(transform.position.x, spriteHeight / 2, 1);
         
        GetAttacks();
    }

    void FixedUpdate() 
    {
        if (rb2d.simulated)
        {
            stateManager.Refresh();
            animationsManager.UpdateFallingVariable();
            movementManager.Move();
            movementManager.SetLayer();
        }
    }

    void GetAttacks()
    {
        //insert code that finds all gameobjects in moveset and assign each move in the dictionary
        attacks["jab"] = transform.Find("MoveSet/Jab").GetComponent<Attack>();
        attacks["downAttack"] = transform.Find("MoveSet/DownAttack").GetComponent<Attack>();
        attacks["upwardAttack"] = transform.Find("MoveSet/UpwardAttack").GetComponent<Attack>();
        attacks["airAttack"] = transform.Find("MoveSet/AirAttack").GetComponent<Attack>();
    }
    public int GetParryDuration()
    {
        return parryStartupFrames + parryActiveFrames + parryRecoveryFrames;
    }
    public IEnumerator TakeHit(Attack attack, Character hitter)
    {

        if (!stateManager.IsParrying())
        {
            yield return StartCoroutine(GameManager.HitSlowMotion()); //slow mo
            StartCoroutine(animationsManager.TakeHit(attack.hitStunDuration)); //animation
            movementManager.TakeHit(attack, hitter);
        }
        else
        {
            //parry the attack
        }

    }
    public void Enable()
    {
        rb2d.simulated = true;
    }
}
