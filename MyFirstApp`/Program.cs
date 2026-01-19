using System;

// ===== КЛАСС САМОКАТ =====
class Scooter
{
    public double Battery { get; private set; } = 90;
    public int TotalMileage { get; private set; } = 450;
    public int BrakeMileage { get; private set; } = 180;

    public event Action LowBattery;
    public event Action ServiceRequired;
    public event Action BrakeCheckRequired;

    private Random rnd = new Random();

    public void Ride(int km)
    {
        for (int i = 1; i <= km; i++)
        {
            double расход = rnd.NextDouble() * (3 - 1.5) + 1.5;
            Battery -= расход;

            TotalMileage++;
            BrakeMileage++;

            Console.WriteLine($"Проехали 1 км | Заряд: {Battery:F1}% | Общий пробег: {TotalMileage} км");

            if (Battery < 20)
                LowBattery?.Invoke();

            if (TotalMileage % 500 == 0)
                ServiceRequired?.Invoke();

            if (BrakeMileage >= 200)
            {
                BrakeCheckRequired?.Invoke();
                BrakeMileage = 0;
            }
        }
    }

    public void Charge() => Battery = 100;
    public void Service() => Console.WriteLine("Самокат прошёл плановое обслуживание.");
}

// ===== КЛАСС КУРЬЕР =====
class Courier
{
    public void ChargeScooter(Scooter s)
    {
        s.Charge();
        Console.WriteLine("Курьер зарядил самокат до 100%");
    }

    public void DoService(Scooter s)
    {
        s.Service();
    }

    public void CheckBrakes()
    {
        Console.WriteLine("Курьер проверил тормоза.");
    }
}

// ===== ОСНОВНАЯ ПРОГРАММА =====
class Program
{
    static void Main()
    {
        Console.WriteLine("Начало смены курьера\n");

        Scooter scooter = new Scooter();
        Courier courier = new Courier();

        // Подписка на события (лямбда-выражения)
        scooter.LowBattery += () =>
        {
            Console.WriteLine("⚠ Событие: Низкий заряд батареи!");
            courier.ChargeScooter(scooter);
        };

        scooter.ServiceRequired += () =>
        {
            Console.WriteLine("🔧 Событие: Плановое обслуживание!");
            courier.DoService(scooter);
        };

        scooter.BrakeCheckRequired += () =>
        {
            Console.WriteLine("🛑 Событие: Проверка тормозов!");
            courier.CheckBrakes();
        };

        Random rnd = new Random();
        int distance = rnd.Next(30, 51);

        Console.WriteLine($"Курьер планирует проехать {distance} км\n");

        scooter.Ride(distance);

        Console.WriteLine("\nСмена завершена.");
        Console.ReadKey();
    }
}
