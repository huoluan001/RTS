using System.Collections.Generic;
using UnityEngine;

public interface IBaseInfo
{
    FactionSO FactionSO { get; }
    int Id { get; }
    string NameChinese { get; }
    string NameEnglish { get; }
    Sprite Icon { get; }
    string CommentChinese { get; }
    string CommentEnglish { get; }
    Troop Troop { get; }
    List<ActionScope> ActionScopes { get; }
    int Exp { get; }
    int Hp { get; }
    int Price { get; }
    List<MainBuildingSO> Requirement { get; }
    Vector2Int WarningAndClearFogRad { get; }
    ArmorSO ArmorType { get; }
    GameObject GameObjectPrefab { get; }

    void SetBaseInfo(FactionSO factionSO, int id, string nameChinese, string nameEnglish, Sprite icon,
                        string commentChinese, string commentEnglish, Troop troop,
                            List<ActionScope> actionScopes, int exp, int hp, int price,
                                List<MainBuildingSO> requirement, Vector2Int warningAndClearFogRad,
                                ArmorSO armorType, GameObject gameObjectPrefab);
}