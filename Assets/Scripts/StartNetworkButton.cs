using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartNetworkButton : MonoBehaviour
{
    public UnityEvent<StartMode> request = new();

    [SerializeField] private StartMode startMode = StartMode.Client;

    private void Start()
    {
       GetComponent<Button>().onClick.AddListener(() =>  request.Invoke(startMode));
    }
}
