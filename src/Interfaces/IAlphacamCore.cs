using System;
using System.Collections.Generic;

namespace AlphacamSDK.Interfaces
{
    public interface IAlphacamCore
    {
        // Управление приложением
        bool Initialize();
        bool Shutdown();
        string GetVersion();
        bool IsInitialized { get; }
        
        // Управление документами
        bool OpenDocument(string path);
        bool SaveDocument(string path);
        bool SaveDocumentAs(string path, SaveFormat format);
        bool CloseDocument(bool saveChanges = true);
        bool NewDocument(DocumentParameters parameters = null);
        string GetCurrentDocument();
        
        // Управление видом
        bool ZoomExtents();
        bool ZoomWindow(double x1, double y1, double x2, double y2);
        bool ZoomPrevious();
        bool Pan(double dx, double dy);
        bool Rotate3D(double angleX, double angleY, double angleZ);
        
        // Управление слоями
        int CreateLayer(string name, LayerProperties properties = null);
        bool DeleteLayer(string name);
        bool SetCurrentLayer(string name);
        string GetCurrentLayer();
        List<string> GetAllLayers();
        bool SetLayerProperties(string name, LayerProperties properties);
        LayerProperties GetLayerProperties(string name);
        
        // Управление материалами
        bool SetMaterial(MaterialParameters parameters);
        MaterialParameters GetMaterial();
        List<string> GetAvailableMaterials();
        bool ImportMaterial(string path);
        bool ExportMaterial(string name, string path);
        
        // Управление инструментами
        bool SelectTool(string toolName);
        bool ConfigureTool(ToolParameters parameters);
        List<string> GetAvailableTools();
        ToolParameters GetToolParameters(string toolName);
        bool ImportTool(string path);
        bool ExportTool(string name, string path);
        
        // Операции с выделением
        bool SelectObject(int objectId);
        bool SelectObjects(List<int> objectIds);
        bool SelectByLayer(string layerName);
        bool SelectByProperties(SelectionCriteria criteria);
        bool ClearSelection();
        List<int> GetSelectedObjects();
        
        // Операции с буфером обмена
        bool Cut();
        bool Copy();
        bool Paste(double x = 0, double y = 0);
        bool Delete();
        
        // Операции отмены/повтора
        bool Undo();
        bool Redo();
        bool BeginUndoGroup(string description);
        bool EndUndoGroup();
        
        // Настройки и конфигурация
        bool SetApplicationSetting(string key, object value);
        object GetApplicationSetting(string key);
        bool SaveSettings(string path);
        bool LoadSettings(string path);
    }

    public enum SaveFormat
    {
        Default,
        DXF,
        DWG,
        IGES,
        STEP,
        XML
    }

    public class DocumentParameters
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public string Units { get; set; }
        public string Template { get; set; }
        public Dictionary<string, object> CustomParameters { get; set; }
    }

    public class LayerProperties
    {
        public bool Visible { get; set; }
        public bool Locked { get; set; }
        public Color Color { get; set; }
        public double LineWeight { get; set; }
        public string LineType { get; set; }
        public int PrintOrder { get; set; }
        public Dictionary<string, object> CustomProperties { get; set; }
    }

    public class MaterialParameters
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Thickness { get; set; }
        public string Grade { get; set; }
        public Dictionary<string, double> Dimensions { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }

    public class ToolParameters
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Diameter { get; set; }
        public double Length { get; set; }
        public double StepOver { get; set; }
        public double StepDown { get; set; }
        public double FeedRate { get; set; }
        public double PlungeRate { get; set; }
        public double SpindleSpeed { get; set; }
        public Dictionary<string, object> CustomParameters { get; set; }
    }

    public class SelectionCriteria
    {
        public List<string> Layers { get; set; }
        public List<string> Types { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public bool IncludeNested { get; set; }
        public bool ExactMatch { get; set; }
    }
}
