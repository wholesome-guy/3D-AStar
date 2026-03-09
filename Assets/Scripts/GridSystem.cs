using UnityEngine;



public class GridNode
{
    public Vector3Int grid_Position;

    //distance from start node
    public int gCost;
    //distance from end node
    public int hCost;
    //g plus h
    public int fCost;

    public GridNode previous_Node;

    //Constructor
    public GridNode(Vector3Int grid_position)
    {
        this.grid_Position = grid_position;
    }

    public void calculate_FCost()
    {
        fCost = gCost + hCost;
    }

    

}
public class GridSystem
{
    //dimensions of the grid
    private int x_Index;
    private int y_Index;
    private int z_Index;

    private float cell_Size;
    private Vector3 _Origin;

    private GridNode[,,] grid_Array;

    public GridSystem(int x, int y, int z, float cell_Size,Vector3 origin)
    {
        this.x_Index = x;
        this.y_Index = y;
        this.z_Index = z;
        this.cell_Size = cell_Size;
        this._Origin = origin;

        grid_Array = new GridNode[x_Index, y_Index, z_Index];

        for (int i = 0; i < grid_Array.GetLength(0); i++)
        {
            for (int j = 0; j < grid_Array.GetLength(1); j++)
            {
                for (int k = 0; k < grid_Array.GetLength(2); k++)
                {
                    grid_Array[i, j, k] = new GridNode(new Vector3Int(i, j, k));
                }
            }
        }
    }

    public Vector3 get_WorldPosition(int x, int y, int z,Vector3 origin) 
    {
        return new Vector3(x, y, z) * cell_Size + origin;
    }
    public Vector3 get_WorldPosition(Vector3 grid_Position, Vector3 origin)
    {
        return grid_Position * cell_Size + origin;
    }

    public Vector3Int get_GridPosition(Vector3 world_Position,Vector3 origin)
    {
        int x = Mathf.FloorToInt((world_Position.x - origin.x) / cell_Size);
        int y = Mathf.FloorToInt((world_Position.y - origin.y) / cell_Size);
        int z = Mathf.FloorToInt((world_Position.z - origin.z) / cell_Size);
        return new Vector3Int(x, y, z);
    }
    public GridNode get_GridNode(Vector3 world_position,Vector3 origin)
    {
        Vector3Int grid_position = get_GridPosition(world_position, origin);
        return grid_Array[grid_position.x,grid_position.y,grid_position.z];
    }
    public GridNode get_GridNode(Vector3Int grid_position)
    {
        return grid_Array[grid_position.x, grid_position.y, grid_position.z];
    }

    public Vector3Int get_GridDimensions()
    {
        return new Vector3Int(x_Index,y_Index,z_Index);
    }

    

}
