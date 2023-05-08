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
            // se controla que si no es el mismo no mueva al resto
            if (!IsOwner)
            {
                return;
            }
            // si es el server o host se mueve directamente sino se llama a la variable tipo ServerRpc que realiza el movmiento
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

        public void MoveBack()
        {
            if (!IsOwner)
            {
                return;
            }
            if (NetworkManager.Singleton.IsServer)
            {
                transform.position += Vector3.back;
                Position.Value = transform.position;
            }
            else
            {
                MoveToBackServerRpc();
            }
        }

        public void MoveLeft()
        {
            if (!IsOwner)
            {
                return;
            }
            if (NetworkManager.Singleton.IsServer)
            {
                transform.position += Vector3.left;
                Position.Value = transform.position;
            }
            else
            {
                MoveToLeftServerRpc();
            }
        }

        public void MoveRight()
        {
            if (!IsOwner)
            {
                return;
            }
            if (NetworkManager.Singleton.IsServer)
            {
                transform.position += Vector3.right;
                Position.Value = transform.position;
            }
            else
            {
                MoveToRightServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }

        // se crea un metodo ServerRpc para cada tipo de movimiento
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
            // se controla la entrada de input para el movimiento del player
            // se llama a la funcion que realiza el movimiento acorde
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("mover arriba");
                MoveForward();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("mover abajo");
                MoveBack();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("mover izquierda");
                MoveLeft();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("mover derecha");
                MoveRight();
            }

            // se ajusta el valor de la variable de red al transform del player
            transform.position = Position.Value;
        }
    }
}
