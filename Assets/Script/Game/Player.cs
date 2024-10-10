using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    #region 开局控制信息
    public string PlayerName;
    public Color color;
    public TeamId teamId = TeamId.None;
    public FactionSO factionSO;
    #endregion

    public Identity identity = Identity.Dependent;
    public Technology MaxTechnology = Technology.T1;
    public uint PowerProduction = 0;
    public uint PowerConsumption = 0;
    public uint Fund;

    public CommandModel commandModel = CommandModel.None;
    public CommandType commandType = CommandType.None;


    public List<Army> armies = new List<Army>();

    public GameAsset gameAsset;
    private void Awake()
    {
        gameAsset.factionSO = factionSO;
    }

}

