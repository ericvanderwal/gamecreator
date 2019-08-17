using UnityEngine;
using System.Collections;
using GameCreator.Characters;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Game Creator")]
    [Tooltip("Stop character movement")]
    public class GameCreatorCharacterStopMovement : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(Character))]
        public FsmOwnerDefault gameObject;

        public FsmBool everyFrame;

        // private vars
        private Character _character;

        public override void Reset()
        {
            everyFrame = false;
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

            StopMovement();

            if (!everyFrame.Value)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            StopMovement();
        }

        private void StopMovement()
        {
            _character.characterLocomotion.Stop();
        }
    }
}