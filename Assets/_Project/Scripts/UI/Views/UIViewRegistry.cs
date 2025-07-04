using System.Collections.Generic;
using UnityEngine;

namespace ColourMatch
{
    public class UIViewRegistry : MonoBehaviour
    {
        public List<UIViewEntry> views;

        [System.Serializable]
        public struct UIViewEntry
        {
            public UIViewType uiViewType;
            public UIViewBase uiView;
        }
    }
}
