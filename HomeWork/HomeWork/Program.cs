using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

abstract class Animal
{
    public int Id { get; set; }
    public string Name { get; set; }

    public abstract string FoodRequired();
}

class Carnivore : Animal
{
    public Carnivore(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string FoodRequired()
    {
        return "Мясо";
    }
}

class Omnivore : Animal
{
    public Omnivore(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string FoodRequired()
    {
        return "Мясо и растения";
    }
}

class Herbivore : Animal
{
    public Herbivore(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string FoodRequired()
    {
        return "Растения";
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Animal> animals = new List<Animal>()
        {
            new Carnivore(1, "Тигр"),
            new Omnivore(2, "Медведь"),
            new Herbivore(3, "Олень"),
            new Carnivore(4, "Лев"),
            new Omnivore(5, "Енот"),
            new Herbivore(6, "Жирач")
        };

        // Сортировка по убыванию количества пищи и по алфавиту по имени
        var sortedAnimals = animals.OrderByDescending(a => a.FoodRequired().Length)
                                  .ThenBy(a => a.Name);

        // Вывод информации о животных
        Console.WriteLine("ID\tИмя\tТип\tЕда запрошена");
        foreach (var animal in sortedAnimals)
        {
            Console.WriteLine($"{animal.Id}\t{animal.Name}\t{animal.GetType().Name}\t{animal.FoodRequired()}");
        }

        // Вывод первых 5 имен животных
        Console.WriteLine("Первые 5 имен:");
        foreach (var animal in sortedAnimals.Take(5))
        {
            Console.WriteLine(animal.Name);
        }

        // Вывод последних 3 идентификаторов животных
        Console.WriteLine("Последние 3 IDs:");
        foreach (var animal in sortedAnimals.Skip(sortedAnimals.Count() - 3))
        {
            Console.WriteLine(animal.Id);
        }

        // Запись коллекции в файл
        using (StreamWriter file = new StreamWriter("animals.csv"))

        {
            file.WriteLine("ID,Имя,Тип,Еда запрошена");
            foreach (var animal in animals)
            {
                file.WriteLine($"{animal.Id},{animal.Name},{animal.GetType().Name},{animal.FoodRequired()}");
            }
        }

        // Чтение коллекции из файла
        try
        {
            List<Animal> animalsFromFile = new List<Animal>();
            using (StreamReader file = new StreamReader("animals.csv"))
            {
                file.ReadLine(); // пропускаем заголовок
                while (!file.EndOfStream)
                {
                    string[] fields = file.ReadLine().Split(',');
                    int id = int.Parse(fields[0]);
                    string name = fields[1];
                    string type = fields[2];
                    Animal animal;
                    switch (type)
                    {
                        case nameof(Carnivore):
                            animal = new Carnivore(id, name);
                            break;
                        case nameof(Omnivore):
                            animal = new Omnivore(id, name);
                            break;
                        case nameof(Herbivore):
                            animal = new Herbivore(id, name);
                            break;
                        default:
                            throw new InvalidDataException($"Неизвестный тип животного: {type}");
                    }
                    animalsFromFile.Add(animal);
                }
            }

            foreach (var animal in animalsFromFile)
            {
                Console.WriteLine($"{animal.Id}\t{animal.Name}\t{animal.GetType().Name}\t{animal.FoodRequired()}");
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Файл не найден!");
        }
        catch (InvalidDataException ex)
        {
            Console.WriteLine("Неверный формат файла!");
            Console.WriteLine(ex.Message);
        }
    }
}
