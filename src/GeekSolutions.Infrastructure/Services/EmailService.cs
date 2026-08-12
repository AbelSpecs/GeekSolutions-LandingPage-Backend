using GeekSolutions.Application.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GeekSolutions.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
       
        string apiKey = _configuration["EmailSettings:ResendApiKey"]!;

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var payload = new
        {
            from = "Geek Solutions",
            to = new[] { toEmail },
            subject = subject,
            html = body
        };

        request.Content = JsonContent.Create(payload);

        try
        {
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error enviando correo a través de Resend: {errorResponse}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[RESEND API ERROR]: {ex.Message}");
            throw;
        }
    }
}