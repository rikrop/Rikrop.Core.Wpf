using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Rikrop.Core.Framework;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcFaviconViewer : Control
    {
        private const string FaviconImageSource = "http://www.google.com/s2/favicons?domain={0}";

        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof (string), typeof (RrcFaviconViewer),
                                        new PropertyMetadata(null, OnUrlChangedCallback));

        public static readonly DependencyProperty NoUrlImageTooltipProperty =
            DependencyProperty.Register("NoUrlImageTooltip", typeof (string), typeof (RrcFaviconViewer),
                                        new PropertyMetadata(null));

        private static readonly Dictionary<string, string> Exclucions;

        private Image _image;
        private FrameworkElement _errorElement;
      

        public string Url
        {
            get { return (string) GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        public string NoUrlImageTooltip
        {
            get { return (string) GetValue(NoUrlImageTooltipProperty); }
            set { SetValue(NoUrlImageTooltipProperty, value); }
        }

        static RrcFaviconViewer()
        {
            Exclucions = new Dictionary<string, string>
                             {
                                 {"blogspot.com", "http://www.blogger.com/favicon.ico"},
                                 {"2krota.ru", "http://2krota.ru/templates/2krota/img/favicon.ico"},
                             };

            DefaultStyleKeyProperty.OverrideMetadata(typeof (RrcFaviconViewer),
                                                     new FrameworkPropertyMetadata(typeof (RrcFaviconViewer)));
        }

        private static void OnUrlChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs args)
        {
            var d = dobj as RrcFaviconViewer;
            if (d != null)
            {
                d.UpdateVisual();
            }
        }

        private static string GetImageSource(string siteHost)
        {
            string exSource;
            return string.IsNullOrWhiteSpace(siteHost)
                       ? null
                       : Exclucions.TryGetValue(siteHost, out exSource)
                             ? exSource
                             : string.Format(FaviconImageSource, siteHost);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _image = (Image) Template.FindName("PART_Image", this);

            _errorElement = (FrameworkElement) Template.FindName("PART_ErrorElement", this);
            var tb = new Binding(ExpressionHelper.GetName(o => NoUrlImageTooltip))
                         {
                             RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof (RrcFaviconViewer), 1),
                         };
            _errorElement.SetBinding(ToolTipProperty, tb);

            UpdateVisual();
        }

        private async void UpdateVisual()
        {
            var url = Url;
            var host = GetHost(url);
            var source = GetImageSource(host);
            if (!string.IsNullOrWhiteSpace(source))
            {
                if (_image != null)
                {
                    try
                    {                        
                        var data = await Task.Run(() => new WebClient().DownloadData(new Uri(source)));
                        var bitmRrcmage = new BitmapImage();
                        bitmRrcmage.BeginInit();
                        bitmRrcmage.StreamSource = new MemoryStream(data);
                        bitmRrcmage.EndInit();

                        _image.Source = bitmRrcmage;

                        _image.Visibility = Visibility.Visible;
                        _image.ToolTip = host;                        
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }

                if (_errorElement != null)
                {
                    _errorElement.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (_image != null)
                {
                    _image.Source = null;
                    _image.Visibility = Visibility.Collapsed;
                    _image.ToolTip = null;
                }

                if (_errorElement != null)
                {
                    _errorElement.Visibility = Visibility.Visible;
                }
            }
        }


        private string GetHost(string url)
        {
            Uri uri;
            return Uri.TryCreate(url, UriKind.Absolute, out uri)
                       ? uri.Host
                       : null;
        }
    }
}