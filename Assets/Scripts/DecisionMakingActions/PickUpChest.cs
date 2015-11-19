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
            return 0.0f;
        }

        public override bool CanExecute()
        {
            if (this.Target == null) return false;
            return true;
        }

        public override void Execute()
        {
            Debug.Log("Chest " + Target.transform.position);
            this.Character.Targeter.UpdateTarget(Target.transform.position);
            this.Character.GameManager.PickUpChest(this.Target);
        }
    }
}
