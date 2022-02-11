using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    PlayersManager playersManager;
    bool pauseEachFrame;
    public static float slowMotionDuration = 5f; // number of frames
    public static int slowMotionAmount = 3;
    void Start()
    {
        DontDestroyOnLoad(this);
        playersManager = Instantiate(Resources.Load<GameObject>("PlayersManager")).GetComponent<PlayersManager>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        playersManager.DisableJoin();
        playersManager.LoadPlayersStats();
        playersManager.EnablePlayersInputs();

    }
    public static IEnumerator HitSlowMotion() //should be called slowgamespeed
    {
        Time.timeScale = Time.timeScale / slowMotionAmount; // reduce gamespeed
        Time.fixedDeltaTime = Time.fixedDeltaTime / slowMotionAmount; // increase fixedUpdate refresh rate to keep framerate high
        yield return new WaitForSecondsRealtime(slowMotionDuration / 60); // wait
        //revert
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
    public GameObject LoadBackground()
    {
        return Instantiate(Resources.Load<GameObject>("Background"), transform);
    }
}
