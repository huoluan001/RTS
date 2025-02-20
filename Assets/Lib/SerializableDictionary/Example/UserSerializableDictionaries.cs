using System.Collections;
using System.Collections.Generic;
using System;
using GameData.Script.Enum;
using UnityEngine;

[Serializable]
public class CrushList : SerializableDictionary<CrushLabel, Vector2> {}
[Serializable]
public class MainBuildingSoTableElement : SerializableDictionary<int, MainBuildingSo> {}
[Serializable]
public class OtherBuildingSoTableElement : SerializableDictionary<int, OtherBuildingSo> {}
[Serializable]
public class ArmySoTableElement : SerializableDictionary<int, ArmySo> {}
[Serializable]
public class ArmorSoTableElement : SerializableDictionary<int, ArmorSo> {}

[Serializable]
public class DamageModifiers : SerializableDictionary<DamageTypeEnum, int> {}

[Serializable]
public class StringStringDictionary : SerializableDictionary<string, string> {}

[Serializable]
public class ObjectColorDictionary : SerializableDictionary<UnityEngine.Object, Color> {}

[Serializable]
public class ColorArrayStorage : SerializableDictionary.Storage<Color[]> {}

[Serializable]
public class StringColorArrayDictionary : SerializableDictionary<string, Color[], ColorArrayStorage> {}

[Serializable]
public class MyClass
{
    public int i;
    public string str;
}

[Serializable]
public class QuaternionMyClassDictionary : SerializableDictionary<Quaternion, MyClass> {}