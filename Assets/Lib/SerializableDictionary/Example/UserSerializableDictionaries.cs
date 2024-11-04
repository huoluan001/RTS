using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class CrushList : SerializableDictionary<CrushLabel, Vector2> {}
[Serializable]
public class MainBuildingSOTableElement : SerializableDictionary<int, MainBuildingSO> {}
[Serializable]
public class OtherBuildingSOTableElement : SerializableDictionary<int, OtherBuildingSO> {}
[Serializable]
public class ArmySOTableElement : SerializableDictionary<int, ArmySO> {}
[Serializable]
public class ArmorSOTableElement : SerializableDictionary<int, ArmorSO> {}

[Serializable]
public class DamageModifiers : SerializableDictionary<DamageTypeSO, int> {}

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