using Plugin.Permissions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.Media;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Views;

namespace SeeFoodProject.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        //string ComputerVisionApiURL = @"https://northeurope.api.cognitive.microsoft.com/vision/v2.0/recognizeText?mode=Printed";
        string SubscriptionKey = "API_KEY_ViSION";
        
        
        public MainViewModel()
        {
            TakePictureCommand = new RelayCommand(TakePictureAsync);
        }


        public ObservableCollection<Model.MenuItem> MenuItems { get; set; }
        public ICommand NavigateCommand { get; set; }
        public Boolean IsLoading { get { return isLoading; } set { isLoading = value; RaisePropertyChanged("IsLoading"); } }
        private Boolean isLoading = false;
        #region Commands
        public ICommand TakePictureCommand { get; private set; }
        #endregion

        private async void TakePictureAsync()
        {
            
            await RequestStorageWritePermission();
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                // Supply media options for saving our photo after it's taken.
                var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Pictures",
                    Name = $"{DateTime.UtcNow}.jpg"
                };

                // Take a photo of the business receipt.
                var file = await CrossMedia.Current.PickPhotoAsync();// mediaOptions);
                await MakeRequest(file);
            }
        }

        private async Task RequestStorageWritePermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Storage))
                    {
                        await Application.Current.MainPage.DisplayAlert("Storage Permission", "OK?", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    status = results[Permission.Storage];
                }

                if (status == PermissionStatus.Granted)
                {
                    //await Application.Current.MainPage.DisplayAlert("Storage Permission", "Granted!", "OK");
                }
                else if (status != PermissionStatus.Unknown)
                {
                    //await Application.Current.MainPage.DisplayAlert("Storage Permission", "Denied!", "OK");
                }
            }
            catch (Exception)
            {}
        }
        private async void RequestCameraPermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Camera))
                    {
                        await Application.Current.MainPage.DisplayAlert("Camera Permission", "OK?", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    status = results[Permission.Camera];
                }

                if (status == PermissionStatus.Granted)
                {
                    await Application.Current.MainPage.DisplayAlert("Camera Permission", "Granted!", "OK");
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await Application.Current.MainPage.DisplayAlert("Camera Permission", "Denied!", "OK");
                }
            }
            catch (Exception)
            {}
        }

        private async Task MakeRequest(Plugin.Media.Abstractions.MediaFile myFile)
        {
            IsLoading = true;
            ApiKeyServiceClientCredentials mykey = new ApiKeyServiceClientCredentials(SubscriptionKey);
            var visionApiClient = new ComputerVisionClient(mykey);
            //visionApiClient.AzureRegion = AzureRegions.Northeurope;
            var recognizedResult = await visionApiClient.RecognizePrintedTextAsync(false,"https://b.zmtcdn.com/data/menus/804/17883804/ccb6c1fbcc945d2824e9f5508cda9098.jpg");
            //var recognizedResult = await visionApiClient.RecognizePrintedTextInStreamAsync(false, myFile.GetStream);


            List<Model.MenuItem> menuItems = new List<Model.MenuItem>();
            foreach (var region in recognizedResult.Regions)
            {
                foreach (var line in region.Lines)
                {
                    string item1 = "";
                    foreach (var word in line.Words)
                        item1 += word;
                    menuItems.Add(new Model.MenuItem() { Title = item1 });
                }
            }
            IsLoading = false;
            await App.NavigationService.PushAsync(new MenuListingPage());
            
            /*var client = new HttpClient();
            var queryString =  HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);

            // Request parameters
            string queryparam = "mode=printed";
            var uri = "https://northeurope.api.cognitive.microsoft.com/vision/v1.0/OCR?" + queryparam;
            
            HttpResponseMessage response;*/
            /*   //to use for sending files
            using (var memoryStream = new System.IO.MemoryStream())
            {
                myFile.GetStream().CopyTo(memoryStream);
                myFile.Dispose();
                using (var content = new ByteArrayContent(memoryStream.ToArray()))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(ComputerVisionApiURL, content);
                }
            }
            *//*
            string urlbody = "{'url':'https://i.pinimg.com/originals/bc/f4/c2/bcf4c20c441c9b67ac43ff399992adf7.jpg'}";
            using (var content = new StringContent(urlbody))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(ComputerVisionApiURL, content);
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var test = new List<string>(response.Headers.GetValues("Operation-Location"));
                    var test2 = test[0];
                    response = await client.PostAsync(test2, new StringContent(""));
                    string contentString = await response.Content.ReadAsStringAsync();
                }
                else
                { string contentString = await response.Content.ReadAsStringAsync(); }
            }*/

        }

    }
}
