namespace ColourMatch
{
    public readonly struct ViewTransitionEvent
    {
        public UIViewType UIViewToShow { get; }

        public ViewTransitionEvent(UIViewType uiViewToShow)
        {
            UIViewToShow = uiViewToShow;
        }
    }
}
