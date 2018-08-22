using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using System.Linq;

namespace SeeFoodProject.ViewModel
{
    public class MenuListingViewModel : ViewModelBase
    {
        public ObservableCollection<Model.MenuItem> MenuItems { get; set; }

        private readonly GalaSoft.MvvmLight.Views.INavigationService _navigationService;
        private string bingSearchSubscriptionKey = "API_KEY_BING_SEARCH";

        public MenuListingViewModel(GalaSoft.MvvmLight.Views.INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            var vmlocator = (ViewModelLocator) App.Current.Resources["Locator"];
            MenuItems = vmlocator.MainVM.MenuItems;

            //design elements for XAML view
            if (IsInDesignMode)
            {
                MenuItems = new ObservableCollection<Model.MenuItem>
                {
                    new Model.MenuItem(){Image="https://bluewater.co.uk/sites/bluewater/files/styles/image_spotlight_large/public/images/spotlights/burger-cropped.jpg" , Title = "Item 1", Content=""},
                    new Model.MenuItem(){Image="https://image.shutterstock.com/display_pic_with_logo/136306/722718082/stock-photo-healthy-food-clean-eating-selection-fruit-vegetable-seeds-superfood-cereals-leaf-vegetable-on-722718082.jpg" , Title = "Item 2", Content=""},
                    new Model.MenuItem(){Image="https://dynaimage.cdn.cnn.com/cnn/q_auto,w_602,c_fill,g_auto,h_339,ar_16:9/http%3A%2F%2Fcdn.cnn.com%2Fcnnnext%2Fdam%2Fassets%2F170302153529-garlic-crab.jpg" , Title = "Item 3", Content=""},
                    new Model.MenuItem(){Image="https://www.google.com/imgres?imgurl=https%3A%2F%2Fmedia1.popsugar-assets.com%2Ffiles%2Fthumbor%2FD0OYajmdcatHUC1-b4Axbf-uNxo%2Ffit-in%2F2048xorig%2Ffilters%3Aformat_auto-!!-%3Astrip_icc-!!-%2F2017%2F02%2F08%2F859%2Fn%2F1922195%2Fa7a42800589b73af54eda9.99423697_edit_img_image_43136859_1486581354%2Fi%2FKFC-Fried-Chicken-Pizza.jpg&imgrefurl=https%3A%2F%2Fwww.popsugar.com%2Flatest%2FFried-Chicken&docid=YKKBm1HG99tGJM&tbnid=QmIaQIR7UdyMhM%3A&vet=10ahUKEwijq9fymdTcAhVGxxoKHchtCUgQMwiHAigQMBA..i&w=1440&h=1440&bih=823&biw=1034&q=food&ved=0ahUKEwijq9fymdTcAhVGxxoKHchtCUgQMwiHAigQMBA&iact=mrc&uact=8" , Title = "Item 4", Content=""},
                    new Model.MenuItem(){Image="https://cdn.cnn.com/cnnnext/dam/assets/171027052520-processed-foods-exlarge-tease.jpg" , Title = "Item 5", Content=""},
                };
            }
        }

        //Variable to activate when loading to show loading screen.
        public Boolean IsLoading { get { return IsLoading; } set { isLoading = value; RaisePropertyChanged("IsLoading"); } }
        private Boolean isLoading = false;

        //retrieve an image for each elements on the menu using the SDK for Bing Search
        private async Task SearchMenuBingImageAsync()
        {
            IsLoading = true;

            var clientImageSearch = new ImageSearchAPI(new ApiKeyServiceClientCredentials(bingSearchSubscriptionKey));
            foreach (var menuItem in MenuItems)
            { 
                var imageResults = await clientImageSearch.Images.SearchAsync(query: menuItem.Title);
                if (imageResults.Value.Count > 0)
                {
                    var firstImageResult = imageResults.Value.First();
                    menuItem.Image = firstImageResult.ThumbnailUrl;
                    menuItem.Content = firstImageResult.ContentUrl;
                }
            }

            IsLoading = false;
        }

        /*
        private void BingImageSearch(string searchQuery)
        {
            // Construct the URI of the search request
            var uriQuery = uriBase + "?q=" + Uri.EscapeDataString(searchQuery);

            // Perform the Web request and get the response
            WebRequest request = HttpWebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = accessKey;
            HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Create result object for return
            var searchResult = new SearchResult()
            {
                jsonResult = json,
                relevantHeaders = new Dictionary<String, String>()
            };

            // Extract Bing HTTP headers
            foreach (String header in response.Headers)
            {
                if (header.StartsWith("BingAPIs-") || header.StartsWith("X-MSEdge-"))
                    searchResult.relevantHeaders[header] = response.Headers[header];
            }

            //Need to put data in MenuItem
        } */
        /*
        public async Task<string> GetImageInsights(byte[] image)
        {
            var uri = "https://api.cognitive.microsoft.com/bing/v5.0/images/search?modulesRequested=similarimages";
            var response = await RequestHelper.MakePostRequest(uri, new string(Encoding.UTF8.GetChars(image)), key, "multipart/form-data");
            var respString = await response.Content.ReadAsStringAsync();
            return respString;
        }*/

        /*
        public static async Task<HttpResponseMessage> MakePostRequest(string uri, string body, string key, string contentType)
        {
            var client = new HttpClient();

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(body);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                return await client.PostAsync(uri, content);
            }
        }*/




    }
    
    
    
}
