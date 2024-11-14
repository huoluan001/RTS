using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject currentBuildTarget;

    public LayerMask groundLayer;
    public GameObject tileNoScriptPrefab;
    public GameObject tileWithScriptPrefab;
    public GameObject tileMap;

    public List<TileMapTest> tileMapTests = new List<TileMapTest>();

    private float _xOffset;
    private float _yOffset;
    private Camera _camera;
    private Vector3 _currentMousePos;
    private Action _currentUpdateAction;
    private bool _allowBuild;

    // ReSharper disable Unity.PerformanceAnalysis
    private void Start()
    {
        _camera = Camera.main;
    }

    public void StartBuilding(GameObject targetPrefab)
    {
        // 实例化建筑，并计算偏移
        currentBuildTarget = Instantiate(targetPrefab);

        MainBuilding mb = currentBuildTarget.GetComponentAtIndex(1) as MainBuilding;
        Vector2Int area = mb.mainBuildingSo.Area;

        _xOffset = area.x % 2 == 0 ? 0.5f : 0f;
        _yOffset = area.y % 2 == 0 ? 0.5f : 0f;

        Vector3 originPos = new Vector3((int)(area.x * -3 / 2), 0, (int)(area.y * -3 / 2));
        for (int i = 0; i < area.x * 3; i++)
        {
            for (int j = 0; j < area.y * 3; j++)
            {
                GameObject go;
                if (i >= area.x && i < 2 * area.x && j >= area.y && j < 2 * area.y)
                {
                    go = Instantiate(tileWithScriptPrefab, tileMap.transform, false);
                    tileMapTests.Add(go.GetComponent<TileMapTest>());
                }
                else
                {
                    go = Instantiate(tileNoScriptPrefab, tileMap.transform, false);
                }

                go.transform.position = new Vector3(originPos.x + i, 0.0001f, originPos.z + j);
            }
        }

        _currentUpdateAction = OnBuildingAndTileMapMoveByMousePos;

        GameManager.gameAsset.MouseLeftClickPerformed = () =>
        {
            _currentUpdateAction = OnBuildingAndTileMapRotationByMousePos;
            _currentUpdateAction += () =>
            {
                _allowBuild = tileMapTests.All(tileMapTest => tileMapTest.gameObjects.Count == 0);
                if (_allowBuild)
                    GameManager.gameAsset.MouseLeftClickCanceled = () =>
                    {
                        BuildingManagerEnd();
                        Debug.Log("success build");
                    };
                else
                    GameManager.gameAsset.MouseLeftClickCanceled =
                        () => _currentUpdateAction = OnBuildingAndTileMapMoveByMousePos;
            };
        };
        BuildingManagerStart();
    }

    public void Awake()
    {
        GameManager.gameAsset.buildingManager = this;
        BuildingManagerEnd();
    }

    private void Update()
    {
        _currentUpdateAction?.Invoke();
    }

    private void OnBuildingAndTileMapMoveByMousePos()
    {
        Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, groundLayer))
        {
            _currentMousePos = new Vector3(Mathf.Round(raycastHit.point.x), 0, Mathf.Round(raycastHit.point.z));
            currentBuildTarget.transform.position = new Vector3(_currentMousePos.x + 0.5f - _xOffset, 0f,
                _currentMousePos.z + 0.5f - _yOffset);
            tileMap.transform.position = new Vector3(_currentMousePos.x + _xOffset, 0f, _currentMousePos.z + _yOffset);
        }
    }

    private void OnBuildingAndTileMapRotationByMousePos()
    {
        Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, groundLayer);
        var pos = new Vector3(Mathf.Round(raycastHit.point.x), 0, Mathf.Round(raycastHit.point.z));
        if(pos == _currentMousePos)
            return;
        var markDir = (pos - _currentMousePos).normalized;
        
        int dnum = (int)Angle_360(Vector3.back, markDir) / 90 * 90;
        currentBuildTarget.transform.eulerAngles = new Vector3(0f, dnum, 0f);
    }

    private void BuildingManagerEnd()
    {
        int count = tileMap.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(tileMap.transform.GetChild(i).gameObject);
        }
        tileMap.transform.position = Vector3.zero;
        this.enabled = false;
    }

    private void BuildingManagerStart()
    {

        
        this.enabled = true;
    }

    private float Angle_360(Vector3 from, Vector3 to)
    {
        Vector3 v3 = Vector3.Cross(from, to);
        if (v3.y > 0)
            return Vector3.Angle(from, to);
        else
            return 360 - Vector3.Angle(from, to);
    }
}