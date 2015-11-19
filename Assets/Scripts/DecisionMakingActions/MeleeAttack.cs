using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using UnityEngine;

namespace Assets.Scripts.DecisionMakingActions
{
    public class MeleeAttack : Action
    {
        private AutonomousCharacter Character{ get; set; }
        private GameObject Target { get; set; }

        public MeleeAttack(AutonomousCharacter character, GameObject target) : base("MeleeAttack("+target.name+")")
        {
            this.Character = character;
            this.Target = target;
        }

        public override float GetDuration()
        {
            //assume a velocity of 20.0f/s to get to the target
            return (this.Target.transform.position - this.Character.Character.KinematicData.position).magnitude/20.0f;
        }

        public override float GetGoalChange(Goal goal)
        {
            if (goal.Name == AutonomousCharacter.EAT_GOAL) return -2.0f;
            else if (goal.Name == AutonomousCharacter.REST_GOAL)
            {
                var distance =
                    (this.Target.transform.position - this.Character.Character.KinematicData.position).magnitude;
                //0.5 for the attack action and +0.01 * distance because of the walk 
                return 0.5f + distance*0.01f;
            }
            else if (goal.Name == AutonomousCharacter.SURVIVE_GOAL) return +2.0f;

            return 0;
        }

        public override bool CanExecute()
        {
            if (this.Target == null) return false;
            return this.Character.GameManager.characterData.Energy > 1.0f &&
                   this.Character.GameManager.characterData.HP > 2;
        }
        public override void Execute()
        {
            this.Character.Targeter.UpdateTarget(Target.transform.position);
            this.Character.GameManager.MeleeAttack(this.Target);
        }
    }
}
