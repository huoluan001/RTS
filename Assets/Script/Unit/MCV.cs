using NodeCanvas.StateMachines;
using UnityEngine;

public class MVC : MonoBehaviour
{
    public Player command;
    public TroopType currentState = TroopType.Vehicle;

    public FactionEnum factionEnum;
    public FactionSO factionSO;

    public int mainBuildingSequenceID;
    public int OtherBuildingSequenceID;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Open();
            Debug.Log("ddd");
        }
    }

    public virtual void Open()
    {
        var id = GameManager.gameAsset.UIManager.AddMCVSequence(factionSO);
        mainBuildingSequenceID = id.x;
        OtherBuildingSequenceID = id.y;
    }

    public virtual void Pack()
    {

    }


} 