using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class EnemyMovement : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] float minAttackDistance;

    ISpline spline;
    GameObject player;
    float distance = Mathf.Infinity;


    private void Start()
    {
        spline = GameObject.Find("Spline").GetComponent<ISplineContainer>().Splines[0];


        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        distance = Vector3.Distance( transform.position , player.transform.position );
        if (distance < minAttackDistance)
        {
            //agent.SetDestination(player.transform.position);
            /*if (spline != null)
            {
                Debug.Log("spline is a thing");
            }
            else
            {

            }*/

            SplineUtility.GetNearestPoint(spline, new float3(transform.position.x , transform.position.y, transform.position.z), out float3 nearest, out float t);
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


}
