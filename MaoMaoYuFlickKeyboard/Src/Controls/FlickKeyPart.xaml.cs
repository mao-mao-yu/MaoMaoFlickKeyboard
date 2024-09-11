using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace MaoMaoYuFlickKeyboard.Src.Controls
{
    public partial class FlickKeyPart : UserControl
    {
        #region Fields
        private bool _isFlickModeActive;
        private bool _isFlickCanvasSlowShowed;
        private bool _isFastFlickKeyShowed = false;
        private BindingExpression _originalBindingExpression;
        private static readonly Brush _buttonHighlightBackgroundColor = Brushes.LightBlue;

        private ButtonInfo[] _buttons;
        private System.Timers.Timer _flickTimer;
        #endregion

        #region Constructor

        public FlickKeyPart()
        {
            InitializeComponent();
            InitializeButtons();
            InitializeTimer();

            FontSize = 30;
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the button information for flick handling.
        /// </summary>
        private void InitializeButtons()
        {
            _buttons =
            [
                new ButtonInfo(TopButton, TopButtonInputText),
                new ButtonInfo(BottomButton, BottomButtonInputText),
                new ButtonInfo(LeftButton, LeftButtonInputText),
                new ButtonInfo(RightButton, RightButtonInputText),
                new ButtonInfo(FlickButton, CenterButtonInputText),
            ];
        }

        /// <summary>
        /// Initializes the flick timer.
        /// </summary>
        private void InitializeTimer()
        {
            _flickTimer = new System.Timers.Timer
            {
                Interval = 350,
                AutoReset = false
            };
            _flickTimer.Elapsed += OnFlickTimerElapsed;
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty CenterButtonUIContentProperty =
            DependencyProperty.Register(nameof(CenterButtonUIContent), typeof(object), typeof(FlickKeyPart));

        public static readonly DependencyProperty CenterButtonInputTextProperty =
            DependencyProperty.Register(nameof(CenterButtonInputText), typeof(object), typeof(FlickKeyPart), new PropertyMetadata(""));

        public static readonly DependencyProperty LeftButtonInputTextProperty =
            DependencyProperty.Register(nameof(LeftButtonInputText), typeof(object), typeof(FlickKeyPart), new PropertyMetadata(""));

        public static readonly DependencyProperty TopButtonInputTextProperty =
            DependencyProperty.Register(nameof(TopButtonInputText), typeof(object), typeof(FlickKeyPart), new PropertyMetadata(""));

        public static readonly DependencyProperty RightButtonInputTextProperty =
            DependencyProperty.Register(nameof(RightButtonInputText), typeof(object), typeof(FlickKeyPart), new PropertyMetadata(""));

        public static readonly DependencyProperty BottomButtonInputTextProperty =
            DependencyProperty.Register(nameof(BottomButtonInputText), typeof(object), typeof(FlickKeyPart), new PropertyMetadata(""));

        public static readonly DependencyProperty IsFlickKeyEnableProperty =
            DependencyProperty.Register(nameof(IsFlickKeyEnable), typeof(bool), typeof(FlickKeyPart), new PropertyMetadata(true));
        
        public static readonly DependencyProperty ButtonInputTextsProperty =
            DependencyProperty.Register(nameof(ButtonInputTexts), typeof(string), typeof(FlickKeyPart), new PropertyMetadata(string.Empty, OnButtonInputTextsChanged));
        
        public static readonly DependencyProperty FlickButtonFontSizeProperty =
            DependencyProperty.Register(nameof(FlickButtonFontSize), typeof(int), typeof(FlickKeyPart), new PropertyMetadata(30));

        #endregion

        #region Properties
        public string ButtonInputTexts
        {
            get => (string)GetValue(ButtonInputTextsProperty);
            set => SetValue(ButtonInputTextsProperty, value);
        }

        public int FlickButtonFontSize
        {
            get => (int)GetValue(FlickButtonFontSizeProperty);
            set => SetValue(FlickButtonFontSizeProperty, value);
        }

        public string CenterButtonInputText
        {
            get => (string)GetValue(CenterButtonInputTextProperty);
            set => SetValue(CenterButtonInputTextProperty, value);
        }

        public string LeftButtonInputText
        {
            get => (string)GetValue(LeftButtonInputTextProperty);
            set => SetValue(LeftButtonInputTextProperty, value);
        }

        public string TopButtonInputText
        {
            get => (string)GetValue(TopButtonInputTextProperty);
            set => SetValue(TopButtonInputTextProperty, value);
        }

        public string RightButtonInputText
        {
            get => (string)GetValue(RightButtonInputTextProperty);
            set => SetValue(RightButtonInputTextProperty, value);
        }

        public string BottomButtonInputText
        {
            get => (string)GetValue(BottomButtonInputTextProperty);
            set => SetValue(BottomButtonInputTextProperty, value);
        }

        public bool IsFlickKeyEnable
        {
            get => (bool)GetValue(IsFlickKeyEnableProperty);
            set => SetValue(IsFlickKeyEnableProperty, value);
        }

        public object CenterButtonUIContent
        {
            get => GetValue(CenterButtonUIContentProperty);
            set => SetValue(CenterButtonUIContentProperty, value);
        }

        #endregion

        #region Events

        public event EventHandler<string> OnCharacterSelected;

        #endregion

        #region Event Handlers
        private static void OnButtonInputTextsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlickKeyPart flickKeyPart && e.NewValue is string inputTexts)
            {
                flickKeyPart.SetButtonTexts(inputTexts);
            }
        }

        private void SetButtonTexts(string inputTexts)
        {
            if (inputTexts.Length == 5)
            {
                CenterButtonInputText = inputTexts[0].ToString();
                LeftButtonInputText = inputTexts[1].ToString();
                TopButtonInputText = inputTexts[2].ToString();
                RightButtonInputText = inputTexts[3].ToString();
                BottomButtonInputText = inputTexts[4].ToString();
                LeftButton.Visibility = string.IsNullOrWhiteSpace(inputTexts[1].ToString()) ? Visibility.Collapsed : Visibility.Visible;
                TopButton.Visibility = string.IsNullOrWhiteSpace(inputTexts[2].ToString()) ? Visibility.Collapsed : Visibility.Visible;
                RightButton.Visibility = string.IsNullOrWhiteSpace(inputTexts[3].ToString()) ? Visibility.Collapsed : Visibility.Visible;
                BottomButton.Visibility = string.IsNullOrWhiteSpace(inputTexts[4].ToString()) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void FlickButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HandleButtonDown();
        }

        private void FlickButton_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            HandleButtonDown();
        }

        private void FlickButton_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isFlickModeActive && e.LeftButton == MouseButtonState.Pressed)
            {
                HandleButtonMove(e.GetPosition(this));
            }
        }

        private void FlickButton_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            if (_isFlickModeActive)
            {
                HandleButtonMove(e.GetTouchPoint(this).Position);
            }
        }

        private void FlickButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HandleButtonUp();
        }

        private void FlickButton_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            HandleButtonUp();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the button down action to initiate flick mode.
        /// </summary>
        private void HandleButtonDown()
        {
            if (!IsFlickKeyEnable)
                return;

            _flickTimer.Stop();
            _flickTimer.Start();

            _isFlickModeActive = true;
        }

        /// <summary>
        /// Handles the button move action to detect flick direction.
        /// </summary>
        /// <param name="currentPoint">The current position of the pointer.</param>
        private void HandleButtonMove(Point currentPoint)
        {
            if (_isFlickCanvasSlowShowed)
            {
                SetFlickCanvasButtonBackground(currentPoint);
                return;
            }

            if (IsPointInButton(currentPoint, FlickButton))
            {
                ResetKeyPart();
                FlickButton.Opacity = 1;
                return;
            }

            ShowKeyPartByCurrentPoint(currentPoint);
        }

        private bool IsPointInButton(Point currentPoint, Button button)
        {
            var buttonPosition = button.TransformToAncestor(this)
                                              .Transform(new Point(0, 0));

            double buttonWidth = button.ActualWidth;
            double buttonHeight = button.ActualHeight;

            if (currentPoint.X >= buttonPosition.X && currentPoint.X <= buttonPosition.X + buttonWidth &&
                currentPoint.Y >= buttonPosition.Y && currentPoint.Y <= buttonPosition.Y + buttonHeight)
            {
                return true;
            }

            return false;
        }

        private void ShowKeyPartByCurrentPoint(Point currentPoint)
        {
            ResetKeyPart();
            _isFastFlickKeyShowed = true;
            FlickCanvasFast.Visibility = Visibility.Visible;
            FlickButton.Opacity = 0.2;

            var buttonLeftTop = FlickButton.TranslatePoint(new Point(0, 0), null);

            var buttonWidth = FlickButton.ActualWidth;
            var buttonHeight = FlickButton.ActualHeight;

            double leftMiddle = buttonLeftTop.X;
            double rightMiddle = buttonLeftTop.X + buttonWidth;
            double topMiddle = buttonLeftTop.Y;
            double bottomMiddle = buttonLeftTop.Y + buttonHeight;

            var globalCurrentPoint = TranslatePoint(currentPoint, null);

            if (globalCurrentPoint.X > rightMiddle)
            {
                ShowRightKeyPart();
            }
            else if (globalCurrentPoint.X < leftMiddle)
            {
                ShowLeftKeyPart();
            }
            else if (globalCurrentPoint.Y > bottomMiddle)
            {
                ShowTopKeyPart();
            }
            else if (globalCurrentPoint.Y < topMiddle)
            {
                ShowBottomKeyPart();
            }
        }

        private void ShowRightKeyPart()
        {
            RightButtonFast.Visibility = Visibility.Visible;
        }

        private void ShowBottomKeyPart()
        {
            BottomButtonFast.Visibility = Visibility.Visible;
        }

        private void ShowLeftKeyPart()
        {
            LeftButtonFast.Visibility = Visibility.Visible;
        }

        private void ShowTopKeyPart()
        {
            TopButtonFast.Visibility = Visibility.Visible;
        }

        private void ResetKeyPart()
        {
            TopButtonFast.Visibility = Visibility.Hidden;
            LeftButtonFast.Visibility = Visibility.Hidden;
            RightButtonFast.Visibility = Visibility.Hidden;
            BottomButtonFast.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPoint"></param>
        private void SetFlickCanvasButtonBackground(Point point)
        {
            ResetButtonBackgrounds();
            foreach (var buttonInfo in _buttons)
            {
                if (IsPointInButton(point, buttonInfo.Button))
                {
                    HighlightButton(buttonInfo.Button);
                    return;
                }
            }
        }

        /// <summary>
        /// Handles the button up action to finalize the character selection.
        /// </summary>
        private void HandleButtonUp()
        {
            _flickTimer.Stop();

            if (_isFlickModeActive)
            {
                var selectedCharacter = GetSelectedCharacter();
                ResetButtonBackgrounds();
                RestoreOriginalButtonState();
                HideFlickButtons();
                _isFlickModeActive = false;
                _isFlickCanvasSlowShowed = false;
                _isFastFlickKeyShowed = false;
                FlickButton.ClearValue(BackgroundProperty);
                OnCharacterSelected?.Invoke(this, selectedCharacter);
            }
        }

        /// <summary>
        /// Handles the tick event of the flick timer.
        /// </summary>
        private void OnFlickTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _flickTimer.Stop();
            if (_isFastFlickKeyShowed)
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_isFlickModeActive)
                {
                    ShowFlickButtons();
                }
            });
        }

        /// <summary>
        /// Highlights the specified button by changing its background color.
        /// </summary>
        /// <param name="button">The button to highlight.</param>
        private void HighlightButton(Button button)
        {
            if (button.Background != _buttonHighlightBackgroundColor)
            {
                button.Background = _buttonHighlightBackgroundColor;
            }
        }

        /// <summary>
        /// Retrieves the currently selected character based on the highlighted button.
        /// </summary>
        /// <returns>The selected character.</returns>
        private string GetSelectedCharacter()
        {
            foreach (var buttonInfo in _buttons)
            {
                if (buttonInfo.Button.Background == _buttonHighlightBackgroundColor)
                {
                    return buttonInfo.InputText;
                }
            }
            return CenterButtonInputText;
        }

        /// <summary>
        /// Saves the original state of the FlickButton.
        /// </summary>
        private void SaveOriginalButtonState()
        {
            _originalBindingExpression = FlickButton.GetBindingExpression(ContentProperty);
        }

        /// <summary>
        /// Restores the original state of the FlickButton.
        /// </summary>
        private void RestoreOriginalButtonState()
        {
            if (_originalBindingExpression != null)
            {
                FlickButton.SetBinding(ContentProperty, _originalBindingExpression.ParentBinding);
            }
        }

        /// <summary>
        /// Shows the flick option buttons.
        /// </summary>
        internal void ShowFlickButtons()
        {
            SaveOriginalButtonState();
            FlickButton.Content = CenterButtonInputText;
            FlickCanvasSlow.Visibility = Visibility.Visible;
            _isFlickCanvasSlowShowed = true;
            HighlightButton(FlickButton);
        }

        /// <summary>
        /// Hides the flick option buttons.
        /// </summary>
        private void HideFlickButtons()
        {
            FlickCanvasSlow.Visibility = Visibility.Hidden;
            FlickCanvasFast.Visibility = Visibility.Hidden;

            LeftButtonFast.Visibility = Visibility.Hidden;
            TopButtonFast.Visibility = Visibility.Hidden;
            RightButtonFast.Visibility = Visibility.Hidden;
            BottomButtonFast.Visibility = Visibility.Hidden;

            FlickButton.Opacity = 1;
        }

        /// <summary>
        /// Resets the background color of all buttons.
        /// </summary>
        private void ResetButtonBackgrounds()
        {
            foreach (var buttonInfo in _buttons)
            {
                buttonInfo.Button.Background = Brushes.White;
            }
        }

        #endregion

        #region Helper Classes

        /// <summary>
        /// Helper class to store button and its associated input text.
        /// </summary>
        private class ButtonInfo(Button button, string inputText)
        {
            public Button Button { get; } = button;
            public string InputText { get; } = inputText;
        }

        #endregion
    }
}
