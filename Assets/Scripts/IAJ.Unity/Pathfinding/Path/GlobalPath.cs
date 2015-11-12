using System;
using System.Collections.Generic;
using RAIN.Navigation.Graph;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Path
{
    public class GlobalPath : Path
    {
        public const float PATH_OFFSET = 0.2f;

        public List<NavigationGraphNode> PathNodes { get; protected set; }
        public List<Vector3> PathPositions { get; protected set; } 
        public bool IsPartial { get; set; }
        public List<LocalPath> LocalPaths { get; protected set; } 
        public float PathDistance { get; protected set; }

        public GlobalPath()
        {
            this.PathNodes = new List<NavigationGraphNode>();
            this.PathPositions = new List<Vector3>();
            this.LocalPaths = new List<LocalPath>();
        }

        public void CalculateLocalPathsFromPathPositions(Vector3 initialPosition)
        {
            this.PathDistance = 0.0f;
            Vector3 previousPosition = initialPosition;
            for (int i = 0; i < this.PathPositions.Count; i++)
            {

				if(!previousPosition.Equals(this.PathPositions[i]))
				{
                    LineSegmentPath local = new LineSegmentPath(previousPosition, this.PathPositions[i]);
					previousPosition = this.PathPositions[i];
                    this.PathDistance += Vector3.Distance(local.EndPosition, local.StartPosition);
                    this.LocalPaths.Add(local);
        }
            }
        }

        public override float GetParam(Vector3 position, float previousParam)
        {
            int i = (int)Math.Floor(previousParam);

            LineSegmentPath path = LocalPaths[i] as LineSegmentPath;

            // here do stuff

            if (path.PathEnd(i - previousParam))
                return i + 1.0f;
           
            return i + path.GetParam(position, previousParam-i);

        }

        public override Vector3 GetPosition(float param)
        {
            int i = (int)Math.Floor(param);
            //HERE
            LineSegmentPath path = LocalPaths[i] as LineSegmentPath;

            return path.GetPosition(param - i);
        }

        public override bool PathEnd(float param)
        {
       
            if (param >= LocalPaths.Count - 1)
                return true;
            else
            {
                return false;
            }
        }

        public float CalculateOffset(float previousParam)
        {
            int i = (int)Math.Floor(previousParam);

            LineSegmentPath path = LocalPaths[i] as LineSegmentPath;

            float pathDistance = Vector3.Distance(path.EndPosition, path.StartPosition);
            float relativePathDistance = pathDistance / this.PathDistance;
            Debug.Log(relativePathDistance);

            if (relativePathDistance < 0.01f)
                return 1.0f - (i - previousParam);

            return 0.2f + relativePathDistance;

            return 0.2f;
        }
    }
}
