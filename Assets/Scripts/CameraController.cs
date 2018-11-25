using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCameraController : NetworkBehaviour
{

    private GameObject player;
    private Vector3 offset;

    // Use this for initialization
    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        player = players.FirstOrDefault(p => { return p.GetComponent<NetworkIdentity>().isLocalPlayer; });
        System.Collections.Generic.List<NetworkIdentity> identities = players.Select(p => p.GetComponent<NetworkIdentity>()).ToList();
        offset = transform.position - player.transform.position;

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
            transform.position = player.transform.position + offset;
        }
    }


}
