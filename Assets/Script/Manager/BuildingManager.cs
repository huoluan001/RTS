using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject currentBuildTarget;

    public LayerMask groundLayer;

    public void StartBuilding(GameObject targetPrefab)
    {
        currentBuildTarget = Instantiate(targetPrefab);
        BuildingManagerStart();
    }
    public void Awake()
    {
        GameManager.gameAsset.buildingManager = this;
        BuildingManagerEnd();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, groundLayer))
        {
            Vector3 originPos = raycastHit.point;
            float rx = Mathf.Round(originPos.x);
            float rz = Mathf.Round(originPos.z);
            float diffx = Mathf.Abs(originPos.x - rx);
            float diffy = Mathf.Abs(originPos.z - rz);
            if(Mathf.Abs(diffx - 0.5f) < 0.25f && Mathf.Abs(diffy - 0.5f) < 0.25f)
            {
                
            }
           
            Debug.Log(v3);
            currentBuildTarget.transform.position = v3;
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