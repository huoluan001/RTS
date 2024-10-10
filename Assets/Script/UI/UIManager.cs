using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject selectionBoxUI;
    public GameAsset gameAsset;
    public MainBuildingPage mainBuildingPage;

    public GameObject sequencesParent;

    private void Start()
    {
        gameAsset.selectionBoxUI = selectionBoxUI;
        gameAsset.graphicRaycaster = GetComponent<GraphicRaycaster>();

        CreateSequence(SequenceType.MainBuildingSequence);
    }
    public void CreateMainBuildingSequence()
    {
        GameObject sequenceGameObject = Instantiate(gameAsset.sequencePagePrefab);
        var page = sequenceGameObject.GetComponent<MainBuildingPage>();
        page.sequenceType = SequenceType.MainBuildingSequence;
        page.transform.SetParent(sequencesParent.transform, false);
        page.Infos = gameAsset.factionSO.MainBuildings;
        page.isShow = true;
    }
    // public void CreateArmyPage(SequenceType sequenceType)
    // {
    //     GameObject sequenceGameObject = Instantiate(gameAsset.sequencePagePrefab);
    //     var page = sequenceGameObject.GetComponent<Page>();
    //     page.sequenceType = sequenceType;
    //     page.transform.SetParent(sequencesParent.transform, false);
    //     if(sequenceType == SequenceType.MainBuildingSequence)
    //     {
    //         page.buildingInfos = gameAsset.factionSO.MainBuildings;
    //     }
    //     else if(sequenceType == SequenceType.InfantrySequence || 
    //                 sequenceType == SequenceType.VehicleSequence || 
    //                     sequenceType == SequenceType.VehicleSequence || 
    //                         sequenceType == SequenceType.DockSequence) 
    //     {
    //         page.armyInfos = gameAsset.factionSO.Infantry;
    //     }
    //     page.isShow = true;
    // }

    public void CreateSequence(SequenceType sequenceType)
    {
        if(sequenceType == SequenceType.MainBuildingSequence)
        {
            if(mainBuildingPage)
            {
                CreateMainBuildingSequence();
            }
            mainBuildingPage.AddSequence();
        }
        // if(!pages.ContainsKey(sequenceType))
        // {
        //     CreatePage(sequenceType);
        // }
        // pages[sequenceType].AddSequence();
    }
}
