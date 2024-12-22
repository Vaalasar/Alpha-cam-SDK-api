# ALPHACAM API SDK

## Описание
SDK для работы с ALPHACAM, предоставляющий унифицированный интерфейс для автоматизации и программного управления системой ALPHACAM.

## Структура проекта
```
D:\ALPHACAM API\
├── src/                    # Исходный код SDK
│   ├── AlphacamSDK.cs     # Основной класс SDK
│   └── Interfaces/         # Интерфейсы компонентов
│       ├── IAlphacamCore.cs       # Базовые операции
│       ├── IAlphacamGeometry.cs   # Геометрические операции
│       └── IAlphacamAutomation.cs # Автоматизация
├── bin/                    # Скомпилированные библиотеки
├── docs/                   # Документация
├── examples/               # Примеры использования
├── tests/                  # Тесты
└── sdk_config.json         # Конфигурация SDK
```

## Зависимости
- ALPHACAM 2024 (установленная версия)
- .NET Framework 4.8 или выше
- Пути к библиотекам:
  - C:\Program Files\Hexagon\ALPHACAM 2024
  - F:\LICOMDIR

## Основные компоненты

### 1. Core API (IAlphacamCore)
- Управление приложением
- Работа с документами
- Управление видом
- Работа со слоями
- Управление материалами
- Работа с инструментами
- Операции выделения
- Буфер обмена
- Отмена/повтор действий

### 2. Geometry API (IAlphacamGeometry)
- Создание базовых геометрических элементов
- Сложные геометрические операции
- Модификация геометрии
- Группировка элементов
- Измерения и анализ
- Проверки и валидация
- Преобразования
- Работа со слоями и свойствами

### 3. Automation API (IAlphacamAutomation)
- Управление макросами
- Пакетная обработка
- Управление очередью
- Автоматизация процессов
- Настройка автоматизации
- События и уведомления
- Логирование и отчеты

## Примеры использования

### Инициализация SDK
```csharp
using AlphacamSDK;

var sdk = SDKManager.Instance;
sdk.Initialize();
```

### Работа с геометрией
```csharp
var geometry = sdk.GetGeometryInterface();

// Создание прямоугольника
var rectId = geometry.CreateRectangle(0, 0, 100, 50);

// Поворот геометрии
geometry.RotateGeometry(rectId, 45, new Point3D(50, 25, 0));
```

### Автоматизация
```csharp
var automation = sdk.GetAutomationInterface();

// Выполнение макроса
var parameters = new Dictionary<string, object>
{
    { "param1", "value1" },
    { "param2", 42 }
};
automation.ExecuteMacro(@"F:\LICOMDIR\VBMacros\MyMacro.mac", parameters);
```

## Рекомендации по использованию

1. Инициализация:
   - Всегда проверяйте успешность инициализации SDK
   - Используйте корректные пути в конфигурации

2. Работа с геометрией:
   - Группируйте связанные операции
   - Используйте оптимизацию при работе с большим количеством объектов
   - Проверяйте валидность созданных объектов

3. Автоматизация:
   - Используйте обработку ошибок
   - Логируйте важные операции
   - Проверяйте статус выполнения длительных операций

4. Производительность:
   - Используйте пакетную обработку для множественных операций
   - Минимизируйте количество обращений к ALPHACAM
   - Освобождайте ресурсы после использования

## Безопасность

1. Проверка входных данных:
   - Валидация параметров
   - Проверка путей к файлам
   - Контроль типов данных

2. Управление доступом:
   - Использование безопасных методов доступа к файлам
   - Проверка прав доступа
   - Логирование критических операций

## Поддержка и обновления

- Регулярно проверяйте обновления SDK
- Следите за совместимостью с версией ALPHACAM
- Создавайте резервные копии важных файлов
- Документируйте изменения в проекте

## Дополнительная информация

Для получения более подробной информации обратитесь к документации в папке docs/ или свяжитесь с технической поддержкой.