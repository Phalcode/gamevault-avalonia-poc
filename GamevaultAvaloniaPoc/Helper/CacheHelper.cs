using GamevaultAvaloniaPoc.Models;
using GamevaultAvaloniaPoc.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GamevaultAvaloniaPoc.Helper
{
    internal class CacheHelper
    {
        internal static async Task<Avalonia.Media.Imaging.Bitmap> HandleImageCacheAsync(int identifier, int imageId, string cachePath, ImageCache cacheType)
        {
            string cacheFile = $"{cachePath}/{identifier}.{imageId}";
            try
            {
                if (imageId == -1)
                {
                    throw new Exception("image id does not exist");
                }
                if (File.Exists(cacheFile))
                {
                    //if file exists then return it directly
                    return new Avalonia.Media.Imaging.Bitmap(cacheFile);
                }
                else
                {
                    if (!Directory.Exists(cachePath))
                    { Directory.CreateDirectory(cachePath); }
                    //Otherwise see if there are still cache images with the same identifier
                    string[] files = Directory.GetFiles(cachePath, $"{identifier}.*");
                    if (LoginManager.Instance.IsLoggedIn())
                    {
                        //when we are online we download the new image and delete an outdated one if available
                        if (files.Length > 0)
                        {
                            File.Delete(files[0]);
                        }
                        await TaskQueue.Instance.Enqueue(() => WebHelper.DownloadImageFromUrlAsync($"{SettingsViewModel.Instance.ServerUrl}api/v1/images/{imageId}", cacheFile), imageId);
                        return new Avalonia.Media.Imaging.Bitmap(cacheFile);
                    }
                    else
                    {
                        if (files.Length > 0)
                        {
                            //if we are offline, we will try to load an old image with the same identifier
                            cacheFile = files[0];
                            return new Avalonia.Media.Imaging.Bitmap(cacheFile);
                        }
                        else
                        {
                            //otherwise we load the 'No Boxart' image
                            throw new Exception();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //try
                //{
                //    if (TaskQueue.Instance.IsAlreadyInProcess(imageId))
                //    {
                //        await TaskQueue.Instance.WaitForProcessToFinish(imageId);
                //        return new Avalonia.Media.Imaging.Bitmap(cacheFile);
                //    }
                //}
                //catch { }
                //switch (cacheType)
                //{
                //    case ImageCache.BoxArt:
                //        {
                //            return BitmapHelper.GetBitmapImage("pack://application:,,,/gamevault;component/Resources/Images/library_NoBoxart.png");
                //        }
                //    case ImageCache.UserIcon:
                //        {
                //            return BitmapHelper.GetBitmapImage("pack://application:,,,/gamevault;component/Resources/Images/com_NoUserIcon.png");
                //        }
                //    default:
                //        {
                //            return BitmapHelper.GetBitmapImage("pack://application:,,,/gamevault;component/Resources/Images/gameView_NoBackground.png");
                //        }
                //}
                return null;
            }
        }
    }
}
