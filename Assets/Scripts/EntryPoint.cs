using Assets.Scripts;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private NetworkButtons networkButtons;
    [SerializeField] private NetworkManager network;

    private void Start()
    {
        networkButtons.request.AddListener(mode =>
        {
            switch (mode)
            {
                case NetworkButton.Mode.Host:
                    network.StartHost();
                    break;
                case NetworkButton.Mode.Client:
                    network.StartClient();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            networkButtons.Hide();
        }
        );
    }
}
