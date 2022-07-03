using Sylph.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Sylph.Web.Models;
using Sylph.Data;
using Sylph.Data.Models;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Support.UI;

namespace Sylph.Web.Services
{
    public class SearchService : ISearchService
    {
        private readonly Repository<Site> _siteRepository;
        public SearchService(Repository<Site> siteRepository) 
        {
            _siteRepository = siteRepository;
        }

        public List<string> Browse(string searchedItem)
        {
            var result = new List<string>();
            WebDriver driver = new ChromeDriver();
            driver.Url = "https://www.google.com/search?q=" + "купити "+searchedItem.Replace(' ', '+') + "&num=" + "купити "+searchedItem;
            var pages = driver.FindElements(By.ClassName("yuRUbf"));
            foreach (var page in pages)
            {
                if (!String.IsNullOrWhiteSpace(page.Text))
                {
                    var url = page.FindElement(By.TagName("a")).GetAttribute("href");
                    if (url.Contains(".ua") && !url.Contains(".ru")) result.Add(url);
                }
            }
            driver.Close();
            return result;
        }

        public string CutString(string stringToCut) 
        {
            var result = stringToCut.Remove(0, 8);
            var indexToCut = result.IndexOf(".ua");
            result = result.Remove(indexToCut, result.Length-1 - (indexToCut-1));
            return result;
        }

