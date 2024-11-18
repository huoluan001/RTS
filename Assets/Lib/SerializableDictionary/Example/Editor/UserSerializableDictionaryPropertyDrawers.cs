using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StringStringDictionary))]
[CustomPropertyDrawer(typeof(ObjectColorDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
[CustomPropertyDrawer(typeof(DamageModifiers))]
[CustomPropertyDrawer(typeof(CrushList))]
[CustomPropertyDrawer(typeof(MainBuildingSoTableElement))]
[CustomPropertyDrawer(typeof(OtherBuildingSoTableElement))]
[CustomPropertyDrawer(typeof(ArmySoTableElement))]
[CustomPropertyDrawer(typeof(ArmorSoTableElement))]

public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(ColorArrayStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}
