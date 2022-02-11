using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerInputHandler playerInputHandler;
    public Character character;
    public PlayerInput playerInput;
    public int index { get; private set; }

    void Awake()
    {
        //get character informations
        character = Instantiate(Resources.Load<GameObject>("Characters/Shana/Character"), transform).GetComponent<Character>();
        index = playerInput.playerIndex;
    }

    public void EnableInputs()
    {
        Destroy(GetComponent<MenuPlayerInputHandler>());
        playerInputHandler = gameObject.AddComponent<PlayerInputHandler>();
    }
    public void EnableMovement()
    {
        gameObject.AddComponent<CharacterMovementManager>();
    }

}
