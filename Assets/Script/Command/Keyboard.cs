using UnityEngine;
using UnityEngine.InputSystem;
public class Keyboard : MonoBehaviour
{
    private GameAsset gameAsset;

    private RTSInputSystem.PlayerActions player;

    private void Awake()
    {

    }
    private void Start()
    {
        this.player = GameManager.InputAsset.player;
        gameAsset = GameManager.GameAsset;
        player.CamareMove.performed += GetArrow;
        player.CamareMove.canceled += CameraMoveVector2Clear;
        player.LeftShiftRightMouse.performed += Click;

        player.LeftShift.performed += OnLeftShiftPerformed;
        player.LeftShift.canceled += OnRightShiftCanceled;



    }
    private void OnLeftShiftPerformed(InputAction.CallbackContext callbackContext)
    {
        GameManager.InputAsset.LeftShiftDown = true;
        Debug.Log("f");
    }

    private void OnRightShiftCanceled(InputAction.CallbackContext callbackContext)
    {
        GameManager.InputAsset.LeftShiftDown = false;
    }
    private void GetArrow(InputAction.CallbackContext callbackContext)
    {
        gameAsset.cameraMoveVector2 = callbackContext.ReadValue<Vector2>();
    }
    private void CameraMoveVector2Clear(InputAction.CallbackContext callbackContext)
    {
        gameAsset.cameraMoveVector2 = Vector2.zero;
    }

    public void Click(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Debug.Log("Shift + clickRight");
        }
    }
}