using Game.Physics.Transform;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Game.Pathfinding
{
    public class UnitPathMoverComponent : Component
    {
        private LinkedList<Vector3> _path;
        private Vector3 _destinationPoint;
        private TransformComponent _transform;
        private float _pathIndex;
        private float _speed = 5.0f;

        public bool IsMove => enabled;

        public override void Init()
        {
            _transform = GetComponent<TransformComponent>();
        }

        internal void MoveAlongPath(Vector3[] path)
        {
            if (path != null && path.Length > 0)
            {
                _destinationPoint = path[^1];
            }
            _path = new LinkedList<Vector3>(path);
            enabled = true;
        }
        
        public override void Update()
        {
            if (_path == null || _path.Count < 2)
            {
                enabled = false;
                return;
            }

            float deltaTime = MakeStep(Time.deltaTime);
            while (deltaTime > 0.001f)
            {
                deltaTime = MakeStep(deltaTime);
            }

            if (_path.Count == 1)
            {
                Move(_path.Last.Value - _transform.Position);
            }
            else if (_path.Count >= 2)
            {
                Vector3 a = _path.First.Value;
                Vector3 b = _path.First.Next.Value;

                Vector3 targetPoint = Vector3.Lerp(a, b, _pathIndex);
                Vector3 step = targetPoint - _transform.Position;

                if (step.Length() > 0.001f)
                {
                    Move(step);
                    step.Y = 0;
                }
            }
            else Debug.Log.Error("Path is empty");
        }

        private void Move(Vector3 step)
        {
            _transform.UpdatePosition(_transform.Position + step);
        }

        private float MakeStep(float deltaTime)
        {
            if (_path.Count < 2)
            {
                return 0f;
            }

            Vector3 a = _path.First.Value;
            Vector3 b = _path.First.Next.Value;

            float distance = Vector3.Distance(a, b);
            float timeToMove = distance / _speed;
            float step = deltaTime / timeToMove;

            _pathIndex += step;

            if (_pathIndex > 1.0f)
            {
                float t = _pathIndex % 1.0f;
                deltaTime = (t / step) * deltaTime;
                _pathIndex = 0f;
                _path.RemoveFirst();
                if(_path.Count >= 2)
                {
                    // Rotate to next point
                    Rotate(_path.First.Next.Value - _path.First.Value);
                }
                return deltaTime;
            }

            return 0f;
        }

        private void Rotate(Vector3 direction)
        {
            direction = Vector3.Normalize(direction);
           float angle = (float)Math.Atan2(direction.X, direction.Z);
            _transform.UpdateRotation(angle);
        }

        internal void StopMove()
        {
            _path = null;
            enabled = false;
        }
    }
}
