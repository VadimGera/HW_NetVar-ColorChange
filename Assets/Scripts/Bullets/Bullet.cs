using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction;
    [SerializeField] private NetworkObject networkObject;

    private void Update()
    {
        transform.Translate(direction * (Time.deltaTime * speed));
    }

    public void Spawn(Vector3 direct)
    {
        direction = direct.normalized;
        networkObject.Spawn(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer)
        {
            if (collision.collider!.TryGetComponent(out Health health))
            {
                health!.Damage(damage);
                GetComponent<NetworkObject>()!.Despawn();
            }
        }
    }
}
