using UnityEngine;
using System.Collections;
using GameCreator.Characters;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Game Creator")]
    [Tooltip("Set character can jump property")]
    public class GameCreatorCharacteCanJump : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(Character))]
        public FsmOwnerDefault gameObject;

        public FsmBool canJump;
        public FsmBool everyFrame;

        // private vars
        private Character _character;


        public override void Reset()
        {
            canJump = true;
            everyFrame = false;
        }

        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                Debug.LogError("No owner gameobject found for 'Game Creator Move To' in playmaker");
                return;
            }

            _character = go.GetComponent<Character>();
            if (_character == null)
            {
                Debug.LogError("No character component found for 'Game Creator Move To' in playmaker");
                return;
            }

            SetProperty();

            if (!everyFrame.Value)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            if (!everyFrame.Value) return;

            SetProperty();
        }

        private void SetProperty()
        {
            _character.characterLocomotion.canJump = canJump.Value;
        }
    }
}