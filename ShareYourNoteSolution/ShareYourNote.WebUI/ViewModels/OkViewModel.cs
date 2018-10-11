using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShareYourNote.WebUI.ViewModels;

namespace ShareYourNote.WebUI.ViewModels
{
    public class OkViewModel : NotificationViewModelBase<string>
    {
        public OkViewModel()
        {
            Title = "İşlem Başarılı";
        }
    }
}