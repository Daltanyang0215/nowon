using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    struct Coord
    {
        public static Coord Zero = new Coord(0, 0);
        public static Coord error = new Coord(-1, -1);

        public int y;
        public int x;

        public Coord(int y, int x)
        {
            this.y = y;
            this.x = x;
        }

        public static bool operator ==(Coord op1, Coord op2)
            => (op1.y == op2.y && op1.x == op2.x);

        public static bool operator !=(Coord op1, Coord op2)
            => !(op1 == op2);

    }

    enum NodeType
    {
        Path,
        Obstacle,
        None
    }

    public enum FindingOptions
    {
        BFS,
        DFS,
        FixedWayPoints
    }
    [SerializeField] private FindingOptions _option;

    struct NodePair
    {
        public Transform node;
        public Coord coord;
        public NodeType type;
    }
    private static Transform _leftBottom;
    private static Transform _rightTop;
    private static float _width => _rightTop.position.x - _leftBottom.position.x;
    private static float _height => _rightTop.position.z - _leftBottom.position.z;
    private static float _nodeTerm = 1.0f;
    private static NodePair[,] _map;
    private static bool[,] _visited;
    private static int[,] _direction = new int[2, 4]
    {
        {-1,0,1,0 },
        {0,-1,0,1 }
    };
    private static List<List<Transform>> _pathList = new List<List<Transform>>();
    private static List<Transform> _tmpPathForDFS = new List<Transform>();

    public static void SetNodeMap()
    {
        Transform nodeParent = GameObject.Find("Nodes").transform;
        Transform[] nodes = new Transform[nodeParent.childCount];
        for (int i = 0; i < nodeParent.childCount; i++)
            nodes[i] = nodeParent.GetChild(i);

        Transform enemyPathParent = GameObject.Find("EnemyPaths").transform;
        Transform[] enemyPaths = new Transform[enemyPathParent.childCount];
        for (int i = 0; i < enemyPathParent.childCount; i++)
            enemyPaths[i] = enemyPathParent.GetChild(i);

        SetNodeMap(enemyPaths.ToList(), nodes.ToList());
    }
    public bool FindOptimizedPath(Transform startNode, Transform endNode, out List<Transform> optimizedPath)
    {
        bool found = false;
        optimizedPath = null;

        foreach (var path in _pathList)
        {
            path.Clear();
        }
        _pathList.Clear();

        for (int i = 0; i < _visited.GetLength(0); i++)
        {
            for (int j = 0; j < _visited.GetLength(1); j++)
            {
                _visited[i, j] = false;
            }
        }

        switch (_option)
        {
            case FindingOptions.BFS:
                found = BFS(FindNode(startNode).coord, FindNode(endNode).coord);
                optimizedPath = new List<Transform>(_pathList.OrderBy(path => path.Count).First());
                break;
            case FindingOptions.DFS:
                _tmpPathForDFS.Clear();
                found = DFS(FindNode(startNode).coord, FindNode(endNode).coord);
                optimizedPath = _tmpPathForDFS;
                return true;
            case FindingOptions.FixedWayPoints:
                found = FindFixedWayPoints();
                break;
            default:
                break;
        }
        return found;
    }


    private static void SetNodeMap(List<Transform> pathNodes, List<Transform> obstacleNodes)
    {
        List<Transform> nodes = new List<Transform>();
        nodes.AddRange(pathNodes);
        nodes.AddRange(obstacleNodes);
        IOrderedEnumerable<Transform> nodesFiltered = nodes.OrderBy(node => node.position.x + node.position.z);

        _leftBottom = nodesFiltered.First();
        _rightTop = nodesFiltered.Last();

        _map = new NodePair[(int)(_height / _nodeTerm) + 1, (int)(_width / _nodeTerm) + 1];
        _visited = new bool[_map.GetLength(0), _map.GetLength(1)];

        Coord tmpCoord;
        foreach (var node in pathNodes)
        {
            tmpCoord = TransformToCoord(node);
            _map[tmpCoord.y, tmpCoord.x] = new NodePair()
            {
                node = node,
                coord = tmpCoord,
                type = NodeType.Path
            };
        }
        foreach (var node in obstacleNodes)
        {
            tmpCoord = TransformToCoord(node);
            _map[tmpCoord.y, tmpCoord.x] = new NodePair()
            {
                node = node,
                coord = tmpCoord,
                type = NodeType.Obstacle
            };
        }
    }

    private static bool BFS(Coord start, Coord end)
    {
        bool isFinished = false;
        List<KeyValuePair<Coord, Coord>> parentsPairs = new List<KeyValuePair<Coord, Coord>>();
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(start);
        parentsPairs.Add(new KeyValuePair<Coord, Coord>(Coord.error, Coord.error));
        _visited[start.y, start.x] = true;

        int searchCount = 0;
        while (queue.Count > 0)
        {
            Coord parent = queue.Dequeue();

            for (int i = 0; i < _direction.GetLength(1); i++)
            {
                Coord next = new Coord(parent.y + _direction[0, i], parent.x + _direction[1, i]);

                // ¸ÊÀ» ¹þ¾î³µ´ÂÁö È®ÀÎ
                if (next.y < 0 || next.y >= _map.GetLength(0) ||
                    next.x < 0 || next.x >= _map.GetLength(1))
                    continue;

                // º®ÀÎÁö
                if (_map[next.y, next.x].type == NodeType.Obstacle)
                    continue;

                // ¹æ¹® ¿©ºÎ
                if (_visited[next.y, next.x])
                    continue;

                // Å½»ö
                searchCount++;
                parentsPairs.Add(new KeyValuePair<Coord, Coord>(parent, next));
                _visited[next.y, next.x] = true;

                // µµÂø¿©ºÎ
                if (next.y == end.y && next.x == end.x)
                {
                    isFinished = true;
                    _pathList.Add(CalcPath(parentsPairs, start, end));
                }
                else
                {
                    queue.Enqueue(next);
                }
            }
        }
        return isFinished;
    }

    private static bool DFS(Coord start, Coord end)
    {
        _visited[start.y, start.x] = true;

        _tmpPathForDFS.Add(GetNode(start));

        if (start == end) return true;

        Coord next;
        for (int i = 0; i < _direction.GetLength(1); i++)
        {
            next.y = start.y + _direction[0, i];
            next.x = start.x + _direction[1, i];

            // ¸ÊÀ» ¹þ¾î³µ´ÂÁö È®ÀÎ
            if (next.y < 0 || next.y >= _map.GetLength(0) ||
                next.x < 0 || next.x >= _map.GetLength(1))
                continue;

            // º®ÀÎÁö
            if (_map[next.y, next.x].type == NodeType.Obstacle)
                continue;

            // ¹æ¹® ¿©ºÎ
            if (_visited[next.y, next.x])
                continue;

            // Å½»ö

            if (DFS(next, end)) return true;

        }
        _tmpPathForDFS.RemoveAt(_tmpPathForDFS.Count - 1);
        _visited[start.y, start.x] = false;

        return false;
    }

    private static List<Transform> CalcPath(List<KeyValuePair<Coord, Coord>> parentPairs, Coord start, Coord end)
    {
        List<Transform> path = new List<Transform>();
        Coord tmpCoord = parentPairs.Last().Value;
        path.Add(GetNode(tmpCoord));

        int index = parentPairs.Count - 1;
        while (index > 0 && parentPairs[index].Key != start)
        {
            path.Add(GetNode(parentPairs[index].Key));
            index = parentPairs.FindLastIndex(pair => pair.Value == parentPairs[index].Key);
        }
        path.Add(GetNode(start));

        path.Reverse();
        return path;
    }

    private static bool FindFixedWayPoints()
    {
        if (WayPoints.instance == null)
            return false;

        _pathList.Add(WayPoints.instance.points.ToList());
        return true;
    }

    private static Coord TransformToCoord(Transform node)
    {
        return new Coord((int)((node.position.z - _leftBottom.position.z) / _nodeTerm), (int)((node.position.x - _leftBottom.position.x) / _nodeTerm));
    }

    private static NodePair FindNode(Transform node)
    {
        Coord coord = TransformToCoord(node);
        for (int i = 0; i < _map.GetLength(1); i++)
        {
            for (int j = 0; j < _map.GetLength(0); j++)
            {
                if (_map[j, i].coord == coord)
                {
                    return _map[j, i];
                }
            }

        }

        return new NodePair() { coord = Coord.error, node = null, type = NodeType.None };
    }
    private static Transform GetNode(Coord coord)
    {
        return _map[coord.y, coord.x].node;
    }

}
