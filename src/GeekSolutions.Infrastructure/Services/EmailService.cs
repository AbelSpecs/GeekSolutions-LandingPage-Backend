using GeekSolutions.Application.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace GeekSolutions.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var email = new MimeMessage();

            // Cargar Remitente
            var fromEmail = _configuration["EmailSettings:FromEmail"] ?? _configuration["EmailSettings:Username"];
            email.From.Add(MailboxAddress.Parse(fromEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            // Construir Cuerpo
            var builder = new BodyBuilder
            {
                HtmlBody = body
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            // Obtener puerto de forma segura (por defecto 587 si no existe en config)
            int port = int.TryParse(_configuration["EmailSettings:Port"], out var parsedPort) ? parsedPort : 465;

            string smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";

            // Usamos Auto para que MailKit elija automáticamente entre StartTls (587) o SslOnConnect (465)
            await smtp.ConnectAsync(smtpServer, port, SecureSocketOptions.SslOnConnect);

            await smtp.AuthenticateAsync(
                _configuration["EmailSettings:Username"],
                _configuration["EmailSettings:Password"]
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            // Es recomendable loguear el error aquí para que puedas verlo en los logs de Render
            // sin interrumpir ni colgar la petición HTTP de tu controlador.
            Console.WriteLine($"Error al enviar el correo: {ex.Message}");
            throw; // Puedes quitar este throw si prefieres que la API responda exitosamente aunque falle el correo
        }
    }
}