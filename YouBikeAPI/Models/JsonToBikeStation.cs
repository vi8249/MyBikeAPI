using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouBikeAPI.Models
{
    /*
      sno(站點代號)
      sna(場站中文名稱)
      tot(場站總停車格)
      sbi(場站目前車輛數量)
      sarea(場站區域)
      mday(資料更新時間)
      lat(緯度)、lng(經度)
      ar(地點)
      sareaen(場站區域英文)
      snaen(場站名稱英文)
      aren(地址英文)
      bemp(空位數量)
      act(全站禁用狀態)
      srcUpdateTime
      updateTime
      infoTime
      infoDate
    */
    public class JsonToBikeStation
    {
        public string Scity { get; set; }
        public string Scityen { get; set; }
        public string Sna { get; set; }
        public string Sarea { get; set; }
        public string Ar { get; set; }
        public string Snaen { get; set; }
        public string Sareaen { get; set; }
        public string Aren { get; set; }
        public string Sno { get; set; }
        public string Tot { get; set; }
        public string Sbi { get; set; }
        public string Mday { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Bemp { get; set; }
        public int Act { get; set; }
    }
}
