using System.Collections.Generic;
using UnityEngine;

namespace ColourMatch
{
    public class ViewRegistry : MonoBehaviour
    {
        public List<UIViewEntry> views;

        [System.Serializable]
        public struct UIViewEntry
        {
            public ViewType viewType;
            public ViewBase view;
        }
    }
}
