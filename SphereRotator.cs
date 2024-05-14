using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class SphereRotator : MonoBehaviour
{
    // Tells us when the game is active or not.
    public bool isGameActive;

    // Start is called before the first frame update.
    void Start()
    {
        // Makes the game active when u boot/start it up
        isGameActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is active before rotating the spheres
        if (isGameActive)
        {
            // Rotate the object on X, Y, and Z axes by specified amounts, adjusted for frame rate.
            transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        }
    }

    public void GameOver()
    {
        // Makes it so the game stops once u die
        isGameActive = false;
    }
}