// Class1.cs
using System;
using System.Linq;

public class Program
{
    static void Main(string[] args)
    {
        var tanks = GetTanks();
        var units = GetUnits();
        var factories = GetFactories();

        Console.WriteLine($"Количество резервуаров: {tanks.Length}, установок: {units.Length}");

        // Вывод всех резервуаров с именами установок и заводов
        Console.WriteLine("\nСписок всех резервуаров:");
        foreach (var tank in tanks)
        {
            var unit = FindUnit(units, tanks, tank.Name);
            var factory1 = FindFactory(factories, unit);
            Console.WriteLine($"Резервуар: {tank.Name}, Установка: {unit?.Name}, Завод: {factory1?.Name}");
        }

        var foundUnit = FindUnit(units, tanks, "Резервуар 2");
        var factory = FindFactory(factories, foundUnit);
        Console.WriteLine($"\nРезервуар 2 принадлежит установке {foundUnit.Name} и заводу {factory.Name}");

        var totalVolume = GetTotalVolume(tanks);
        Console.WriteLine($"\nОбщий объем резервуаров: {totalVolume}");

        // Поиск по наименованию
        Console.WriteLine("\nВведите название резервуара для поиска:");
        string searchName = Console.ReadLine();
        var foundTank = tanks.FirstOrDefault(t => t.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase));
        if (foundTank != null)
        {
            var foundUnitForTank = FindUnit(units, tanks, foundTank.Name);
            var foundFactoryForTank = FindFactory(factories, foundUnitForTank);
            Console.WriteLine($"Найден резервуар: {foundTank.Name}, Установка: {foundUnitForTank.Name}, Завод: {foundFactoryForTank.Name}");
        }
        else
        {
            Console.WriteLine("Резервуар с таким названием не найден.");
        }
    }

    public static Tank[] GetTanks()
    {
        return new Tank[]
        {
            new Tank { Id = 1, Name = "Резервуар 1", Description = "Надземный - вертикальный", Volume = 1500, MaxVolume = 2000, UnitId = 1 },
            new Tank { Id = 2, Name = "Резервуар 2", Description = "Надземный - горизонтальный", Volume = 2500, MaxVolume = 3000, UnitId = 1 },
            new Tank { Id = 3, Name = "Дополнительный резервуар 24", Description = "Надземный - горизонтальный", Volume = 3000, MaxVolume = 3000, UnitId = 2 },
            new Tank { Id = 4, Name = "Резервуар 35", Description = "Надземный - вертикальный", Volume = 3000, MaxVolume = 3000, UnitId = 2 },
            new Tank { Id = 5, Name = "Резервуар 47", Description = "Подземный - двустенный", Volume = 4000, MaxVolume = 5000, UnitId = 2 },
            new Tank { Id = 6, Name = "Резервуар 256", Description = "Подводный", Volume = 500, MaxVolume = 500, UnitId = 3 }
        };
    }

    public static Unit[] GetUnits()
    {
        return new Unit[]
        {
            new Unit { Id = 1, Name = "ГФУ-2", Description = "Газофракционирующая установка", FactoryId = 1 },
            new Unit { Id = 2, Name = "АВТ-6", Description = "Атмосферно-вакуумная трубчатка", FactoryId = 1 },
            new Unit { Id = 3, Name = "АВТ-10", Description = "Атмосферно-вакуумная трубчатка", FactoryId = 2 }
        };
    }

    public static Factory[] GetFactories()
    {
        return new Factory[]
        {
            new Factory { Id = 1, Name = "НП3№1", Description = "Первый нефтеперерабатывающий завод" },
            new Factory { Id = 2, Name = "НП3№2", Description = "Второй нефтеперерабатывающий завод" }
        };
    }

    public static Unit FindUnit(Unit[] units, Tank[] tanks, string tankName)
    {
        var tank = tanks.FirstOrDefault(t => t.Name == tankName);
        if (tank == null) return null;

        return units.FirstOrDefault(u => u.Id == tank.UnitId);
    }

    public static Factory FindFactory(Factory[] factories, Unit unit)
    {
        if (unit == null) return null;

        return factories.FirstOrDefault(f => f.Id == unit.FactoryId);
    }

    public static int GetTotalVolume(Tank[] tanks)
    {
        return tanks.Sum(t => t.Volume);
    }
}

/// <summary>
/// Установка
/// </summary>
public class Unit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int FactoryId { get; set; }
}

/// <summary>
/// Завод
/// </summary>
public class Factory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

/// <summary>
/// Резервуар
/// </summary>
public class Tank
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Volume { get; set; }
    public int MaxVolume { get; set; }
    public int UnitId { get; set; }
}