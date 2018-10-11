using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShareYourNote.Entities.ValueObjects
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı Boş Geçilemez")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Şifre boş olmamalı"),DataType(DataType.Password)]
        public string Password { get; set; }
    }
}