using UnityEngine;

public class CameraController : MultiplayerBehaviour
{
    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }


    //Follow cameras, Procedural Animations, Gathering last known states
    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            playerPosition.y = 25;
            transform.position = playerPosition;// + offset;
        }
    }


}
