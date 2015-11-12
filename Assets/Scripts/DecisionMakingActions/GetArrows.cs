using System;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using UnityEngine;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.GOB.Action;

namespace Assets.Scripts.DecisionMakingActions
{
    public class GetArrows : Action
    {
        private AutonomousCharacter Character{ get; set; }
        private GameObject Target { get; set; }

        public GetArrows(AutonomousCharacter character, GameObject target) : base("GetArrows("+target.name+")")
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
            //TODO implement
            throw new NotImplementedException();
        }

        public override void Execute()
        {
            //this.Character.Targeter.Target.Position = this.Target.transform.position;
            this.Character.GameManager.GetArrows(this.Target);
        }
    }
}
