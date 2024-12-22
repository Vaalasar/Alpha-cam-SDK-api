using System;
using System.Collections.Generic;
using AlphacamSDK;
using AlphacamSDK.Interfaces;

namespace AlphacamSDK.Examples
{
    public class BasicExample
    {
        public static void Main()
        {
            try
            {
                Console.WriteLine("Запуск примера использования ALPHACAM SDK...\n");

                // Инициализация SDK
                var sdk = SDKManager.Instance;
                if (!sdk.Initialize())
                {
                    throw new Exception("Ошибка инициализации SDK");
                }

                // Вывод информации о загруженных библиотеках
                Console.WriteLine("\nЗагруженные библиотеки:");
                var libraries = sdk.GetLoadedLibraries();
                foreach (var lib in libraries)
                {
                    Console.WriteLine($"- {lib.Key}: {lib.Value}");
                }

                // Получение интерфейсов
                var core = sdk.GetCoreInterface();
                var geometry = sdk.GetGeometryInterface();
                var automation = sdk.GetAutomationInterface();

                // Работа со справочной системой
                Console.WriteLine("\nРабота со справочной системой:");
                var helpFiles = sdk.GetAllHelpFiles();
                Console.WriteLine("Доступные файлы справки:");
                foreach (var helpFile in helpFiles)
                {
                    Console.WriteLine($"- {helpFile.Key}");
                }

                // Открытие справки по API
                Console.WriteLine("\nОткрытие справки по API...");
                sdk.OpenHelp("ACAMAPI.chm");

                // Создание нового документа
                Console.WriteLine("\nСоздание нового документа...");
                var docParams = new DocumentParameters
                {
                    Width = 1000,
                    Height = 800,
                    Units = "mm"
                };
                core.NewDocument(docParams);

                // Создание геометрии
                Console.WriteLine("\nСоздание базовой геометрии...");

                // Создание прямоугольника
                var rectId = geometry.CreateRectangle(100, 100, 300, 200);
                Console.WriteLine($"Создан прямоугольник (ID: {rectId})");
                
                // Создание окружности
                var circleId = geometry.CreateCircle(200, 150, 50);
                Console.WriteLine($"Создана окружность (ID: {circleId})");

                // Группировка объектов
                var groupId = geometry.CreateGroup(new List<int> { rectId, circleId });
                Console.WriteLine($"Создана группа объектов (ID: {groupId})");

                // Применение трансформаций
                geometry.RotateGeometry(groupId, 45, new Point3D(200, 150, 0));
                Console.WriteLine("Применен поворот к группе объектов");

                // Работа со слоями
                Console.WriteLine("\nНастройка слоев...");
                var layerProps = new LayerProperties
                {
                    Color = new Color(255, 0, 0),
                    LineWeight = 0.5,
                    Visible = true
                };
                core.CreateLayer("MyLayer", layerProps);
                core.SetCurrentLayer("MyLayer");

                // Настройка материала
                Console.WriteLine("\nНастройка материала...");
                var material = new MaterialParameters
                {
                    Name = "Aluminum",
                    Type = "Metal",
                    Thickness = 10,
                    Grade = "6061",
                    Properties = new Dictionary<string, object>
                    {
                        { "Density", 2.7 },
                        { "YoungModulus", 69 }
                    }
                };
                core.SetMaterial(material);

                // Выполнение макроса
                Console.WriteLine("\nВыполнение макроса...");
                var macroParams = new Dictionary<string, object>
                {
                    { "ToolDiameter", 10 },
                    { "CuttingDepth", 5 },
                    { "FeedRate", 1000 }
                };

                // Настройка обработчиков событий автоматизации
                HandleAutomationEvents(automation);

                // Выполнение макроса
                automation.ExecuteMacro(@"F:\LICOMDIR\VBMacros\ProcessGeometry.mac", macroParams);

                // Сохранение результата
                Console.WriteLine("\nСохранение результатов...");
                core.SaveDocument(@"D:\ALPHACAM API\examples\output\example_result.amb");

                // Открытие индекса справки
                Console.WriteLine("\nОткрытие индекса справки...");
                sdk.OpenHelpIndex();

                // Вывод информации о геометрии
                Console.WriteLine("\nИнформация о созданных объектах:");
                ShowGeometryInfo(geometry, rectId);
                ShowGeometryInfo(geometry, circleId);

                Console.WriteLine("\nПример успешно выполнен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        public static void ShowGeometryInfo(IAlphacamGeometry geometry, int geometryId)
        {
            var bbox = geometry.GetBoundingBox(geometryId);
            var length = geometry.GetLength(geometryId);
            var area = geometry.GetArea(geometryId);

            Console.WriteLine($"\nИнформация о геометрии {geometryId}:");
            Console.WriteLine($"Размеры: {bbox.Width}x{bbox.Height}");
            Console.WriteLine($"Длина: {length}");
            Console.WriteLine($"Площадь: {area}");
        }

        public static void HandleAutomationEvents(IAlphacamAutomation automation)
        {
            automation.OnProcessStarted += (sender, e) =>
            {
                Console.WriteLine($"\nПроцесс {e.ProcessId} начат:");
                Console.WriteLine($"Время: {e.Timestamp}");
                Console.WriteLine($"Сообщение: {e.Message}");
            };

            automation.OnProcessCompleted += (sender, e) =>
            {
                Console.WriteLine($"\nПроцесс {e.ProcessId} завершен:");
                Console.WriteLine($"Время: {e.Timestamp}");
                Console.WriteLine($"Сообщение: {e.Message}");
                
                if (e.AdditionalData != null)
                {
                    Console.WriteLine("Дополнительная информация:");
                    foreach (var item in e.AdditionalData)
                    {
                        Console.WriteLine($"{item.Key}: {item.Value}");
                    }
                }
            };

            automation.OnError += (sender, e) =>
            {
                Console.WriteLine($"\nОшибка в процессе {e.ProcessId}:");
                Console.WriteLine($"Время: {e.Timestamp}");
                Console.WriteLine($"Сообщение: {e.Message}");
                if (e.Error != null)
                {
                    Console.WriteLine($"Исключение: {e.Error.Message}");
                    Console.WriteLine($"Stack Trace: {e.Error.StackTrace}");
                }
            };
        }
    }
}
