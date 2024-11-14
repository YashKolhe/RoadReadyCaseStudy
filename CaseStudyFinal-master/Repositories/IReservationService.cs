using RoadReady.Models;

namespace RoadReady.Repositories
{
    public interface IReservationService
    {
        List<Reservation> GetAllReservations();
        Reservation GetReservationById(int id);
        int AddReservation(Reservation reservation);
        string UpdateReservation(Reservation reservation);
        string DeleteReservation(int id);
    }
}
