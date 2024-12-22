using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace AlphacamSDK
{
    public class LibraryManager
    {
        private static readonly string[] RequiredLibraries = new string[]
        {
            // Основные библиотеки ALPHACAM
            "ACAM.dll",
            "AcamAddIns.dll",
            "AcamAddInsInterface.dll",
            "AcamDBWrapper.dll",
            "AcamDBWrapper.tlb",
            "AlgoInterface.dll",
            "GeoTools.dll",
            "GeoToolsCOM.dll",
            "Primitives.dll",
            
            // Библиотеки автоматизации
            "ACAMAutomation.dll",
            "LoadAddIn.dll",
            "LoadAddIn.tlb",
            "NestUtilities.dll",
            "Analytics.dll",
            "AnalyticsCLR.dll",
            
            // Интерфейсные библиотеки
            "Interop.AlphaCAMFeature.dll",
            "Interop.AlphaCAMGeoUtilities.dll",
            "Interop.AlphaCAMMill.dll",
            "Interop.AlphaCAMNesting.dll",
            "Interop.AlphaCAMParametric.dll",
            "Interop.AlphaCAMPrimitives.dll",
            
            // Дополнительные компоненты
            "AcamAddInLoader32.dll",
            "AcamAddInsUtils.dll",
            "AcamWPFInterface.dll",
            "AcamWPFInterface.tlb",
            "Feature.dll",
            "HullLibrary.dll",
            "MinimumBBox.dll",
            "ApplicationInsightsHelper.dll"
        };

        private Dictionary<string, Assembly> _loadedLibraries;
        private string _alphacamPath;
        private string _sdkPath;
        private bool _isInitialized;

        public LibraryManager(string alphacamPath, string sdkPath)
        {
            _alphacamPath = alphacamPath;
            _sdkPath = sdkPath;
            _loadedLibraries = new Dictionary<string, Assembly>();
            _isInitialized = false;
        }

        public bool Initialize()
        {
            try
            {
                // Создаем директорию для библиотек в SDK если её нет
                string sdkLibPath = Path.Combine(_sdkPath, "lib");
                if (!Directory.Exists(sdkLibPath))
                {
                    Directory.CreateDirectory(sdkLibPath);
                }

                // Копируем и загружаем все необходимые библиотеки
                foreach (string library in RequiredLibraries)
                {
                    string sourcePath = Path.Combine(_alphacamPath, library);
                    string targetPath = Path.Combine(sdkLibPath, library);

                    // Проверяем существование исходной библиотеки
                    if (!File.Exists(sourcePath))
                    {
                        Console.WriteLine($"Предупреждение: Библиотека {library} не найдена в {sourcePath}");
                        continue;
                    }

                    // Копируем библиотеку если её нет в SDK или она устарела
                    if (!File.Exists(targetPath) || 
                        File.GetLastWriteTime(sourcePath) > File.GetLastWriteTime(targetPath))
                    {
                        File.Copy(sourcePath, targetPath, true);
                    }

                    // Загружаем библиотеку
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(targetPath);
                        _loadedLibraries[library] = assembly;
                        Console.WriteLine($"Библиотека {library} успешно загружена");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при загрузке библиотеки {library}: {ex.Message}");
                    }
                }

                _isInitialized = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при инициализации LibraryManager: {ex.Message}");
                return false;
            }
        }

        public Assembly GetLibrary(string libraryName)
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("LibraryManager не инициализирован");
            }

            if (_loadedLibraries.ContainsKey(libraryName))
            {
                return _loadedLibraries[libraryName];
            }

            throw new FileNotFoundException($"Библиотека {libraryName} не загружена");
        }

        public bool IsLibraryLoaded(string libraryName)
        {
            return _loadedLibraries.ContainsKey(libraryName);
        }

        public List<string> GetLoadedLibraries()
        {
            return new List<string>(_loadedLibraries.Keys);
        }

        public void UnloadLibraries()
        {
            _loadedLibraries.Clear();
            _isInitialized = false;
        }

        public bool ValidateLibraries()
        {
            bool allValid = true;
            foreach (string library in RequiredLibraries)
            {
                if (!IsLibraryLoaded(library))
                {
                    Console.WriteLine($"Отсутствует необходимая библиотека: {library}");
                    allValid = false;
                }
            }
            return allValid;
        }

        public string GetLibraryVersion(string libraryName)
        {
            if (IsLibraryLoaded(libraryName))
            {
                try
                {
                    Assembly assembly = _loadedLibraries[libraryName];
                    return assembly.GetName().Version.ToString();
                }
                catch
                {
                    return "Версия неизвестна";
                }
            }
            return "Библиотека не загружена";
        }

        public Dictionary<string, string> GetAllLibraryVersions()
        {
            var versions = new Dictionary<string, string>();
            foreach (var library in _loadedLibraries)
            {
                versions[library.Key] = GetLibraryVersion(library.Key);
            }
            return versions;
        }
    }
}
