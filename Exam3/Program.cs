using Microsoft.Data.SqlClient;


namespace Exam3
{
    class Program
    {
        // не забудьте свои данные ввести
        private static string cs = "Server=localhost;Database=OlympicsDB;User Id=sa;Password=ЗДЕСЬ ДОЛЖЕН БЫТЬ ВАШ ПАРОЛЬ ОТ БАНКОВСКОГО ПРИЛОЖЕНИЯ;TrustServerCertificate=True;";

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("1) Добавить данные");
                Console.WriteLine("2) Удалить данные");
                Console.WriteLine("3) Редактировать данные");
                Console.WriteLine("4) Показать статистику");
                Console.WriteLine("5) Вывести содержимое таблиц");
                Console.WriteLine("60 Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": AddData(); break;
                    case "2": DeleteData(); break;
                    case "3": EditData(); break;
                    case "4": ShowStatistics(); break;
                    case "5": ShowTables(); break;
                    case "6": return;
                    default: Console.WriteLine("Неверный выбор"); break;
                }
            }
        }
        
        static void ShowTables()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                Console.WriteLine("\n----------------------- Олимпиады -----------------------");
                SqlCommand cmd = new SqlCommand("SELECT * FROM Olympics", conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["OlympicId"]}) Год: {reader["Year"]}, Сезон: {reader["Season"]}, Страна: {reader["HostCountry"]}, Город: {reader["HostCity"]}");
                    }
                }

                Console.WriteLine("\n----------------------- Спорт -----------------------");
                cmd = new SqlCommand("SELECT * FROM Sports", conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["SportId"]}) Название: {reader["Name"]}");
                    }
                }
                
                Console.WriteLine("\n----------------------- Спортсмены -----------------------");
                cmd = new SqlCommand("SELECT * FROM Athletes", conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["AthleteId"]}) ФИО: {reader["FullName"]}, Страна: {reader["Country"]}, Дата рождения: {reader["BirthDate"]}, Фото: {reader["PhotoPath"]}");
                    }
                }

                Console.WriteLine("\n----------------------- События -----------------------");
                cmd = new SqlCommand("SELECT e.EventId, o.Year, o.HostCity, s.Name, e.ParticipantCount FROM Events e JOIN Olympics o ON e.OlympicId = o.OlympicId JOIN Sports s ON e.SportId = s.SportId", conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["EventId"]}) Год: {reader["Year"]}, Город: {reader["HostCity"]}, Спорт: {reader["Name"]}, Участников: {reader["ParticipantCount"]}");
                    }
                }
                
                Console.WriteLine("\n----------------------- Результаты -----------------------");
                cmd = new SqlCommand("SELECT r.ResultId, e.EventId, a.FullName, a.Country, s.Name, r.Medal FROM Results r JOIN Events e ON r.EventId = e.EventId JOIN Athletes a ON r.AthleteId = a.AthleteId JOIN Sports s ON e.SportId = s.SportId", conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["ResultId"]}) Событие: {reader["EventId"]}, Спортсмен: {reader["FullName"]} ({reader["Country"]}), Спорт: {reader["Name"]}, Медаль: {reader["Medal"]}");
                    }
                }
                Console.WriteLine();
            }
        }
        
        static void AddData()
        {
            Console.WriteLine("Что добавить? (1 - Олимпиада, 2 - Спортсмен, 3 - Вид спорта, 4 - Событие, 5 - Результат)");
            string choice = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand cmd;
                switch (choice)
                {
                    case "1": 
                        cmd = new SqlCommand("INSERT INTO Olympics (Year, Season, HostCountry, HostCity) VALUES (@Year, @Season, @HostCountry, @HostCity)", conn);
                        Console.Write("Год: "); cmd.Parameters.AddWithValue("@Year", int.Parse(Console.ReadLine()));
                        Console.Write("Сезон (Summer/Winter): "); cmd.Parameters.AddWithValue("@Season", Console.ReadLine());
                        Console.Write("Страна: "); cmd.Parameters.AddWithValue("@HostCountry", Console.ReadLine());
                        Console.Write("Город: "); cmd.Parameters.AddWithValue("@HostCity", Console.ReadLine());
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Готово");
                        break;

                    case "2": 
                        cmd = new SqlCommand("INSERT INTO Athletes (FullName, Country, BirthDate, PhotoPath) VALUES (@FullName, @Country, @BirthDate, @PhotoPath)", conn);
                        Console.Write("ФИО: ");
                        cmd.Parameters.AddWithValue("@FullName", Console.ReadLine());
                        Console.Write("Страна: ");
                        cmd.Parameters.AddWithValue("@Country", Console.ReadLine());
                        Console.Write("Дата рождения: ");
                        cmd.Parameters.AddWithValue("@BirthDate", DateTime.Parse(Console.ReadLine()).Date);
                        Console.Write("Фото: ");
                        cmd.Parameters.AddWithValue("@PhotoPath", Console.ReadLine());
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Готово");
                        break;

                    case "3":
                        cmd = new SqlCommand("INSERT INTO Sports (Name) VALUES (@Name)", conn);
                        Console.Write("Название вида спорта: ");
                        cmd.Parameters.AddWithValue("@Name", Console.ReadLine());
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Готово");
                        break;

                    case "4":
                        cmd = new SqlCommand("INSERT INTO Events (OlympicId, SportId, ParticipantCount) VALUES (@OlympicId, @SportId, @ParticipantCount)", conn);
                        Console.Write("ID Олимпиады: ");
                        cmd.Parameters.AddWithValue("@OlympicId", int.Parse(Console.ReadLine()));
                        Console.Write("ID Вида спорта: ");
                        cmd.Parameters.AddWithValue("@SportId", int.Parse(Console.ReadLine()));
                        Console.Write("Количество участников: ");
                        cmd.Parameters.AddWithValue("@ParticipantCount", int.Parse(Console.ReadLine()));
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Готово");
                        break;

                    case "5":
                        cmd = new SqlCommand("INSERT INTO Results (EventId, AthleteId, Medal) VALUES (@EventId, @AthleteId, @Medal)", conn);
                        Console.Write("ID События: ");
                        cmd.Parameters.AddWithValue("@EventId", int.Parse(Console.ReadLine()));
                        Console.Write("ID Спортсмена: ");
                        cmd.Parameters.AddWithValue("@AthleteId", int.Parse(Console.ReadLine()));
                        Console.Write("Медаль (Gold/Silver/Bronze или     ): "); 
                        string medalInput = Console.ReadLine();
                        
                        if (string.IsNullOrEmpty(medalInput))
                            cmd.Parameters.AddWithValue("@Medal", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@Medal", medalInput);
                        
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Готово");
                        break;

                    default:
                        Console.WriteLine("Не попал!");
                        break;
                }
            }
        }
        static void DeleteData()
        {
            Console.WriteLine("Что удалить? (1 - Олимпиада, 2 - Спортсмен)");
            string choice = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand cmd;
                switch (choice)
                {
                    case "1":
                        cmd = new SqlCommand("DELETE FROM Olympics WHERE OlympicId = @Id", conn);
                        Console.Write("ID олимпиады: ");
                        cmd.Parameters.AddWithValue("@Id", int.Parse(Console.ReadLine()));
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Готово");
                        break;

                    case "2":
                        cmd = new SqlCommand("DELETE FROM Athletes WHERE AthleteId = @Id", conn);
                        Console.Write("ID спортсмена: ");
                        cmd.Parameters.AddWithValue("@Id", int.Parse(Console.ReadLine()));
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Готово");
                        break;
                }
            }
        }
        
        static void EditData()
        {
            Console.WriteLine("Что редактировать? (1 - Олимпиада, 2 - Спортсмен)");
            string choice = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand cmd;
                switch (choice)
                {
                    case "1":
                        cmd = new SqlCommand("UPDATE Olympics SET Year = @Year, Season = @Season, HostCountry = @HostCountry, HostCity = @HostCity WHERE OlympicId = @Id", conn);
                        Console.Write("ID олимпиады: ");
                        cmd.Parameters.AddWithValue("@Id", int.Parse(Console.ReadLine()));
                        Console.Write("Новый год: ");
                        cmd.Parameters.AddWithValue("@Year", int.Parse(Console.ReadLine()));
                        Console.Write("Новый сезон (Summer/Winter): ");
                        cmd.Parameters.AddWithValue("@Season", Console.ReadLine());
                        Console.Write("Новая страна: ");
                        cmd.Parameters.AddWithValue("@HostCountry", Console.ReadLine());
                        Console.Write("Новый город: ");
                        cmd.Parameters.AddWithValue("@HostCity", Console.ReadLine());
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Готово");
                        break;

                    case "2":
                        cmd = new SqlCommand("UPDATE Athletes SET FullName = @FullName, Country = @Country, BirthDate = @BirthDate, PhotoPath = @PhotoPath WHERE AthleteId = @Id", conn);
                        Console.Write("ID спортсмена: ");
                        cmd.Parameters.AddWithValue("@Id", int.Parse(Console.ReadLine()));
                        Console.Write("Новое ФИО: ");
                        cmd.Parameters.AddWithValue("@FullName", Console.ReadLine());
                        Console.Write("Новая страна: ");
                        cmd.Parameters.AddWithValue("@Country", Console.ReadLine());
                        Console.Write("Новая дата рождения: ");
                        cmd.Parameters.AddWithValue("@BirthDate", DateTime.Parse(Console.ReadLine()).Date);
                        Console.Write("Фото: ");
                        cmd.Parameters.AddWithValue("@PhotoPath", Console.ReadLine());
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Готово");
                        break;
                }
            }
        }
        static void ShowStatistics()
        {
            Console.WriteLine("1) Медальный зачет по странам");
            Console.WriteLine("2) Медалисты по видам спорта");
            Console.WriteLine("3) Страна с максимумом золотых медалей");
            Console.WriteLine("4) Спортсмен с максимумом золотых медалей");
            Console.WriteLine("5) Страна-хозяйка чаще всех");
            Console.WriteLine("6) Состав команды страны");
            Console.WriteLine("7) Статистика страны");
            string choice = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand cmd;

                switch (choice)
                {
                    case "1":
                        Console.Write("ID олимпиады (0 для всей истории): ");
                        int olympicId = int.Parse(Console.ReadLine());
                        cmd = new SqlCommand(
                            @"SELECT a.Country, 
                                     SUM(CASE WHEN r.Medal = 'Gold' THEN 1 ELSE 0 END) as Gold,
                                     SUM(CASE WHEN r.Medal = 'Silver' THEN 1 ELSE 0 END) as Silver,
                                     SUM(CASE WHEN r.Medal = 'Bronze' THEN 1 ELSE 0 END) as Bronze
                              FROM Results r
                              JOIN Athletes a ON r.AthleteId = a.AthleteId
                              JOIN Events e ON r.EventId = e.EventId
                              " + (olympicId > 0 ? "WHERE e.OlympicId = @OlympicId" : "") +
                              @" GROUP BY a.Country
                              ORDER BY Gold DESC, Silver DESC, Bronze DESC", conn);
                        if (olympicId > 0) cmd.Parameters.AddWithValue("@OlympicId", olympicId);
                        break;

                    case "2":
                        Console.Write("ID олимпиады (0 для всей истории): ");
                        olympicId = int.Parse(Console.ReadLine());
                        cmd = new SqlCommand(
                            @"SELECT s.Name, a.FullName, r.Medal
                              FROM Results r
                              JOIN Athletes a ON r.AthleteId = a.AthleteId
                              JOIN Events e ON r.EventId = e.EventId
                              JOIN Sports s ON e.SportId = s.SportId
                              " + (olympicId > 0 ? "WHERE e.OlympicId = @OlympicId" : "") +
                              @" AND r.Medal IS NOT NULL", conn);
                        if (olympicId > 0) cmd.Parameters.AddWithValue("@OlympicId", olympicId);
                        break;

                    case "3":
                        Console.Write("ID олимпиады (0 для всей истории): ");
                        olympicId = int.Parse(Console.ReadLine());
                        cmd = new SqlCommand(
                            @"SELECT TOP 1 a.Country, COUNT(*) as GoldCount
                              FROM Results r
                              JOIN Athletes a ON r.AthleteId = a.AthleteId
                              JOIN Events e ON r.EventId = e.EventId
                              " + (olympicId > 0 ? "WHERE e.OlympicId = @OlympicId" : "") +
                              @" AND r.Medal = 'Gold'
                              GROUP BY a.Country
                              ORDER BY GoldCount DESC", conn);
                        if (olympicId > 0)
                            cmd.Parameters.AddWithValue("@OlympicId", olympicId);
                        break;

                    case "4":
                        Console.Write("Название вида спорта: ");
                        string sportName = Console.ReadLine();
                        cmd = new SqlCommand(
                            @"SELECT TOP 1 a.FullName, a.Country, s.Name, COUNT(*) as GoldCount
                              FROM Results r
                              JOIN Athletes a ON r.AthleteId = a.AthleteId
                              JOIN Events e ON r.EventId = e.EventId
                              JOIN Sports s ON e.SportId = s.SportId
                              WHERE r.Medal = 'Gold' AND s.Name = @SportName
                              GROUP BY a.FullName, a.Country, s.Name
                              ORDER BY GoldCount DESC", conn);
                        cmd.Parameters.AddWithValue("@SportName", sportName);
                        break;

                    case "5":
                        cmd = new SqlCommand(
                            @"SELECT TOP 1 HostCountry, COUNT(*) as HostCount
                              FROM Olympics
                              GROUP BY HostCountry
                              ORDER BY HostCount DESC", conn);
                        break;

                    case "6":
                        Console.Write("Название страны: ");
                        string country = Console.ReadLine();
                        cmd = new SqlCommand(
                            @"SELECT a.FullName, s.Name
                              FROM Athletes a
                              JOIN Results r ON a.AthleteId = r.AthleteId
                              JOIN Events e ON r.EventId = e.EventId
                              JOIN Sports s ON e.SportId = s.SportId
                              WHERE a.Country = @Country", conn);
                        cmd.Parameters.AddWithValue("@Country", country);
                        break;

                    case "7":
                        Console.Write("Название страны: ");
                        country = Console.ReadLine();
                        Console.Write("ID Олимпиады ( 0 для всей истории): ");
                        olympicId = int.Parse(Console.ReadLine());
                        cmd = new SqlCommand(
                            @"SELECT a.Country, 
                                     COUNT(DISTINCT e.OlympicId) as OlympicsCount,
                                     SUM(CASE WHEN r.Medal = 'Gold' THEN 1 ELSE 0 END) as Gold,
                                     SUM(CASE WHEN r.Medal = 'Silver' THEN 1 ELSE 0 END) as Silver,
                                     SUM(CASE WHEN r.Medal = 'Bronze' THEN 1 ELSE 0 END) as Bronze
                              FROM Results r
                              JOIN Athletes a ON r.AthleteId = a.AthleteId
                              JOIN Events e ON r.EventId = e.EventId
                              WHERE a.Country = @Country" +
                              (olympicId > 0 ? " AND e.OlympicId = @OlympicId" : "") +
                              @" GROUP BY a.Country", conn);
                        cmd.Parameters.AddWithValue("@Country", country);
                        if (olympicId > 0)
                            cmd.Parameters.AddWithValue("@OlympicId", olympicId);
                        break;

                    default:
                        Console.WriteLine("Неверный выбор");
                        return;
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        switch (choice)
                        {
                            case "1":
                                Console.WriteLine($"{reader["Country"]}: Gold={reader["Gold"]}, Silver={reader["Silver"]}, Bronze={reader["Bronze"]}");
                                break;
                            case "2":
                                Console.WriteLine($"{reader["Name"]} - {reader["FullName"]} ({reader["Medal"]})");
                                break;
                            case "3":
                                Console.WriteLine($"{reader["Country"]}: {reader["GoldCount"]} золотых");
                                break;
                            case "4":
                                Console.WriteLine($"{reader["FullName"]} ({reader["Country"]}): {reader["GoldCount"]} золотых в {reader["Name"]}");
                                break;
                            case "5":
                                Console.WriteLine($"{reader["HostCountry"]}: {reader["HostCount"]} раз");
                                break;
                            case "6":
                                Console.WriteLine($"{reader["FullName"]} - {reader["Name"]}");
                                break;
                            case "7":
                                Console.WriteLine($"{reader["Country"]}: Участий={reader["OlympicsCount"]}, Gold={reader["Gold"]}, Silver={reader["Silver"]}, Bronze={reader["Bronze"]}");
                                break;
                        }
                    }
                }
            }
        }
    }
}