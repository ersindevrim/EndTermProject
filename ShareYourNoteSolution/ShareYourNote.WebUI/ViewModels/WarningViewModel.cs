using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareYourNote.WebUI.ViewModels
{
    public class WarningViewModel : NotificationViewModelBase<string>
    {
        public WarningViewModel()
        {
            Title = "İşlem başarısız..";
        }
    }
}