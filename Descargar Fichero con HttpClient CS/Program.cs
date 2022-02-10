//--------------------------------------------------------------------------------
// Descargar un fichero de un sitio web usando HttpClient        (10/Feb/22 19.25)
//
// Ejemplo basado en:
// https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient
//
// (c) Guillermo Som (Guille), 2022
//--------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Descargar_Fichero_con_HttpClient_CS
{
    class Program
    {
        /// <summary>
        /// El objeto HttpClient se recomiendo instanciarlo solo 1 vez en la aplicación.
        /// </summary>
        private readonly static System.Net.Http.HttpClient ClienteHttp = new();

        // En C# 7.1 y superior se puede usar Main como Task y async.
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            var ficWeb = "https://www.elguille.info/pruebaGuille.txt";
            var ficLocal = "prueba.txt";

            Console.WriteLine("Descargando {0}...", ficWeb);

            var res = await DownloadFileAsync(ficWeb, ficLocal);
            if (res)
            {
                Console.WriteLine("Descarga completada.");

                // Mostrar el contenido del fichero local.
                Process.Start("notepad", ficLocal);
            }

            Console.WriteLine();
            Console.WriteLine("Pulsa INTRO para finalizar.");



            // Las versiones de C# anteriores a 7.1 no pueden esperar en Main,
            // por tanto, el código anterior ponerlo en un método y llamarlo desde aquí
            // y esperar a que se termine todo...
            //descargar();

            Console.ReadLine();
        }

        /// <summary>
        /// Descarga el fichero indicado (url) y lo guarda en el fichero destino (usando HttpClient).
        /// </summary>
        /// <param name="ficWeb">El fichero a descargar (de una dirección URL).</param>
        /// <param name="ficDest">El fichero de destino, donde se guardará el descargado.</param>
        /// <returns>True o false según haya tenido éxito la descarga o no.</returns>
        public async static Task<bool> DownloadFileAsync(string ficWeb, string ficDest)
        {
            try
            {
                // Simplificando la descarga.
                var contenido = await ClienteHttp.GetByteArrayAsync(ficWeb);
                // Si se ha podido descargar.
                if (contenido != null && contenido.Length > 0)
                {
                    // Guardarlo en el fichero de destino.
                    // Si el fichero destino existe, se sobreescribe.
                    using System.IO.FileStream fs = new(ficDest, System.IO.FileMode.Create, 
                                                                 System.IO.FileAccess.Write, 
                                                                 System.IO.FileShare.None);
                    await fs.WriteAsync(contenido.AsMemory(0, contenido.Length));
                }
                else
                {
                    Console.WriteLine("No se ha podido descargar.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Se ha producido un error al descargar o guardar.
                Console.WriteLine("Error: {0}", ex.Message);
                return false;
            }

            return true;
        }
    }
}
