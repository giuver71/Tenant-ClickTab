using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ClickTab.Core.HelperService
{
    public static class ExceptionLogHelperService
    {
        static ReaderWriterLock locker = new ReaderWriterLock();

        /// <summary>
        /// Crea/modifica un file con i log degli errori specificando data e ora di quando si sono verificati e tutta l'eccezione generata. 
        /// Viene creato un file al giorno e ogni errore viene concatenato a quelli già esistenti.
        /// </summary>
        /// <param name="exception"></param>
        public static void CreateLog(Exception exception)
        {
            try
            {
                locker.AcquireWriterLock(int.MaxValue);

                string customLogPath = CheckLogFolder();

                // Creo il path del file di log giornaliero
                string logFilePath = customLogPath + "\\Log_" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";

                // Recupero il contenuto del file se già esiste
                List<string> logFile = ReadExistingLog(logFilePath);

                // Aggiunge il nuovo errore al file di log
                logFile.Add("ERROR: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ffffff") + "\n");
                logFile.Add(exception.ToString());
                logFile.Add("\n******************************************************\n");

                // Scrivo/aggiorno il file di log
                WriteLog(logFile, logFilePath);
            }
            finally
            {
                locker.ReleaseWriterLock();
            }

        }

        /// <summary>
        /// Controlla se esiste la cartella dei log e se serve la crea restituendo il path completo.
        /// </summary>
        /// <returns></returns>
        private static string CheckLogFolder()
        {
            string customLogPath = Path.Combine(Environment.CurrentDirectory, "CustomLogs");
            if (!Directory.Exists(customLogPath))
                Directory.CreateDirectory(customLogPath);
            return customLogPath;
        }

        /// <summary>
        /// Controlla se nella cartella dei log esiste un file creato nella data odierna.
        /// Se esiste lo legge e restituisce le sue righe in una lista di strnghe, altrimenti restituisce una lista vuota.
        /// </summary>
        /// <param name="logPath"></param>
        /// <returns></returns>
        private static List<string> ReadExistingLog(string logPath)
        {
            // Contenuto del file
            List<string> logFile = new List<string>();

            // Controlla se esiste un file di log del giorno odierno e nel caso ne legge il contenuto per poi aggiungere in coda il nuovo errore
            if (File.Exists(logPath))
            {
                string fileLine;
                StreamReader file = new StreamReader(logPath);
                while ((fileLine = file.ReadLine()) != null)
                {
                    logFile.Add(fileLine);
                }
                file.Close();
            }

            return logFile;
        }

        /// <summary>
        /// Scrive la lista di stringhe nel file a cui punta il path passato.
        /// </summary>
        /// <param name="logFile"></param>
        /// <param name="logFilePath"></param>
        private static void WriteLog(List<string> logFile, string logFilePath)
        {
            // Scrive il file di log creato o aggiornato
            using (StreamWriter sw = File.CreateText(logFilePath))
            {
                foreach (string line in logFile)
                {
                    sw.WriteLine(line);
                }

                sw.Close();
            }
        }
    }
}
