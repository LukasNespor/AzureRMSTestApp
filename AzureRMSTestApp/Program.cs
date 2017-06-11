using Microsoft.InformationProtectionAndControl;
using System;
using System.Collections.ObjectModel;
using System.Configuration;

namespace AzureRMSTestApp
{
    class Program
    {
        static Uri extranetUrl = new Uri(ConfigurationManager.AppSettings["ExtranetUrl"]);
        static ConnectionInfo conn;

        static void Main(string[] args)
        {
            Console.WriteLine("Initialization");
            SafeNativeMethods.IpcInitialize();
            SafeNativeMethods.IpcSetAPIMode(APIMode.Server);

            conn = new ConnectionInfo(extranetUrl, null);

            Console.WriteLine("1. Encrypt");
            Console.WriteLine("2. Decrypt");
            Console.Write("Choice: ");
            string choice = Console.ReadLine();

            Console.Write("File path: ");
            string filePath = Console.ReadLine();

            if (choice == "1")
                EncryptFile(filePath);
            else
                DecryptFile(filePath);
        }

        // Works fine
        static Collection<TemplateInfo> GetTemplates()
        {
            try
            {
                Console.WriteLine("Loading templates");
                var templates = SafeNativeMethods.IpcGetTemplateList(
                    connectionInfo: conn,
                    forceDownload: false,
                    suppressUI: true,
                    offline: false,
                    hasUserConsent: true,
                    parentForm: null,
                    cultureInfo: null,
                    symmKey: null);
                Console.WriteLine("  Loaded templates: {0}", templates.Count);

                return templates;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine("Error occured while loading of templates");
                Console.WriteLine(e.ToString());
                Console.ResetColor();
                return null;
            }
        }

        // Works fine
        static void DecryptFile(string filePath)
        {
            try
            {
                SafeFileApiNativeMethods.IpcfDecryptFile(
                    inputFile: filePath,
                    flags: SafeFileApiNativeMethods.DecryptFlags.IPCF_DF_FLAG_OPEN_AS_RMS_AWARE,
                    suppressUI: true, 
                    offline: false, 
                    hasUserConsent: true, 
                    parentForm: null, 
                    symmKey: null,
                    outputDirectory: null);
            }
            catch (InformationProtectionException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while decrtypting file");
                Console.WriteLine(e.ToString());
                Console.ResetColor();
            }
        }

        // Does not work
        static void EncryptFile(string filePath)
        {
            try
            {
                var templates = GetTemplates();
                var template = templates[0];

                SafeFileApiNativeMethods.IpcfEncryptFile(
                    inputFile: filePath,
                    templateId: template.TemplateId,
                    flags: SafeFileApiNativeMethods.EncryptFlags.IPCF_EF_FLAG_DEFAULT,
                    suppressUI: true,
                    offline: false,
                    hasUserConsent: true,
                    parentForm: null,
                    symmKey: null,
                    outputDirectory: null);
            }
            catch (InformationProtectionException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine("Error occured while encrtypting file");
                Console.WriteLine(e.ToString());
                Console.ResetColor();
            }
        }
    }
}