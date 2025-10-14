using GestioneAccounts.BE.Domain.Models;
using GestioneTickets.Model;

namespace GestioneTickets.Abstractions
{
    public interface ITicketRepository
    {
        Task<ICollection<Ticket>> GetAllTicket();
        Task<Ticket> GetTicketById(string ticketId);
        Task<Ticket> CreateTicket(Ticket ticket);
        Task<Ticket> UpdateTicket(string? nome, int ticketId);
        Task<bool> DeleteTicket(int ticketId);
        Task<ICollection<Ticket>> SearchTickets(string? nome, string?ticketId, DateTime? dataCreazione, DateTime? dataChiusura, string? categoria);

    }
}
