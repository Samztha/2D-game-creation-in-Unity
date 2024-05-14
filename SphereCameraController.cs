using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class SphereCameraController : MonoBehaviour
{
    // Reference to the player GameObject.
    public GameObject player;

    // The distance between the camera and the player.
    private Vector3 offset;

    // Tells us when the game is active or not.
    public bool isGameActive;

    // Start is called before the first frame update.
    void Start()
    {
        // Makes the game active when u boot/start it up
        isGameActive = true;

        // Calculate the initial offset between the camera's position and the player's position.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called once per frame after all Update functions have been completed.
    void LateUpdate()
    {
        // Check if the game is currently active.
        if (isGameActive)
        {
            // Maintain the same offset between the camera and player throughout the game.
            transform.position = player.transform.position + offset;
        }
    }
    

    public void GameOver()
        {
            // Makes it so the game stops once u die
            isGameActive = false;
        }
}
