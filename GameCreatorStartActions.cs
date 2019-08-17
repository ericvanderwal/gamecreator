using UnityEngine;
using UnityEngine.Events;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Game Creator")]
    [Tooltip("Start action sequence for game creator.")]
    public class GameCreatorStartActions : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(GameCreator.Core.Actions))]
        public FsmOwnerDefault gameObject;
        public FsmEvent successEvent;
        public FsmEvent errorEvent;

        private GameCreator.Core.Actions _action;

        public override void Reset()
        {
        }

        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                return;
            }

            _action = go.GetComponent<GameCreator.Core.Actions>();
            if (_action == null)
            {
                Debug.LogError("Missing action component on " + go);
                Fsm.Event(errorEvent);
            }

            _action.Execute();

            Fsm.Event(successEvent);
        }
    }
}