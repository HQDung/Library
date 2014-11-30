using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Models;
using System.Text;
using PagedList;

namespace Library.Controllers
{
    public class QLDocGiaController : Controller
    {
        //
        Models.QLThuVienEntities entities = new Models.QLThuVienEntities();

        public ActionResult Index(string madg, string ten, string sdt, int? page)
        {
            IEnumerable<Models.DocGia> listDG = null;
            ViewBag.maxID = int.Parse(entities.DocGias.Max(x => x.MaDocGia)) + 1;

            if (madg == null && ten == null && sdt == null)
            {
                listDG = (from p in entities.DocGias select p).ToList();
            }
            else
            {

                ViewBag.madg = madg;
                ViewBag.ten = ten;
                ViewBag.sdt = sdt;

                if (madg == null)
                    madg = "";
                if (ten == null)
                    ten = "";
                if (sdt == null)
                    sdt = "";

                if (madg != "" && ten != "" && sdt != "")
                    listDG = (from p in entities.DocGias where p.MaDocGia == madg && p.HoTen.Contains(ten) && p.SDT == sdt select p).ToList();
                else if (madg == "" && ten != "" && sdt != "")
                    listDG = (from p in entities.DocGias where p.HoTen.Contains(ten) && p.SDT == sdt select p).ToList();
                else if (ten == "" && madg != "" && sdt != "")
                    listDG = (from p in entities.DocGias where p.MaDocGia == madg && p.SDT == sdt select p).ToList();
                else if (sdt == "" && madg != "" && ten != "")
                    listDG = (from p in entities.DocGias where p.MaDocGia == madg && p.HoTen.Contains(ten) select p).ToList();
                else if (madg == "" && ten == "" && sdt != "")
                    listDG = (from p in entities.DocGias where p.SDT == sdt select p).ToList();
                else if (ten == "" && madg != "" && sdt == "")
                    listDG = (from p in entities.DocGias where p.MaDocGia == madg select p).ToList();
                else if (sdt == "" && madg == "" && ten != "")
                    listDG = (from p in entities.DocGias where p.HoTen.Contains(ten) select p).ToList();
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(listDG.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult GetInfor(string id)
        {
            StringBuilder sb = new StringBuilder();

            var result = entities.DocGias.Where(x => x.MaDocGia == id).FirstOrDefault();
            string ns, hh, dk;
            if (result.NgaySinh != null)
                ns = result.NgaySinh.Value.ToString("dd/MM/yyyy");
            else
                ns = null; ;
            if (result.NgayHetHan != null)
                hh = result.NgayHetHan.Value.ToString("dd/MM/yyyy");
            else
                hh = null; ;
            if (result.NgayDangKy != null)
                dk = result.NgayDangKy.Value.ToString("dd/MM/yyyy");
            else
                dk = null; ;

            sb.AppendFormat("<form action='/QLDocGia/SaveInfor' method='post' id='QLDG_form' onsubmit='return validate();'>");
            sb.AppendFormat("<table class='dg_left'><tr><td>Họ tên</td><td colspan='3'>");
            sb.AppendFormat("<input type='hidden' value='{0}' name='id'  id='id_stored'  />", result.MaDocGia);
            sb.AppendFormat("<input type='text' disabled='disabled' name='hoten' value='{0}' class='QL_form' style='width: 275px;' /><span style='color: red;' class='noticeform_QL' hidden>*</span></td></tr>", result.HoTen);
            sb.AppendFormat("<tr><td>SĐT</td><td colspan='3'><input type='text' disabled='disabled' value='{0}' name='sdt' class='QL_form' style='width: 275px;' /><span style='color: red;' class='noticeform_QL' hidden>*</span></td></tr>", result.SDT);
            sb.AppendFormat("<tr><td>CMND</td><td colspan='3'><input type='text' disabled='disabled' value='{0}' name='cmnd' class='QL_form' style='width: 275px;' /><span style='color: red;' class='noticeform_QL' hidden>*</span></td></tr>", result.CMND);
            sb.AppendFormat("<tr><td>Email</td><td colspan='3'><input type='text' disabled='disabled' class='QL_form' name='email' value='{0}' style='width: 275px;' /><span style='color: red;' class='noticeform_QL' hidden>*</span></td></tr>", result.EMail);
            sb.AppendFormat("<tr><td>Địa chỉ</td><td colspan='3'><input type='text' disabled='disabled' name='dc' value='{0}' class='QL_form' style='width: 275px;' /><span style='color: red;' class='noticeform_QL' hidden>*</span></td></tr>", result.DiaChi);
            sb.AppendFormat("<tr><td>Ngày sinh</td><td colspan='2'><input type='text' value='{0}' name='dob' disabled='disabled' class='QL_form' /><span style='color: red;' class='noticeform_QL' hidden>*</span></td>", ns);
            sb.AppendFormat("<td><input type='button' value='Thêm mới' onclick='clearInfor()' id='TM_QL' class='btnMenu2' /></td></tr>");
            sb.AppendFormat("<tr><td>Ngày ĐK</td><td colspan='2'><input type='text' disabled='disabled' name='dk' value='{0}' class='QL_form'  /><span style='color: red;' class='noticeform_QL' hidden>*</span></td>", dk);
            sb.AppendFormat("<td><input type='button' value='Cập nhật ' id='CN_QL' disabled onclick='enable()' class='btnMenu2' /></td></tr>");
            sb.AppendFormat("<tr><td>Ngày hết hạn</td><td colspan='2'><input type='text' disabled='disabled' name='hh' value='{0}' class='QL_form'  /><span style='color: red;' class='noticeform_QL' hidden>*</span></td>", hh);
            sb.AppendFormat("<td><input type='submit' value='Lưu' disabled id='sm_QLForm' class='btnMenu2' /></td></tr>");
            if (result.GioiTinh.Trim().CompareTo("Nam") == 0)
                sb.AppendFormat("<tr><td>Giới tính</td><td colspan='2'><input type='radio' name='gt' checked='checked' value='Nam'/>Nam<input type='radio' name='gt' value='Nu'/>Nữ <span style='color: red;' class='noticeform_QL' hidden>*</span></td>");
            else
                sb.AppendFormat("<tr><td>Giới tính</td><td colspan='2'><input type='radio' name='gt' value='Nam' />Nam<input type='radio' name='gt' checked='checked' value='Nu' />Nữ <span style='color: red;' class='noticeform_QL' hidden>*</span></td>");
            sb.AppendFormat("<td><input type='button' value='Xóa' class='btnMenu2' disabled id='btnDel_QLForm' onclick='btnDelete()' /></td></tr></table></form>");
            Response.Write(sb.ToString());
            return null;
        }

        public ActionResult SaveInfor(string id, string hoten, string sdt, string dob, string dk, string hh, string gt, string cmnd, string email, string dc)
        {

            var result = entities.DocGias.Where(x => x.MaDocGia == id).FirstOrDefault();
            if (result != null)
            {
                //update
                result.HoTen = hoten;
                result.GioiTinh = gt;
                result.EMail = email;
                result.SDT = sdt;
                result.DiaChi = dc;
                result.CMND = cmnd;
                if (dob.CompareTo("") != 0)
                    result.NgaySinh = DateTime.ParseExact(dob, "dd/MM/yyyy", null);
                else
                    result.NgaySinh = null;
                if (dk.CompareTo("") != 0)
                    result.NgayDangKy = DateTime.ParseExact(dk, "dd/MM/yyyy", null);
                else
                    result.NgayDangKy = null;
                if (hh.CompareTo("") != 0)
                    result.NgayHetHan = DateTime.ParseExact(hh, "dd/MM/yyyy", null);
                else
                    result.NgayHetHan = null;

                entities.SaveChanges();
            }
            else
            {
                //insert

                Library.Models.DocGia dg = new Models.DocGia();
                dg.MaDocGia = id;
                dg.HoTen = hoten;
                dg.GioiTinh = gt;
                dg.EMail = email;
                dg.SDT = sdt;
                dg.DiaChi = dc;
                dg.CMND = cmnd;
                if (dob.CompareTo("") != 0)
                    dg.NgaySinh = DateTime.Parse(dob);
                else
                    dg.NgaySinh = null;
                if (dk.CompareTo("") != 0)
                    dg.NgayDangKy = DateTime.Parse(dk);
                else
                    dg.NgayDangKy = null;
                if (hh.CompareTo("") != 0)
                    dg.NgayHetHan = DateTime.Parse(hh);
                else
                    dg.NgayHetHan = null;

                entities.DocGias.Add(dg);
                entities.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult del(string id)
        {
            var rs1 = entities.PhieuMuonSaches.Where(x => x.MaDocGia == id).ToList();

            for (int i = 0; i < rs1.Count(); i++)
            {
                entities.PhieuMuonSaches.Remove(rs1[i]);
                entities.SaveChanges();
            }

            var rs = entities.DocGias.Where(x => x.MaDocGia == id).First();
            entities.DocGias.Remove(rs);
            entities.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ThongKe()
        {
            return View();
        }

        public JsonResult Chart(List<String> input)
        {
            var tn = DateTime.Parse(input[0]);
            var dn = DateTime.Parse(input[1]);

            IEnumerable<Models.ChartModels> rs = null;

            if (input[2].CompareTo("1") == 0)//tháng
            {
                if (input[3].CompareTo("1") == 0) //đk mới
                {
                    string query = "select month(NgayDangKy) as time,count(*) as data from DocGia where NgayDangKy between '"+tn+"' and '"+dn+"'group by month(NgayDangKy)";
                    rs = entities.Database.SqlQuery<Models.ChartModels>(query).ToList();
                }
                else if (input[3].CompareTo("2") == 0)//trả trễ
                {
                    string query = "select month(p.NgayTra) as time,count(*) as data from DocGia d,PhieuMuonSach p where p.NgayTra > p.NgayTraQuyDinh and p.NgayTra between '"+tn+"' and '"+dn+"' and p.MaDocGia = d.MaDocGia group by month(p.NgayTra)";
                    rs = entities.Database.SqlQuery<Models.ChartModels>(query).ToList();
                }
                else //sách mượn nhiều
                { 
                }
            }
            else //năm
            {
                if (input[3].CompareTo("1") == 0) //đk mới
                {
                    string query = "select year(NgayDangKy) as time,count(*) as data from DocGia where NgayDangKy between '" + tn + "' and '" + dn + "'group by year(NgayDangKy)";
                    rs = entities.Database.SqlQuery<Models.ChartModels>(query).ToList();
                }
                else if (input[3].CompareTo("2") == 0)//trả trễ
                {
                    string query = "select year(p.NgayTra) as time,count(*) as data from DocGia d,PhieuMuonSach p where p.NgayTra > p.NgayTraQuyDinh and p.NgayTra between '" + tn + "' and '" + dn + "' and p.MaDocGia = d.MaDocGia group by year(p.NgayTra)";
                    rs = entities.Database.SqlQuery<Models.ChartModels>(query).ToList();
                }
                else //sách mượn nhiều
                {
                }
            }
            return Json(rs,JsonRequestBehavior.AllowGet);
        }

        public ActionResult rsThongKe(string tngay, string dngay, string loai, string tk, int? page)
        {
            var tn = DateTime.Parse(tngay);
            var dn = DateTime.Parse(dngay);
            ViewBag.tn = tngay;
            ViewBag.dn = dngay;
            ViewBag.loai = loai;
            ViewBag.tk = tk;
            IEnumerable<Models.DocGia> rs = null;

            if (loai.CompareTo("1") == 0)
            {   //dk mới
                if (tk.CompareTo("1") == 0)
                    rs = (from p in entities.DocGias where p.NgayDangKy >= tn && p.NgayDangKy <= dn select p).ToList();
                else if (tk.CompareTo("2") == 0)//trễ hạn
                {
                    string query = "select d.MaDocGia,d.HoTen,d.NgaySinh,d.DiaChi,d.LoaiDocGia,d.SDT,d.GioiTinh,d.NgayDangKy,d.NgayHetHan,d.CMND,d.MatKhau,d.EMail"
                                    + " from DocGia d,PhieuMuonSach p "
                                    + "where p.NgayTra > p.NgayTraQuyDinh and p.NgayTra between '" + tn + "' and '" + dn + "' and p.MaDocGia = d.MaDocGia";
                    rs = entities.Database.SqlQuery<Models.DocGia>(query).ToList();
                }
            }
            
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(rs.ToPagedList(pageNumber, pageSize));
        }
    }
}
