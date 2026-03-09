using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    private List<Vector3> path;
    private AstarAlgorithm Astar;
    private int index = 0;
    private bool reached_Destination = false;

    [SerializeField] private Transform _End;
    [SerializeField] private float rotation_Speed = 10;
    [SerializeField] private float movement_Speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        Astar = AINavigation.instance.Astar;
        //path = Astar.find_Path(transform.position, _End.position);
        InvokeRepeating(nameof(Update_Path), 0f, 0.5f);
        reached_Destination = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null || path.Count == 0) return;
        Movement();
    }

    private void OnDrawGizmos()
    {
        if (path == null) return;
        for (int i = 0; i < path.Count-1; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(path[i], 0.5f);
        }

    }

    private void Movement()
    {
        if (reached_Destination)
        {
            return;
        }
        
        Vector3 Goal = path[index];
        Vector3 direction = (Goal - transform.position).normalized;

        if (direction.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(direction),Time.deltaTime * rotation_Speed);
            transform.Translate(0,0,movement_Speed*Time.deltaTime);
        }
        else
        {
            if (index < path.Count-1)
            {
                index++;
            }
            else
            {
                reached_Destination = true;
            }
        }

    }
    private void Update_Path()
    {
        List<Vector3> new_Path = Astar.find_Path(transform.position, _End.position);

        if (new_Path == null || new_Path.Count == 0) return;

        path = new_Path;

        reached_Destination = false;
    }

}
