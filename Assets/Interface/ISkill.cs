using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public interface ISkill
{
    List<Skill> Skills { get; }
    void SetSkill(string skillNameZH, string skillNameEN, string commentChinese, int skillCooling, int skillPre_Swing, int skillPost_Swing);
}