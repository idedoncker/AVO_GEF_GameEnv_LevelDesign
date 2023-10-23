using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Generates maze according to the "recursive backtracker" algorithm,
 * also known as a randomized version of the depth-first search algorithm.
 */
public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeCell mazeCellPrefab;

    [SerializeField] int mazeWidth;

    [SerializeField] int mazeDepth;

    MazeCell[,] mazeGrid;

    void Start()
    {
        // initialise array with specified width and depth
        mazeGrid = new MazeCell[mazeWidth, mazeDepth];

        // fill array with maze cells
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeDepth; z++)
            {
                mazeGrid[x, z] = Instantiate(mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity); // Quaternion.identity = "no rotation"
            }
        }

        // start algorithm at the first cell
        GenerateMaze(null, mazeGrid[0, 0]);

        // create entrance and exit
        var entranceCell = mazeGrid[mazeWidth / 2, 0];
        entranceCell.ClearBackWall();

        // var exitCell = mazeGrid[mazeWidth / 2, mazeDepth - 1];
        // exitCell.ClearFrontWall();
    }

    /**
     * Recursively generates a maze until every cell has been visited.
     */
    void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        // make all cell walls visible
        currentCell.Visit();

        // knock down walls between previous and current cell
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        // keep the algorithm going until there are no unvisited cells remaining
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    /**
     * Returns a random unvisited neighbouring cell for a given cell.
     */
    MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        // get all unvisited neigbours for current cell
        var unvisitedCells = GetUnvisitedCells(currentCell);

        // order cells randomly and return the first of the list, or null
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    /**
     * Returns all unvisted neighbours for a given cell.
     */
    IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        // corresponds to the index of the cell in the grid, as each cell is one unit in size
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;


        // collect unvisited cell to the right
        if (x + 1 < mazeWidth)
        {
            var cellToRight = mazeGrid[x + 1, z];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        // collect unvisited cell to the left
        if (x - 1 >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        // collect unvisited cell in front
        if (z + 1 < mazeDepth)
        {
            var cellToFront = mazeGrid[x, z + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        // // collect unvisited cell behind
        if (z - 1 >= 0)
        {
            var cellToBack = mazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    /** 
     * Knocks down the walls between the previous cell and the current one.
     */
    void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        // don't clear walls if there is no previous cell (currentCell = "first cell")
        if (previousCell == null)
        {
            return;
        }

        // clear walls from left to right
        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        // clear walls from right to left
        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        // clear walls from back to front
        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        // clear walls from front to back
        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }
}
