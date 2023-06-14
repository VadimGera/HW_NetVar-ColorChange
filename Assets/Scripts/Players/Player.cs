using System.Collections;
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
        
        
        [SerializeField] private NetworkVariable<Vector2> input = new(
            Vector2.zero,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
            );
        [SerializeField] private NetworkVariable<bool> jump = new(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
            );



        private void Update()
        {
            if (IsOwner)
            {
                input.Value = new Vector2(
                    Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical")
                    );

                jump.Value = Input.GetKey(KeyCode.Space);

                    
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
                    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
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
