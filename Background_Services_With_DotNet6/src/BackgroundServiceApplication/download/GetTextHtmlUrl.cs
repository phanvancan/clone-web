using BackgroundServiceApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundServiceApplication.download;

public class GetTextHtmlUrl : IGetTextHtmlUrl
{

    private readonly ILogger<GetTextHtmlUrl> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public GetTextHtmlUrl(ILogger<GetTextHtmlUrl> logger
        , IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
    }


    string str = "";
    string str1 = "";
    string urlAddress = "https://taisancong.vn/niem-yet-gia-dau-gia-dau-thau/thong-tin-ban-dau-gia";
    string urlreplace = "https://...vn/";
    string urlreplace1 = "...vn";
    string rootpath = "admin";

    

    public string GetHtmlToText()
    {
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
            str1 = str = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
        }

     

        return str1;
    }

    private FileElement creatfolder(string url, string urlrp, string rootpath = "")
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
            if (r.IndexOf("https:") != -1)
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
        fileElement.fileName = arrString[totalElements - 1];
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
