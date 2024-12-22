using System;
using System.IO;
using System.Collections.Generic;

namespace AlphacamSDK
{
    public class HelpManager
    {
        private readonly string[] HelpFiles = new string[]
        {
            // Основные файлы справки
            "ACAM4.chm",          // Основная справка ALPHACAM
            "ACAM4LK.chm",        // Справка по лицензированию
            "ACAMAPI.chm",        // Справка по API
            "AcamReports.chm",    // Справка по отчетам
            "AEdit3.chm",         // Справка по редактору
            "AEDITAPI.chm",       // Справка по API редактора
            "ConstraintsAPI.chm", // Справка по API ограничений
            "Feature.chm",        // Справка по функциям
            "ModuleWorks_-_Documentation.chm", // Документация ModuleWorks
            "primitives.chm",     // Справка по примитивам
            "R2V.chm"            // Справка по R2V
        };

        private readonly string _alphacamPath;
        private readonly string _sdkPath;
        private Dictionary<string, string> _helpPaths;

        public HelpManager(string alphacamPath, string sdkPath)
        {
            _alphacamPath = alphacamPath;
            _sdkPath = sdkPath;
            _helpPaths = new Dictionary<string, string>();
        }

        public bool Initialize()
        {
            try
            {
                Console.WriteLine("Инициализация системы справки...");

                // Создаем директорию для файлов справки в SDK
                string sdkHelpPath = Path.Combine(_sdkPath, "help");
                if (!Directory.Exists(sdkHelpPath))
                {
                    Directory.CreateDirectory(sdkHelpPath);
                }

                // Копируем файлы справки
                foreach (string helpFile in HelpFiles)
                {
                    string sourcePath = Path.Combine(_alphacamPath, helpFile);
                    string targetPath = Path.Combine(sdkHelpPath, helpFile);

                    if (File.Exists(sourcePath))
                    {
                        // Копируем только если файл не существует или более старый
                        if (!File.Exists(targetPath) || 
                            File.GetLastWriteTime(sourcePath) > File.GetLastWriteTime(targetPath))
                        {
                            File.Copy(sourcePath, targetPath, true);
                            Console.WriteLine($"Скопирован файл справки: {helpFile}");
                        }
                        _helpPaths[helpFile] = targetPath;
                    }
                    else
                    {
                        Console.WriteLine($"Предупреждение: Файл справки не найден: {helpFile}");
                    }
                }

                // Создаем индекс справки
                CreateHelpIndex(sdkHelpPath);

                Console.WriteLine("Система справки инициализирована успешно");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при инициализации системы справки: {ex.Message}");
                return false;
            }
        }

        private void CreateHelpIndex(string helpPath)
        {
            string indexPath = Path.Combine(helpPath, "help_index.html");
            using (StreamWriter writer = new StreamWriter(indexPath))
            {
                writer.WriteLine("<!DOCTYPE html>");
                writer.WriteLine("<html>");
                writer.WriteLine("<head>");
                writer.WriteLine("<title>ALPHACAM SDK Help Index</title>");
                writer.WriteLine("<style>");
                writer.WriteLine("body { font-family: Arial, sans-serif; margin: 20px; }");
                writer.WriteLine("h1 { color: #333; }");
                writer.WriteLine(".help-item { margin: 10px 0; padding: 10px; border: 1px solid #ddd; }");
                writer.WriteLine(".help-item:hover { background-color: #f5f5f5; }");
                writer.WriteLine("</style>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                writer.WriteLine("<h1>ALPHACAM SDK Help Index</h1>");

                foreach (var helpFile in _helpPaths)
                {
                    writer.WriteLine("<div class='help-item'>");
                    writer.WriteLine($"<h3>{Path.GetFileNameWithoutExtension(helpFile.Key)}</h3>");
                    writer.WriteLine($"<p>Path: {helpFile.Value}</p>");
                    writer.WriteLine($"<a href='{helpFile.Key}'>Open Help File</a>");
                    writer.WriteLine("</div>");
                }

                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }
        }

        public string GetHelpFilePath(string fileName)
        {
            return _helpPaths.ContainsKey(fileName) ? _helpPaths[fileName] : null;
        }

        public Dictionary<string, string> GetAllHelpFiles()
        {
            return new Dictionary<string, string>(_helpPaths);
        }

        public bool OpenHelp(string fileName)
        {
            try
            {
                string helpPath = GetHelpFilePath(fileName);
                if (helpPath != null && File.Exists(helpPath))
                {
                    System.Diagnostics.Process.Start(helpPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при открытии файла справки: {ex.Message}");
                return false;
            }
        }

        public bool OpenHelpIndex()
        {
            try
            {
                string indexPath = Path.Combine(_sdkPath, "help", "help_index.html");
                if (File.Exists(indexPath))
                {
                    System.Diagnostics.Process.Start(indexPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при открытии индекса справки: {ex.Message}");
                return false;
            }
        }

        public void GenerateHelpDocumentation()
        {
            try
            {
                string docsPath = Path.Combine(_sdkPath, "docs", "help");
                if (!Directory.Exists(docsPath))
                {
                    Directory.CreateDirectory(docsPath);
                }

                // Создаем markdown документацию
                string mdPath = Path.Combine(docsPath, "HELP_DOCUMENTATION.md");
                using (StreamWriter writer = new StreamWriter(mdPath))
                {
                    writer.WriteLine("# ALPHACAM Help Documentation");
                    writer.WriteLine("\n## Available Help Files\n");

                    foreach (var helpFile in _helpPaths)
                    {
                        writer.WriteLine($"### {Path.GetFileNameWithoutExtension(helpFile.Key)}");
                        writer.WriteLine($"- File: {helpFile.Key}");
                        writer.WriteLine($"- Path: {helpFile.Value}");
                        writer.WriteLine();
                    }

                    writer.WriteLine("\n## Using Help System\n");
                    writer.WriteLine("To access the help system, you can use the following methods:");
                    writer.WriteLine("```csharp");
                    writer.WriteLine("// Open specific help file");
                    writer.WriteLine("sdk.GetHelpManager().OpenHelp(\"ACAMAPI.chm\");");
                    writer.WriteLine("\n// Open help index");
                    writer.WriteLine("sdk.GetHelpManager().OpenHelpIndex();");
                    writer.WriteLine("```\n");
                }

                Console.WriteLine($"Документация по справке создана: {mdPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании документации: {ex.Message}");
            }
        }
    }
}
