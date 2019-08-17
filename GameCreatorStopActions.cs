using UnityEngine;
using UnityEngine.Events;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Game Creator")]
    [Tooltip("Stop actions sequence for game creator.")]
    public class GameCreatorStopActions : FsmStateAction
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
                Debug.LogError("Missing actions component on " + go);
                Fsm.Event(errorEvent);
            }

            _action.Stop();

            Fsm.Event(successEvent);
        }
    }
}