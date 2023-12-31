﻿using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private float speed = 10;
        [SerializeField] private float jumpPower = 10;
        [SerializeField] private float jumpDelay = 0.5f;
        [SerializeField] private bool jumpReady = false;
        [SerializeField] private Renderer playerRenderer;
        [SerializeField] private Bullet[] bulletPrefab;
        [SerializeField] private Transform shootPoint;


        [SerializeField]
        private NetworkVariable<Vector2> input = new(
            Vector2.zero,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
            );
        [SerializeField]
        private NetworkVariable<bool> jump = new(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
            );
        [SerializeField]
        private NetworkVariable<Color> playerColor = new(
           Color.white,
           NetworkVariableReadPermission.Everyone,
           NetworkVariableWritePermission.Owner
       );

        private Color previousColor;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                playerColor.OnValueChanged += UpdatePlayerColor;

                playerColor.Value = playerRenderer.material.color;
            }

            else
            {
                playerRenderer.material.color = playerColor.Value;
            }
            base.OnNetworkSpawn();
        }

        private void UpdatePlayerColor(Color previousColor, Color newColor)
        {
            playerRenderer.material.color = newColor;

            if (IsServer)
            {
                playerColor.Value = newColor;
            }

            else if (IsClient)
            {
                UpdatePlayerColorOnServerRpc(newColor);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void UpdatePlayerColorOnServerRpc(Color newColor)
        {
            playerRenderer.material.color = newColor;
        }

        [ServerRpc]
        private void ShootServerRpc()
        {
            var randomBullet = Random.Range(0, bulletPrefab.Length);
            var bullet = Instantiate(bulletPrefab[randomBullet], shootPoint.position, Quaternion.identity);
            bullet.OnNetworkSpawn(shootPoint.forward);
        }
        private void Update()
        {
            if (IsOwner)
            {
                input.Value = new Vector2(
                    Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical")
                    );

                jump.Value = Input.GetKey(KeyCode.Space);

                if (Input.GetKeyDown(KeyCode.C))
                {
                    playerColor.Value = new Color(Random.value, Random.value, Random.value);
                }

                if (Input.GetMouseButtonDown(0)) 
                {
                    ShootServerRpc();
                }


            }

            if (IsServer)
            {
                var direction = new Vector3(input.Value.x, 0, input.Value.y);
                rigidbody.AddForce(
                    direction * speed * Time.deltaTime,
                    ForceMode.Impulse
                );
                if (direction != Vector3.zero)
                {
                    rigidbody.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                }


                if (jump.Value && jumpReady)
                {
                    StartCoroutine(JumpDelay());
                    rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                }

            }
        }

        private IEnumerator JumpDelay()
        {
            jumpReady = false;
            yield return new WaitForSeconds(jumpDelay);
            jumpReady = true;
        }
    }


}
