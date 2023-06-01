
using System;

namespace PatternsSecond;

public class Program
{
    //Симулятор автомеханика. Паттерны: Строитель, Адаптер, Состояние
    public static void Main(string[] args)
    {
        //Создается новый строитель автомобилей марки Феррари
        CarBuilder builder = new FerrariBuilder();
        //Строится автомобиль с заданным набором частей.
        var car = builder.BuildMainParts().BuildExtraParts().GetResult();

        //Пример работы паттерна состояние
        car.StartEngine();
        car.Drive();
        car.TurnOffEngine();
        car.Drive();
        car.TurnOffEngine();
        car.StartEngine();
        car.StartEngine();
        car.TurnOffEngine();
        
        //Пример работы паттерна Адаптер.
        var shipService = new ShipService();
        var ship = new Ship();
        ship.Sail();

        //С помощью услуги перевозка на корабле, перевозим автомобиль на пароме.
        IShipTransport carOnShip = new CarToShipAdapter(car);
        carOnShip.Sail();

    }
}

public enum Part
{
    Wheels,
    Body,
    Glass,
    Electronics,
    Conditioner,
    Lights,
    PowerWindows,
    Radio,
    SeatAdjustment,
    Hatch,
    Wipers,
    Signalization,
    Armor
}

public interface ICarState
{
    void StartEngine(Car car);
    void TurnOffEngine(Car car);
    void Drive(Car car);
    void TravelShip(Car car);
}

public class OffEngineState : ICarState
{
    public void StartEngine(Car car)
    {
        Console.WriteLine("Двигатель запускается....");
        car.State = new WorkEngineState();
    }

    public void TurnOffEngine(Car car)
    {
        Console.WriteLine("Двигатель уже заглушен!");
    }

    public void Drive(Car car)
    {
        Console.WriteLine("Движение с заглушенным двигателем невозможно!");
    }

    public void TravelShip(Car car)
    {
        Console.WriteLine("Автомобиль передвигается на пароме...");
    }
}

public class WorkEngineState : ICarState
{
    public void StartEngine(Car car)
    {
        Console.WriteLine("Двигатель уже запущен!");
    }

    public void TurnOffEngine(Car car)
    {
        Console.WriteLine("Двигатель выключается...");
        car.State = new OffEngineState();
    }

    public void Drive(Car car)
    {
        Console.WriteLine("Автомобиль начинает движение...");
        car.State = new DriveState();
    }

    public void TravelShip(Car car)
    {
        Console.WriteLine("Автомобиль передвигается на пароме...");
    }
}

public class DriveState : ICarState
{
    public void StartEngine(Car car)
    {
        Console.WriteLine("Двигатель уже запущен!");
    }

    public void TurnOffEngine(Car car)
    {
        Console.WriteLine("Двигатель выключается...");
        car.State = new OffEngineState();
    }

    public void Drive(Car car)
    {
        Console.WriteLine("Автомобиль уже едет");
    }

    public void TravelShip(Car car)
    {
        Console.WriteLine("Автомобиль передвигается на пароме...");
    }
}


public class Car
{
    public string CarName { get; set; }
    public List<Part> CarParts { get; set; } = new List<Part>();
    
    public ICarState State { get; set; }

    public Car(ICarState ws)
    {
        State = ws;
    }


    public void StartEngine()
    {
        State.StartEngine(this);
    }

    public void TurnOffEngine()
    {
        State.TurnOffEngine(this);  
    }

    public void Drive()
    {
        State.Drive(this);
    }

    public void TravelShip()
    {
        State.TravelShip(this);
    }

}

public abstract class CarBuilder
{
    public abstract FerrariBuilder BuildMainParts();
    public abstract FerrariBuilder BuildExtraParts();
    public abstract FerrariBuilder BuildPremiumParts();
    public abstract Car GetResult();
}

public class FerrariBuilder : CarBuilder
{
    private Car car = new Car(new OffEngineState());
    public override FerrariBuilder BuildMainParts()
    {
        car.CarName = "Ferrari";
        car.CarParts.Add(Part.Body);
        car.CarParts.Add(Part.Wheels);
        car.CarParts.Add(Part.Lights);
        car.CarParts.Add(Part.Wipers);
        car.CarParts.Add(Part.Glass);
        car.CarParts.Add(Part.Electronics);
        return this;
    }

    public override FerrariBuilder BuildExtraParts()
    {
        car.CarParts.Add(Part.Signalization);
        car.CarParts.Add(Part.Radio);
        car.CarParts.Add(Part.SeatAdjustment);
        car.CarParts.Add(Part.PowerWindows);
        return this;
    }

    public override FerrariBuilder BuildPremiumParts()
    {
        car.CarParts.Add(Part.Conditioner);
        car.CarParts.Add(Part.Hatch);
        car.CarParts.Add(Part.Armor);
        return this;
    }

    public override Car GetResult()
    {
        return car;
    }
}

public interface IShipTransport
{
    public void Sail();
}

public class Ship : IShipTransport
{
    public void Sail()
    {
        Console.WriteLine("Корабль перевозит автомобили...");
    }
}

public class ShipService
{
    public void Travel(IShipTransport shipTransport)
    {
        shipTransport.Sail();
    }
}

public class CarToShipAdapter : IShipTransport
{
    private Car car;

    public CarToShipAdapter(Car car)
    {
        this.car = car;
    }

    public void Sail()
    {
        car.TravelShip();
    }
}
