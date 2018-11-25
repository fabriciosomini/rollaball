using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
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

    // Use this for initialization
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");
        pickUpCount = pickups.Length;
        winText.gameObject.SetActive(false);
        //joystickButton = GetComponent<JoystickButton>();
        joystick = FindObjectOfType<Joystick>();
        hitSound = GetComponent<AudioSource>();
        
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
        if (!isGameOver)
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
        
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            //rb.AddForce(movement * speed);
            rb.velocity = movement * speed;
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
}
