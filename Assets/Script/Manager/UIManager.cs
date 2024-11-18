using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject selectionBoxUI;

    // save page info
    public Page mainBuildingPage;
    public Page otherBuildingPage;
    public Page infantryPage;
    public Page vehiclePage;
    public Page aircraftPage;
    public Page dockPage;

    public GameObject pageParent;
    public GameObject mainBuildingPagePrefab;
    public GameObject otherBuildingPagePrefab;


    public GameObject MCV;
    private void Awake()
    {
        GameManager.GameAsset.UIManager = this;
        GameManager.GameAsset.selectionBoxUI = selectionBoxUI;
        GameManager.GameAsset.graphicRaycaster = GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        // CreatePage(SequenceType.MainBuildingSequence);
    }
    public Vector2Int AddMCVSequence(FactionSo factionSo)
    {
        if (mainBuildingPage is null)
            return CreateMainAndOtherBuildingPage(factionSo);
        else
        {
            int mbID = mainBuildingPage.AddSequence(factionSo);
            int obID = otherBuildingPage.AddSequence(factionSo);
            return new Vector2Int(mbID, obID);
        }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            GameManager.GameAsset.buildingManager.StartBuilding(MCV);
        }
    }
    public Vector2Int CreateMainAndOtherBuildingPage(FactionSo factionSo)
    {
        GameObject mbPageGameObject = Instantiate(mainBuildingPagePrefab, pageParent.transform, false);
        GameObject obPageGameObject = Instantiate(otherBuildingPagePrefab, pageParent.transform, false);

        var mbpage = mbPageGameObject.GetComponent<Page>();
        var obpage = obPageGameObject.GetComponent<Page>();
        int mbID = mbpage.Init(SequenceType.MainBuildingSequence, true, factionSo);
        int obID = obpage.Init(SequenceType.OtherBuildingSequence, false, factionSo);
        mainBuildingPage = mbpage;
        otherBuildingPage = obpage;
        return new Vector2Int(mbID, obID);
    }

    public void PageShowSwitch(int sequenceType)
    {
        Page currentPage = ConvertToPage((SequenceType)sequenceType);
        if (currentPage == null) return;
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
