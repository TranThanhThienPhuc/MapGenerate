using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenInstant : MonoBehaviour
{
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] Vector2Int mazeSize;
    [SerializeField] GameObject player;
    [SerializeField] GameObject destination;
    int speed = 25;
    int startingPoint = new();
    int endPoint = new();
    public GeneratingEstimating waitCode;

    private void Start()
    {
        //Instant map generate
        StartCoroutine(GenerateMapInstant(mazeSize));

    }

    IEnumerator GenerateMapInstant(Vector2Int size)
    {
        
        List<MazeNode> nodes = new List<MazeNode>();
        //Create nodes
        int count = 0;
        for (int x = 0; x < size.y; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                count++;
                Vector3 nodePos = new Vector3(x - (size.x / 2f), 0, y - (size.y / 2f));
                MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity);
                nodes.Add(newNode);
                if (count % speed == 0) { yield return new WaitForEndOfFrame(); count = 0; }
            }
        }

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        //Choosing start node
        startingPoint = Random.Range(0, nodes.Count);
        currentPath.Add(nodes[startingPoint]); ;

        //Choosing end node
        endPoint = Random.Range(0, nodes.Count);
        while (endPoint < Mathf.Abs(startingPoint + 5))
        {
            endPoint = Random.Range(0, nodes.Count);
        }
        count = 0;
        //Maze Generation Loop
        while (completedNodes.Count < nodes.Count)
        {
            count++;
            if (completedNodes.Count % speed * 5 == 0) { yield return new WaitForEndOfFrame(); count = 0; }

            //Path Finding(Neighbors)
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            //Calculates the (x,y) positions
            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            if (currentNodeX < size.x - 1)
            {
                // Check node to the right of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }

            }
            if (currentNodeX > 0)
            {
                // Check node to the left of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            if (currentNodeY < size.y - 1)
            {
                // Check node above the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0)
            {
                // Check node below the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // Choose next node
            if (possibleDirections.Count > 0)
            {
                // Checking if there are valid directions
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                // Chose random direction
                switch (possibleDirections[chosenDirection])
                {
                    case 1: // If go right
                        chosenNode.RemoveWalls(1);
                        currentPath[currentPath.Count - 1].RemoveWalls(0); // Remove Right Wall
                        break;
                    case 2: // If go left
                        chosenNode.RemoveWalls(0);
                        currentPath[currentPath.Count - 1].RemoveWalls(1); // Remove Left Wall
                        break;
                    case 3: // If go up
                        chosenNode.RemoveWalls(3);
                        currentPath[currentPath.Count - 1].RemoveWalls(2); // Remove Up Wall
                        break;
                    case 4: // If go down
                        chosenNode.RemoveWalls(2);
                        currentPath[currentPath.Count - 1].RemoveWalls(3); // Remove Down Wall
                        break;
                }

                currentPath.Add(chosenNode);
            }
            // Track Back if none valid directions
            else
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);

                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }

        for (int i = 0; i < nodes.Count; i++) 
        {
            if (i % speed == 0) { yield return new WaitForEndOfFrame();}

            MeshRenderer[] reders = completedNodes[i].GetComponentsInChildren<MeshRenderer>();
            for (int j = 0; j < reders.Length; j++) {
                reders[j].enabled = true;
            }
        }

        waitCode.WaitingDone();
        Instantiate(player, nodes[startingPoint].transform.position, transform.rotation);
        Instantiate(destination, nodes[endPoint].transform.position, transform.rotation);
    }
}
