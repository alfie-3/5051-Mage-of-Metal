using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

}
