using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_HW.Models
{
    public class 客戶資料清單VM
    {
        [Required(ErrorMessage = "{0}不可為空！")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "只能輸入數字！")]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0}不可為空！")]
        [StringLength(50, ErrorMessage = "{0}長度不可超過{1}個字元！")]
        public string 客戶名稱 { get; set; }
        public int 聯絡人數量 { get; set; }
        public int 銀行帳戶數量 { get; set; }
    }
}