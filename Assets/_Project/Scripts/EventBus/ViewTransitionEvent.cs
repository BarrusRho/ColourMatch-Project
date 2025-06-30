namespace ColourMatch
{
    public readonly struct ViewTransitionEvent
    {
        public ViewType ViewToShow { get; }

        public ViewTransitionEvent(ViewType viewToShow)
        {
            ViewToShow = viewToShow;
        }
    }
}
