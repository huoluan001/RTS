using System.Collections.Generic;
using NPOI.HSSF.Model;
using UnityEngine;

public interface IBaseInfo
{
    FactionSo FactionSo { get; }
    int Id { get; }
    int Priority { get; }
    string NameChinese { get; }
    string NameEnglish { get; }
    Sprite Icon { get; }
    string CommentChinese { get; }
    string CommentEnglish { get; }
    TroopType TroopType { get; }
    List<ActionScope> ActionScopes { get; }
    int Exp { get; }
    int Hp { get; }
    int Price { get; }
    List<MainBuildingSo> Requirement { get; }
    Vector2Int WarningAndClearFogRad { get; }
    ArmorSo ArmorType { get; }
    GameObject GameObjectPrefab { get; }

    void SetBaseInfo(FactionSo factionSo, int id, string nameChinese, string nameEnglish, Sprite icon,
                        string commentChinese, string commentEnglish, TroopType troopType,
                            List<ActionScope> actionScopes, int exp, int hp, int price,
                                List<MainBuildingSo> requirement, Vector2Int warningAndClearFogRad,
                                ArmorSo armorType, GameObject gameObjectPrefab);
}