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
            Goal grg = null;
            Action act = null;
            foreach(Goal g in goals)
            {
                if (g.Name.Equals("GetRich"))
                {
                    grg = g;
                }
                else if(g.InsistenceValue > iv)
                {
                    iv = g.InsistenceValue;
                    bestg = g;
                }
            }
            if (iv < 8.0)
            {
                bestg = grg;
            }
                foreach (Action a in actions)
            {
                if (a.Name.Contains("PickUpChest")){
                    act = a;
                }
            }
            return act;
        }
    }
}
