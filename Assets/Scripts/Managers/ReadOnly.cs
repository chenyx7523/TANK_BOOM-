﻿//using UnityEditor;

//namespace UnityEngine
//{
//    /// <summary>
//    /// 设置属性只读
//    /// </summary>
//    public class ReadOnly : PropertyAttribute
//    {

//    }
//    [CustomPropertyDrawer(typeof(ReadOnly))]
//    public class ReadOnlyDrawer : PropertyDrawer
//    {
//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//        {
//            return EditorGUI.GetPropertyHeight(property, label, true);
//        }
//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            GUI.enabled = false;
//            EditorGUI.PropertyField(position, property, label, true);
//            GUI.enabled = true;
//        }
//    }
//}

