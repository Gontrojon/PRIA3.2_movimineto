using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        public void MoveForward()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                transform.position += Vector3.forward;
                Position.Value = transform.position;
            }
            else
            {
                MoveToForwardServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }
        [ServerRpc]
        void MoveToForwardServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value += Vector3.forward;
        }
        [ServerRpc]
        void MoveToBackServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value += Vector3.back;
        }
        [ServerRpc]
        void MoveToRightServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value += Vector3.right;
        }
        [ServerRpc]
        void MoveToLeftServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value += Vector3.left;
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("mover arriba");
                MoveForward();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("mover abajo");
                MoveToBackServerRpc();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("mover izquierda");
                MoveToLeftServerRpc();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("mover derecha");
                MoveToRightServerRpc();
            }
            transform.position = Position.Value;
        }
    }
}
