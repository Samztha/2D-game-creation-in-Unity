using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpherePlayerController : MonoBehaviour
{
    [SerializeField] private GameObject pivot;

    // Rigidbody of the player
    private Rigidbody rb;

    // Variable to keep track of collected "PointSphere" objects
    private int count;

    // Movement along X axis
    private float movementX;

    //
    private float Move;

    //
    private bool isDragging;

    //
    private bool canDrag;

    //
    private SpringJoint springJoint;

    //
    private Camera main;

    // Speed at which the player moves
    public float speed = 0;

    //
    public float jump;

    //
    public bool isJumping;

    // UI text component to display count of "PickUp" objects collected
    public TextMeshProUGUI countText;

    // UI text component to display the "Game Over" message
    public TextMeshProUGUI gameOverText;

    // 
    public Button restartButton;

    // Tells us when the game is active or not.
    public bool isGameActive;

    // Reference to the SphereCameraController script
    public SphereCameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        // Makes the game active when u boot/start it up
        isGameActive = true;

        // Get and store the Rigidbody component attached to the player
        rb = GetComponent<Rigidbody>();

        //
        springJoint = GetComponent<SpringJoint>();

        //
        canDrag = true;

        //
        main = Camera.main;

        // Initialize count to zero
        count = 0;

        // Update the count display
        SetCountText();
    }

    /*----- Movement -----*/
    // This function is called when a move input is detected
    void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X component of the movement
        movementX = movementVector.x;
    }

    private void DetectTouch()
    {
        // If screen isnt being touched
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            // If screen was touched
            if (isDragging)
            {
                LaunchPlayer();
            }
            isDragging = false;
            return;
        }

        if (canDrag)
        {
            isDragging = true;
            rb.isKinematic = true;

            // Get position of touch in pixels
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            // Convert position from pixels to world
            Vector3 worldPosition = main.ScreenToWorldPoint(touchPosition);
            worldPosition.z = transform.position.z;

            // Set player position to finger
            transform.position = worldPosition;
        }
    }

    private void LaunchBall()
    {
        rb.isKinematic = false;
        canDrag = false;
    }

    // FixedUpdate is called once per fixed frame-rate frame
    private void FixedUpdate()
    {
        if (isGameActive)
        {
            // Create a 3D movement vector using the X input
            Vector3 movement = new Vector3(movementX, 0.0f);

            // 
            Move = Input.GetAxis("Horizontal");

            // Makes the character jump when pressing the space bar, and it wont jump if the character is mid air
            if (Input.GetButtonDown("Jump") && isJumping == false)
            {
                rb.AddForce(new Vector2(rb.velocity.x, jump));
            }

            // Apply force to the Rigidbody to move the player
            rb.AddForce(movement * speed);
        }
    }

    /*----- Collision -----*/
    void OnTriggerEnter(Collider other)
    {
        /*----- PointOrb -----*/
        // Check if the object the player collided with has the "Poi    here" tag
        if (other.gameObject.CompareTag("PointSphere"))
        {
            // Deactivate the collided object (making it disappear)
            other.gameObject.SetActive(false);

            // Increment the count of "PointSphere" objects collected
            count = count + 1;

            // Update the count display
            SetCountText();
        }

        /*----- DamageOrb -----*/
        if (other.gameObject.CompareTag("DamageSphere"))
        {
            //Kills and removes the player when hit by a damage orb + displays the "Game Over" text
            gameObject.SetActive(false);
            GameOver();
        }

        /*----- Spike -----*/
        if (other.gameObject.CompareTag("Spike"))
        {
            //Kills and removes the player when hit by a spike + displays the "Game Over" text
            gameObject.SetActive(false);
            GameOver();
        }

        /*----- BottomOfTheMap -----*/
        if (other.gameObject.CompareTag("BottomOfTheMap"))
        {
            //Kills and removes the player when the player hits the bottom of the map + displays the "Game Over" text
            gameObject.SetActive(false);
            GameOver();
        }
    }

    // Checks if the player is touching the floor, making it possible to jump
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
        }
    }

    // Checks if the player is not touching the floor, making it impossible to jump 
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isJumping = true;
        }
    }

    // Function to update the displayed count of "PointSphere" objects collected
    void SetCountText()
    {
        // Update the count text with the current count
        countText.text = "Points: " + count.ToString();
    }

    // The "GameOver" method, when called, activates the "Game Over" text object (gameOverText), making it visible in the game
    public void GameOver()
    {
        // Shows game over text and a restart button once the player loses
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        // Set isGameActive to false
        isGameActive = false;

        // Notify the camera controller that the game is over.
        cameraController.GameOver();
    }

    // The "RestartGame" method, this lets us restart the game once the player gets destroyed
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}