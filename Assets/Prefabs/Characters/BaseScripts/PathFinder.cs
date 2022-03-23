using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private DungeonGenerator dungeonGenerator;

    private List<Vector2> currentPath = new List<Vector2>();

    private PathData pathData;

    public new void Start() {
        dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();    
    }

    public void FindPath(Vector2 from, Vector2 to) {
        pathData = new PathData();
        pathData.from = from;
        pathData.to = to;
        
        pathData.unexploredNodes.Add(new PathNode(from,null,0,pathData));

        int tries = 0;
        while(tries < 100) {
            PathNode exploreTarget = GetLowestScoreNode();
            tries++;

            if(exploreTarget.coord == to) {
                BuildPath(exploreTarget);
                break;
            } else {
                ExploreAroundCoord(exploreTarget);
            }
        }
    }

    private void BuildPath(PathNode pathStart) {
        PathNode currentPathNode = pathStart;

        List<PathNode> path = new List<PathNode>();
        while(currentPathNode != null) {
            path.Add(currentPathNode);
            currentPathNode = currentPathNode.parent;
        }

        path.Reverse();
        currentPath.Clear();

        foreach(PathNode pathNode in path) {
            currentPath.Add(pathNode.coord);
        }
    }

    private PathNode GetLowestScoreNode() {
        PathNode node = null;
        float lowestScore = -1.0f;

        foreach(PathNode pathNode in pathData.unexploredNodes) {
            if(lowestScore == -1.0f || pathNode.score < lowestScore) {
                node = pathNode;
                lowestScore = pathNode.score;
            }
        }

        return node;
    }

    private void ExploreAroundCoord(PathNode pathNode) {
        pathData.unexploredNodes.Remove(pathNode);
        pathData.exploredCoords.Add(pathNode.coord);

        if(!dungeonGenerator.IsEmpty(pathNode.coord)) return;

        ExploreCoord(pathNode.coord + new Vector2(1,0),pathNode);
        ExploreCoord(pathNode.coord + new Vector2(-1,0),pathNode);
        ExploreCoord(pathNode.coord + new Vector2(0,1),pathNode);
        ExploreCoord(pathNode.coord + new Vector2(0,-1),pathNode);
    }

    private void ExploreCoord(Vector2 coord, PathNode parent) {
        if(pathData.exploredCoords.Contains(coord)) return;

        PathNode pathNode = new PathNode(coord,parent,parent.stepsFromOrigin + 1,pathData);
        pathData.unexploredNodes.Add(pathNode);
    }


    private class PathData {
        public List<PathNode> unexploredNodes = new List<PathNode>();
        public List<Vector2> exploredCoords = new List<Vector2>();

        public Vector2 from;
        public Vector2 to;
    }
    private class PathNode {
        public Vector2 coord;
        public PathNode parent;

        public float stepsFromOrigin = 0.0f;
        public float distanceToTarget = 0.0f;
        public float score = 0.0f;

        public PathNode(Vector2 coord, PathNode parent, float stepsFromOrigin, PathData pathData) {
            this.coord = coord;
            this.parent = parent;
            this.stepsFromOrigin = stepsFromOrigin;
            this.distanceToTarget = PathFinder.QuickDistanceBetweenVectors(coord,pathData.to);
            this.score = distanceToTarget + stepsFromOrigin;
        }
    }

    public static float QuickDistanceBetweenVectors(Vector2 a, Vector2 b)
    {
        return (diff(a.x, b.x) + diff(a.y, b.y));
    }

    private static float diff(float a, float b)
    {
        if (a < b)
        {
            return b - a;
        }
        else
        {
            return a - b;
        }
    }
}
