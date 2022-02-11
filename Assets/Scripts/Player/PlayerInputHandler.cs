using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputHandler : MonoBehaviour
{
    Character character;
    private GameManager gameManager;
    public float moveValue;
    private void Awake()
    {
        character = GetComponent<Player>().character; //get the character from the player script
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void OnMove(InputValue inputValue)
    {
        character.movementManager.SetMovement(inputValue.Get<Vector2>());
    }
    public void OnAttack()
    {
        character.actionsManager.AttackIfPossible();
    }
    public void OnShield()
    {
        character.actionsManager.Parry();
    }
    public void OnJump(InputValue inputValue)
    {
        character.movementManager.JumpIfPossible(inputValue.isPressed);
    }
}
