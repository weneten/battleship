using Unity.Netcode;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = NetworkManager.Singleton.ConnectedClients.Count < 2; //Allow only 2 players
        response.CreatePlayerObject = true; // Spawn a player object for the client
    }
}