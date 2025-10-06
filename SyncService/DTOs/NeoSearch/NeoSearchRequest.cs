namespace SyncService.DTOs.NeoSearch;

/// <summary>
/// Запрос для поиска объектов по различным критериям.
/// Поддерживает фильтрацию по имени, диаметру, статусу опасности и пагинацию.
/// </summary>
public class NeoSearchRequest
{
    /// <summary>
    /// Поиск по имени объекта (частичное совпадение)
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Минимальный диаметр для фильтрации (в метрах)
    /// </summary>
    public double? MinDiameter { get; set; }
    
    /// <summary>
    /// Максимальный диаметр для фильтрации (в метрах)
    /// </summary>
    public double? MaxDiameter { get; set; }
    
    /// <summary>
    /// Фильтр по статусу опасности
    /// </summary>
    public bool? IsHazardous { get; set; }
    
    /// <summary>
    /// Максимальное количество результатов (опционально)
    /// </summary>
    public int? Limit { get; set; }
    
    /// <summary>
    /// Номер страницы для пагинации (начинается с 1)
    /// </summary>
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// Размер страницы для пагинации
    /// </summary>
    public int PageSize { get; set; } = 50;
}