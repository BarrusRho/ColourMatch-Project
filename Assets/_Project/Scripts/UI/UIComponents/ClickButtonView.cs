﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ColourMatch
{
    public class ClickButtonView : MonoBehaviour, IPointerClickHandler
    {
        public bool IsButtonClicked { get; set; }

        public event Action OnClicked;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            IsButtonClicked = true;
            OnClicked?.Invoke();
        }
    }
}