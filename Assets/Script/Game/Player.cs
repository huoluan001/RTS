using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    #region 开局控制信息
    public string PlayerName;
    public Color color;
    public TeamId teamId = TeamId.None;
    public FactionSO factionSO;
    public FactionEnum factionEnum;
    #endregion

    public Identity identity = Identity.Dependent;
    public Technology MaxTechnology = Technology.T1;
    public int PowerProduction = 0;
    public int PowerConsumption = 0;
    public float Fund;
    public bool isRunningProduce = true;

    public CommandModel commandModel = CommandModel.None;
    public CommandType commandType = CommandType.None;


    public List<Army> armies = new List<Army>();

    public GameAsset gameAsset;
    private void Awake()
    {
        gameAsset.commander = this;
        GameManager.gameAsset = gameAsset;
        
    }


    public void Pay(float value)
    {
        Fund -= value;
    }

    public void ProduceStart()
    {
        isRunningProduce = true;
    }
    public void ProduceStop()
    {
        isRunningProduce = false;
    }

}


