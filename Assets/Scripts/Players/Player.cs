using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private float speed = 10;
        [SerializeField] private NetworkVariable<Vector2> input = new(
            Vector2.zero,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);



        private void Update()
        {
            if (IsOwner)
            {
                input.Value = new Vector2(
                    Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical")
                    );

                    
            }

            if (IsServer)
            {
                var direction = new Vector3(input.Value.x, 0, input.Value.y);
                rigidbody.AddForce(
                    direction * Time.deltaTime, 
                    ForceMode.Impulse
                );
                if( direction != Vector3.zero )
                {
                    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                } 
                
            }
        }
    }

    
}
