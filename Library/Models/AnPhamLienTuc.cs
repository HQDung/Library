//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Library.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AnPhamLienTuc
    {
        public AnPhamLienTuc()
        {
            this.CTDonHangs = new HashSet<CTDonHang>();
            this.KyAnPhams = new HashSet<KyAnPham>();
        }
    
        public int ID { get; set; }
        public string TenAnPham { get; set; }
        public string TheLoai { get; set; }
        public Nullable<System.DateTime> NgayNhap { get; set; }
        public Nullable<int> TacGia { get; set; }
    
        public virtual TacGia TacGia1 { get; set; }
        public virtual ICollection<CTDonHang> CTDonHangs { get; set; }
        public virtual ICollection<KyAnPham> KyAnPhams { get; set; }
    }
}