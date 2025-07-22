using System.Globalization;
using Microsoft.Extensions.Logging;

namespace ACPR.Helper;

public class LoggerHelper
{
    private string? _cheminFichierLog;
    private static readonly ILogger Logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("ACPR-DORA");

    private static LoggerHelper? _instance;

    private LoggerHelper() {}

    /// <summary>
    /// Assurez vous d'appeler <see cref="InitialiserLog"></see> afin d'avoir un fichier de log valide
    /// </summary>
    public static LoggerHelper Instance
    {
        get
        {
            _instance ??= new LoggerHelper();
            return _instance;
        }
    }

    /// <summary>
    /// Quitte le programme en cours
    /// </summary>
    /// <param name="ex">Exception stoppant le programme</param>
    /// <param name="message">Message d'erreur si différent du message d'exception</param>
    public void QuitterProgramme(Exception ex)
    {
        Log(ex.ToString(),LogLevel.Error);
        System.Environment.Exit(1);
    }

    /// <summary>
    /// Envoie des messages de logs au fichier logs ainsi qu'au terminal
    /// </summary>
    /// <param name="message">message de log</param>
    /// <param name="niveau">niveau de log</param>
    public void Log(string message, LogLevel niveau)
    {
        using FileStream fileStream = new(_cheminFichierLog!, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
        using StreamWriter log = new(fileStream);

        var date = DateTime.Now;
        log.WriteLine($"{date.ToString("F", CultureInfo.CurrentCulture)} {niveau} : {message}");
        Logger.Log(niveau, "{message}",message);
    }

    /// <summary>
    /// Supprime le fichier de logs pour le vider
    /// </summary>
    public void NettoyerLog()
    {
        File.Delete(_cheminFichierLog!);
    }

    /// <summary>
    /// Initialise le fichier de log
    /// </summary>
    /// <param name="cheminFichier">où se trouve le fichier logs.log</param>
    public void InitialiserLog(string cheminFichier)
    {
        _cheminFichierLog = cheminFichier;
    }
}