using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AstarAlgorithm : Physics
{
    public GridSystem _Grid;
    //Open list is the nodes TO BE SEARCHED
    private List<GridNode> open_List;
    //Closed list is the nodes which are already SEARCHED
    private List<GridNode> closed_List;

    
    private const int move_Straight_Cost = 10;
    //ROOT 2 = 1.4
    private const int move_Diagonal2D_Cost = 14;
    //ROOT 3 = 1.7
    private const int move_Diagonal3D_Cost = 17;

    private Vector3 _Origin;
    private float cell_Size;

    //Constructor
    public AstarAlgorithm(int x,int y, int z, float cell_size,Vector3 origin)
    {
        _Grid = new GridSystem(x, y, z, cell_size, origin);
        _Origin = origin;
        cell_Size = cell_size;
    }

    public List<Vector3> find_Path(Vector3 start_position, Vector3 end_position)
    {
        //start and end nodes intialised
        GridNode start_node = _Grid.get_GridNode(start_position,_Origin);
        GridNode end_node = _Grid.get_GridNode(end_position, _Origin);

        //adding start node to open list
        open_List = new List<GridNode> { start_node };
        //empty closed list
        closed_List = new List<GridNode>();

        //cycling the grid to intialise all the nodes
        for (int i = 0; i < _Grid.get_GridDimensions().x; i++)
        {
            for (int j = 0; j < _Grid.get_GridDimensions().y; j++)
            {
                for (int k = 0; k < _Grid.get_GridDimensions().z; k++)
                {
                    //getting node at each index
                    GridNode node = _Grid.get_GridNode(new Vector3Int(i, j, k));
                    //settings all values
                    // g cost is set to ininity because the algorithm hasnt explored this node yet, it will be changed onces explored
                    node.gCost = int.MaxValue;
                    node.calculate_FCost();
                    node.previous_Node = null;

                }
            }
        }

        //start node values, g = 0(distance from start node)
        start_node.gCost = 0;
        start_node.hCost = calculate_Move_Cost(start_node, end_node);
        start_node.calculate_FCost();

        //runs until open lists has nodes to be searched
        while (open_List.Count > 0)
        {
            //calculating current node with lowest f cost
            GridNode current_node = get_LowestFCost_Node(open_List);

            if(current_node == end_node)
            {
                //reached end
                return get_Path(end_node);
            }

            //removing searched nodes from open list
            open_List.Remove(current_node);
            //adding searched nodes to closed list
            closed_List.Add(current_node);

            //neighbor loop, gets neigbors of the lowest f cost current node 
            foreach(GridNode neighbor_node in get_Neighbor_Nodes(current_node))
            {
               //close the loop if its in closed list
                if (closed_List.Contains(neighbor_node)) continue;

                if (!is_node_Navigatable(neighbor_node))
                {
                    closed_List.Add(neighbor_node);
                    continue;
                }

                //estimate g cost
                int tentative_gcost = current_node.gCost + calculate_Move_Cost(current_node, neighbor_node);

                // g cost < infinity(we are checking a unexplored node!!!!)
                if (tentative_gcost < neighbor_node.gCost)
                {
                    //setting neigbors previous node as current node to back track
                    neighbor_node.previous_Node = current_node;
                    neighbor_node.gCost = tentative_gcost;
                    neighbor_node.hCost = calculate_Move_Cost(neighbor_node, end_node);
                    neighbor_node.calculate_FCost();

                    if (!open_List.Contains(neighbor_node))
                    {
                        open_List.Add(neighbor_node);
                    }
                }
            }
        }
        //out of nodes
        return null;
    }

    private int calculate_Move_Cost(GridNode a, GridNode b)
    {
        int xdist = Mathf.Abs(a.grid_Position.x - b.grid_Position.x);
        int ydist = Mathf.Abs(a.grid_Position.y - b.grid_Position.y);
        int zdist = Mathf.Abs(a.grid_Position.z - b.grid_Position.z);

        //looks for the largest value in xdist,ydist,zdist
        int largest = Mathf.Max(xdist, Mathf.Max(ydist, zdist));

        //looks for the smallest value in xdist,ydist,zdist (bottle neck for 3d diagonals)
        int smallest = Mathf.Min(xdist, Mathf.Min(ydist, zdist));

        //total-large- small = middle
        int middle = xdist + ydist + zdist - largest - smallest;

        //bottle necked by lowest coordinate value(3d diagonal uses all 3 axis at once)
        //(10,5,7) -> 3d diagonal ->(9,4,6) {bottled necked by y}
        int diag3D = smallest;

        int diag2D = middle - smallest;

        int straight = largest - middle;

        return move_Diagonal3D_Cost * diag3D
             + move_Diagonal2D_Cost * diag2D
             + move_Straight_Cost * straight;

    }

    private GridNode get_LowestFCost_Node(List<GridNode> node_list)
    {
        //linear search fro lowest f node

        //node list is open list and 0th index is always start node
        GridNode lowest_FCost_Node = node_list[0];

        for(int i =0;i < node_list.Count; i++)
        {
            //is f cost of index i'th node less than start node f cost
            if (node_list[i].fCost < lowest_FCost_Node.fCost)
            {
                lowest_FCost_Node = node_list[i];
            }
        }

        return lowest_FCost_Node;
    }

    private List<GridNode> get_Neighbor_Nodes(GridNode current_node)
    {
        //gets neigbors 
        List<GridNode> neighbor_list = new List<GridNode>();

        //current node coordinates
        int x = current_node.grid_Position.x;
        int y = current_node.grid_Position.y;
        int z = current_node.grid_Position.z;

        //grid dimenisions
        Vector3Int dimensions = _Grid.get_GridDimensions();

        //cycle from -1 to 1, neigbhors in all 3 axis
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    // Skip the current node 
                    if (i == 0 && j == 0 && k == 0) continue;

                    // x plus -1, or 1 adjacent nodes
                    int nx = x + i;
                    int ny = y + j;
                    int nz = z + k;

                    // Bounds check
                    if ((nx >= 0 && nx < dimensions.x) && (ny >= 0 && ny < dimensions.y) && (nz >= 0 && nz < dimensions.z))
                    {
                        neighbor_list.Add(get_Node(nx, ny, nz));
                    }
                }
            }
        }

        return neighbor_list;
    }

    private GridNode get_Node(int x,int y,int z)
    {
        return _Grid.get_GridNode(new Vector3Int(x,y,z));
    }

    private List<Vector3> get_Path(GridNode node)
    {
        List<GridNode> node_list = new List<GridNode>();

        //end node added to path
        node_list.Add(node);

        GridNode current_node = node;

        //backtracking, as long as the current node has a previous node(parent node) it will get all the nodes,
        //Start node has no parent node. path ends there
        while (current_node.previous_Node != null)
        {
            //adding previous node
            node_list.Add(current_node.previous_Node);
            //current node is now previours Until it reaches Start, which has no previous node
            current_node = current_node.previous_Node;
        }
        //reverse the list to get true path
        node_list.Reverse();

        List<Vector3> path_list = new List<Vector3>(node_list.Count);

        for (int i = 0; i < node_list.Count; i++)
        {
            path_list.Add(_Grid.get_WorldPosition(node_list[i].grid_Position, _Origin));
        }
        return path_list;

    }

    public bool is_node_Navigatable(GridNode node)
    {
        Vector3 world_Position = _Grid.get_WorldPosition(node.grid_Position, _Origin);

        Collider[] hits = Physics.OverlapSphere(world_Position, cell_Size * 0.5f);

        foreach (Collider hit in hits)
        {
            // Ignore triggers
            if (!hit.isTrigger)
            {
                return false;
            }
        }
        return true;
    }
}
