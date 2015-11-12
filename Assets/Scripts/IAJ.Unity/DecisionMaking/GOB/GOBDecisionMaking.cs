using System;
using System.Collections.Generic;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.GOB
{
    public static class GOBDecisionMaking
    {
        public static float CalculateDiscontentment(Action action, List<Goal> goals)
        {
            //TODO implement
            throw new NotImplementedException();
        }

        public static Action ChooseAction(List<Action> actions, List<Goal> goals)
        {
            float iv = 0.0f;
            Goal bestg = null;
            Action act = null;
            foreach(Goal g in goals)
            {
                if(g.InsistenceValue > iv)
                {
                    iv = g.InsistenceValue;
                    bestg = g;
                }
            }
            foreach(Action a in actions)
            {
                if (a.Name.Equals(bestg.Name)){
                    act = a;
                }
            }
            return act;
        }
    }
}