        public List<Item> Search(string searchedItem)
        {
            var result = new List<Item>();
            List<string> urlsToSearch = Browse(searchedItem);
            var options = new ChromeOptions(); options.AddArgument("--no-sandbox");
            WebDriver driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));
            var scroller = (IJavaScriptExecutor)driver;
            var keyWords = searchedItem.Split(' ');
            var sitesNames = new List<string>();
            foreach (var url in urlsToSearch) sitesNames.Add(CutString(url));
            var siteFrames =  _siteRepository.GetAll().Where(x => sitesNames.Contains(x.Url)).ToList();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            foreach (var siteUrl in urlsToSearch)
            {
                var siteName = CutString(siteUrl);
                var currentFrames = siteFrames.FindAll(x => x.Url == siteName);
                if (currentFrames.Count == 0) continue;
                try
                {
                    driver.Url = siteUrl;
                }
                catch (WebDriverTimeoutException) 
                {
                    continue;
                }
                foreach (var frame in currentFrames)
                {
                    int itemIndex = 0;
                    var items = driver.FindElements(By.XPath(frame.ContentPathOption));
                    foreach (var item in items)
                    {
                        itemIndex++;
                        var name = "";
                        var price = "";
                        var imageSrc = "";

                        //Checking whether item is available
                        try
                        {
                            var xpath = "";
                            if (frame.Option == 1) xpath = frame.ContentPathOption + $"[{itemIndex}]";
                            else xpath = frame.ContentPathOption;

                            var itemContainer = driver.FindElement(By.XPath(xpath));
                            if (itemContainer.Text.Contains(frame.NotAvailableFilter)) continue;
                        }
                        catch (StaleElementReferenceException) 
                        {
                            var xpath = "";
                            if (frame.Option == 1) xpath = frame.ContentPathOption + $"[{itemIndex}]";
                            else xpath = frame.ContentPathOption;

                            var itemContainer = driver.FindElement(By.XPath(xpath));
                            if (itemContainer.Text.Contains(frame.NotAvailableFilter)) continue;
                        }
                        //Getting Name
                        var nameText = "";
                        try
                        {
                            var xpath = "";
                            if (frame.Option == 1)xpath = frame.NamePathOption + $"[{itemIndex}]";
                            else xpath = frame.NamePathOption;
                            
                            var nameObject = driver.FindElement(By.XPath(xpath));
                            nameText = nameObject.Text;
                        }
                        catch (StaleElementReferenceException) 
                        {
                            var xpath = "";
                            if (frame.Option == 1) xpath = frame.NamePathOption + $"[{itemIndex}]";
                            else xpath = frame.NamePathOption;

                            var nameObject = driver.FindElement(By.XPath(xpath));
                            nameText = nameObject.Text;
                        }
                        if (nameText.Contains(searchedItem, StringComparison.OrdinalIgnoreCase)) name = nameText;
                        else if (keyWords.All(x => nameText.Contains(x, StringComparison.OrdinalIgnoreCase))) name = nameText;

                        //Getting ImageSrc
                        
                        try
                        {
                            try
                            {
                                scroller.ExecuteScript("window.scrollBy(0,500)", "");
                                var xpath = "";
                                if (frame.Option == 1) xpath = frame.ImagePathOption + $"[{itemIndex}]";
                                else xpath = frame.ImagePathOption;

                                var image = driver.FindElement(By.XPath(xpath));
                                imageSrc = image.GetAttribute("src");
                                if(string.IsNullOrEmpty(imageSrc)) 
                                {
                                    imageSrc = image.FindElement(By.TagName("img")).GetAttribute("src");
                                }
                            }
                            catch (NoSuchElementException) 
                            {
                                scroller.ExecuteScript("window.scrollBy(0,500)", "");
                                var xpath = "";
                                if (frame.Option == 1) xpath = frame.ImagePathOption + $"[{itemIndex}]";
                                else xpath = frame.ImagePathOption;

                                var image = driver.FindElement(By.XPath(xpath));
                                imageSrc = image.GetAttribute("src");
                                if (string.IsNullOrEmpty(imageSrc))
                                {
                                    imageSrc = image.FindElement(By.TagName("img")).GetAttribute("src");
                                }
                            }
                        }
                        catch (StaleElementReferenceException) 
                        {
                            try
                            {
                                scroller.ExecuteScript("window.scrollBy(0,500)", "");
                                var xpath = "";
                                if (frame.Option == 1) xpath = frame.ImagePathOption + $"[{itemIndex}]";
                                else xpath = frame.ImagePathOption;

                                var image = driver.FindElement(By.XPath(xpath));
                                imageSrc = image.GetAttribute("src");
                                if (string.IsNullOrEmpty(imageSrc))
                                {
                                    imageSrc = image.FindElement(By.TagName("img")).GetAttribute("src");
                                }
                            }
                            catch (NoSuchElementException)
                            {
                                scroller.ExecuteScript("window.scrollBy(0,500)", "");
                                var xpath = "";
                                if (frame.Option == 1) xpath = frame.ImagePathOption + $"[{itemIndex}]";
                                else xpath = frame.ImagePathOption;

                                var image = driver.FindElement(By.XPath(xpath));
                                imageSrc = image.GetAttribute("src");
                                if (string.IsNullOrEmpty(imageSrc))
                                {
                                    imageSrc = image.FindElement(By.TagName("img")).GetAttribute("src");
                                }
                            }
                        }
                        //Getting Price
                        var priceText = "";
                        try
                        {
                            var xpath = "";
                            if (frame.Option == 1) xpath = frame.PricePathOption + $"[{itemIndex}]";
                            else xpath = frame.PricePathOption;
                            
                            var priceObject = driver.FindElement(By.XPath(xpath));
                            priceText = priceObject.Text;
                        }
                        catch (StaleElementReferenceException)
                        {
                            var xpath = "";
                            if (frame.Option == 1) xpath = frame.PricePathOption + $"[{itemIndex}]";
                            else xpath = frame.PricePathOption;

                            var priceObject = driver.FindElement(By.XPath(xpath));
                            priceText = priceObject.Text;
                        }
                        
                        var symbolContains = priceText.IndexOf('₴');
                        var wordContains = priceText.IndexOf("грн");
                        var index = -1;

                        if (symbolContains != -1) { index = symbolContains; }
                        else if (wordContains != -1) { index = wordContains; }
                        if (index == -1) continue;
                        while (index >= 0)
                        {
                            price += priceText[index];
                            index--;
                        }

                        var priceArray = price.ToCharArray();
                        Array.Reverse(priceArray);
                        var r = new Regex("[^0-9.]");
                        price = r.Replace(new string(priceArray), "");
                        double.TryParse(price, out var priceSum);

                        if (result.Exists(x => x.Price == price && x.SiteName == siteName && x.Name == name && x.ImageSrc == imageSrc)) continue;
                        if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(price) && !string.IsNullOrEmpty(imageSrc)) 
                        {
                            Item resultItem = new Item
                            {
                                
                                Name = name,
                                Price = price,
                                SiteName = siteName,
                                ImageSrc = imageSrc,
                                Url = siteUrl
                            };
                            result.Add(resultItem);
                        }
                    }
                } 
            }
            driver.Close();
            return result;
        }
    }
}
