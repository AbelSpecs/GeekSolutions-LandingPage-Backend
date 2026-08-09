using GeekSolutions.Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GeekSolutions.Application.Interfaces.Persistence
{
    public interface IContactsRepository
    {
        Task<Contact> InsertContactAsync(Contact contact);
        Task<Contact> GetContactsByIdAsync(string id);
        //Task<Contact> InsertContactAsync(Contact contact);
    }
}