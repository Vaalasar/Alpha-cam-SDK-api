using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;
using AlphacamSDK.Interfaces;

namespace AlphacamSDK
{
    public class SDKManager
    {
        private static SDKManager _instance;
        private SDKConfiguration _config;
        private LibraryManager _libraryManager;
        private HelpManager _helpManager;
        private bool _isInitialized;

        // Интерфейсы для работы с ALPHACAM
        private IAlphacamCore _coreInterface;
        private IAlphacamGeometry _geometryInterface;
        private IAlphacamAutomation _automationInterface;

        public static SDKManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SDKManager();
                }
                return _instance;
            }
        }

        private SDKManager()
        {
            _isInitialized = false;
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sdk_config.json");
            if (File.Exists(configPath))
            {
                string jsonConfig = File.ReadAllText(configPath);
                _config = JsonConvert.DeserializeObject<SDKConfiguration>(jsonConfig);
            }
            else
            {
                throw new FileNotFoundException("SDK configuration file not found");
            }
        }

        public bool Initialize()
        {
            try
            {
                Console.WriteLine("Инициализация SDK...");

                // Инициализация менеджера библиотек
                _libraryManager = new LibraryManager(
                    _config.Paths.Installation,
                    _config.Paths.Sdk.Root
                );

                if (!_libraryManager.Initialize())
                {
                    throw new Exception("Ошибка инициализации менеджера библиотек");
                }

                // Инициализация менеджера справки
                _helpManager = new HelpManager(
                    _config.Paths.Installation,
                    _config.Paths.Sdk.Root
                );

                if (!_helpManager.Initialize())
                {
                    throw new Exception("Ошибка инициализации системы справки");
                }

                // Генерация документации по справке
                _helpManager.GenerateHelpDocumentation();

                // Проверка наличия всех необходимых библиотек
                if (!_libraryManager.ValidateLibraries())
                {
                    throw new Exception("Не все необходимые библиотеки доступны");
                }

                // Вывод информации о версиях библиотек
                var versions = _libraryManager.GetAllLibraryVersions();
                Console.WriteLine("\nВерсии загруженных библиотек:");
                foreach (var version in versions)
                {
                    Console.WriteLine($"{version.Key}: {version.Value}");
                }

                // Инициализация интерфейсов
                InitializeInterfaces();

                _isInitialized = true;
                Console.WriteLine("SDK успешно инициализирован");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при инициализации SDK: {ex.Message}");
                return false;
            }
        }

        private void InitializeInterfaces()
        {
            // Здесь происходит создание экземпляров интерфейсов
            // с использованием загруженных библиотек
        }

        public IAlphacamCore GetCoreInterface()
        {
            CheckInitialization();
            return _coreInterface;
        }

        public IAlphacamGeometry GetGeometryInterface()
        {
            CheckInitialization();
            return _geometryInterface;
        }

        public IAlphacamAutomation GetAutomationInterface()
        {
            CheckInitialization();
            return _automationInterface;
        }

        public HelpManager GetHelpManager()
        {
            CheckInitialization();
            return _helpManager;
        }

        private void CheckInitialization()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("SDK не инициализирован. Вызовите метод Initialize()");
            }
        }

        public void Shutdown()
        {
            try
            {
                if (_isInitialized)
                {
                    // Освобождение ресурсов интерфейсов
                    _coreInterface = null;
                    _geometryInterface = null;
                    _automationInterface = null;

                    // Выгрузка библиотек
                    _libraryManager.UnloadLibraries();

                    _isInitialized = false;
                    Console.WriteLine("SDK успешно завершил работу");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при завершении работы SDK: {ex.Message}");
                throw;
            }
        }

        public string GetVersion()
        {
            return "1.0.0"; // Версия SDK
        }

        public Dictionary<string, string> GetLoadedLibraries()
        {
            CheckInitialization();
            return _libraryManager.GetAllLibraryVersions();
        }

        public bool IsLibraryLoaded(string libraryName)
        {
            CheckInitialization();
            return _libraryManager.IsLibraryLoaded(libraryName);
        }

        public string GetLibraryVersion(string libraryName)
        {
            CheckInitialization();
            return _libraryManager.GetLibraryVersion(libraryName);
        }

        // Методы для работы со справкой
        public bool OpenHelp(string helpFileName)
        {
            CheckInitialization();
            return _helpManager.OpenHelp(helpFileName);
        }

        public bool OpenHelpIndex()
        {
            CheckInitialization();
            return _helpManager.OpenHelpIndex();
        }

        public Dictionary<string, string> GetAllHelpFiles()
        {
            CheckInitialization();
            return _helpManager.GetAllHelpFiles();
        }
    }

    public class SDKConfiguration
    {
        public string SdkVersion { get; set; }
        public string AlphacamVersion { get; set; }
        public PathConfiguration Paths { get; set; }
    }

    public class PathConfiguration
    {
        public string Installation { get; set; }
        public string UserData { get; set; }
        public SDKPathConfiguration Sdk { get; set; }
    }

    public class SDKPathConfiguration
    {
        public string Root { get; set; }
        public string Bin { get; set; }
        public string Include { get; set; }
        public string Lib { get; set; }
        public string Docs { get; set; }
        public string Samples { get; set; }
        public string Tests { get; set; }
    }
}
