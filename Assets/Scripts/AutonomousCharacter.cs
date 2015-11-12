using System.Collections.Generic;
using Assets.Scripts.DecisionMakingActions;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Pathfinding;
using Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics;
using RAIN.Navigation;
using RAIN.Navigation.NavMesh;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AutonomousCharacter : MonoBehaviour
    {
        //constants
        public const string SURVIVE_GOAL = "Survive";
        public const string REST_GOAL = "Rest";
        public const string EAT_GOAL = "Eat";
        public const string GET_RICH_GOAL = "GetRich";

        public const float DECISION_MAKING_INTERVAL = 0.5f;
        //public fields to be set in Unity Editor
        public GameManager.GameManager GameManager;
        public TextMesh ActionText;
        public Text SurviveGoalText;
        public Text EatGoalText;
        public Text RestGoalText;
        public Text GetRichGoalText;


        public Goal RestGoal { get; private set; }
        public Goal SurviveGoal { get; private set; }
        public Goal GetRichGoal { get; private set; }
        public Goal EatGoal { get; private set; }
        public List<Goal> Goals { get; set; }
        public List<Action> Actions { get; set; }
        public Action CurrentAction { get; private set; }
        public DynamicCharacter Character { get; private set; }
        //public FixedTargeter Targeter { get; set; }


        //private fields for internal use only
        private NavMeshPathGraph navMesh;
        private AStarPathfinding aStarPathFinding;
        //private PathFindingDecomposer decomposer;
        

        private bool draw;

        private float nextUpdateTime = 0.0f;
        private float previousGold = 0.0f;

        public void Start()
        {
            this.draw = true;
            this.navMesh = NavigationManager.Instance.NavMeshGraphs[0];
            this.Character = new DynamicCharacter(this.gameObject);


            //initialization of the movement algorithms
            this.aStarPathFinding = new NodeArrayAStarPathFinding(this.navMesh, new EuclideanDistanceHeuristic());
            this.aStarPathFinding.NodesPerSearch = 100;

            /*var steeringPipeline = new SteeringPipeline
            {
                MaxAcceleration = 40.0f,
                MaxConstraintSteps = 2,
                Character = this.Character.KinematicData,
            };

            this.decomposer = new PathFindingDecomposer(steeringPipeline, this.aStarPathFinding);
            this.Targeter = new FixedTargeter(steeringPipeline);
            steeringPipeline.Targeters.Add(this.Targeter);
            steeringPipeline.Decomposers.Add(this.decomposer);
            steeringPipeline.Actuator = new FollowPathActuator(steeringPipeline);

            this.Character.Movement = steeringPipeline;
             * */

            //initialization of the GOB decision making
            //let's start by creating 4 main goals
            //the eatgoal is the only goal that increases at a fixed rate per second, it increases at a rate of 0.1 per second
            this.SurviveGoal = new Goal(SURVIVE_GOAL, 2.0f);
            this.EatGoal = new Goal(EAT_GOAL, 1.0f)
            {
                ChangeRate = 0.1f
            };
            this.GetRichGoal = new Goal(GET_RICH_GOAL, 1.0f)
            {
                InsistenceValue = 5.0f,
                ChangeRate = 0.2f
            };
            this.RestGoal = new Goal(REST_GOAL, 1.0f);

            this.Goals = new List<Goal>();
            this.Goals.Add(this.SurviveGoal);
            this.Goals.Add(this.EatGoal);
            this.Goals.Add(this.GetRichGoal);
            this.Goals.Add(this.RestGoal);

            //initialize the available actions

            var restAction = new Rest(this);
            this.Actions = new List<Action>();
            this.Actions.Add(restAction);

            foreach (var chest in GameObject.FindGameObjectsWithTag("Chest"))
            {
                this.Actions.Add(new PickUpChest(this, chest));
            }

            foreach (var tree in GameObject.FindGameObjectsWithTag("Tree"))
            {
                this.Actions.Add(new GetArrows(this, tree));
            }

            foreach (var bed in GameObject.FindGameObjectsWithTag("Bed"))
            {
                this.Actions.Add(new Sleep(this, bed));
            }

            foreach (var boar in GameObject.FindGameObjectsWithTag("Boar"))
            {
                this.Actions.Add(new MeleeAttack(this, boar));
                this.Actions.Add(new Shoot(this, boar));
            }
        }

        void Update()
        {
            if (Time.time > this.nextUpdateTime)
            {
                this.nextUpdateTime = Time.time + DECISION_MAKING_INTERVAL;

                //first step, perceptions
                //update the agent's goals based on the state of the world
                this.SurviveGoal.InsistenceValue = 10 - this.GameManager.characterData.HP;
                this.EatGoal.InsistenceValue = this.GameManager.characterData.Hunger;
                this.RestGoal.InsistenceValue = 10 - this.GameManager.characterData.Energy;

                //the get rich goal is managed by DecisionMaking process because it does not correspond to a physiological need handled in the world
                this.GetRichGoal.InsistenceValue += 0.1f; //decay
                if (this.GetRichGoal.InsistenceValue > 10)
                {
                    this.GetRichGoal.InsistenceValue = 10.0f;
                }
                if (this.GameManager.characterData.Money > this.previousGold)
                {
                    this.GetRichGoal.InsistenceValue -= this.GameManager.characterData.Money - this.previousGold;
                    this.previousGold = this.GameManager.characterData.Money;
                }

                this.SurviveGoalText.text = "Survive: " + this.SurviveGoal.InsistenceValue;
                this.EatGoalText.text = "Eat: " + this.EatGoal.InsistenceValue;
                this.RestGoalText.text = "Rest: " + this.RestGoal.InsistenceValue;
                this.GetRichGoalText.text = "GetRich: " + this.GetRichGoal.InsistenceValue;

                //choose an action using the GOB Decision Making process
                var action = GOBDecisionMaking.ChooseAction(this.Actions, this.Goals);
                if (action != null)
                {
                    action.Execute();
                    this.CurrentAction = action;
                    this.ActionText.text = this.CurrentAction.Name;
                }
                else
                {
                    this.ActionText.text = "";
                }
            }

            this.Character.Update();
        }

        public void OnDrawGizmos()
        {
            if (this.draw)
            {
                //draw the current Solution Path if any (for debug purposes)
                //if (this.decomposer.UnsmoothedPath != null)
                //{
                //    var previousPosition = this.Character.KinematicData.position;
                //    foreach (var pathPosition in this.decomposer.UnsmoothedPath.PathPositions)
                //    {
                //        Debug.DrawLine(previousPosition, pathPosition, Color.red);
                //        previousPosition = pathPosition;
                //    }

                //    previousPosition = this.Character.KinematicData.position;
                //    foreach (var pathPosition in this.decomposer.CurrentPath.PathPositions)
                //    {
                //        Debug.DrawLine(previousPosition, pathPosition, Color.green);
                //        previousPosition = pathPosition;
                //    }
                //}

                //draw the nodes in Open and Closed Sets
                if (this.aStarPathFinding != null)
                {
                    Gizmos.color = Color.cyan;

                    if (this.aStarPathFinding.Open != null)
                    {
                        foreach (var nodeRecord in this.aStarPathFinding.Open.All())
                        {
                            Gizmos.DrawSphere(nodeRecord.node.LocalPosition, 1.0f);
                        }
                    }

                    Gizmos.color = Color.blue;

                    if (this.aStarPathFinding.Closed != null)
                    {
                        foreach (var nodeRecord in this.aStarPathFinding.Closed.All())
                        {
                            Gizmos.DrawSphere(nodeRecord.node.LocalPosition, 1.0f);
                        }
                    }
                }
            }
        }
    }
}
