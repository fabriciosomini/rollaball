using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    public float speed;
    private Rigidbody rb;
    public Text countText;
    public Text winText;
    private int count;
    private int pickUpCount;
    private bool isGameOver = false;
    protected JoystickButton joystickButton;
    protected Joystick joystick;
    public AudioSource hitSound;
    public Camera playerCamera;
    private Vector3 offset;

    // Use this for initialization
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");
        pickUpCount = pickups.Length;
        //winText.gameObject.SetActive(false);
        joystick = FindObjectOfType<Joystick>();
        hitSound = GetComponent<AudioSource>();
        UpdatePlayerRandomColor(rb);
        offset = transform.position - rb.transform.position;

        playerCamera = GameObject.Instantiate<Camera>(playerCamera);
        if (isLocalPlayer)
        {
            playerCamera.enabled = true;
        }
        else
        {
            playerCamera.enabled = false;
        }
    }

    private void UpdatePlayerRandomColor(Rigidbody rb)
    {
      
        System.Random random = new System.Random();
        Color randomColor = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
        rb.GetComponent<Renderer>().material.color = randomColor;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    //Follow cameras, Procedural Animations, Gathering last known states
    private void LateUpdate()
    {
        
    }

    //Called after processing physics
    private void FixedUpdate()
    {
        if (!isGameOver && isLocalPlayer)
        {
            float moveHorizontal = 0;
            float moveVertical = 0;

            #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
                        moveHorizontal = joystick.Horizontal;
                        moveVertical = joystick.Vertical;
            #else
                        moveHorizontal = Input.GetAxis("Horizontal");
                        moveVertical = Input.GetAxis("Vertical");
            #endif

            bool isMoving = moveHorizontal > 0 || moveVertical > 0;

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * speed;
            rb.velocity = movement;

        }
    }

    //Starting to collide
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count++;
            UpdateCountText();
            hitSound.Play();

            if (count == pickUpCount)
            {
                UpdateWinText();
                isGameOver = true;
            }
        }
    }

    private void UpdateCountText()
    {
        countText.text = "Score: " + count;
    }

    private void UpdateWinText()
    {
        winText.text = "You Win!";
        winText.gameObject.SetActive(true);
    }

   

    [Command]
    public void CmdSpawn() {
    }
}
