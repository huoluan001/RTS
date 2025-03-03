﻿#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using NavMesh = UnityEngine.AI.NavMesh;
using NavMeshAgent = UnityEngine.AI.NavMeshAgent;
using ParadoxNotion.Serialization;

namespace ParadoxNotion.Design
{

    ///<summary>Collection of preferred user types and utilities for types, type colors and icons</summary>
    public static class TypePrefs
    {

        //Raised when the preferred types list change
        public static event Action onPreferredTypesChanged;

        public const string SYNC_FILE_NAME = "PreferredTypes.typePrefs";
        public const string LIST_MENU_STRING = "Collections/List (T)/";
        public const string DICT_MENU_STRING = "Collections/Dictionary (T)/";
        private static readonly string TYPES_PREFS_KEY = string.Format("ParadoxNotion.{0}.PreferedTypes", PlayerSettings.productName);

        private static List<Type> _preferedTypesAll;
        private static List<Type> _preferedTypesFiltered;

        private static readonly List<Type> defaultTypesList = new List<Type>
        {
            typeof(object),
            typeof(System.Type),

			//Primitives
			typeof(string),
            typeof(float),
            typeof(int),
            typeof(bool),

			//Unity basics
			typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
            typeof(Quaternion),
            typeof(Color),
            typeof(LayerMask),
            typeof(AnimationCurve),
            typeof(RaycastHit),
            typeof(RaycastHit2D),

			//Unity functional classes
			typeof(Debug),
            typeof(Application),
            typeof(Mathf),
            typeof(Physics),
            typeof(Physics2D),
            typeof(Input),
            typeof(NavMesh),
            typeof(PlayerPrefs),
            typeof(UnityEngine.Random),
            typeof(Time),
            typeof(UnityEngine.SceneManagement.SceneManager),

			//Unity Objects
			typeof(UnityEngine.Object),
            typeof(UnityEngine.MonoBehaviour),
            typeof(UnityEngine.ScriptableObject),
            typeof(GameObject),
            typeof(Transform),
            typeof(Animator),
            typeof(Rigidbody),
            typeof(Rigidbody2D),
            typeof(Collider),
            typeof(Collider2D),
            typeof(NavMeshAgent),
            typeof(CharacterController),
            typeof(AudioSource),
            typeof(Camera),
            typeof(Light),
            typeof(Renderer),

			//UGUI
			typeof(UnityEngine.UI.Button),
            typeof(UnityEngine.UI.Slider),

			//Unity Asset Objects
			typeof(Texture2D),
            typeof(Sprite),
            typeof(Material),
            typeof(AudioClip),
            typeof(AnimationClip),
            typeof(UnityEngine.Audio.AudioMixer),
            typeof(TextAsset),
        };

        //These types will be filtered out when requesting types with 'filterOutFunctionalOnlyTypes' true.
        //The problem with these is that they are not static thus instance can be made, but still, there is no reason to have an instance cause their members are static.
        //Most of them are also probably singletons.
        //Hopefully this made sense :)
        public static readonly List<Type> functionalTypesBlacklist = new List<Type>
        {
            typeof(Debug),
            typeof(Application),
            typeof(Mathf),
            typeof(Physics),
            typeof(Physics2D),
            typeof(Input),
            typeof(NavMesh),
            typeof(PlayerPrefs),
            typeof(UnityEngine.Random),
            typeof(Time),
            typeof(UnityEngine.SceneManagement.SceneManager),
        };


        //The default prefered types list
        private static string defaultTypesListString {
            get { return string.Join("|", defaultTypesList.OrderBy(t => t.Namespace).ThenBy(t => t.Name).Select(t => t.FullName).ToArray()); }
        }

        ///----------------------------------------------------------------------------------------------

