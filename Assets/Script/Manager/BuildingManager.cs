using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject currentBuildTarget;

    public LayerMask groundLayer;
    public GameObject tileNoScriptPrefab;
    public GameObject tileWithScriptPrefab;
    public GameObject mark;

    public List<TileMapTest> tileMapTests = new List<TileMapTest>();

    public void StartBuilding(GameObject targetPrefab)
    {
        currentBuildTarget = Instantiate(targetPrefab);
        
        int l = 4;
        int w = 4;

        Vector3 originPos = new Vector3((int)(l * -3 / 2), 0, (int)(w * -3 / 2));
        for (int i = 0; i < l * 3; i++)
        {
            for (int j = 0; j < w * 3; j++)
            {
                GameObject go = null;
                if (i >= l && i < 2 * l && j >= w && j < 2 * w)
                {
                    go = Instantiate(tileWithScriptPrefab, mark.transform, false);
                    tileMapTests.Add(go.GetComponent<TileMapTest>());
                }
                else
                {
                    go = Instantiate(tileNoScriptPrefab, mark.transform, false);
                }

                go.transform.position = new Vector3(originPos.x + i, 0.0001f, originPos.z + j);
            }
        }

        BuildingManagerStart();
    }

    public void Awake()
    {
        GameManager.gameAsset.buildingManager = this;
        BuildingManagerEnd();
    }

    private void Update()
    {
        Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, groundLayer))
        {
            Vector3 position = raycastHit.point;
            float rx = Mathf.Round(position.x);
            float rz = Mathf.Round(position.z);
            position = new Vector3(rx, 0f, rz);
            
            mark.transform.position = new Vector3(position.x + 0.5f, 0, position.z + 0.5f);
            currentBuildTarget.transform.position = position;
        }

        if (tileMapTests.All(tileMapTest => tileMapTest.gameObjects.Count == 0))
        {
            GameManager.gameAsset.MouseLeftClick = () => { Debug.Log("允许"); };
        }
        else
        {
            GameManager.gameAsset.MouseLeftClick = () => { Debug.Log("不允许"); };
        }
    }

    private void BuildingManagerEnd()
    {
        this.enabled = false;
    }

    private void BuildingManagerStart()
    {
        this.enabled = true;
    }
}