namespace Proyecto.PruebasCargaEstres
{
    public class Pruebas
    {
        private const int TotalUsers = 100;
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            var tasks = new Task[TotalUsers];

            for (int i = 0; i < TotalUsers; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    try
                    {
                        var response = await client.GetAsync("https://saludmental.somee.com/t");
                        Console.WriteLine($"Response: {response.StatusCode}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("Pruebas completadas.");
        }

    }
}