        [InitializeOnLoadMethod]
        static void LoadTypes() {
            _preferedTypesAll = new List<Type>();

            if ( TryLoadSyncFile(ref _preferedTypesAll) ) {
                SetPreferedTypesList(_preferedTypesAll);
                return;
            }

            foreach ( var s in EditorPrefs.GetString(TYPES_PREFS_KEY, defaultTypesListString).Split('|') ) {
                var resolvedType = ReflectionTools.GetType(s, /*fallback?*/ true);
                if ( resolvedType != null ) {
                    _preferedTypesAll.Add(resolvedType);
                } else { ParadoxNotion.Services.Logger.Log("Missing type in Preferred Types Editor. Type removed."); }
            }
            //re-write back, so that fallback type resolved are saved
            SetPreferedTypesList(_preferedTypesAll);
        }

        ///<summary>Get the prefered types set by the user.</summary>
        public static List<Type> GetPreferedTypesList(bool filterOutFunctionalOnlyTypes = false) { return GetPreferedTypesList(typeof(object), filterOutFunctionalOnlyTypes); }
        public static List<Type> GetPreferedTypesList(Type baseType, bool filterOutFunctionalOnlyTypes = false) {

            if ( _preferedTypesAll == null || _preferedTypesFiltered == null ) {
                LoadTypes();
            }

            if ( baseType == typeof(object) ) {
                return filterOutFunctionalOnlyTypes ? _preferedTypesFiltered : _preferedTypesAll;
            }

            if ( filterOutFunctionalOnlyTypes ) {
                return _preferedTypesFiltered.Where(t => t != null && baseType.IsAssignableFrom(t)).ToList();
            }
            return _preferedTypesAll.Where(t => t != null && baseType.IsAssignableFrom(t)).ToList();
        }

        ///<summary>Set the prefered types list for the user</summary>
        public static void SetPreferedTypesList(List<Type> types) {
            var finalTypes = types
            .Where(t => t != null && !t.IsGenericType)
            .OrderBy(t => t.Namespace)
            .ThenBy(t => t.Name)
            .ToList();
            var joined = string.Join("|", finalTypes.Select(t => t.FullName).ToArray());
            EditorPrefs.SetString(TYPES_PREFS_KEY, joined);
            _preferedTypesAll = finalTypes;

            var finalTypesFiltered = finalTypes
            .Where(t => !functionalTypesBlacklist.Contains(t) /*&& !t.IsInterface && !t.IsAbstract*/ )
            .ToList();
            _preferedTypesFiltered = finalTypesFiltered;

            TrySaveSyncFile(finalTypes);

            if ( onPreferredTypesChanged != null ) {
                onPreferredTypesChanged();
            }
        }

        ///<summary>Append a type to the list</summary>
        public static void AddType(Type type) {
            var current = GetPreferedTypesList(typeof(object));
            if ( !current.Contains(type) ) {
                current.Add(type);
            }
            SetPreferedTypesList(current);
        }

        ///<summary>Reset the prefered types to the default ones</summary>
        public static void ResetTypeConfiguration() {
            SetPreferedTypesList(defaultTypesList);
        }

        ///----------------------------------------------------------------------------------------------

        ///<summary>Is there a typePrefs file in sync? returns it's path.</summary>
        public static string SyncFilePath() {
            var syncFile = EditorGUIUtility.Load(SYNC_FILE_NAME);
            var absPath = EditorUtils.AssetToSystemPath(syncFile);
            if ( !string.IsNullOrEmpty(absPath) ) {
                return absPath;
            }
            return null;
        }

        ///<summary>Will try load from file found in DefaultEditorResources</summary>
        static bool TryLoadSyncFile(ref List<Type> result) {
            var absPath = SyncFilePath();
            if ( !string.IsNullOrEmpty(absPath) ) {
                var json = System.IO.File.ReadAllText(absPath);
                var temp = JSoNSerializer.Deserialize<List<Type>>(json);
                if ( temp != null ) {
                    result = temp;
                    return true;
                }
            }
            return false;
        }

        ///<summary>Will try save to file found in DefaultEditorResources</summary>
        static void TrySaveSyncFile(List<Type> types) {
            var absPath = SyncFilePath();
            if ( !string.IsNullOrEmpty(absPath) ) {
                var json = JSoNSerializer.Serialize(typeof(List<Type>), types, null, true);
                System.IO.File.WriteAllText(absPath, json);
            }
        }

