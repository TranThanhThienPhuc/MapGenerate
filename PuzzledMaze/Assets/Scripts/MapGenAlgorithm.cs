using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenAlgorithm : MonoBehaviour
{
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] Vector2Int mazeSize;
    int startingPoint = new();
    int endPoint = new();

    private void Start()
    {
        // See how map generate work
        StartCoroutine(GenerateMap(mazeSize));
    }

    IEnumerator GenerateMap(Vector2Int size)
    {
        List<MazeNode> nodes = new List<MazeNode>();

        //Create nodes
        for (int x = 0; x < size.y; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 nodePos = new Vector3(x - (size.x / 2f), 0, y - (size.y / 2f));
                MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity);
                nodes.Add(newNode);

                yield return null;
            }
        }

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        //Choosing start node
        startingPoint = Random.Range(0, nodes.Count);
        currentPath.Add(nodes[startingPoint]);
        nodes[startingPoint].SetState(NodeState.Start);

        //Choosing end node
        endPoint = Random.Range(0, nodes.Count);
        while (endPoint < Mathf.Abs(startingPoint + 5))
        {
            endPoint = Random.Range(0, nodes.Count);
        }
        nodes[endPoint].SetState(NodeState.End);

        while (completedNodes.Count < nodes.Count)
        {
            //Check nodes next to the current node
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

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
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];
                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWalls(1);
                        currentPath[currentPath.Count - 1].RemoveWalls(0);
                        break;
                    case 2:
                        chosenNode.RemoveWalls(0);
                        currentPath[currentPath.Count - 1].RemoveWalls(1);
                        break;
                    case 3:
                        chosenNode.RemoveWalls(3);
                        currentPath[currentPath.Count - 1].RemoveWalls(2);
                        break;
                    case 4:
                        chosenNode.RemoveWalls(2);
                        currentPath[currentPath.Count - 1].RemoveWalls(3);
                        break;
                }

                currentPath.Add(chosenNode);
                chosenNode.SetState(NodeState.Current);
            }

            // Track Back
            else if (possibleDirections.Count <= 0)
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);

                currentPath[currentPath.Count - 1].SetState(NodeState.Completed);
                currentPath.RemoveAt(currentPath.Count - 1);
                print("Tracking back");
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
