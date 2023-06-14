using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private List<StartNetworkButton> buttons;




    private void Start()
    {
        if (networkManager == null)
        {
            throw new NullReferenceException("Network manager is not assigned");
        }


        foreach (var startNetworkButton in buttons)
        {
            startNetworkButton.request.AddListener( e =>
            {
                switch (e)
                {
                    case Assets.Scripts.StartMode.Host:
                        networkManager.StartHost();
                        break;
                    case Assets.Scripts.StartMode.Client:
                        networkManager.StartClient();
                        break;
                }

            });

        }

    }



}
