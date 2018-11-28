using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : MultiplayerBehaviour
{
    public float speed;
    private Rigidbody rb;
    public Text countText;
    public Text winText;
    public GameObject winTextGO;
    private int count;
    private int pickUpCount;
    private bool isGameOver = false;
    protected JoystickButton joystickButton;
    protected Joystick joystick;
    public AudioSource hitSound;
    public Camera playerCamera;
    public Text debugText;
    private int playerCount = 0;

    
    public override void OnStartLocalPlayer()
    {
        if (isServer)
        {
            /*//empty player for showing nothing in client, just server's player
            GameObject newPlayer = Instantiate(gameObject, transform.position, transform.rotation);

            //add to Registered Spawnable list the resource for spawn in client
            GameObject resource = Resources.Load("Cube") as GameObject;
            System.Collections.Generic.List<GameObject> spawnPrefabs = FindObjectOfType<NetworkManager>().spawnPrefabs;
            FindObjectOfType<NetworkManager>().spawnPrefabs.Add(resource.gameObject);

            NetworkServer.ReplacePlayerForConnection(base.connectionToClient, newPlayer, 0);

            NetworkServer.Destroy(gameObject); //This line of code will destroy Server's emptyPlayer after it's new playerObj is Instantiated.

            //spawn this player in client
            NetworkServer.Spawn(newPlayer);


            foreach (GameObject prefab in spawnPrefabs)
            {
                if (prefab != null)
                {
                    ClientScene.RegisterPrefab(prefab);
                }

            }*/

           /* GameObject ground = GameObject.FindGameObjectWithTag("Ground");
            Vector3 groundSize = ground.GetComponent<Collider>().bounds.size;
            float x = UnityEngine.Random.Range(UnityEngine.Random.Range((groundSize.x / 2)*-1, groundSize.x / 2), groundSize.x / 2);
            float z = UnityEngine.Random.Range(UnityEngine.Random.Range((groundSize.y / 2) * -1, groundSize.y / 2), groundSize.z / 2);
            float y = gameObject.transform.position.y;

            player.transform.position = new Vector3(x, y, z);*/
            //transform.Translate(x, y, z, Space.World);
        }


    }


    // Use this for initialization
    private void Start()
    {
        count = 0;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");

        pickUpCount = pickups.Length;
        joystick = FindObjectOfType<Joystick>();
        hitSound = GetComponent<AudioSource>();

        playerCamera = Instantiate<Camera>(playerCamera);

        winTextGO = GameObject.FindGameObjectWithTag("WinText");

        if (isLocalPlayer)
        {
            UpdatePlayerRandomColor(rb);
            playerCount++;
            rb.name = "P" + Guid.NewGuid();
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
        PrintVariables();
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

            //Change player rotation to follow direction
            transform.rotation = Quaternion.LookRotation(movement, Vector3.up);
           
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
    public void CmdSpawn()
    {


    }

    private void OnGUI()
    {

    }

    private void PrintVariables()
    {
        if (rb != null && winTextGO != null)
        {
            winTextGO.GetComponent<Text>().text = "CurrentPlayer: " + rb.name.ToString() + ", IsLocal: " + rb.GetComponent<NetworkIdentity>().isLocalPlayer;
        }

    }
}
