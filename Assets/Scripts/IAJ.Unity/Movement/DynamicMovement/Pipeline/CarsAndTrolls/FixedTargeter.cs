using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement.Pipeline
{
    public class FixedTargeter : Targeter
    {   
        public FixedTargeter()
        {
            this.Target = new KinematicData();
        }

        public override TargetGoal GetGoal(KinematicData character)
        {

            TargetGoal g = new TargetGoal() { Position = Target.Position };
            g.hasPosition = true;

            return g;
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
