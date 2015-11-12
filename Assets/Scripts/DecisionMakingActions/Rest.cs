using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;

namespace Assets.Scripts.DecisionMakingActions
{
    public class Rest : Action
    {
        private AutonomousCharacter Character{ get; set; }

        public Rest(AutonomousCharacter character) : base("Rest")
        {
            this.Character = character;
        }

        public override float GetDuration()
        {
            return 0.5f;
        }

        public override float GetGoalChange(Goal goal)
        {
            if (goal.Name == AutonomousCharacter.REST_GOAL) return -0.1f;
            return 0.0f;
        }

        public override bool CanExecute()
        {
            return this.Character.GameManager.characterData.Energy < 5.0f;
        }

        public override void Execute()
        {
            this.Character.GameManager.Rest();
        }
    }
}
