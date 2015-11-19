using System;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using UnityEngine;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.GOB.Action;

namespace Assets.Scripts.DecisionMakingActions
{
    public class PickUpChest : Action
    {
        private AutonomousCharacter Character{ get; set; }
        private GameObject Target { get; set; }

        public PickUpChest(AutonomousCharacter character, GameObject target) : base("PickUpChest("+target.name+")")
        {
            this.Character = character;
            this.Target = target;
        }

        public override float GetDuration()
        {
            return (this.Target.transform.position - this.Character.Character.KinematicData.position).magnitude / 20.0f;
        }

        public override float GetGoalChange(Goal goal)
        {
            if (goal.Name == AutonomousCharacter.GET_RICH_GOAL) return -0.5f;
            else if (goal.Name == AutonomousCharacter.REST_GOAL)
            {
                var distance =
                    (this.Target.transform.position - this.Character.Character.KinematicData.position).magnitude;
                //0.5 for the attack action and +0.01 * distance because of the walk 
                return 0.5f + distance * 0.01f;
            }
            return 0.0f;
        }

        public override bool CanExecute()
        {
            if (this.Target == null) return false;
            return true;
        }

        public override void Execute()
        {
            this.Character.Targeter.UpdateTarget(Target.transform.position);
            this.Character.GameManager.PickUpChest(this.Target);
        }
    }
}
