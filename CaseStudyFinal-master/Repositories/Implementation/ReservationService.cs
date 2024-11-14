using System.Collections.Generic;
using RoadReady.Data;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;

        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Reservation> GetAllReservations()
        {
            return _context.Reservations.ToList();
        }

        public Reservation GetReservationById(int id)
        {
            return _context.Reservations.Find(id);
        }

        public int AddReservation(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return reservation.ReservationId;
        }

        public string UpdateReservation(Reservation reservation)
        {
            var existingReservation = _context.Reservations.Find(reservation.ReservationId);
            if (existingReservation == null) return "Reservation not found";

            existingReservation.UserId = reservation.UserId;
            existingReservation.CarId = reservation.CarId;
            existingReservation.PickupDate = reservation.PickupDate;
            existingReservation.DropoffDate = reservation.DropoffDate;
            existingReservation.TotalAmount = reservation.TotalAmount;
            existingReservation.Status = reservation.Status;

            _context.SaveChanges();
            return "Reservation updated successfully";
        }

        public string DeleteReservation(int id)
        {
            var existingReservation = _context.Reservations.Find(id);
            if (existingReservation == null) return "Reservation not found";

            _context.Reservations.Remove(existingReservation);
            _context.SaveChanges();
            return "Reservation deleted successfully";
        }
    }
}
