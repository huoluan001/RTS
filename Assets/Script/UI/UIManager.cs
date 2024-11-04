using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject selectionBoxUI;

    public Page mainBuildingPage;
    public Page otherBuildingPage;
    public Page infantryPage;
    public Page vehiclePage;
    public Page aircraftPage;
    public Page dockPage;

    public GameObject pageParent;
    public GameObject mainBuildingPagePrefab;
    public GameObject otherBuildingPagePrefab;
    private void Awake()
    {
        GameManager.gameAsset.UIManager = this;
        GameManager.gameAsset.selectionBoxUI = selectionBoxUI;
        GameManager.gameAsset.graphicRaycaster = GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        // CreatePage(SequenceType.MainBuildingSequence);
    }
    public Vector2Int AddMCVSequence(FactionSO factionSO)
    {
        if (mainBuildingPage == null)
            CreateMainAndOtherBuildingPage();
        int mainID = mainBuildingPage.AddSequence(factionSO);
        int otherID = otherBuildingPage.AddSequence(factionSO);
        return new Vector2Int(mainID, otherID);

    }
    public void CreateMainAndOtherBuildingPage()
    {
        GameObject mbPageGameObject = Instantiate(mainBuildingPagePrefab);
        GameObject obPageGameObject = Instantiate(otherBuildingPagePrefab);

        var mbpage = mbPageGameObject.AddComponent<Page>();
        var obPage = obPageGameObject.AddComponent<Page>();

        mbpage.transform.SetParent(pageParent.transform, false);
        obPage.transform.SetParent(pageParent.transform, false);
        mbpage.sequenceType = SequenceType.MainBuildingSequence;
        obPage.sequenceType = SequenceType.OtherBuildingSequence;
        mbpage.isShow = true;
        obPage.isShow = false;
        mainBuildingPage = mbpage;
        otherBuildingPage = obPage;
    }

    public void PageShowSwitch(int sequenceType)
    {
        Debug.Log(sequenceType);
        Page currentPage = ConvertToPage((SequenceType)sequenceType);
        bool clickingShow = currentPage.isShow;
        HideAllPage();
        currentPage.isShow = !clickingShow;
        currentPage.ShowAndHideSwitch();
    }

    public void HideAllPage()
    {
        mainBuildingPage?.Hide();
        otherBuildingPage?.Hide();
        infantryPage?.Hide();
        vehiclePage?.Hide();
        aircraftPage?.Hide();
        dockPage?.Hide();
    }
    public Page ConvertToPage(SequenceType sequenceType)
    {
        return sequenceType switch
        {
            SequenceType.MainBuildingSequence => mainBuildingPage,
            SequenceType.OtherBuildingSequence => otherBuildingPage,
            SequenceType.InfantrySequence => infantryPage,
            SequenceType.VehicleSequence => vehiclePage,
            SequenceType.AircraftSequence => aircraftPage,
            SequenceType.DockSequence => dockPage,
            _ => null
        };
    }
}
