using System;

namespace TodoListApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Работу выполнили Илиадис и Дедкова");

            Console.WriteLine("Введите имя:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Введите фамилию:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Введите год рождения:");
            string birthYearText = Console.ReadLine();

            int birthYear = int.Parse(birthYearText);
                        // ===== Хранилище задач =====
            string[] todos = new string[2]; // Начальная ёмкость — 2 задачи
            int todoCount = 0;              // Счётчик реально добавленных задач

            Console.WriteLine("\nДобро пожаловать в TodoList!");
            Console.WriteLine("Введите help для списка команд.");

            // ===== Основной цикл команд =====
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                string command = parts[0].ToLower();

                switch (command)
                {
                    case "help":
                        Console.WriteLine("Доступные команды:");
                        Console.WriteLine("  help        — показать этот список");
                        Console.WriteLine("  profile     — показать данные пользователя");
                        Console.WriteLine("  add \"текст\" — добавить новую задачу");
                        Console.WriteLine("  view        — показать все задачи");
                        Console.WriteLine("  exit        — выйти из программы");
                        break;

                    case "profile":
                        // Используем переменные, которые ввёл пользователь
                        Console.WriteLine($"{firstName} {lastName}, {birthYear}");
                        break;

                    case "add":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Ошибка: укажите текст задачи в кавычках. Пример: add \"Купить хлеб\"");
                            break;
                        }

                        string rawTask = parts[1].Trim();

                        if (rawTask.Length < 2 || rawTask[0] != '"' || rawTask[rawTask.Length - 1] != '"')
                        {
                            Console.WriteLine("Ошибка: текст задачи должен быть в кавычках.");
                            break;
                        }

                        string taskText = rawTask.Substring(1, rawTask.Length - 2);

                        // Расширение массива при переполнении
                        if (todoCount >= todos.Length)
                        {
                            string[] newTodos = new string[todos.Length * 2];
                            for (int i = 0; i < todos.Length; i++)
                            {
                                newTodos[i] = todos[i];
                            }
                            todos = newTodos;
                            Console.WriteLine($"Массив расширен до {todos.Length} элементов.");
                        }

                        todos[todoCount] = taskText;
                        todoCount++;
                        Console.WriteLine($"Задача добавлена: \"{taskText}\"");
                        break;

                    case "view":
                        if (todoCount == 0)
                        {
                            Console.WriteLine("Список задач пуст.");
                        }
                        else
                        {
                            Console.WriteLine("Ваши задачи:");
                            for (int i = 0; i < todoCount; i++)
                            {
                                if (todos[i] != null)
                                    Console.WriteLine($"  {i + 1}. {todos[i]}");
                            }
                        }
                        break;

                    case "exit":
                        Console.WriteLine("До свидания!");
                        return;

                    default:
                        Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
                        break;
                }
            }
        }
    }
}