using System.Collections.Generic;

namespace ExtendableDataGridLib.Models;

public class ExtendableModel : IExtendableModel
{
    private Dictionary<string, object> _properties = new();

    public object this[string name]
    {
        get
        {
            if (_properties.ContainsKey(name))
                return _properties[name];
            return null!;
        }
        set { _properties[name] = value; }
    }

    /// <inheritdoc/>
    public string[] GetAllPropertyNames()
    {
        var array = new string[_properties.Count];

        int index = 0;
        foreach (var keyValPair in _properties)
            array[index++] = keyValPair.Key;

        return array;
    }
}
