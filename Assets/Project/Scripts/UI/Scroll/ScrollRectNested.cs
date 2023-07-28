using System;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RedPanda.Project.UI.Scroll
{
    [CustomEditor(typeof(ScrollRectNested))]
    public class ScrollRectNestedEditor : ScrollRectEditor
    {
        private SerializedProperty _parentScrollRect;

        protected override void OnEnable()
        {
            base.OnEnable();
            _parentScrollRect = serializedObject.FindProperty("_parentScrollRect");
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(_parentScrollRect);
            serializedObject.ApplyModifiedProperties();
        }
    }

    public class ScrollRectNested : ScrollRect
    {
        [SerializeField] private ScrollRect _parentScrollRect;

        private bool _routeToParent;

        public void SetParentScroll(ScrollRect parenScroll)
        {
            _parentScrollRect = parenScroll;
        }
        
        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            _parentScrollRect.OnInitializePotentialDrag(eventData);
            base.OnInitializePotentialDrag(eventData);
        }
        
        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (vertical && Math.Abs (eventData.delta.x) > Math.Abs (eventData.delta.y) || 
                horizontal && Math.Abs (eventData.delta.x) < Math.Abs (eventData.delta.y))
            {
                _routeToParent = true;
            }

            if (_routeToParent)
            {
                _parentScrollRect.OnBeginDrag(eventData);
            }
            else
            {
                base.OnBeginDrag(eventData);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (_routeToParent)
            {
                _parentScrollRect.OnDrag(eventData);
            }
            else
            {
                base.OnDrag(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (_routeToParent)
            {
                _parentScrollRect.OnEndDrag(eventData);
            }
            else
            {
                base.OnEndDrag(eventData);
            }

            _routeToParent = false;
        }
    }
}