        //----------------------------------------------------------------------------------------------

        private static readonly Color DEFAULT_TYPE_COLOR = Colors.Grey(0.75f);
        ///<summary>A Type to color lookup initialized with some types already</summary>
        private static Dictionary<Type, Color> typeColors = new Dictionary<Type, Color>()
        {
            {typeof(Delegate),           new Color(1,0.4f,0.4f)},
            {typeof(bool?),              new Color(1,0.4f,0.4f)},
            {typeof(float?),             new Color(0.6f,0.6f,1)},
            {typeof(int?),               new Color(0.5f,1,0.5f)},
            {typeof(string),             new Color(0.55f,0.55f,0.55f)},
            {typeof(Vector2?),           new Color(1f,0.7f,0.2f)},
            {typeof(Vector3?),           new Color(1f,0.7f,0.2f)},
            {typeof(Vector4?),           new Color(1f,0.7f,0.2f)},
            {typeof(Quaternion?),        new Color(1f,0.7f,0.2f)},
            {typeof(GameObject),         new Color(0.537f, 0.415f, 0.541f)},
            {typeof(UnityEngine.Object), Color.grey}
        };

        ///<summary>Get color for type</summary>
        public static Color GetTypeColor(MemberInfo info) {
            if ( !EditorGUIUtility.isProSkin ) { return Color.white; }
            if ( info == null ) { return Color.black; }
            var type = info is Type ? info as Type : info.ReflectedType;
            if ( type == null ) { return Color.black; }

            Color color;
            if ( typeColors.TryGetValue(type, out color) ) {
                return color;
            }

            foreach ( var pair in typeColors ) {

                if ( pair.Key.IsAssignableFrom(type) ) {
                    return typeColors[type] = pair.Value;
                }

                if ( typeof(IEnumerable).IsAssignableFrom(type) ) {
                    var elementType = type.GetEnumerableElementType();
                    if ( elementType != null ) {
                        return typeColors[type] = GetTypeColor(elementType);
                    }
                }
            }

            return typeColors[type] = DEFAULT_TYPE_COLOR;
        }

        ///<summary>Get the hex color preference for a type</summary>
        public static string GetTypeHexColor(Type type) {
            if ( !EditorGUIUtility.isProSkin ) {
                return "#000000";
            }
            return ColorUtils.ColorToHex(GetTypeColor(type));
        }


        //----------------------------------------------------------------------------------------------

        private const string DEFAULT_TYPE_ICON_NAME = "System.Object";
        private const string IMPLICIT_ICONS_PATH = "TypeIcons/Implicit/";
        private const string EXPLICIT_ICONS_PATH = "TypeIcons/Explicit/";

        ///<summary>A Type.FullName to texture lookup. Use string instead of type to also handle attributes</summary>
        private static Dictionary<string, Texture> typeIcons = new Dictionary<string, Texture>(StringComparer.OrdinalIgnoreCase);

