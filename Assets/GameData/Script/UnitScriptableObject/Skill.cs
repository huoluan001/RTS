using System;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Skill
{
    public string skillNameZh;
    public string skillNameEn;
    public Sprite icon;
    [TextArea(2,2)] public string commentChinese;
    [Tooltip("技能冷却时间")] public float skillCooling;
    [Tooltip("技能前摇")] public float skillPre_Swing;
    [Tooltip("技能后摇")] public float skillPost_Swing;
}