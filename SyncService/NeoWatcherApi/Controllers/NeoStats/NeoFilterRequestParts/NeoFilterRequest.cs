namespace SyncService.Api.NeoWatcher.NeoFilterRequestParts;

/// <summary>
/// Отдает данные об околоземных объектах.
/// Позволяет пользователю гибко фильтровать и агрегировать эти данные.
/// </summary>
public class NeoFilterRequest
{
    /// <summary>
    /// Начало периода
    /// </summary>
    public DateTime? From { get; set; }
    
    /// <summary>
    /// Конец периода
    /// </summary>
    public DateTime? To { get; set; }
    
    /// <summary>
    /// Только потенциально опасные
    /// </summary>
    public bool? IsHazardous { get; set; }
    
    /// <summary>
    /// Минимальный диаметр объекта (км)
    /// </summary>
    public double? MinDiameter { get; set; }
    
    /// <summary>
    /// Максимальный диаметр объекта (км)
    /// </summary>
    public double? MaxDiameter { get; set; }
    
    /// <summary>
    /// Сортировка: дата, число, масса
    /// </summary>
    public SortBy? SortBy { get; set; }
    
    /// <summary>
    /// Направление сортировки (asc, desc)
    /// </summary>
    public SortDirections? SortDirection { get; set; }
}