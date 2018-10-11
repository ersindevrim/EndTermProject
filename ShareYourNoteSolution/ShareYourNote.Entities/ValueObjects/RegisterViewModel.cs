using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShareYourNote.Entities.ValueObjects
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Boş Geçilemez"),
         StringLength(25, ErrorMessage = "Max. {0} karakter olmalı")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Boş Geçilemez"),
         StringLength(70, ErrorMessage = "Max. {0} karakter olmalı"),
         EmailAddress(ErrorMessage = "Geçerli bir eposta giriniz")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Boş Geçilemez"),
         StringLength(25, ErrorMessage = "Max. {0} karakter olmalı"),
         DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Boş Geçilemez"),
         StringLength(25, ErrorMessage = "Max. {0} karakter olmalı"),
         DataType(DataType.Password),
         Compare("Password",ErrorMessage = "Şifreler Uyuşmuyor.")]
        public string VerifyPassword { get; set; }
    }
}