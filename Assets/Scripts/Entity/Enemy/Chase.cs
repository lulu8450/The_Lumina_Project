using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public List<Transform> points = new ();
    public int pointNumber;
    public Transform currentPoint;
    public float moveSpeed;
    public GameObject player;
    public float chaseDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PatrolPointUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        SearchChase();
    }

    public void PatrolPointUpdate() 
    { 
        pointNumber++;

        if (pointNumber > points.Count-1) 
        {
            pointNumber = 0;
        }

        currentPoint = points[pointNumber];
    }


    public float Distance() 
    {
        float distance = Vector2.Distance(transform.position, currentPoint.position);

        return distance;
    }

    public void SearchChase() 
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance <= chaseDistance)
        {
            Chase();
        }
        else 
        {
            EnemyPatrol();
        }

    }


    public void Chase() 
    {

        transform.position = Vector2.MoveTowards(transform.position,player.transform.position, moveSpeed * Time.deltaTime);


    }

    public void EnemyPatrol() 
    {
        if (currentPoint != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, moveSpeed * Time.deltaTime);


            if (Distance() <= 2f)
            {
                PatrolPointUpdate();
            }

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}