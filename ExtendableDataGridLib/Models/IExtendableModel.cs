using System;

namespace ExtendableDataGridLib.Models;

public interface IExtendableModel
{
    object this[string name] { get; set; }

    /// <summary>
    /// Получить массив нименований св-в.
    /// </summary>
    /// <returns> Массив наименований. </returns>
    string[] GetAllPropertyNames();
}