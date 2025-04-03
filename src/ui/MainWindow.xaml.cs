using System.Windows;
using System.Windows.Media.Animation;

namespace PhoenixExecutor.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Анимация при запуске
            this.Loaded += (s, e) => 
            {
                AnimateWindowEntrance();
            };
            
            // Анимация при наведении на боковую панель
            SidePanel.MouseEnter += (s, e) => 
            {
                AnimatePanelHover(true);
            };
            
            SidePanel.MouseLeave += (s, e) => 
            {
                AnimatePanelHover(false);
            };
        }

        private void AnimateWindowEntrance()
        {
            var opacityAnim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            var marginAnim = new ThicknessAnimation(
                new Thickness(0, 20, 0, 0), 
                new Thickness(0), 
                TimeSpan.FromSeconds(0.7))
            {
                EasingFunction = new ElasticEase { EasingMode = EasingMode.EaseOut }
            };

            MainGrid.BeginAnimation(OpacityProperty, opacityAnim);
            MainGrid.BeginAnimation(MarginProperty, marginAnim);
        }

        private void AnimatePanelHover(bool isHovering)
        {
            var scaleAnim = new DoubleAnimation(
                isHovering ? 1.03 : 1.0, 
                TimeSpan.FromSeconds(0.3))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            SidePanel.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            SidePanel.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            // Анимация нажатия кнопки
            var clickAnim = new DoubleAnimation(0.9, 1.0, TimeSpan.FromSeconds(0.3))
            {
                EasingFunction = new BounceEase { EasingMode = EasingMode.EaseOut }
            };
            
            ExecuteButton.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, clickAnim);
            ExecuteButton.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, clickAnim);
            
            // Выполнение скрипта
            Core.LuaEngine.Execute(CodeEditor.Text);
        }
    }
}