using EmailNotification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_Notification : System.Web.UI.UserControl
{
    public String CsvFile { get; set; }

    private ILog _logging;
    private IDate _date;
    private ISettings _settings;
    private IEmailNotificationItems _emailNotificationItems;
    private IEmail _sendEmail;
    private IStatus _status;


    protected void Page_Load(object sender, EventArgs e)
    {
        _logging = IoC.IoC.Get<ILog>();
        _date = IoC.IoC.Get<IDate>();
        _settings = IoC.IoC.Get<ISettings>();
        _emailNotificationItems = IoC.IoC.Get<IEmailNotificationItems>();
        _sendEmail = IoC.IoC.Get<IEmail>();
        _status = IoC.IoC.Get<IStatus>();

        var url = Request.Url.AbsoluteUri;

        if (Request.QueryString["csvfile"] == null)
        {
            _sendEmail.SendEmail("nns@email.dk", "nns@email.dk", "Cronejob startet", "");
            url = null;
        }

        Run(url);
        
    }

    void Run(String url)
    {
        var service = new EmailNotification.EmailNotification(_logging, _date, _settings, _emailNotificationItems, _sendEmail, _status,
            "noreply@datajonglering.dk", url);

        service.Run();
    }

}