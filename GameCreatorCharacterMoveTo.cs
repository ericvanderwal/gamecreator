using UnityEngine;
using System.Collections;
using GameCreator.Characters;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Game Creator")]
    [Tooltip("Move character to a specific location, marker or gameobject. If a Navigation marker is on the target, it will use that information")]
    public class GameCreatorCharacterMoveTo : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(Character))]
        public FsmOwnerDefault gameObject;

        public FsmBool useNaviationMarker;

        [RequiredField]
        public FsmGameObject targetMoveTo;

        public FsmVector3 targetLocation;

        public FsmBool waitForArrival;
        public FsmFloat stopThreshold = 0.0f;

        [RequiredField]
        public FsmEvent successEvent;

        [RequiredField]
        public FsmEvent errorEvent;

        public FsmBool everyFrame;
        private bool markerCached;

        // private vars

        private NavigationMarker _marker;
        private Character _character;
        Vector3 cPosition = Vector3.zero;
        ILocomotionSystem.TargetRotation cRotation = null;
        float cStopThresh = 0f;

        public override void Reset()
        {
            targetMoveTo = null;
            waitForArrival = false;
            stopThreshold = 1f;
            successEvent = null;
            everyFrame = false;
            markerCached = false;
            useNaviationMarker = false;
            targetLocation = new FsmVector3() {UseVariable = true};
        }

        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                Debug.LogError("No owner gameobject found for 'Game Creator Move To' in playmaker");
                Fsm.Event(errorEvent);
            }

            _character = go.GetComponent<Character>();
            if (_character == null)
            {
                Debug.LogError("No character component found for 'Game Creator Move To' in playmaker");
                Fsm.Event(errorEvent);
            }

            MoveTo();

            if (!everyFrame.Value)
            {
                // finish if not wait for arrival.
                if (!waitForArrival.Value) Fsm.Event(successEvent);
            }
        }

        public override void OnUpdate()
        {
            MoveTo();
        }


        /// <summary>
        /// Manage all move to logic here.
        /// </summary>
        private void MoveTo()
        {
            if (useNaviationMarker.Value)
            {
                if (!markerCached) _marker = targetMoveTo.Value.GetComponent<NavigationMarker>();

                if (_marker != null)
                {
                    markerCached = true;
                    GetMarkerInformation();
                }
                else
                {
                    Debug.LogError("No destination marker component found for 'Game Creator Move To' in playmaker");
                    Fsm.Event(errorEvent);
                }
            }

            else
            {
                GetDesinationInformation();
            }

            SetDesitination();
        }

        /// <summary>
        /// Get destination information if not using a marker
        /// </summary>
        private void GetDesinationInformation()
        {
            if (targetMoveTo.IsNone || targetMoveTo.Value == null)
            {
                cPosition = targetLocation.Value;
                Debug.Log("Using vector 3");
            }
            else
            {
                cPosition = targetMoveTo.Value.transform.position;
                Debug.Log("Using transform");
            }

            cStopThresh = stopThreshold.Value;
            cRotation = null;
        }

        /// <summary>
        /// Set character destination
        /// </summary>
        private void SetDesitination()
        {
            _character.characterLocomotion.SetTarget(cPosition, cRotation, cStopThresh, this.CharacterArrivedCallback);
        }

        /// <summary>
        /// Call back for arrival completed
        /// </summary>
        private void CharacterArrivedCallback()
        {
            if (waitForArrival.Value) Fsm.Event(successEvent);
        }

        /// <summary>
        /// Get marker information if found
        /// </summary>
        private void GetMarkerInformation()
        {
            Debug.Log("Using marker");
            cPosition = _marker.transform.position;
            cRotation = new ILocomotionSystem.TargetRotation(true, _marker.transform.forward);
            cStopThresh = _marker.stopThreshold;
            cStopThresh = Mathf.Max(cStopThresh, stopThreshold.Value);
        }
    }
}