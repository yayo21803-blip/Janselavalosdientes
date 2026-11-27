using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GamepadClickOnVRUI : MonoBehaviour
{
    [Header("Arrastra aquí el Near-Far Interactor del Right Controller")]
    [SerializeField] private NearFarInteractor uiInteractor;

    [Header("Arrastra aquí la acción del botón A o el gatillo derecho")]
    [SerializeField] private InputActionReference gamepadClickAction;

    private void OnEnable()
    {
        gamepadClickAction.action.performed += OnGamepadClick;
    }

    private void OnDisable()
    {
        gamepadClickAction.action.performed -= OnGamepadClick;
    }

    private void OnGamepadClick(InputAction.CallbackContext context)
    {
        if (!uiInteractor.TryGetCurrentUIRaycastResult(out var uiHit))
            return;

        Button targetButton = uiHit.gameObject.GetComponentInParent<Button>();

        if (targetButton != null && targetButton.interactable)
        {
            targetButton.onClick.Invoke();
            targetButton.OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }
}