using UnityEngine;
using UnityEngine.InputSystem;
public class Keyboard : MonoBehaviour
{
    public GameAsset gameAsset;

    private void Awake()
    {
       
    }
    private void Start()
    {
        gameAsset.player.CamareMove.performed += GetArrow;
        gameAsset.player.CamareMove.canceled += CameraMoveVector2Clear;

    
    }
    private void GetArrow(InputAction.CallbackContext callbackContext)
    {
        gameAsset.cameraMoveVector2 = callbackContext.ReadValue<Vector2>();
    }
    private void CameraMoveVector2Clear(InputAction.CallbackContext callbackContext)
    {
        gameAsset.cameraMoveVector2 = Vector2.zero;
    }
}