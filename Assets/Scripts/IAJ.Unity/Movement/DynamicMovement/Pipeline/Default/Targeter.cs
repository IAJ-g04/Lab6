using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement.Pipeline
{
    public abstract class Targeter
    {
        public KinematicData Target { get; set; }
        public float LookAhead = 7.5f;

        public abstract TargetGoal GetGoal(KinematicData character);
    }
}
