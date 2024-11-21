using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{
    private Vector3 mouseLeftPerformPosition;
    private GameObject selectionBoxUI;
    private EventSystem eventSystem;
    private RTSInputSystem.PlayerActions player;
    private void Awake()
    {
        GameManager.InputAsset.inputSystem = new RTSInputSystem();
        GameManager.InputAsset.inputSystem.Enable();
        GameManager.InputAsset.player = GameManager.InputAsset.inputSystem.Player;
        this.player = GameManager.InputAsset.player;
    }
    private void Start()
    {
        eventSystem = GameManager.GameAsset.eventSystem;
        player.MouseLeft.performed += OnMouseLeftPerformed;
        player.MouseLeft.canceled += OnMouseLeftCanceled;

        player.MouseRight.performed += OnMouseRightPerformed;
        player.MouseLeftDoubleClick.performed += MouseLeftDoubleClick;

        player.AggressionModel.performed += AggressionModel;
        player.VigilanceModel.performed += VigilanceMode;
        player.StickingModel.performed += SattingModel;
        player.CeasefireModel.performed += CeasefireModel;





        selectionBoxUI = GameManager.GameAsset.selectionBoxUI;
    }

    private void Update()
    {
        //        SelectionBoxUIUpdate();
    }



    void OnMouseLeftPerformed(InputAction.CallbackContext callbackContext)
    {
        GameManager.EventAsset.MouseLeftClickPerformed?.Invoke();
        mouseLeftPerformPosition = Input.mousePosition;

        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;

        List<RaycastResult> list = new List<RaycastResult>();
        GameManager.GameAsset.graphicRaycaster.Raycast(eventData, list);
        if (list.Count != 0)
        {
            Debug.Log("点击到了UI");
        }
        else
        {
            Debug.Log("点击到了场景");
            return;
        }
        GameObject gameObject = list.First().gameObject;
        if (gameObject.tag == "TaskAvatar") // 添加序列任务
        {
            var page = gameObject.transform.parent.parent.GetComponent<Page>();
            page.CurrentSequenceAddTask(gameObject);
        }
        else if (gameObject.tag == "SeqAvatar") // 序列转换
        {
            var page = gameObject.transform.parent.parent.GetComponent<Page>();
            int targetIndex = int.Parse(gameObject.transform.GetComponent<TMP_Text>().text);
            page.SwitchCurrentSequence(targetIndex);
        }

    }
    void OnMouseLeftCanceled(InputAction.CallbackContext callbackContext)
    {
        GameManager.EventAsset.MouseLeftClickCanceled?.Invoke();
        // var currentMousePosition = Input.mousePosition;
        // if (mouseLeftPerformPosition != currentMousePosition)
        // {
        //     var gameObjects = MarqueeSelect(mouseLeftPerformPosition, currentMousePosition);
        // }
    }
    public void OnMouseRightPerformed(InputAction.CallbackContext callbackContext)
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        GameManager.GameAsset.graphicRaycaster.Raycast(eventData, list);

        GameObject gameObject = list.First().gameObject;
        if (gameObject.tag == "TaskAvatar") // 添加序列任务
        {
            var page = gameObject.transform.parent.parent.GetComponent<Page>();
            page.CurrentSequencePauseOrCancelTask(gameObject);
        }
        if (callbackContext.performed)
        {
            Debug.Log("RightClick");
        }
    }

    public void MouseLeftDoubleClick(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Debug.Log("MouseLeftDoubleclick");
        }
    }

    #region 键盘
    public void AggressionModel(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Debug.Log("AggressionModel");
        }
    }
    public void VigilanceMode(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Debug.Log("VigilanceMode");
        }
    }
    public void SattingModel(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Debug.Log("SattingModel");
        }
    }
    public void CeasefireModel(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Debug.Log("CeasefireModel");
        }
    }
    #endregion

    /// <summary>
    /// 拖动结束时，返回框选士兵
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    private GameObject[] MarqueeSelect(Vector3 v1, Vector3 v2)
    {
        var leftUp = Camera.main.ScreenPointToRay(v1);
        var rightDown = Camera.main.ScreenPointToRay(v2);
        Physics.Raycast(leftUp, out RaycastHit leftUpHitinfo, 1000, 1 << LayerMask.NameToLayer("Ground"));
        Physics.Raycast(rightDown, out RaycastHit rightDownHitinfo, 1000, 1 << LayerMask.NameToLayer("Ground"));
        var leftUpV3 = leftUpHitinfo.point;
        var rightDownV3 = rightDownHitinfo.point;
        var halfExtents = new Vector3(Math.Abs(leftUpV3.x - rightDownV3.x) / 2, 10, Math.Abs(rightDownV3.z - rightDownV3.z) / 2);
        var colliders = Physics.OverlapBox((leftUpV3 + rightDownV3) / 2, halfExtents);
        return colliders.Select(collider => collider.gameObject).ToArray();
    }

    /// <summary>
    /// 当鼠标按下并拖动时，根据鼠标的位置更新选择框的样式
    /// </summary>
    private void SelectionBoxUIUpdate()
    {
        if (this.player.MouseLeft.IsInProgress())
        {
            var leftUp = mouseLeftPerformPosition;
            var rightDown = Input.mousePosition;
            selectionBoxUI.SetActive(true);
            selectionBoxUI.GetComponent<SelectionBoxUI>().leftUp = leftUp;
            selectionBoxUI.GetComponent<SelectionBoxUI>().rightDown = rightDown;
        }
        else
            selectionBoxUI.SetActive(false);
    }



}
