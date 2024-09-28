using BackgroundServiceApplication.download;
using BackgroundServiceApplication.Wrappers.Contracts;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Reflection;
using System.Text;
using HtmlAgilityPack;
using BackgroundServiceApplication.Models;
using System.ComponentModel;
 

namespace BackgroundServiceApplication.Wrappers;


public interface ISalaryCalculateWrapper
{
    Task SalaryCalculateAsync();

}


public class SalaryCalculateWrapper : ISalaryCalculateWrapper
{
  //private readonly  IGetTextHtmlUrl _htmlUrl;
    //private readonly IWebHostEnvironment _webHostEnvironment;
    private string urlAddress = "https://taisancong.vn/niem-yet-gia-dau-gia-dau-thau/thong-tin-ban-dau-gia&BRSR=";
   
    
    
    public SalaryCalculateWrapper( )
    {
         
    
    }

    public async Task SalaryCalculateAsync( )
    {

        string str = "";
        HtmlDocument doc = new HtmlDocument();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Calculate Personnel Salary In Samery Subsystem In This Time {DateTime.Now} \n");

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
             str = readStream.ReadToEnd();
            

            doc.LoadHtml(str);

            var itemList = doc.DocumentNode.SelectNodes("//div[@class='tbody']") 
                              .Select(p => p.InnerHtml)
                              .ToList();
            str = itemList.FirstOrDefault();

            // Console.ForegroundColor = ConsoleColor.Green;
          
            response.Close();
            readStream.Close();
        }
        //tbody
        string[] regexImgSrc =  {
                @"<script[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>",
                @"<link[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>" ,
                @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>"
            };
        // html = new  ();

      

        doc = new HtmlDocument();
        doc.LoadHtml(str);
        var iLists = doc.DocumentNode.SelectNodes("//div[@class='row']")
                          .Select(p => p.InnerHtml)
                          .ToList();

        List<ThongTinNiemYet> niemYets = new List<ThongTinNiemYet>();
        foreach (var item in iLists) 
        {
            niemYets.Add(thongTinNiem(item));
        }

          var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location); //Assembly.GetEntryAssembly().Location;
        // SaveToCsv(niemYets, path);

        //foreach (var item in niemYets) 

        //{ 
        //    //Console.WriteLine(item.DonViCoTaiSan);
        //    //Console.WriteLine(item.TaiSanDauGia);
        //    Console.WriteLine(item.NgayDauGia);

        //}
      //  var json = new JavaScriptSerializer().Serialize(niemYets);

        string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(niemYets).ToString();

        // File.WriteAllLines(path, jsonString,Encoding.UTF8);

       // string applicationDirectory = Path.GetDirectoryName(typeof(Logger).Assembly.Location);
        string counterDirectory = Path.Join(path, "Logs");
        string counterFile = Path.Join(counterDirectory, "data-json.txt");
        int count = 0;
        if (!Directory.Exists(counterDirectory))
        {
            Directory.CreateDirectory(counterDirectory);
        }

   

        string[] toWrite = { jsonString };
        

        File.WriteAllLines(counterFile, toWrite);
        



        await Task.Delay(5000);
    }
    private string getOneNode(string st,string classname) {
        HtmlDocument doc1 = new HtmlDocument();
        doc1.LoadHtml(st);
        ThongTinNiemYet thong = new ThongTinNiemYet();
        return doc1.DocumentNode.SelectNodes("//div[@class='" + classname + "']").Select(p => p.InnerHtml).FirstOrDefault().ToString();


    }
    private void SaveToCsv<T>(List<T> reportData, string path)
    {
        var lines = new List<string>();
        IEnumerable<PropertyDescriptor> props = TypeDescriptor.GetProperties(typeof(T)).OfType<PropertyDescriptor>();
        var header = string.Join(",", props.ToList().Select(x => x.Name));
        lines.Add(header);
        var valueLines = reportData.Select(row => string.Join(",", header.Split(',').Select(a => row.GetType().GetProperty(a).GetValue(row, null))));
        lines.AddRange(valueLines);
        File.WriteAllLines(path, lines.ToArray());
    }

    private ThongTinNiemYet thongTinNiem(string st)
    {
        HtmlDocument doc1 = new HtmlDocument();
        doc1.LoadHtml(st);
        ThongTinNiemYet tt = new ThongTinNiemYet();
        
        int.TryParse(getOneNode(st, "b-stt"), out int stt);
        tt.stt = stt;
        tt.TaiSanDauGia = getOneNode(st, "b-tsb");
        tt.DonViCoTaiSan = getOneNode(st, "b-dvico");
        tt.DonViBanDauGia = getOneNode(st, "b-dvb");
        tt.ThoiDienbanHoSo = getOneNode(st, "b-tdban");
        tt.NgayDauGia = getOneNode(st, "b-ngayban");
        tt.DiaDiemBanDauGia = getOneNode(st, "b-address");

        //var gt = @"<div class=""row"">
        //                            <div class=""b-stt"">1</div>
        //                            <div class=""b-tsb""><a href=""https://taisancong.vn/doanh-nghiep-dau-gia-tu-nhan-dai-phat-dau-gia-tai-san-tich-thu-trong-linh-vuc-lam-nghiep-tren-dia-ban-huyen-dak-po-huyen-dak-po-tinh-gia-lai-36964.html""><span class=""article_publish_color"">Đấu giá tài sản tịch thu trong lĩnh vực lâm nghiệp trên địa bàn huyện Đak Pơ, huyện Đak Pơ, tỉnh Gia Lai gồm: Tang vật: 5,920 m3 Gỗ các loại; Phế liệu: 250 kg ( gồm 04 xe máy và 02 máy cưa xăng)</span></a></div>
        //                            <div class=""b-dvico"">Hạt Kiểm Lâm huyện Đak Pơ</div>
        //                            <div class=""b-dvb"">Doanh nghiệp Đấu giá tư nhân Đại Phát</div>
        //                            <div class=""b-tdban"">26-09-2024 07:00 - <br>08-10-2024 15:00</div>
        //                            <div class=""b-ngayban"">11-10-2024 14:30</div>
        //                            <div class=""b-address"">Hạt Kiểm Lâm huyện Đak Pơ</div>
        //                        </div>";


    //          public int stt { get; set; } = 0;
    //public string? TaiSanDauGia { get; set; }
    //public string? DonViCoTaiSan { get; set; }
    //public string? DonViBanDauGia { get; set; }
    //public string? ThoiDienbanHoSo { get; set; }
    //public string? NgayDauGia { get; set; }
    //public string? DiaDiemBanDauGia { get; set; }


        return tt;
    }
}
