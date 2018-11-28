namespace Utilities
{
    using UnityEngine;
    using UnityEngine.Assertions;
    using System.Collections;

    public class CameraTagalong : MonoBehaviour
    {
        [Tooltip("How often should we check if the object needs to move?")]
        public float UpdateIntervalInSeconds = 0.5f;
        [Tooltip("How close should the object be allowed to come to the viewer?")]
        public float MinDistanceFromViewer = 0.85f;
        [Tooltip("How far should the object be offset from any hit geometry?")]
        public float MinDistanceFromHit = 0.1f;
        [Tooltip("What distance is the object meant to be viewed at?")]
        public float PreferredDistanceFromViewer = 2.0f;
        [Tooltip("Should the object maintain its apparent size as you get closer?")]
        public bool ScaleDownFromPreferredDistance = true;

        [Tooltip("Sphere radius.")]
        public float SphereRadius = 1.0f;

        [Tooltip("How fast the object will move to the target position.")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Display the sphere in red wireframe for debugging purposes.")]
        public bool DebugDisplaySphere = false;

        [Tooltip("Display a small green cube where the target position is.")]
        public bool DebugDisplayTargetPosition = false;

        private Vector3 targetPosition;
        private Vector3 optimalPosition;
        private float initialDistanceToCamera;

        [Tooltip("What layers should we cast rays against to update our object position?")]
        public LayerMask RaycastLayers;

        Interpolator interpolate;
        Vector3 startingLocalScale;
        Vector3 targetScale;
        bool onCoroutine;

        void Start()
        {
            targetPosition = transform.position;
            if (PreferredDistanceFromViewer > 0f)
            {
                initialDistanceToCamera = PreferredDistanceFromViewer;
            }
            else
            {
                initialDistanceToCamera = Vector3.Distance(targetPosition, Camera.main.transform.position);
            }

            //interpolate = GetComponent<Interpolator>(); // It seems odd that they discourage creating and editing this in the inspector
            interpolate = this.GetOrAddComponent<Interpolator>();
            interpolate.PositionPerSecond = MoveSpeed; // tagalong.MoveSpeed

            startingLocalScale = gameObject.transform.localScale;
            targetScale = startingLocalScale;

            onCoroutine = (UpdateIntervalInSeconds > Time.fixedDeltaTime);
            if (onCoroutine)
            {
                StartCoroutine(UpdatePeriodically());
            }
        }

        void Update()
        {
            if (!onCoroutine)
            {
                UpdatePosition();
            }
        }

        IEnumerator UpdatePeriodically()
        {
            while (true)
            {
                yield return new WaitForSeconds(UpdateIntervalInSeconds);
                UpdatePosition();
            }
        }

        void UpdatePosition()
        {
            var wasTargetPosition = targetPosition;
            var cameraPosition = Camera.main.transform.position;
            var cameraDirection = Camera.main.transform.forward;
            optimalPosition = cameraPosition + cameraDirection * initialDistanceToCamera;

            Vector3 offsetDir = this.transform.position - optimalPosition;
            if (offsetDir.magnitude > SphereRadius)
            {
                targetPosition = optimalPosition + offsetDir.normalized * SphereRadius;
            }

            RaycastHit hitInfo;
            var targetDelta = targetPosition - cameraPosition;
            float targetDistance = Vector3.Magnitude(targetDelta);
            if (Physics.Raycast(cameraPosition, cameraDirection, out hitInfo, targetDistance, RaycastLayers.value, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("Raycast hit " + hitInfo.collider.gameObject.name);
                float distance = Mathf.Min(targetDistance, hitInfo.distance - MinDistanceFromHit);
                distance = Mathf.Max(distance, MinDistanceFromViewer);
                targetPosition = cameraPosition + targetDelta * distance / targetDistance;
                targetDistance = distance;
            }
            if (wasTargetPosition != targetPosition)
            {
                interpolate.SetTargetPosition(targetPosition);
            }
            if (ScaleDownFromPreferredDistance)
            {
                Assert.IsTrue(PreferredDistanceFromViewer > 0f);
                var newTargetScale = startingLocalScale * Mathf.Min(1f, targetDistance / PreferredDistanceFromViewer);
                if (newTargetScale != targetScale)
                {
                    interpolate.SetTargetLocalScale(newTargetScale);
                    targetScale = newTargetScale;
                }
            }
        }

        public void OnDrawGizmos()
        {
            if (Application.isPlaying == false) return;

            Color oldColor = Gizmos.color;

            if (DebugDisplaySphere)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(optimalPosition, SphereRadius);
            }

            if (DebugDisplayTargetPosition)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(targetPosition, new Vector3(0.1f, 0.1f, 0.1f));
            }

            Gizmos.color = oldColor;
        }
    }
}
