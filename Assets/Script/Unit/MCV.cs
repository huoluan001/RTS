using NodeCanvas.StateMachines;
using UnityEngine;

public class MVC : MainBuilding
{
    public ArmySo armySo;
    
    public TroopType currentState = TroopType.Vehicle;

    

    public int mainBuildingSequenceID;
    public int OtherBuildingSequenceID;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Open();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            GameManager.GameAsset.UIManager.AddMCVSequence(factionSo);
        }
    }

    public virtual void Open()
    {
        var id = GameManager.GameAsset.UIManager.AddMCVSequence(factionSo);
        mainBuildingSequenceID = id.x;
        OtherBuildingSequenceID = id.y;
    }

    public virtual void Pack()
    {

    }


} 