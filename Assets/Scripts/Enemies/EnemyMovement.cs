using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;
using Unity.Mathematics;

public class EnemyMovement : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] float minAttackDistance;

    GameObject player;
    ISpline spline;
    float distance = Mathf.Infinity;


    private void Start()
    {
        spline = LevelManager.spline.GetComponent<ISplineContainer>().Splines[0];
        agent = GetComponent<NavMeshAgent>();
        player = LevelManager.player;
    }

    private void Update()
    {
        if (Vector3.Distance(Camera.main.transform.position, transform.position) > minAttackDistance) { return; }

        if (spline == null) { return; }

        SplineUtility.GetNearestPoint(spline, new float3(transform.position.x, transform.position.y, transform.position.z), out float3 nearest, out float t);
        Vector3 point = new Vector3(nearest.x, nearest.y, nearest.z);
        float distance = Vector3.Distance(transform.position, point);

        if (distance < 2)
        {
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.SetDestination(point);
        }
    }

}
