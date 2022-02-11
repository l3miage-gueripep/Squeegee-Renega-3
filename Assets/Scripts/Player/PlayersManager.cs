using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayersManager : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    GameObject healthBar;
    List<Player> players = new List<Player>();
    Vector3[] playersCoordinates = { new Vector3(-7, 0, 0), new Vector3(7, 0, 0) };
    Quaternion[] playersRotations = { new Quaternion(0, 0, 0, 0), new Quaternion(0, -180, 0, 0) };
    int playerIndex;
    void Start()
    {
        DontDestroyOnLoad(this);
        playerInputManager = GetComponent<PlayerInputManager>();
/*        healthBar = Resources.Load<GameObject>("HealthBar");*/
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerIndex = playerInputManager.playerCount - 1;
        //change positions and stuff depending on the player number
        players.Add(playerInput.gameObject.GetComponent<Player>());
        Player player = players[playerIndex];
        player.transform.SetParent(transform);
        player.transform.SetParent(transform);
        player.transform.position = playersCoordinates[playerIndex];
        player.transform.rotation = playersRotations[playerIndex];
        CreateHealthBar(player);
        
    }

    public void LoadPlayersStats()
    {
        foreach(Player player in players)
        {
            player.character.Enable();
        }
    }
    public void EnablePlayersInputs()
    {
        foreach (Player player in players)
        {
            player.EnableInputs();
        }
    }

    public void DisableJoin()
    {
        playerInputManager.DisableJoining();
    }

    public void CreateHealthBar(Player player)
    {
/*        GameObject healthBarInstance = Instantiate(healthBar);
        DontDestroyOnLoad(healthBarInstance);
        healthBarInstance.GetComponent<HealthBar>().Initialize(player);*/
    }
    public void SetHealthBarsPositions()
    {
        foreach (Player player in players)
        {
/*            character.healthBar.SetPosition();*/
        }
    }
}
