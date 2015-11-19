using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement.Pipeline
{
    public class FixedTargeter : Targeter

    {
        public bool hasTarget { get; set; }
        
        public FixedTargeter()
        {
            this.Target = new KinematicData();
            this.hasTarget = false;
        }

        public override TargetGoal GetGoal(KinematicData character)
        {
            if (!this.hasTarget)
                return new TargetGoal();
            
            TargetGoal g = new TargetGoal() { position = this.Target.position };
            g.hasPosition = true;
            
            return g;
        }
        
        public void UpdateTarget (Vector3 target)
        {
            this.Target.position = target;
            this.hasTarget = true;
        }

        public void RemoveTarget()
        {
            this.hasTarget = false;
        }

        public override string ToString()
        {
            string returnvalue = string.Empty;
            returnvalue += "Targeter- ";

            returnvalue += "Position: " + Target.position;

            return returnvalue;
        }
    }
}
