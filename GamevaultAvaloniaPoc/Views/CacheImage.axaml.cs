using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using GamevaultAvaloniaPoc.Helper;
using GamevaultAvaloniaPoc.Models;
using System.Threading.Tasks;
using System;
using Avalonia.Interactivity;
using Avalonia.Data.Converters;

namespace GamevaultAvaloniaPoc.Views
{
    public partial class CacheImage : UserControl
    {
        //Avalonia.Media.Imaging.Bitmap bmp = new Avalonia.Media.Imaging.Bitmap("");
        #region Dependency Property
        //public static readonly DependencyProperty ImageCacheTypeProperty = DependencyProperty.Register("ImageCacheType", typeof(ImageCache), typeof(CacheImage));
        public static readonly StyledProperty<ImageCache> ImageCacheTypeProperty = AvaloniaProperty.Register<CacheImage, ImageCache>(nameof(ImageCacheType));
        public ImageCache ImageCacheType
        {
            get { return GetValue(ImageCacheTypeProperty); }
            set { SetValue(ImageCacheTypeProperty, value); }
        }
        //public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(CacheImage), new PropertyMetadata(OnDataChangedCallBack));
        public static readonly StyledProperty<object> DataProperty = AvaloniaProperty.Register<CacheImage, object>(nameof(Data), null, false, Avalonia.Data.BindingMode.OneWay, null, Data_Changed);

        private static async Task Data_Changed(AvaloniaObject @object, object arg2)
        {
            if (arg2 != null)
                await ((CacheImage)@object).DataChanged(arg2);
        }

        //Add Data as DependencyProperty, because previous DataContext_Changed is called before ImageCacheType is set, when inside XAML DataTemplate. So there was a bug, where i could not choose ImageCacheType. 
        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        //private static async void OnDataChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    await ((CacheImage)sender).DataChanged(e.NewValue);
        //}

        //public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(CacheImage), new PropertyMetadata(OnStretchChangedCallBack));

        //public Stretch Stretch
        //{
        //    get { return (Stretch)GetValue(StretchProperty); }
        //    set { SetValue(StretchProperty, value); }
        //}
        //private static void OnStretchChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if ((Stretch)e.NewValue != Stretch.None)
        //    {
        //        ((CacheImage)sender).uiImg.Stretch = (Stretch)e.NewValue;
        //    }
        //}
        //public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CacheImage), new PropertyMetadata(OnCornerRadiusChangedCallBack));

        //public CornerRadius CornerRadius
        //{
        //    get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        //    set { SetValue(CornerRadiusProperty, value); }
        //}
        //private static void OnCornerRadiusChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    ((CacheImage)sender).uiBorder.CornerRadius = (CornerRadius)e.NewValue;
        //}
        #endregion

        public CacheImage()
        {
            InitializeComponent();
        }

        private async Task DataChanged(object newData)
        {
            int identifier = -1;
            int imageId = -1;
            string cachePath = AppDomain.CurrentDomain.BaseDirectory + "/cache";
            object data = newData;
            if (data == null)
                return;

            try
            {
                if (typeof(Progress) == data.GetType())
                {
                    data = ((Progress)data).Game;
                }
                switch (ImageCacheType)
                {
                    case ImageCache.BoxArt:
                        {
                            cachePath += "/gbox";
                            var game = ((Game)data);
                            identifier = (game == null ? -1 : game.ID);
                            imageId = ((game == null || game.BoxImage == null) ? -1 : game.BoxImage.ID);
                            break;
                        }
                    case ImageCache.GameBackground:
                        {
                            cachePath += "/gbg";
                            var game = ((Game)data);
                            identifier = (game == null ? -1 : game.ID);
                            imageId = ((game == null || game.BackgroundImage == null) ? -1 : game.BackgroundImage.ID);
                            break;
                        }
                    case ImageCache.UserIcon:
                        {
                            cachePath += "/uico";
                            var user = ((User)data);
                            identifier = (user == null ? -1 : user.ID);
                            imageId = ((user == null || user.ProfilePicture == null) ? -1 : user.ProfilePicture.ID);
                            break;
                        }
                    case ImageCache.UserBackground:
                        {
                            cachePath += "/ubg";
                            var user = ((User)data);
                            identifier = (user == null ? -1 : user.ID);
                            imageId = ((user == null || user.BackgroundImage == null) ? -1 : user.BackgroundImage.ID);
                            break;
                        }
                }
            }
            catch (Exception ex) { }
            uiImg.Source = await CacheHelper.HandleImageCacheAsync(identifier, imageId, cachePath, ImageCacheType);
        }
    }
}
