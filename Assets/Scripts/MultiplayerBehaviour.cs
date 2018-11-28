using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerBehaviour : NetworkBehaviour
{
    protected GameObject[] players { get { return GameObject.FindGameObjectsWithTag("Player"); } }
    protected GameObject player { get { return players.FirstOrDefault(p => { return p.GetComponent<NetworkIdentity>().isLocalPlayer; }); } }

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }
}
