using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareYourNote.WebUI.ViewModels
{
    public class InfoViewModel : NotificationViewModelBase<string>
    {
        public InfoViewModel()
        {
            Title = "Bilgilendirme..";
        }
    }
}