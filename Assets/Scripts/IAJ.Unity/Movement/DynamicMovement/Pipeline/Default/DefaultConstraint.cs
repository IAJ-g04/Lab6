﻿using Assets.Scripts.IAJ.Unity.Pathfinding.Path;
using Assets.Scripts.IAJ.Unity.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement.Pipeline
{
    public class DefaultConstraint : Constraint
    {
        public float margin { set; get; }
        public float radius { set; get; }
        public Vector3 center { set; get; }
        public int probInd { set; get; }

       
        public override Boolean WillViolate(LineSegmentPath path)
        {
                //maybe not right func TO DO
           if (MathHelper.closestParamInLineSegmentToPoint(path.StartPosition, path.EndPosition, center) < radius){
               return true;
           }
           return false;
        }

        public override TargetGoal Suggest(LineSegmentPath path, KinematicData character, TargetGoal goal)
        {

            // procurar ponto do segmento mais próximo ao centro da esfera
            Vector3 closest = path.GetPosition(MathHelper.closestParamInLineSegmentToPoint(path.StartPosition, path.EndPosition, center));
            // Check if we pass through the center point
            Vector3 newPt;
            if (closest.sqrMagnitude == 0)
            {
                // Get any vector at right angles to the segment
                Vector3 dirn =  path.EndPosition - path.StartPosition;
                //pode nao ser esta func TO DO
                Vector3 newdirn = Vector3.Cross(dirn, Vector3.Cross(dirn, Vector3.right));
                newPt = center + newdirn * radius * margin;
            }
            else
            {
                // Otherwise project the point out beyond the radius
                newPt = center + (closest - center) * radius * margin / closest.sqrMagnitude;
            }

            // Set up the goal and return
            goal.position = newPt;
            return goal;
        }
    }
}
