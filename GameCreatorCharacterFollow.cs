using UnityEngine;
using System.Collections;
using GameCreator.Characters;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Game Creator")]
    [Tooltip("Set target to follow a character. Does not need to be set in everyframe")]
    public class GameCreatorCharacterFollow : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(Character))]
        public FsmOwnerDefault gameObject;

        [RequiredField]
        public FsmGameObject targetObject;

        public FsmFloat followMaxRadius;

        public FsmFloat followMinRadius;

        public FsmBool everyFrame;

        // private vars
        private Character _character;

        public override void Reset()
        {
            everyFrame = false;
            followMaxRadius = null;
            followMinRadius = null;
            targetObject = null;
        }

        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                Debug.LogError("No owner gameobject found for 'Game Creator Move To' in playmaker");
                Finish();
            }

            _character = go.GetComponent<Character>();
            if (_character == null)
            {
                Debug.LogError("No character component found for 'Game Creator Stop Movement' in playmaker");
                Finish();
            }

            FollowTarget();

            if (!everyFrame.Value)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            _character.characterLocomotion.FollowTarget(targetObject.Value.transform, followMinRadius.Value, followMaxRadius.Value);
        }
    }
}