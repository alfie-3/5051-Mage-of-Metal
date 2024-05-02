//Enemy movement behaviour based on player and spline distances

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;
using Unity.Mathematics;

public class EnemyMovement : MonoBehaviour
{
    [Header("Agent settings")]
    NavMeshAgent agent;
    [SerializeField] float minAttackDistance;

    [Header("Spline references")]
    ISpline spline;
    float splineDistance = Mathf.Infinity;

    //Setup enemy and spline references
    private void Start()
    {
        spline = LevelManager.spline.GetComponent<ISplineContainer>().Splines[0];
        agent = GetComponent<NavMeshAgent>();
    }

    //Enemy movement update
    private void Update()
    {
        //Stop update function if enemy is too far from player
        if (Vector3.Distance(LevelManager.player.transform.position, transform.position) > minAttackDistance) { return; }

        //Get nearest spline point
        SplineUtility.GetNearestPoint(spline, new float3(transform.position.x, transform.position.y, transform.position.z), out float3 nearest, out float t);
        Vector3 point = new Vector3(nearest.x, nearest.y, nearest.z);

        //If spline distance is small enough, chase player instead
        splineDistance = Vector3.Distance(transform.position, point);
        if (splineDistance < 2)
        {
            //Head towards player
            agent.SetDestination(LevelManager.player.transform.position);
        }
        else
        {
            //Head towards spline point
            agent.SetDestination(point);
        }
    }
}
