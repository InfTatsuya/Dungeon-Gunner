using System;
using System.Collections.Generic;
using UnityEngine;

public static class AStar 
{
    public static Stack<Vector3> BuildPath(Room room, Vector3Int startGridPosition, Vector3Int endGridPosition)
    {
        startGridPosition -= (Vector3Int)room.templateLowerBounds;
        endGridPosition -= (Vector3Int)room.templateLowerBounds;

        List<Node> openNodeList = new List<Node>();
        HashSet<Node> closedNodeHashSet = new HashSet<Node>();

        GridNodes gridNodes = new GridNodes(
            room.templateUpperBounds.x - room.templateLowerBounds.x + 1,
            room.templateUpperBounds.y - room.templateLowerBounds.y + 1);

        Node startNode = gridNodes.GetGridNode(startGridPosition.x, startGridPosition.y);
        Node targetNode = gridNodes.GetGridNode(endGridPosition.x, endGridPosition.y);

        Node endPathNode = FindShortestPath(startNode, targetNode, gridNodes, openNodeList,
            closedNodeHashSet, room.instantiatedRoom);

        if(endPathNode != null)
        {
            return CreatePathStack(endPathNode, room);
        }
        else
        {
            return null;
        }
    }

    private static Node FindShortestPath(Node startNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, InstantiatedRoom instantiatedRoom)
    {
        openNodeList.Add(startNode);

        while(openNodeList.Count > 0)
        {
            openNodeList.Sort();

            Node currentNode = openNodeList[0];
            openNodeList.RemoveAt(0);

            if(currentNode == targetNode)
            {
                return currentNode;
            }

            closedNodeHashSet.Add(currentNode);

            EvaluateCurrentNodeNeighbours(currentNode, targetNode, gridNodes,
                openNodeList, closedNodeHashSet, instantiatedRoom);
        }

        return null;
    }

    private static Stack<Vector3> CreatePathStack(Node targetNode, Room room)
    {
        Stack<Vector3> movementPathStack = new Stack<Vector3>();

        Node nextNode = targetNode;

        //Get mid point of cell
        Vector3 cellMidPoint = room.instantiatedRoom.grid.cellSize * 0.5f;
        cellMidPoint.z = 0f;

        while(nextNode != null)
        {
            Vector3 worldPos = room.instantiatedRoom.grid.CellToWorld(new Vector3Int(
                nextNode.gridPosition.x + room.templateLowerBounds.x,
                nextNode.gridPosition.y + room.templateLowerBounds.y,
                0));

            worldPos += cellMidPoint;

            movementPathStack.Push(worldPos);

            nextNode = nextNode.parentNode;
        }

        return movementPathStack;
    }

    private static void EvaluateCurrentNodeNeighbours(Node currentNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, InstantiatedRoom instantiatedRoom)
    {
        Vector2Int currentNodeGridPosition = currentNode.gridPosition;

        Node validNeighbourNode;

        for(int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                validNeighbourNode = GetValidNodeNeighbour(
                    currentNodeGridPosition.x + i,
                    currentNodeGridPosition.y + j, 
                    gridNodes, 
                    closedNodeHashSet,
                    instantiatedRoom);

                if(validNeighbourNode != null)
                {
                    int newCostToNeighbour;

                    int movementPenaltyForGridSpace = instantiatedRoom.aStarMovementPenalty[
                        validNeighbourNode.gridPosition.x,
                        validNeighbourNode.gridPosition.y];

                    newCostToNeighbour =    currentNode.gCost + 
                                            GetDistance(currentNode, validNeighbourNode) +
                                            movementPenaltyForGridSpace;

                    bool isValidNeighbourNodeInOpenList = openNodeList.Contains(validNeighbourNode);

                    if(newCostToNeighbour < validNeighbourNode.gCost || !isValidNeighbourNodeInOpenList)
                    {
                        validNeighbourNode.gCost = newCostToNeighbour;
                        validNeighbourNode.hCost = GetDistance(validNeighbourNode, targetNode);
                        validNeighbourNode.parentNode = currentNode;

                        if (!isValidNeighbourNodeInOpenList)
                        {
                            openNodeList.Add(validNeighbourNode);
                        }
                    }
                }
            }
        }
    }


    private static int GetDistance(Node node1, Node node2)
    {
        int deltaX = Mathf.Abs(node1.gridPosition.x - node2.gridPosition.x);
        int deltaY = Mathf.Abs(node1.gridPosition.y - node2.gridPosition.y);

        if(deltaX > deltaY)
        {
            return 14 * deltaY + 10 * (deltaX - deltaY);
        }
        else
        {
            return 14 * deltaX + 10 * (deltaY - deltaX);
        }
    }
    private static Node GetValidNodeNeighbour(int neightbourNodeXPos, int neightbourNodeYPos, GridNodes gridNodes, HashSet<Node> closedNodeHashSet, InstantiatedRoom instantiatedRoom)
    {
        if( neightbourNodeXPos >= instantiatedRoom.room.templateUpperBounds.x - instantiatedRoom.room.templateLowerBounds.x ||
            neightbourNodeXPos < 0 ||
            neightbourNodeYPos >= instantiatedRoom.room.templateUpperBounds.y - instantiatedRoom.room.templateLowerBounds.y ||
            neightbourNodeYPos < 0)
        {
            return null;
        }

        Node neighbourNode = gridNodes.GetGridNode(neightbourNodeXPos, neightbourNodeYPos);

        int movementPenalty = instantiatedRoom.aStarMovementPenalty[neightbourNodeXPos, neightbourNodeYPos];

        if (closedNodeHashSet.Contains(neighbourNode) || movementPenalty == 0)
        {
            return null;
        }
        else
        {
            return neighbourNode;
        }
    }
}