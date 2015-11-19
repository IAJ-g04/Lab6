using System;
using System.Collections.Generic;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.GOB
{
    public static class GOBDecisionMaking
    {
        public static float CalculateDiscontentment(Action action, List<Goal> goals)
        {
            float discontentment = 0.0f;
            float newValue = 0.0f;
            foreach (Goal g in goals)
            {
                newValue = g.InsistenceValue + action.GetGoalChange(g);
                discontentment += g.GetDiscontentment(newValue);
            }
            return discontentment;
        }




        public static Action ChooseAction(List<Action> actions, List<Goal> goals)
        {
            Action besta = actions[0];
            float bestValue = CalculateDiscontentment(actions[0], goals);
            foreach(Action a in actions)
            {
                if (a.CanExecute())
                {
                    float value = CalculateDiscontentment(a, goals);
                    if(value < bestValue)
                    {
                        bestValue = value;
                        besta = a;
                    }
                }
            }
            return besta;
        }
    }
}
