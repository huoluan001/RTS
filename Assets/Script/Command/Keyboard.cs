using UnityEngine;
using UnityEngine.InputSystem;
public class Keyboard : MonoBehaviour
{
    private GameAsset gameAsset;

    private void Awake()
    {
       
    }
    private void Start()
    {
        gameAsset = GameManager.GameAsset;
        gameAsset.player.CamareMove.performed += GetArrow;
        gameAsset.player.CamareMove.canceled += CameraMoveVector2Clear;
        gameAsset.player.LeftShiftRightMouse.performed += Click;
        

    
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