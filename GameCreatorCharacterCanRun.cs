using UnityEngine;
using System.Collections;
using GameCreator.Characters;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Game Creator")]
    [Tooltip("Set character to to true or false")]
    public class GameCreatorCharacteCanRun : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(Character))]
        public FsmOwnerDefault gameObject;

        public FsmBool canRun;
        public FsmBool everyFrame;

        // private vars
        private Character _character;


        public override void Reset()
        {
            canRun = true;
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

            SetRun();

            if (!everyFrame.Value)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            if (!everyFrame.Value) return;

            SetRun();
        }

        private void SetRun()
        {
            _character.characterLocomotion.canRun = canRun.Value;
        }
    }
}