using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject selectionBoxUI;
    public GameAsset gameAsset;
    public Page<MainBuildingSO> mainBuildingPage;
    public Page<OtherBuildingSO> otherBuildingPage;
    // 四个兵种的page。暂时略
    // public Page<ArmySO> ;

    public GameObject sequencesParent;

    private void Start()
    {
        gameAsset.selectionBoxUI = selectionBoxUI;
        gameAsset.graphicRaycaster = GetComponent<GraphicRaycaster>();

        CreatePage(SequenceType.MainBuildingSequence);
    }


    public void CreatePage(SequenceType sequenceType)
    {
        // 主建筑
        if (sequenceType == SequenceType.MainBuildingSequence)
        {
            GameObject sequenceGameObject = Instantiate(gameAsset.mainBuildingSequencePagePrefab);
            var page = sequenceGameObject.GetComponent<Page<MainBuildingSO>>();
            page.sequenceType = sequenceType;
            page.transform.SetParent(sequencesParent.transform, false);
            page.Infos = gameAsset.factionSO.MainBuildings;
            page.isShow = true;
            mainBuildingPage = page;
        }
        // 其他建筑
        else if (sequenceType == SequenceType.OtherBuildingSequence)
        {
            GameObject sequenceGameObject = Instantiate(gameAsset.mainBuildingSequencePagePrefab);
            var page = sequenceGameObject.GetComponent<Page<OtherBuildingSO>>();
            page.sequenceType = sequenceType;
            page.transform.SetParent(sequencesParent.transform, false);
            page.Infos = gameAsset.factionSO.MainBuildings;
            page.isShow = true;
            otherBuildingPage = page;
        }
        // 军队
        else
        {
            GameObject sequenceGameObject = Instantiate(gameAsset.mainBuildingSequencePagePrefab);
            var page = sequenceGameObject.GetComponent<Page<ArmySO>>();
            page.sequenceType = SequenceType.MainBuildingSequence;
            page.transform.SetParent(sequencesParent.transform, false);
            page.Infos = gameAsset.factionSO.MainBuildings;
            page.isShow = true;
            // Page = page;
        }
        


    }
}
