using FluentValidation;
using GeekSolutions.Application.DTO;
using GeekSolutions.Application.Interfaces.Persistence;
using GeekSolutions.Application.Interfaces.Services;
using GeekSolutions.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GeekSolutions.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactsRepository _contactRepository;
    private readonly IEmailService _emailService;
    private readonly IValidator<ContactDto> _validator;

    public ContactsController(IContactsRepository contactRepository, IEmailService emailService, IValidator<ContactDto> validator)
    {
        _contactRepository = contactRepository;
        _emailService = emailService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ContactDto contactDto)
    {
        var validationResult = await _validator.ValidateAsync(contactDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var contact = new Contact
        {
            Name = contactDto.Name,
            Email = contactDto.Email,
            Message = contactDto.Message
        };
        await _contactRepository.InsertContactAsync(contact);

        try
        {
            string htmlBody = $@"
                <h2>¡Hola {contact.Name}!</h2>
                <p>Hemos recibido tu mensaje correctamente.</p>
                <p>Nos pondremos en contacto contigo lo antes posible.</p>
                <br>
                <p>Atentamente,<br><strong>El equipo de Geek Solutions</strong></p>";

            await _emailService.SendEmailAsync(contactDto.Email, "Nuevo contacto", htmlBody);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EMAIL TIMEOUT ERROR]: {ex.Message}");
        }
        return Ok(contact);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContactByIdAsync(string id)
    {
        var contact = await _contactRepository.GetContactsByIdAsync(id);
        if (contact == null)
        {
            return NotFound();
        }
        var contactDto = new ContactDto
        {
            Id = contact.Id,
            Name = contact.Name,
            Email = contact.Email,
            Message = contact.Message
        };
        return Ok(contactDto);
    }
}