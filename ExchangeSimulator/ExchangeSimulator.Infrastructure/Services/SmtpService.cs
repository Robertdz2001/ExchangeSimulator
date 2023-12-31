﻿using ExchangeSimulator.Application.Services;
using System.Net;
using System.Net.Mail;
using ExchangeSimulator.Infrastructure.EF.Options;
using System.Net.Mime;

namespace ExchangeSimulator.Infrastructure.Services;

/// <summary>
/// Implementation for service used for email communication.
/// </summary>
public class SmtpService : ISmtpService {
    private readonly SmtpOptions _smtpOptions;

    public SmtpService(SmtpOptions smtpOptions)
    {
        _smtpOptions = smtpOptions;
    }

    ///<inheritdoc/>
    public async Task SendMessage(string email, string subject, string message)
    {
        string fromMail = _smtpOptions.FromMail;
        string fromPassword = _smtpOptions.FromPassword;

        MailMessage mailMessage = new MailMessage();

        mailMessage.From = new MailAddress(fromMail);
        mailMessage.Subject = subject;
        mailMessage.To.Add(new MailAddress(email));
        mailMessage.AlternateViews.Add(GetEmbeddedImage("..\\logo-dark.png", string.Format(_smtpOptions.Body, message)));
        mailMessage.IsBodyHtml = true;

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromMail, fromPassword),
            EnableSsl = true
        };
        smtpClient.Send(mailMessage);
    }

    private AlternateView GetEmbeddedImage(string imagePath, string emailBody) {
        LinkedResource imageResource = new LinkedResource(imagePath);
        imageResource.ContentId = Guid.NewGuid().ToString();
        string htmlBody = $@"<html><body><img src='cid:{imageResource.ContentId}'/>{emailBody}</body></html>";
        AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
        alternateView.LinkedResources.Add(imageResource);
        return alternateView;
    }
}