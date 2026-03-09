using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;

public class AINavigation : MonoBehaviour
{
    [HideInInspector]
    public static AINavigation instance;
    public AstarAlgorithm Astar;

    [Header("AI Navigation Parameters")]
    [SerializeField] private Vector3Int navigation_Grid_Size = new Vector3Int(5,5,5);
    [SerializeField] private float navigation_Cell_Size = 1;
    [SerializeField] private Vector3 _Origin = Vector3.zero;

    [Header("Debug and Visualation")]
    [SerializeField] private bool debug;
    [SerializeField] private bool show_grid;
    [SerializeField] private bool show_sphere;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }


        Astar = new AstarAlgorithm(navigation_Grid_Size.x, navigation_Grid_Size.y, navigation_Grid_Size.z, navigation_Cell_Size, _Origin);
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            for (int i = 0; i < navigation_Grid_Size.x; i++)
            {
                for (int j = 0; j < navigation_Grid_Size.y; j++)
                {
                    for (int k = 0; k < navigation_Grid_Size.z; k++)
                    {
                        if (show_grid)
                        {
                            Gizmos.color = Color.white;
                            Gizmos.DrawLine(Astar._Grid.get_WorldPosition(i, j, k, _Origin), Astar._Grid.get_WorldPosition(i + 1, j, k, _Origin));
                            Gizmos.DrawLine(Astar._Grid.get_WorldPosition(i, j, k, _Origin), Astar._Grid.get_WorldPosition(i, j + 1, k, _Origin));
                            Gizmos.DrawLine(Astar._Grid.get_WorldPosition(i, j, k, _Origin), Astar._Grid.get_WorldPosition(i, j, k + 1, _Origin));

                            Gizmos.DrawLine(Astar._Grid.get_WorldPosition(i, j + 1, k + 1, _Origin), Astar._Grid.get_WorldPosition(i + 1, j + 1, k + 1, _Origin));
                            Gizmos.DrawLine(Astar._Grid.get_WorldPosition(i + 1, j, k, _Origin), Astar._Grid.get_WorldPosition(i + 1, j, k + 1, _Origin));
                            Gizmos.DrawLine(Astar._Grid.get_WorldPosition(i, j, k + 1, _Origin), Astar._Grid.get_WorldPosition(i + 1, j, k + 1, _Origin));

                            Gizmos.DrawLine(Astar._Grid.get_WorldPosition(i + 1, j, k, _Origin), Astar._Grid.get_WorldPosition(i + 1, j + 1, k, _Origin));
                            Gizmos.DrawLine(Astar._Grid.get_WorldPosition(i, j, k + 1, _Origin), Astar._Grid.get_WorldPosition(i, j + 1, k + 1, _Origin));
                            Gizmos.DrawLine(Astar._Grid.get_WorldPosition(i + 1, j, k + 1, _Origin), Astar._Grid.get_WorldPosition(i + 1, j + 1, k + 1, _Origin));

                            Gizmos.DrawLine(Astar._Grid.get_WorldPosition(i, j + 1, k, _Origin), Astar._Grid.get_WorldPosition(i + 1, j + 1, k, _Origin));
                            Gizmos.DrawLine(Astar._Grid.get_WorldPosition(i, j + 1, k, _Origin), Astar._Grid.get_WorldPosition(i, j + 1, k + 1, _Origin));
                            Gizmos.DrawLine(Astar._Grid.get_WorldPosition(i + 1, j + 1, k, _Origin), Astar._Grid.get_WorldPosition(i + 1, j + 1, k + 1, _Origin));
                        }

                        if (show_sphere)
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawSphere(Astar._Grid.get_WorldPosition(i, j, k, _Origin), navigation_Cell_Size * 0.5f);
                        }

                    }
                }
            }

        }
        
    }

}
