using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShareYourNote.Entities;

namespace ShareYourNote.WebUI.Models
{
    public class SessionManager
    {
        public static ShareYourNoteUser User
        {
            get
            {
                if (HttpContext.Current.Session["login"] != null)
                {
                    return HttpContext.Current.Session["login"] as ShareYourNoteUser;
                }

                return null;
            }
        }

        public static void Set<T>(string key, T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }

        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                return (T) HttpContext.Current.Session[key];
            }

            return default(T);
        }

        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}