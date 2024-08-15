namespace Client.Maui.Behaviors
{
    public class FadeInBehavior : Behavior<VisualElement>
    {
        public static readonly BindableProperty DurationProperty = BindableProperty.Create(
            nameof(Duration),
            typeof(uint),
            typeof(FadeInBehavior),
            (uint)250
        );

        public static readonly BindableProperty DelayProperty = BindableProperty.Create(
            nameof(Delay),
            typeof(uint),
            typeof(FadeInBehavior),
            (uint)0
        );

        public uint Duration
        {
            get => (uint)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        public uint Delay
        {
            get => (uint)GetValue(DelayProperty);
            set => SetValue(DelayProperty, value);
        }

        protected override async void OnAttachedTo(VisualElement bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.Opacity = 0;
            await Task.Delay((int)Delay);
            await bindable.FadeTo(1, Duration);
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            base.OnDetachingFrom(bindable);
        }
    }
}
