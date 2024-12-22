using System;
using System.Collections.Generic;

namespace AlphacamSDK.Interfaces
{
    public interface IAlphacamGeometry
    {
        // Базовые геометрические операции
        int CreatePoint(double x, double y, double z = 0);
        int CreateLine(double x1, double y1, double x2, double y2, double z = 0);
        int CreateArc(double centerX, double centerY, double radius, double startAngle, double endAngle);
        int CreateCircle(double centerX, double centerY, double radius);
        int CreateRectangle(double x1, double y1, double x2, double y2);
        
        // Сложные геометрические операции
        int CreatePolygon(List<Point3D> points);
        int CreateSpline(List<Point3D> points, int degree = 3);
        int CreateText(string text, Point3D position, TextParameters parameters);
        
        // Модификация геометрии
        bool MoveGeometry(int geometryId, double dx, double dy, double dz = 0);
        bool RotateGeometry(int geometryId, double angle, Point3D center);
        bool ScaleGeometry(int geometryId, double scale, Point3D center);
        bool MirrorGeometry(int geometryId, Point3D start, Point3D end);
        bool OffsetGeometry(int geometryId, double distance, bool isInner = true);
        
        // Операции с группами
        int CreateGroup(List<int> geometryIds);
        bool AddToGroup(int groupId, int geometryId);
        bool RemoveFromGroup(int groupId, int geometryId);
        List<int> GetGroupMembers(int groupId);
        
        // Измерения и анализ
        double GetLength(int geometryId);
        double GetArea(int geometryId);
        BoundingBox GetBoundingBox(int geometryId);
        Point3D GetCentroid(int geometryId);
        
        // Проверки и валидация
        bool IsValid(int geometryId);
        bool Intersects(int geometry1Id, int geometry2Id);
        List<Point3D> GetIntersectionPoints(int geometry1Id, int geometry2Id);
        bool IsInside(int containerId, int geometryId);
        
        // Преобразования
        bool ExplodeGeometry(int geometryId);
        bool ConvertToPath(int geometryId);
        bool OptimizeGeometry(int geometryId, OptimizationParameters parameters);
        bool JoinGeometry(List<int> geometryIds);
        
        // Слои и свойства
        bool SetLayer(int geometryId, string layerName);
        string GetLayer(int geometryId);
        bool SetColor(int geometryId, Color color);
        Color GetColor(int geometryId);
        bool SetProperties(int geometryId, Dictionary<string, object> properties);
        Dictionary<string, object> GetProperties(int geometryId);
    }

    public struct Point3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point3D(double x, double y, double z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public struct BoundingBox
    {
        public Point3D MinPoint { get; set; }
        public Point3D MaxPoint { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
    }

    public class TextParameters
    {
        public string FontName { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Angle { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public TextAlignment Alignment { get; set; }
    }

    public enum TextAlignment
    {
        Left,
        Center,
        Right
    }

    public struct Color
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
    }

    public class OptimizationParameters
    {
        public double Tolerance { get; set; }
        public bool RemoveRedundantPoints { get; set; }
        public bool MergeColinearSegments { get; set; }
        public bool SimplifyArcs { get; set; }
        public Dictionary<string, object> AdditionalParameters { get; set; }
    }
}
