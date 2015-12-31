using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUiLibrary.UserControls.StaticDisplay
{
    /// <summary>
    /// Interaction logic for MaskedUiElement.xaml
    /// </summary>
    public partial class MaskedUiElement : UserControl
    {
        public MaskedUiElement()
        {
            InitializeComponent();
        }

        public FrameworkElement Element
        {
            get { return (FrameworkElement) GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }

        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(MaskedUiElement), new PropertyMetadata
            {
                PropertyChangedCallback = (o, args) =>
                {
                    var mui = o as MaskedUiElement;
                    if (mui != null) mui.CoreElement.Content = args.NewValue;
                }
            });

        public FrameworkElement Mask
        {
            get { return (FrameworkElement)GetValue(MaskProperty); }
            set { SetValue(MaskProperty, value); }
        }
        public static readonly DependencyProperty MaskProperty =
            DependencyProperty.Register("Mask", typeof(FrameworkElement), typeof(MaskedUiElement), new PropertyMetadata
            {
                PropertyChangedCallback = (o, args) =>
                {
                    var mui = o as MaskedUiElement;
                    if (mui != null) mui.CoreMask.Content = args.NewValue;
                }
            });
    }
}
