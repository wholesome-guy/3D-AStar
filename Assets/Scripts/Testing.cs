using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

public class Testing : MonoBehaviour
{
    private AstarAlgorithm Astar;
    private List<GridNode> path;
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;


    // Start is called before the first frame update
    void Start()
    {
        Astar = new AstarAlgorithm(10, 10, 10 , 1f, new Vector3(0,0,0));
        path = Astar.find_Path(start.position, end.position);
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i].grid_Position, path[i + 1].grid_Position, Color.green, float.MaxValue);
            }
        }
    }

}