        ///<summary>Get icon for type</summary>
        public static Texture GetTypeIcon(MemberInfo info, bool fallbackToDefault = true) {
            if ( info == null ) { return null; }
            var type = info is Type ? info as Type : info.ReflectedType;
            if ( type == null ) { return null; }

            Texture texture = null;
            if ( typeIcons.TryGetValue(type.FullName, out texture) ) {
                if ( texture != null ) {
                    if ( texture.name != DEFAULT_TYPE_ICON_NAME || fallbackToDefault ) {
                        return texture;
                    }
                }
                return null;
            }

            if ( texture == null ) {
                if ( type.IsEnumerableCollection() ) {
                    var elementType = type.GetEnumerableElementType();
                    if ( elementType != null ) {
                        texture = GetTypeIcon(elementType);
                    }
                }
            }

            if ( typeof(UnityEngine.Object).IsAssignableFrom(type) ) {
                texture = AssetPreview.GetMiniTypeThumbnail(type);
                if ( texture == null && ( typeof(MonoBehaviour).IsAssignableFrom(type) || typeof(ScriptableObject).IsAssignableFrom(type) ) ) {
                    texture = EditorGUIUtility.ObjectContent(EditorUtils.MonoScriptFromType(type), null).image;
                    if ( texture == null ) {
                        texture = Icons.csIcon;
                    }
                }
            }

            if ( texture == null ) {
                texture = Resources.Load<Texture>(IMPLICIT_ICONS_PATH + type.FullName);
            }

            ///<summary>Explicit icons not in dark theme</summary>
            if ( EditorGUIUtility.isProSkin ) {
                if ( texture == null ) {
                    var iconAtt = type.RTGetAttribute<ParadoxNotion.Design.IconAttribute>(true);
                    if ( iconAtt != null ) {
                        texture = GetTypeIcon(iconAtt, null);
                    }
                }
            }

            if ( texture == null ) {
                var current = type.BaseType;
                while ( current != null ) {
                    texture = Resources.Load<Texture>(IMPLICIT_ICONS_PATH + current.FullName);
                    current = current.BaseType;
                    if ( texture != null ) {
                        break;
                    }
                }
            }

            if ( texture == null ) {
                texture = Resources.Load<Texture>(IMPLICIT_ICONS_PATH + DEFAULT_TYPE_ICON_NAME);
            }

            typeIcons[type.FullName] = texture;

            if ( texture != null ) { //it should not be
                if ( texture.name != DEFAULT_TYPE_ICON_NAME || fallbackToDefault ) {
                    return texture;
                }
            }
            return null;
        }

        ///<summary>Get icon from [ParadoxNotion.Design.IconAttribute] info</summary>
        public static Texture GetTypeIcon(IconAttribute iconAttribute, object instance = null) {
            if ( iconAttribute == null ) { return null; }

            if ( instance != null && !string.IsNullOrEmpty(iconAttribute.runtimeIconTypeCallback) ) {
                var callbackMethod = instance.GetType().RTGetMethod(iconAttribute.runtimeIconTypeCallback);
                return callbackMethod != null && callbackMethod.ReturnType == typeof(Type) ? GetTypeIcon((Type)callbackMethod.Invoke(instance, null), false) : null;
            }

            if ( iconAttribute.fromType != null ) {
                return GetTypeIcon(iconAttribute.fromType, true);
            }

            Texture texture = null;
            if ( typeIcons.TryGetValue(iconAttribute.iconName, out texture) ) {
                return texture;
            }

            if ( !string.IsNullOrEmpty(iconAttribute.iconName) ) {
                texture = Resources.Load<Texture>(EXPLICIT_ICONS_PATH + iconAttribute.iconName);
                if ( texture == null ) { //for user made icons where user don't have to know the path
                    texture = Resources.Load<Texture>(iconAttribute.iconName);
                }
                if ( texture == null ) { //for user made icons where user provide a non resources path
                    texture = AssetDatabase.LoadAssetAtPath<Texture>(iconAttribute.iconName);
                }
            }
            return typeIcons[iconAttribute.iconName] = texture;
        }

        ///----------------------------------------------------------------------------------------------

        ///<summary>A Type.FullName to documentation lookup</summary>
        private static Dictionary<string, string> typeDocs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        ///<summary>Get documentation for type fetched either by the [Description] attribute, or it's xml doc.</summary>
        public static string GetTypeDoc(MemberInfo info) {
            if ( info == null ) { return null; }
            var type = info is Type ? info as Type : info.ReflectedType;
            if ( type == null ) { return null; }

            string doc = null;
            if ( typeDocs.TryGetValue(type.FullName, out doc) ) {
                return doc;
            }

            var descAtt = type.RTGetAttribute<DescriptionAttribute>(true);
            if ( descAtt != null ) {
                doc = descAtt.description;
            }

            if ( doc == null ) {
                doc = XMLDocs.GetMemberSummary(type);
            }

            if ( doc == null ) {
                doc = "No Documentation";
            }

            return typeDocs[type.FullName] = doc;
        }

    }
}

#endif