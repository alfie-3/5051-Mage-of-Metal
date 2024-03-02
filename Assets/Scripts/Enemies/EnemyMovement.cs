using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] float minAttackDistance;

    GameObject player;
    float distance = Mathf.Infinity;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        distance = Vector3.Distance( transform.position , player.transform.position );
        Debug.Log(distance);
        if (distance < minAttackDistance)
        {
            agent.SetDestination(player.transform.position);
        }
    }

}
