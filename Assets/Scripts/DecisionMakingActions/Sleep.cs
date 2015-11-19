using System;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using UnityEngine;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.GOB.Action;

namespace Assets.Scripts.DecisionMakingActions
{
    public class Sleep : Action
    {
        private AutonomousCharacter Character{ get; set; }
        private GameObject Target { get; set; }

        public Sleep(AutonomousCharacter character, GameObject target) : base("Sleep("+target.name+")")
        {
            this.Character = character;
            this.Target = target;
        }

        public override float GetDuration()
        {
            //TODO implement
            throw new NotImplementedException();
        }

        public override float GetGoalChange(Goal goal)
        {
            //TODO implement
            throw new NotImplementedException();
        }

        public override bool CanExecute()
        {
            return false;
        }

        public override void Execute()
        {
            this.Character.Targeter.UpdateTarget(Target.transform.position);
            this.Character.GameManager.Sleep(this.Target);
        }
    }
}
