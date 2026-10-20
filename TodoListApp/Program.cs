using System;
using System.Text;

namespace TodoListApp
{
    internal class Program
    {
        private const int InitialCapacity = 2;
        private const int CapacityMultiplier = 2;
        private const int CommandPartsLimit = 2;

        private static string[] todos = new string[InitialCapacity];
        private static bool[] statuses = new bool[InitialCapacity];
        private static DateTime[] dates = new DateTime[InitialCapacity];
        private static int todoCount = 0;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("Работу выполнили Илиадис и Дедкова");

            string firstName = ReadValue("Введите имя:");
            string lastName = ReadValue("Введите фамилию:");
            int birthYear = ReadBirthYear();

            Console.WriteLine("\nДобро пожаловать в TodoList!");
            Console.WriteLine("Введите help для списка команд.");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                string[] parts = input.Split(
                    ' ',
                    CommandPartsLimit,
                    StringSplitOptions.RemoveEmptyEntries);

                string command = parts[0].ToLowerInvariant();

                switch (command)
                {
                    case "help":
                        ShowHelp();
                        break;

                    case "profile":
                        ShowProfile(firstName, lastName, birthYear);
                        break;

                    case "add":
                        AddTask(parts);
                        break;

                    case "view":
                        ViewTasks();
                        break;

                    case "done":
                        MarkTaskDone(parts);
                        break;

                    case "exit":
                        Console.WriteLine("До свидания!");
                        return;

                    default:
                        Console.WriteLine(
                            "Неизвестная команда. Введите help для списка команд.");
                        break;
                }
            }
        }

        private static string ReadValue(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        private static int ReadBirthYear()
        {
            while (true)
            {
                string value = ReadValue("Введите год рождения:");

                if (int.TryParse(value, out int birthYear))
                {
                    return birthYear;
                }

                Console.WriteLine("Ошибка: введите год числом.");
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("  help        — показать список команд");
            Console.WriteLine("  profile     — показать данные пользователя");
            Console.WriteLine("  add \"текст\" — добавить задачу");
            Console.WriteLine("  view        — показать все задачи");
            Console.WriteLine("  done <индекс> — отметить задачу выполненной");
            Console.WriteLine("  exit        — выйти из программы");
        }

        private static void ShowProfile(
            string firstName,
            string lastName,
            int birthYear)
        {
            Console.WriteLine($"{firstName} {lastName}, {birthYear}");
        }

        private static void AddTask(string[] parts)
        {
            if (parts.Length < CommandPartsLimit)
            {
                Console.WriteLine(
                    "Ошибка: укажите текст задачи в кавычках. Пример: add \"Купить хлеб\"");
                return;
            }

            if (!TryExtractQuotedText(parts[1], out string taskText))
            {
                Console.WriteLine("Ошибка: текст задачи должен быть в кавычках.");
                return;
            }

            EnsureCapacity();

            todos[todoCount] = taskText;
            statuses[todoCount] = false;
            dates[todoCount] = DateTime.Now;

            todoCount++;

            Console.WriteLine($"Задача добавлена: \"{taskText}\"");
        }

        private static void ViewTasks()
        {
            if (todoCount == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            Console.WriteLine("Ваши задачи:");

            for (int i = 0; i < todoCount; i++)
            {
                string status = statuses[i] ? "done" : "not done";

                Console.WriteLine(
                    $"{i} {todos[i]} {status} {dates[i]}");
            }
        }

        private static void MarkTaskDone(string[] parts)
        {
            if (parts.Length < CommandPartsLimit ||
                !int.TryParse(parts[1], out int index))
            {
                Console.WriteLine(
                    "Ошибка: укажите индекс задачи. Пример: done 0");
                return;
            }

            if (index < 0 || index >= todoCount)
            {
                Console.WriteLine("Ошибка: задачи с таким индексом нет.");
                return;
            }

            statuses[index] = true;
            dates[index] = DateTime.Now;

            Console.WriteLine($"Задача {index} отмечена как выполненная.");
        }

        private static bool TryExtractQuotedText(
            string rawText,
            out string text)
        {
            rawText = rawText.Trim();
            text = string.Empty;

            if (rawText.Length < 2 ||
                rawText[0] != '"' ||
                rawText[rawText.Length - 1] != '"')
            {
                return false;
            }

            text = rawText.Substring(1, rawText.Length - 2);
            return !string.IsNullOrWhiteSpace(text);
        }

        private static void EnsureCapacity()
        {
            if (todoCount < todos.Length)
            {
                return;
            }

            int newCapacity = todos.Length * CapacityMultiplier;

            string[] newTodos = new string[newCapacity];
            bool[] newStatuses = new bool[newCapacity];
            DateTime[] newDates = new DateTime[newCapacity];

            for (int i = 0; i < todoCount; i++)
            {
                newTodos[i] = todos[i];
                newStatuses[i] = statuses[i];
                newDates[i] = dates[i];
            }

            todos = newTodos;
            statuses = newStatuses;
            dates = newDates;

            Console.WriteLine($"Массивы расширены до {newCapacity} элементов.");
        }
    }
}