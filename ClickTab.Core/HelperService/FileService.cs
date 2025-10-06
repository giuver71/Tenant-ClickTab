using ClickTab.Core.HelperClass;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ClickTab.Core.HelperService
{
    /// <summary>
    /// Servizio di helper per la gestione del caricamento, recupero e cancellazione dei file presenti o sul bucket AWS oppure sulla cartella di progetto
    /// </summary>
    public class FileService
    {
        private ConfigurationService _configService;
        public static readonly string PROJECT_FOLDER_BASE_PATH = "EqpFiles";
        public static readonly string PROJECT_FOLDER_SUB_PATH = "EqpDocuments";

        public FileService(ConfigurationService configService)
        {
            _configService = configService;
        }

        /// <summary>
        /// Costruisce il path del file all'interno del bucket (o della cartella di progetto) concatenando alla cartella HseFiles, l'ID
        /// della company passata come parametro e una guid che rappresenterà il nome univoco del file
        /// </summary>
        /// <param name="FK_Company"></param>
        /// <returns></returns>
        public string CreateFilePath(int FK_Company, string extension = null, bool isSubfolder = false)
        {
            string fileName = Guid.NewGuid().ToString();
            if (!string.IsNullOrEmpty(extension))
                fileName += "." + extension;

            return isSubfolder == false ? Path.Combine(PROJECT_FOLDER_BASE_PATH, FK_Company.ToString(), fileName) : Path.Combine(PROJECT_FOLDER_BASE_PATH, FK_Company.ToString(), PROJECT_FOLDER_SUB_PATH, fileName);
        }

        /// <summary>
        /// Effettua l'upload di un file passato come parametro o nel bucket di s3 o nella cartella di progetto, a seconda della configurazione
        /// del progetto.
        /// </summary>
        /// <param name="FileData">Array di byte contenente il file</param>
        /// <param name="FilePath">Path del file (non include il nome). In caso di upload su BucketS3 deve riportare il nome della chiave da attribuire al file all'interno del bucket</param>
        /// <returns>Restituisce TRUE se l'upload va a buon fine altrimenti restituisce false</returns>
        public bool UploadFile(byte[] FileData, string FilePath)
        {
            bool result = false;

            switch (_configService.FileStorageMode)
            {
                case FileStorageMode.ProjectFolder:
                    string fileFullPath = Path.Combine(Environment.CurrentDirectory, PROJECT_FOLDER_BASE_PATH, FilePath);

                    //Crea la directory, se necessario
                    string fileDirectory = Path.GetDirectoryName(fileFullPath);
                    if (!Directory.Exists(fileDirectory))
                        Directory.CreateDirectory(fileDirectory);

                    File.WriteAllBytes(fileFullPath, FileData);
                    result = true;
                    break;
            }


            return result;
        }

        /// <summary>
        /// Restituisce l'array di byte del file presente nel path richiesto dal parametro.
        /// Il file viene recuperato dal bucket AWS o dalla cartella di progetto in base alla configurazione dell'applicativo
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public byte[] GetFile(string FilePath)
        {
            byte[] fileData = null;


            switch (_configService.FileStorageMode)
            {
                case FileStorageMode.ProjectFolder:
                    string realFilePath = Path.Combine(Environment.CurrentDirectory, PROJECT_FOLDER_BASE_PATH, FilePath);
                    fileData = File.ReadAllBytes(realFilePath);
                    break;
            }


            return fileData;
        }

        /// <summary>
        /// Se il path punta a un file di tipo immagine (i formati supportati sono Bmp-Emf-Exif-Gif-Icon-Jpeg-MemoryBmp-Png-Tiff-Wmf)
        /// allora restituisce il base64 della thumbnail altrimenti restituisce null. Il peso del file sarà di pochi kb, utile 
        /// per recuperare l'anteprima delle immagini da visualizzare in una lista di allegati.
        /// </summary>
        /// <param name="FilePath">Stringa contenente il path completo del file da recuperare</param>
        /// <returns></returns>
        public string GetThumbnailImageBase64(string FilePath)
        {
            byte[] thumbnailData = GetThumbnailImage(FilePath);
            string base64String = thumbnailData != null ? Convert.ToBase64String(thumbnailData) : null;
            return base64String;
        }

        /// <summary>
        /// Se il path punta a un file di tipo immagine (i formati supportati sono Bmp-Emf-Exif-Gif-Icon-Jpeg-MemoryBmp-Png-Tiff-Wmf)
        /// allora restituisce il byte[] della thumbnail altrimenti restituisce null. Il peso del file sarà di pochi kb, utile 
        /// per recuperare l'anteprima delle immagini da visualizzare in una lista di allegati.
        /// </summary>
        /// <param name="FilePath">Stringa contenente il path completo del file da recuperare</param>
        /// <returns></returns>
        public byte[] GetThumbnailImage(string FilePath)
        {
            byte[] thumbnailData = null;

            switch (_configService.FileStorageMode)
            {
                case FileStorageMode.ProjectFolder:
                    try
                    {
                        Image image = Image.FromFile(FilePath);
                        int height = (int)((120 * image.Height) / image.Width);
                        Image thumb = image.GetThumbnailImage(120, height, () => false, IntPtr.Zero);
                        using (MemoryStream m = new MemoryStream())
                        {
                            thumb.Save(m, image.RawFormat);
                            thumbnailData = m.ToArray();
                        }
                    }
                    catch (Exception ex) { }
                    break;
            }

            return thumbnailData;
        }

        public bool DeleteFile(string FilePath)
        {
            bool result = false;

            switch (_configService.FileStorageMode)
            {
                case FileStorageMode.ProjectFolder:
                    File.Delete(FilePath);
                    result = true;
                    break;
            }

            return result;
        }

        public EqpZip CreateZip(List<EqpFile> filledDocuments, string zipName = null)
        {

            EqpZip response = new EqpZip();

            using (var zipStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (var zipItem in filledDocuments)
                    {
                        var entry = archive.CreateEntry(zipItem.Name + "." + zipItem.Extension, CompressionLevel.Optimal);
                        using (var entryStream = entry.Open())
                        using (var fileToCompressStream = new MemoryStream(zipItem.File))
                        {
                            fileToCompressStream.CopyTo(entryStream);
                        }
                    }
                }
                response.Zip = zipStream.ToArray();
                response.Name = zipName != null ? zipName : Guid.NewGuid().ToString() + ".zip";
            }

            return response;

        }

        /// <summary>
        /// Restituisce una stringa contenente il file HTML del template mail contenuto nella cartella TemplateMail del progetto Web
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetTemplateMail(string fileName)
        {
            string mailMessage = null;
            using (StreamReader reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "TemplateMail", fileName)))
            {
                mailMessage = reader.ReadToEnd();
            }

            return mailMessage;
        }
    }
}
