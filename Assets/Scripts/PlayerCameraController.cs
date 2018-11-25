using UnityEngine;
using UnityEngine.Networking;

public class PlayerCameraController : NetworkBehaviour
{

    private GameObject player;
    private Vector3 offset;

    // Use this for initialization
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {

    }


    //Follow cameras, Procedural Animations, Gathering last known states
    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
