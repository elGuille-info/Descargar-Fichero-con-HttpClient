'--------------------------------------------------------------------------------
' Descargar un fichero de un sitio web usando HttpClient        (10/Feb/22 19.05)
'
' Ejemplo basado en:
' https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient
'
' (c) Guillermo Som (Guille), 2022
'--------------------------------------------------------------------------------
Imports System
Imports System.Diagnostics
Imports System.Threading.Tasks

Module Program

    ''' <summary>
    ''' El objeto HttpClient se recomiendo instanciarlo solo 1 vez en la aplicación.
    ''' </summary>
    Private ReadOnly ClienteHttp As New System.Net.Http.HttpClient()

    Sub Main(args As String())
        'Console.WriteLine("Hello World!")

        ' Como en VB no se puede esperar en Main,
        ' hacer el trabajo asíncrono en otro método y esperar a que se termine todo...
        descargar()

        Console.ReadLine()
    End Sub

    Private Async Sub descargar()
        Dim ficWeb = "https://www.elguille.info/pruebaGuille.txt"
        Dim ficLocal = "prueba.txt"

        Console.WriteLine("Descargando {0}...", ficWeb)

        Dim res = Await DownloadFileAsync(ficWeb, ficLocal)
        If res Then
            Console.WriteLine("Descarga completada.")

            ' Mostrar el contenido del fichero local.
            Process.Start("notepad", ficLocal)
        End If

        Console.WriteLine()
        Console.WriteLine("Pulsa INTRO para finalizar.")
    End Sub

    ''' <summary>
    ''' Descarga el fichero indicado (url) y lo guarda en el fichero destino (usando HttpClient).
    ''' </summary>
    ''' <param name="ficWeb">El fichero a descargar (de una dirección URL).</param>
    ''' <param name="ficDest">El fichero de destino, donde se guardará el descargado.</param>
    ''' <returns>True o false según haya tenido éxito la descarga o no.</returns>
    Public Async Function DownloadFileAsync(ficWeb As String, ficDest As String) As Task(Of Boolean)
        Try
            ' Simplificando la descarga.
            Dim contenido = Await ClienteHttp.GetByteArrayAsync(ficWeb)
            ' Si se ha podido descargar.
            If contenido IsNot Nothing AndAlso contenido.Length > 0 Then
                ' Guardarlo en el fichero de destino.
                ' Si el fichero destino existe, se sobreescribe.
                Using fs As New System.IO.FileStream(ficDest, System.IO.FileMode.Create,
                                                              System.IO.FileAccess.Write,
                                                              System.IO.FileShare.None)
                    Await fs.WriteAsync(contenido.AsMemory(0, contenido.Length))
                End Using
            Else
                Console.WriteLine("No se ha podido descargar.")
                Return False
            End If
        Catch ex As Exception
            ' Se ha producido un error al descargar o guardar.
            Console.WriteLine("Error: {0}", ex.Message)
            Return False
        End Try

        Return True
    End Function

End Module
