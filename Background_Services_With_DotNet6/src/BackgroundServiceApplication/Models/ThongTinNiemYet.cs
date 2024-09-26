using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundServiceApplication.Models
{

//STT
//Tài sản bán đấu giá
//Đơn vị có tài sản bán
//Đơn vị bán đấu giá
//Thời điểm bán hồ sơ
//Ngày bán đấu giá
//Địa điểm bán đấu giá

    public class ThongTinNiemYet
    {
        public int stt { get; set; } = 0;
        public string? TaiSanDauGia { get; set; }
        public string? DonViCoTaiSan { get; set; }
        public string? DonViBanDauGia { get; set; }
        public string? ThoiDienbanHoSo { get; set; }
        public string? NgayDauGia { get; set; }
        public string? DiaDiemBanDauGia { get; set; }

    }
}
