using download.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
 
namespace download.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        string str = "";
        string str1 = "";
        string urlAddress = "https://.../?home=1";
        string urlreplace = "https://...vn/";
        string urlreplace1 = "...vn";
        string rootpath = "admin";


        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        
        public IActionResult Index()
        {
            // return View();
            //bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

            //if (!exists)
            //    System.IO.Directory.CreateDirectory(Server.MapPath(subPath));
       
            //Console.WriteLine(urlAddress);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();


            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream,
                        Encoding.GetEncoding(response.CharacterSet));
                str1= str = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }



            //using (WebClient client = new WebClient())
            //{
            //    str = client.DownloadString(urlAddress);
            //}

           // Console.WriteLine( str);

            // .js
            string[] regexImgSrc =  { 
                @"<script[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>",
                @"<link[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>" , 
                @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>" 
            };
            
          //  string[] regexImgSrc = {@"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>" };

            // hinh anh

            // css
            // regexImgSrc+= @"<link[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
            // hinh anh

            // regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";

            foreach (var mrow in regexImgSrc)
            {
                MatchCollection matchesImgSrc = Regex.Matches(str, mrow, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                foreach (Match m in matchesImgSrc)
                {
                    string href = m.Groups[1].Value;
                    
                    Console.WriteLine("filename filePath  {0}", href);

                    var getfiname = GetFileNameFromUrl(href);
                    if (!getfiname.EndsWith(".html"))
                    {

                         
                        var filename = getfiname;
                        string filenameext = filename.ToLower();
                        //  return Path.GetFileName(uri.LocalPath);
                        if (filenameext.EndsWith(".png") ||
                             filenameext.EndsWith(".css") ||
                            filenameext.EndsWith(".js") ||
                            filenameext.EndsWith(".jpg") ||
                            filenameext.EndsWith(".xml") ||
                            filenameext.EndsWith(".svg") ||
                            filenameext.EndsWith(".jpeg")
                            || filenameext.EndsWith(".gif")
                             || filenameext.EndsWith(".tiff")
                              || filenameext.EndsWith(".raw")
                            )
                        // if ( 
                        // filenameext.EndsWith(".css") 
                        //)
                        {


                            var newPath = creatfolder(href, urlreplace, rootpath);
                            if (newPath.removelink != "")
                            {
                                str1 = str1.Replace(newPath.removelink, "/" + rootpath);
                            }
                            Console.WriteLine("filename filePath  {0}", newPath.filePath + "\\" + filename);
                            href = href.Replace("amp;", "");
                             

                           //    if (newPath != null && href.IndexOf("lk8duhdr9gc6dzgmnqa.png") == -1) 
                          //  {
                                if (!System.IO.File.Exists(newPath.fullName))
                                {
                                    using (var client = new HttpClient())
                                    {
                                    if(href.IndexOf("http://")==-1) 
                                            if(href.IndexOf("https://")==-1) href = urlreplace + href;

                                        using (var s = client.GetStreamAsync(href))
                                        {
                                           
                                            using (var fs = new FileStream(newPath.filePath + "\\" + filename, FileMode.OpenOrCreate))
                                            {
                                                s.Result.CopyTo(fs);

                                                Console.WriteLine("New  file ok {0}", newPath.filePath + "\\" + filename);
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("da ton tai  {0}", newPath.fullName);
                                }
                            //}
                        }
                    }

                }
           
            }
            ConTent conTent = new ConTent(); 
            conTent.Textcontent = str1.Replace("=", "").Replace("?", "").Replace( urlreplace, "/" + rootpath + "/").Replace(urlreplace1, "");
            urlAddress= urlAddress.Substring(urlAddress.Length-1,1) =="/" ? urlAddress.Substring(0,urlAddress.Length-1): urlAddress;


            var fn =  urlAddress.Split("/").Last().Replace("=", "").Replace("?","").Replace("/", "")+ ".html";
            
            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;

            string path = "";
            path = Path.Combine(webRootPath, rootpath);
            fn = Path.Combine(path, fn);

            using (StreamWriter writetext = new StreamWriter(fn))
            {
                writetext.WriteLine(conTent.Textcontent);
            }

            return View();
            #region luu
            //foreach (Match m in matchesImgSrc)
            //{
            //    string href = m.Groups[1].Value;
            //    Uri SomeBaseUri = new Uri("http://nhahatcheovietnam.vn/");

            //    Uri uri;
            //    string url = href.Replace("http://nhahatcheovietnam.vn/", "");
            //    if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            //        uri = new Uri(SomeBaseUri, url);
            //    var filename = Path.GetFileName(uri.LocalPath);
            //    string filenameext = filename.ToLower();
            //    //  return Path.GetFileName(uri.LocalPath);
            //    if (filenameext.EndsWith(".png") || filenameext.EndsWith(".css") || filenameext.EndsWith(".js") || filenameext.EndsWith(".jpg"))
            //    {
            //        Console.WriteLine("File name {0}", filename);

            //        var newPath = creatfolder(href, "download");

            //        if (newPath != null)
            //        {
            //            if (!System.IO.File.Exists(newPath.fullName))
            //            {
            //                using (var client = new HttpClient())
            //                {
            //                    using (var s = client.GetStreamAsync(href))
            //                    {
            //                        //using (var fs = new FileStream(newPath.fullName, FileMode.OpenOrCreate))
            //                        //{
            //                        //    s.Result.CopyTo(fs);
            //                        //    Console.WriteLine("New  file ok {0}", newPath.fullName);
            //                        //}

            //                        using (var fs = new FileStream(newPath.filePath +"\\" +  filename, FileMode.OpenOrCreate))
            //                        {
            //                            s.Result.CopyTo(fs);
            //                            Console.WriteLine("New  file ok {0}", newPath.filePath + "\\" + filename);
            //                        }

            //                    }
            //                }
            //            }
            //            else
            //            {
            //                Console.WriteLine("da ton tai  {0}", newPath.fullName);
            //            }
            //        }
            //    }

            //}

            #endregion luu

            // return View();
        }

        public IActionResult Privacy()
        {

            string ds = @"";

            string[] arr = ds.Split('\n');
            foreach (string item in arr)
            {
                string st = item.Replace("\r", "").Trim();

                if (st != string.Empty)
                {
                    Console.WriteLine(st);
                    var href = st;
                    var newPath = creatfolder(href, urlreplace, rootpath);
                    if (newPath.removelink != "")
                    {
                        str1 = str1.Replace(newPath.removelink, "/" + rootpath);
                    }
                    var filename = GetFileNameFromUrl(href);

                    Console.WriteLine("filename filePath  {0}", newPath.filePath + "\\" + filename);
                    href = href.Replace("amp;", "");


                    //    if (newPath != null && href.IndexOf("lk8duhdr9gc6dzgmnqa.png") == -1) 
                    //  {
                    if (!System.IO.File.Exists(newPath.fullName))
                    {
                        using (var client = new HttpClient())
                        {
                            //if (href.IndexOf("http://") == -1)
                               // if (href.IndexOf("https://") == -1) href = urlreplace + href;

                            using (var s = client.GetStreamAsync(href))
                            {

                                using (var fs = new FileStream(newPath.filePath + "\\" + filename, FileMode.OpenOrCreate))
                                {
                                    s.Result.CopyTo(fs);

                                    Console.WriteLine("New  file ok {0}", newPath.filePath + "\\" + filename);
                                }

                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("da ton tai  {0}", newPath.fullName);
                    }


                }
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
      
        private FileElement creatfolder(string url,string urlrp,string rootpath= "")
        {
            FileElement fileElement = new FileElement();
            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;

            string path = "";
            path = Path.Combine(webRootPath, rootpath);
            
            //or path = Path.Combine(contentRootPath , "wwwroot" ,"CSS" );
            //   Console.WriteLine(str);
            // url = "";// +string[] authorsList = authors.Split(", ");
            string urlReplape = url.Replace(urlrp, "");
            string[] arrString = urlReplape.Split("/");
            var totalElements = arrString.Count(); //6
            bool httpstrue = false;
             
            List<string> list = new List<string>();

            foreach (var r in arrString)
            {


                if (r.IndexOf("https:") == -1) 
                {
                    if (r != "")
                    {
                        list.Add(r);
                        Console.WriteLine("list = {0}", r);
                    }
                }
                if (r.IndexOf("https:")!= -1)
                {
                    httpstrue = true;
                }
                 

            }
            fileElement.removelink = "";

            if (list.Count() > 0)
            {
                if (httpstrue)
                {
                    fileElement.removelink = "https://" + list.First();
                    list.RemoveAt(0);
                }
            }

            

             arrString = list.ToArray();
            totalElements = arrString.Count(); //6

            if (totalElements <= 1)
                return fileElement;
           int i = 1;
            string path1 = path;
            foreach (var r in arrString)
            {
                if (totalElements > i)
                {
                    path1 = Path.Combine(path1, r);
                    Console.WriteLine("arrString = {0}", path1);
                   
                    

                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(path1);
                       // Console.WriteLine("path = {0}", path1);
                    }

                  //  Console.WriteLine("array: {0} of {1}, index({2})", r, totalElements, i);
                }
                 
                i++;
            }
            fileElement.fileName = arrString[totalElements-1];
            fileElement.filePath = path1;
            fileElement.fullName = Path.Combine(path1, arrString[totalElements - 1]);

            return fileElement;
        }
        public static string GetFileNameValidChar(string fileName)
        {
            
            foreach (var item in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(item.ToString(), "");
            }
            return fileName;
        }

        public static string GetFileNameFromUrl(string url)
        {


            
            

            //  return Path.GetFileName(uri.LocalPath);

            string fileName = "";
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                fileName = GetFileNameValidChar(Path.GetFileName(uri.LocalPath));
            }
            string ext = "";
            if (!string.IsNullOrEmpty(fileName))
            {
                ext = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(ext))
                    ext = ".html";
                else
                    ext = "";
                return GetFileNameValidChar(fileName + ext);

            }

            fileName = Path.GetFileName(url);
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "noName";
            }
            ext = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(ext))
                ext = ".html";
            else
                ext = "";
            fileName = fileName + ext;
            if (!fileName.StartsWith("?"))
                fileName = fileName.Split('?').FirstOrDefault();
            fileName = fileName.Split('&').LastOrDefault().Split('=').LastOrDefault();
            return GetFileNameValidChar(fileName);
        }

    }
}