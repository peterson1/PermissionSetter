﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace CommonTools.Lib45.TextLabels
{
    public partial class DualText : UserControl
    {
        public DualText()
        {
            InitializeComponent();

            Text1Wrapping = TextWrapping.Wrap;
            Text2Wrapping = TextWrapping.Wrap;

            Text1Ellipsis = Visibility.Collapsed;
            Text2Ellipsis = Visibility.Collapsed;

            VerticalAlign = VerticalAlignment.Top;

            txt1.HandleClick();
            txt2.HandleClick();

            txt1.Bind(nameof(Text1), TextBlock.TextProperty);
            txt2.Bind(nameof(Text2), TextBlock.TextProperty);

            txt1.Bind(nameof(Text1Wrapping), TextBlock.TextWrappingProperty);
            txt2.Bind(nameof(Text2Wrapping), TextBlock.TextWrappingProperty);

            txt1.Bind(nameof(Text1Brush), TextBlock.ForegroundProperty);
            txt2.Bind(nameof(Text2Brush), TextBlock.ForegroundProperty);
            elp2.Bind(nameof(Text2Brush), TextBlock.ForegroundProperty);

            txt1.Bind(nameof(Text1FontStyle), TextBlock.FontStyleProperty);
            txt2.Bind(nameof(Text2FontStyle), TextBlock.FontStyleProperty);
            elp2.Bind(nameof(Text2FontStyle), TextBlock.FontStyleProperty);

            elp2.Bind(nameof(Text2Ellipsis), TextBlock.VisibilityProperty);

            txt1.Bind(nameof(VerticalAlign), FrameworkElement.VerticalAlignmentProperty);
            txt2.Bind(nameof(VerticalAlign), FrameworkElement.VerticalAlignmentProperty);
            elp2.Bind(nameof(VerticalAlign), FrameworkElement.VerticalAlignmentProperty);
            
            Loaded += (s, e) =>
            {
                colDef1.Width = Text1Width ?? new GridLength(70);
                colDefGap.Width = GapWidth ?? new GridLength(8);
                colDef2.Width = Text2Width ?? new GridLength();

                if (Text1Brush == null) Text1Brush = Brushes.Gray;
                if (Text2Brush == null) Text2Brush = Brushes.Black;

                txt1.FontWeight = Text1Weight ?? FontWeights.Medium;
                txt2.FontWeight = Text2Weight ?? FontWeights.Normal;
                elp2.FontWeight = Text2Weight ?? FontWeights.Normal;

                txt1.FontSize = Text1Size ?? 12;
                txt2.FontSize = Text2Size ?? 12;
                elp2.FontSize = Text2Size ?? 12;

                txt1.TextAlignment = Text1Alignment ?? TextAlignment.Right;
                txt2.TextAlignment = Text2Alignment ?? TextAlignment.Left;
            };
        }
    }


    internal static class TextBlockExtensions
    {
        internal static void Bind(this FrameworkElement elm, string path, DependencyProperty dependencyProp)
        {
            var binding = new Binding(path);
            binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DualText), 1);
            elm.SetBinding(dependencyProp, binding);
        }

        internal static void HandleClick(this TextBlock txt)
        {
            txt.MouseRightButtonDown += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftShift))
                    Clipboard.SetText(txt.Text);
            };
        }
    }
}
