using EmailNotification.Interfaces;
using EmailNotification.Repositories;
using EmailNotification.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC
{
    public static class IoC
    {
        public static T Get<T>() where T : class
        {
            if (typeof(T) == typeof(IDate))
                return new DateService() as T;

            if (typeof(T) == typeof(ILog))
                return new LogService(true, System.Web.HttpContext.Current.Server.MapPath("App_Data")) as T;

            if (typeof(T) == typeof(ISettings))
                return new SettingsRepository(Get<IEmailNotificationUrls>()) as T;

            if (typeof(T) == typeof(IEmailNotificationUrls))
                return new EmailNotificationUrls(System.Web.HttpContext.Current.Server.MapPath("App_Config/settings.txt")) as T;

            if (typeof(T) == typeof(IEmailNotificationItems))
                return new EmailNotificationRepository() as T;

            if (typeof(T) == typeof(IEmail))
                return new EmailService(Get<ILog>()) as T;

            if (typeof(T) == typeof(IStatus))
                return new StatusRepository(System.Web.HttpContext.Current.Server.MapPath("App_Data")) as T;


            throw new ApplicationException("Type not found: " + typeof(T));
        }

    }
}
