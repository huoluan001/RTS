using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StringStringDictionary))]
[CustomPropertyDrawer(typeof(ObjectColorDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
[CustomPropertyDrawer(typeof(DamageModifiers))]
[CustomPropertyDrawer(typeof(CrushList))]
[CustomPropertyDrawer(typeof(MainBuildingSOPageElement))]
[CustomPropertyDrawer(typeof(OtherBuildingSOPageElement))]
[CustomPropertyDrawer(typeof(ArmySOPageElement))]


public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(ColorArrayStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}
