﻿using Assets.Scripts.IAJ.Unity.Pathfinding.Path;
using Assets.Scripts.IAJ.Unity.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement.Pipeline
{
    public class TrollConstraint : Constraint
    {
        public float margin { set; get; }
        public const float TrollRadius = 2.0f;
      //  public Vector3 center { set; get; }
        public int probInd { set; get; }
        public DynamicCharacter Troll { set; get; }
        
        public override Boolean WillViolate(LineSegmentPath path)
        {
            Vector3 impactPoint = path.GetPosition(MathHelper.closestParamInLineSegmentToPoint(path.StartPosition, path.EndPosition, Troll.KinematicData.position));
            Vector3 direction = Troll.KinematicData.position - impactPoint;
            float distance = Vector3.Magnitude(direction);

            if(distance < TrollRadius) { 
           // Debug.Log("VIOLATE!");
                return true;
            }
         //   Debug.Log("NO VIOLATE!");
            return false;
            
        }


        public override TargetGoal Suggest(LineSegmentPath path, KinematicData character, TargetGoal goal)
        {
            // procurar ponto do segmento mais próximo ao centro da esfera
            Vector3 closest = path.GetPosition(MathHelper.closestParamInLineSegmentToPoint(path.StartPosition, path.EndPosition, Troll.KinematicData.position));
            // Check if we pass through the center point
            Vector3 newPt;
            
            if (closest.sqrMagnitude == 0)
            {
               
                // Get any vector at right angles to the segment
                Vector3 dirn =  path.EndPosition - path.StartPosition;
                //pode nao ser esta func TO DO
                Vector3 newdirn = Vector3.Cross(dirn, Vector3.Cross(dirn, Vector3.forward));
                newPt = Troll.KinematicData.position + newdirn * TrollRadius * margin;
            }
            else
            {
                
                // Otherwise project the point out beyond the radius
                newPt = (Troll.KinematicData.position + (closest - Troll.KinematicData.position) * TrollRadius * margin) / closest.sqrMagnitude;
            }
            // Set up the goal and return
            goal.position = newPt;
            return goal;
        }
    }
}
