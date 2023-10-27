using UnityEngine;

namespace FPT
{
    [RequireComponent(typeof(Rigidbody))]
    public class ObjectGrabbable : MonoBehaviour
    {
        [SerializeField] private CharacterController _playerCC;
        [SerializeField] private Collider _playerCapsuleCollider;
        private Rigidbody _objectRigidbody;
        private Collider _objectCollider;
        private Transform _objectGrabPointTransform;

        private void Awake()
        {
            _objectRigidbody = GetComponent<Rigidbody>();
            _objectCollider = GetComponent<Collider>();
        }

        private void FixedUpdate()
        {
            if (_objectGrabPointTransform != null)
            {
                // Grabbed
                float lerpSpeed = 10f;
                Vector3 newSmoothPosition = Vector3.Lerp(transform.position, _objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
                _objectRigidbody.MovePosition(newSmoothPosition);
            }
        }

        public void Grab(Transform objectGrabPointTransform)
        {
            _objectGrabPointTransform = objectGrabPointTransform;

            _objectRigidbody.useGravity = false;
            _objectRigidbody.drag = 5f;

            //SetCollisionIgnore(true);
            _objectRigidbody.freezeRotation = true;
        }

        public void Drop()
        {
            _objectGrabPointTransform = null;

            _objectRigidbody.useGravity = true;
            _objectRigidbody.drag = 0;

            //SetCollisionIgnore(false);
            _objectRigidbody.freezeRotation = false;
        }

        private void SetCollisionIgnore(bool value)
        {
            Physics.IgnoreCollision(_playerCC, _objectCollider, value);
            Physics.IgnoreCollision(_playerCapsuleCollider, _objectCollider, value);
        }
    }
}
