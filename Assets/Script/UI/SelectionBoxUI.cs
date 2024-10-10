using System;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionBoxUI : MonoBehaviour
{
    [HideInInspector] public Vector2 leftUp;
    [HideInInspector] public Vector2 rightDown;

    public RectTransform up;
    public RectTransform down;
    public RectTransform right;
    public RectTransform left;
    public GameSetting gameSetting;

    private Vector2 windowSize;
    private float lineWidth;

    private void Start()
    {
        var rectTransform = GetComponent<RectTransform>();
        windowSize = gameSetting.windowSize;
        rectTransform.anchoredPosition =
            new Vector3(windowSize.x / 2, windowSize.y / 2, 0);
        rectTransform.sizeDelta = windowSize;
        lineWidth = gameSetting.selectionBoxLineWidth;

    }
    private void Update()
    {
        float width = Math.Abs(leftUp.x - rightDown.x);
        float height = Math.Abs(leftUp.y - rightDown.y);
        float xCenter = (leftUp.x + rightDown.x) / 2;
        float yCenter = (leftUp.y + rightDown.y) / 2;

        up.anchoredPosition = new Vector2(xCenter, leftUp.y);
        down.anchoredPosition = new Vector2(xCenter, rightDown.y);
        right.anchoredPosition = new Vector2(leftUp.x, yCenter);
        left.anchoredPosition = new Vector2(rightDown.x, yCenter);
        up.sizeDelta = new Vector2(width, lineWidth);
        down.sizeDelta = new Vector2(width, lineWidth);
        left.sizeDelta = new Vector2(lineWidth, height);
        right.sizeDelta = new Vector2(lineWidth, height);
        
    }
}
