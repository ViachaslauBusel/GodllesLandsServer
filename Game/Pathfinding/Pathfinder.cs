using Game.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Game.Pathfinding
{
    public class Node
    {

        public Node(Vector3 position)
        {
           Position = position;
            Coordinate = new Vector2Int((int)(position.X / Pathfinder.CELL_SIZE), (int)(position.Z / Pathfinder.CELL_SIZE));
        }
        public Node(Node parent, Vector2Int offset)
        {
            Coordinate = parent.Coordinate + offset;
            Position = new Vector3(parent.Position.X + offset.X * Pathfinder.CELL_SIZE, parent.Position.Y, parent.Position.Z + offset.Y * Pathfinder.CELL_SIZE);
        }

        public Node(Node cameFrom, int weightToFinish, int weightFromStart, Node currentWaypoint)
        {
            WeightFromStart = weightFromStart;
        }

        public Vector3 Position { get; set; }
        public Vector2Int Coordinate { get; set; }
        public int WeightToFinish { get; internal set; }
        public int Weight => WeightToFinish + WeightFromStart;
        public int WeightFromStart { get; internal set; }
        public Node CameFromNode { get; internal set; }

        public static bool operator ==(Node a, Node b)
        {
            return a.Coordinate == b.Coordinate;
        }
        public static bool operator !=(Node a, Node b)
        {
            if (a is null && b is null) return false;
            if (a is null || b is null) return true;
            return a.Coordinate != b.Coordinate;
        }
    }
    public class Pathfinder
    {
        public const float CELL_SIZE = 1.0f;
        private RaycastingService _raycastingService;
        private List<Node> _closed = new List<Node>();
        private List<Node> _open = new List<Node>();
        private object _locker = new object();

        [Inject]
        private void InjectServices(RaycastingService raycastingService)
        {
            _raycastingService = raycastingService;
        }

        public Vector3[] CalculatePath(Vector3 startPosition, Vector3 endPosition)
        {
            try
            {
                long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                Monitor.Enter(_locker);
                // Clear all lists
                _closed.Clear();
                _open.Clear();

                Node startNode = new Node(startPosition);
                Node endNode = new Node(endPosition);

                //The endpoint is equals current node
                if (startNode == endNode)
                {
                    return new Vector3[] { endNode.Position };
                }

                //Weight of path length to end point
                int weightToFinish = CalculateWeightFinish(startNode, endNode);
                startNode.WeightToFinish = weightToFinish;
                _open.Add(startNode);

                Node currentWaypoint = startNode;
                //Maximum number of search cycles
                int cicle = 0, maxCicle = 1_000;
                while (_open.Count > 0 && cicle++ < maxCicle)
                {
                    //Finding the current node from an open list, with minimum weight
                    currentWaypoint = GetWaypointWithMinWeight(_open);

                    //If the end point has been reached
                    if (currentWaypoint.WeightToFinish == 0)
                    {
                        _closed.Add(currentWaypoint);
                        break;
                    }

                    {//The node that was involved in the search for nearby nodes is moved from the open to the closed list of nodes
                        _open.Remove(currentWaypoint);
                        _closed.Add(currentWaypoint);
                    }

                    ProcessNeighbours(currentWaypoint, endNode);
                }

                Node lastWaypoint = _closed.OrderBy(w => w.WeightToFinish).FirstOrDefault();
                LinkedList<Vector3> result = new LinkedList<Vector3>();

                //We create a chain of nodes from the beginning to the end.
                while (lastWaypoint != null)
                {
                    result.AddFirst(lastWaypoint.Position);
                    lastWaypoint = lastWaypoint.CameFromNode;
                }

                return result.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Monitor.Exit(_locker);
            }
        }

        private void ProcessNeighbours(Node currentWaypoint, Node endCell)
        {
            // Iterate over all neighbors of the current node
            foreach (Node neighbour in GetNeigbourCells(currentWaypoint))
            {
                // If the neighbour node in closed, skip to the next neighbour
                if (_closed.Any(c => c.Equals(neighbour))) continue;

                // Calculate the weight to start for the path
                int weightFromStart = currentWaypoint.WeightFromStart + CalculateWeightStart(neighbour, currentWaypoint);

                // Check if the neighbour node is in the open list
                Node neighbourWaypoint = _open.FirstOrDefault(w => w == neighbour);

                // If the neighbour node is in the open list and its weight from the start is less than the current one, skip to the next neighbour
                if (neighbourWaypoint != null)
                {
                    if (neighbourWaypoint.WeightFromStart < weightFromStart) continue;
                    // If the neighbour node is already in the open list, update its data
                    neighbourWaypoint.CameFromNode = currentWaypoint;
                    neighbourWaypoint.WeightFromStart = weightFromStart;
                }
                else
                {
                    // If the neighbour node is not in the open list, add it as a new waypoint
                    neighbour.WeightToFinish = CalculateWeightFinish(neighbour, endCell);
                    neighbour.WeightFromStart = weightFromStart;
                    neighbour.CameFromNode = currentWaypoint;

                    _open.Add(neighbour);
                }
            }
        }

        private int CalculateWeightFinish(Node currentCell, Node endCell)
        {
            return (int)(Math.Abs(endCell.Coordinate.X - currentCell.Coordinate.X) + Math.Abs(endCell.Coordinate.Y - currentCell.Coordinate.Y) * 10.0f);
        }

        private int CalculateWeightStart(Node cell, Node startCell)
        {
            bool isDiagonal = Math.Abs(cell.Coordinate.X - startCell.Coordinate.X) == 1 && Math.Abs(cell.Coordinate.Y - startCell.Coordinate.Y) == 1;
            return isDiagonal ? 14 : 10;
        }

        private IEnumerable<Node> GetNeigbourCells(Node currentWaypoint)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0
                    || Math.Abs(i) == 1 && Math.Abs(j) == 1) continue;
                    Node cell = new Node(currentWaypoint, new Vector2Int(i, j));
                    yield return cell;
                }
            }
        }

        private Node GetWaypointWithMinWeight(List<Node> list)
        {
            return list.OrderBy(w => w.Weight).First();
        }
    }
}
