using FPT.Base.Entities;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

namespace FPT.FirstPersonController.Camera
{
    public class CameraRaycast : MonoBehaviour
    {
        [SerializeField] private Transform _originCameraTransform;
        [SerializeField] private float _raycastMaxDistance = 1.5f;
        [SerializeField] private LayerMask _layerMask;

        [SerializeField] private Transform _objectGrabPointTransform;
        private ObjectGrabbable _objectGrabbable = null;

        private void Update()
        {
            InteractCheck();
            GrabCheck();

            ScaleCrosshairCheck();
        }

        private void ScaleCrosshairCheck()
        {
            if (Physics.Raycast(_originCameraTransform.position, _originCameraTransform.TransformDirection(Vector3.forward), out RaycastHit raycastHit, _raycastMaxDistance, _layerMask))
            {
                if (raycastHit.transform.TryGetComponent(out BaseItemWorldObject itemObject))
                {
                    ScaleCrosshair(true);
                    InteractionTooltipsPopUp();
                    SetTooltipInteractableText(itemObject.ItemName);

                    //Temp
                    if (_objectGrabbable != null)
                        SetTooltipsVisibility(false);
                }
            }
            else
            {
                ScaleCrosshair(false);
                InteractionTooltipsHide();
            }
        }

        private void GrabCheck()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (_objectGrabbable == null)
                {
                    if (Physics.Raycast(_originCameraTransform.position, _originCameraTransform.TransformDirection(Vector3.forward), out RaycastHit raycastHit, _raycastMaxDistance, _layerMask))
                    {
                        // Not grabbed, grabbing
                        if (raycastHit.transform.TryGetComponent(out _objectGrabbable))
                        {
                            _objectGrabbable.Grab(_objectGrabPointTransform);

                            SetCrosshairVisibility(false);
                            SetTooltipsVisibility(false);
                        }
                    }
                }
                else
                {
                    // Grabbed something. drop now
                    _objectGrabbable.Drop();

                    _objectGrabbable = null;
                    SetCrosshairVisibility(true);
                    SetTooltipsVisibility(true);
                }
            }
        }

        private void InteractCheck()
        {
            //ToDo: Show Interact UI & Reorder lines (outer Raycast)
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Physics.Raycast(_originCameraTransform.position, _originCameraTransform.TransformDirection(Vector3.forward), out RaycastHit raycastHit, _raycastMaxDistance, _layerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out BaseItemWorldObject itemObject))
                    {
                        itemObject.Interact();
                        SetCrosshairVisibility(true);

                        _objectGrabbable = null;
                    }
                }
            }
        }

        //ToDo: Move to 'CrosshairController' or smth.
        [SerializeField] private GameObject _crosshairImageObject;
        private void ScaleCrosshair(bool hover)
        {
            if (hover)
                _crosshairImageObject.GetComponent<Animator>().Play("CrosshairScaleUp");
            else
                _crosshairImageObject.GetComponent<Animator>().Play("CrosshairScaleDown");
        }
        private void SetCrosshairVisibility(bool value)
        {
            _crosshairImageObject.GetComponent<UnityEngine.UI.Image>().enabled = value;
        }

        [SerializeField] private Animator _takeTooltipAnimator;
        [SerializeField] private Animator _grabTooltipAnimator;
        [SerializeField] private Animator _interactableNameAnimator;
        private void InteractionTooltipsPopUp()
        {
            _takeTooltipAnimator.Play("TooltipPopUp");
            _grabTooltipAnimator.Play("TooltipPopUp");
            _interactableNameAnimator.Play("TooltipInteractableNameShow");
        }
        private void InteractionTooltipsHide()
        {
            _takeTooltipAnimator.Play("TooltipHide");
            _grabTooltipAnimator.Play("TooltipHide");
            _interactableNameAnimator.Play("TooltipInteractableNameHide");
        }
        private void SetTooltipInteractableText(string interactableName)
        {
            _interactableNameAnimator.gameObject.GetComponent<TextMeshProUGUI>().text = interactableName;
        }
        private void SetTooltipsVisibility(bool value)
        {
            //ToDo: via refs to texts
            _takeTooltipAnimator.gameObject.SetActive(value);
            _grabTooltipAnimator.gameObject.SetActive(value);
            _interactableNameAnimator.gameObject.SetActive(value);
        }
    }
}
