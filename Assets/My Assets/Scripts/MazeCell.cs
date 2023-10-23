using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] GameObject leftWall;

    [SerializeField] GameObject rightWall;

    [SerializeField] GameObject frontWall;

    [SerializeField] GameObject backWall;

    [SerializeField] GameObject unvisitedBlock;

    public bool IsVisited { get; private set; }

    /**
     * Marks the cell as visited and makes cell walls visible.
     */
    public void Visit()
    {
        IsVisited = true;
        unvisitedBlock.SetActive(false);
    }

    /**
     * Deactivates the left wall.
     */
    public void ClearLeftWall()
    {
        leftWall.SetActive(false);
    }

    /**
     * Deactivates the right wall.
     */
    public void ClearRightWall()
    {
        rightWall.SetActive(false);
    }

    /**
     * Deactivates the front wall.
     */
    public void ClearFrontWall()
    {
        frontWall.SetActive(false);
    }

    /**
     * Deactivates the back wall.
     */
    public void ClearBackWall()
    {
        backWall.SetActive(false);
    }
}
