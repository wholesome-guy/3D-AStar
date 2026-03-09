using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

public class Testing : MonoBehaviour
{
    private AstarAlgorithm Astar;
    private List<Vector3> path;
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;


    // Start is called before the first frame update
    void Start()
    {
        Astar = new AstarAlgorithm(5, 5, 5 , 1f, new Vector3(1,0,0));  
    }
    private void Update()
    {
        path = Astar.find_Path(start.position, end.position);
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i], path[i + 1], Color.green, 1f);
            }
        }
    }

}
