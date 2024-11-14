using RoadReady.Data;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;

        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Car> GetAllCars()
        {
            return _context.Cars.ToList();
        }

        public Car GetCarById(int id)
        {
            return _context.Cars.Find(id);
        }

        public int AddCar(Car car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
            return car.CarId;
        }

        public string UpdateCar(Car car)
        {
            var existingCar = _context.Cars.Find(car.CarId);
            if (existingCar == null) return "Car not found";

            existingCar.Make = car.Make;
            existingCar.Model = car.Model;
            existingCar.Year = car.Year;
            existingCar.Color = car.Color;
            existingCar.PricePerDay = car.PricePerDay;
            existingCar.AvailabilityStatus = car.AvailabilityStatus;
            existingCar.Description = car.Description;

            _context.SaveChanges();
            return "Car updated successfully";
        }

        public string DeleteCar(int id)
        {
            var existingCar = _context.Cars.Find(id);
            if (existingCar == null) return "Car not found";

            _context.Cars.Remove(existingCar);
            _context.SaveChanges();
            return "Car deleted successfully";
        }
    }
}
