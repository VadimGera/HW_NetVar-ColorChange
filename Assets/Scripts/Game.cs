using Assets.Scripts.Players;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class Game : NetworkBehaviour
{
    [SerializeField] private WinnerScreen winScreen;
    [SerializeField] private NetworkManager network;
    [SerializeField] private List<Player> registred = new();

    private void Awake()
    {
        network.OnServerStarted += NetworkOnOnServerStarted;
    }

    public override void OnDestroy()
    {
        network.OnServerStarted -= NetworkOnOnServerStarted;
        if (network.IsServer)
        {
            network.OnClientConnectedCallback -= NetworkOnOnClientConnectedCallback;
        }
    }
    private void NetworkOnOnServerStarted()
    {
        if (network.IsServer)
        {
            foreach (var (key, value) in network.ConnectedClients)
            {
                NetworkOnOnClientConnectedCallback(key);
            }

            network.OnClientConnectedCallback += NetworkOnOnClientConnectedCallback;
        }
    }

    private void NetworkOnOnClientConnectedCallback(ulong obj)
    {
        var player = network.ConnectedClients[obj].PlayerObject.GetComponent<Player>();
        registred.Add(player);
        player.GetComponent<Health>().death.AddListener(() =>
        {
            foreach (var p in registred)
            {
                if (p.GetComponent<Health>().Alive)
                {
                    ShowScreenClientRpc(p.name);
                    return;
                }
            }

            ShowScreenClientRpc("No winners");
        });
    }
    [ClientRpc]
    private void ShowScreenClientRpc(string message)
    {
        winScreen.Show(message);
    }
}
