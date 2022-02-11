using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    List<Character> characters = new List<Character>();
    GameObject[] characterGameObjects;
    Vector3 offset = new Vector3(0, 0, -10);
    int charactersAmount;
    int yAxisOffset = -2;
    float smoothSpeed = 0.1f;
    float minCameraSize = 10;
    float maxCameraSize = 14;
    float cameraSize = 1f;
    void Start()
    {
        characterGameObjects = GameObject.FindGameObjectsWithTag("Character");

        foreach (GameObject characterGameObject in characterGameObjects)
        {
            characters.Add(characterGameObject.GetComponent<Character>());
            charactersAmount += 1;
        }
    }

    void FixedUpdate() //I don't use late update because it makes my characters blurry
    {
        Vector3 desiredPosition = new Vector3(characters.Sum(character => character.transform.position.x) / charactersAmount, (characters.Sum(player => player.transform.position.y) / charactersAmount) + yAxisOffset);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition + offset;
        cameraSize = GetCameraSize();
        GetComponent<Camera>().orthographicSize = cameraSize;
    }
    float GetPlayersDistance()
    {
        return (characters.Max(character => character.transform.position.x) - characters.Min(character => character.transform.position.x)) / 2;
    }
    float GetCameraSize()
    {
        float playersDistance = GetPlayersDistance();
        float desiredCameraSize;
        if(playersDistance < minCameraSize)
        {
            desiredCameraSize = minCameraSize;
        }
        else if (playersDistance < maxCameraSize)
        {
            desiredCameraSize = playersDistance;
        }
        else
        {
            desiredCameraSize = maxCameraSize;
        }
        // using Lerp to smooth camera zoom
        return Mathf.Lerp(GetComponent<Camera>().orthographicSize, desiredCameraSize, 0.1f);
    }
}
