using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField joinInput; // Input field for joining a room
    public Button createButton;  // Button for creating a room
    public Button joinButton;    // Button for joining a room

    // Start is called before the first frame update
    void Start()
    {
        // Check if the client is disconnected before connecting to Photon
        if (!PhotonNetwork.IsConnected)
        {
            // Initially disable buttons until connection is established
            createButton.interactable = false;
            joinButton.interactable = false;

            // Connect to Photon using the settings from the PhotonServerSettings file
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            // If already connected, enable the buttons
            createButton.interactable = true;
            joinButton.interactable = true;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");

        // Enable the buttons for room creation and joining
        createButton.interactable = true;
        joinButton.interactable = true;
    }

    public void CreateRoom()
    {
        string roomName = "1234"; // Example room name
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);

        // Load the game scene (e.g., "BoardGame")
        PhotonNetwork.LoadLevel("BoardGame");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room: " + message);
    }
}
