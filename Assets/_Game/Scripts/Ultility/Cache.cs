using System;
using System.Collections.Generic;
using UnityEngine;

public class Cache : Singleton<Cache>
{
    private Dictionary<Collider, Character> _characterCache = new Dictionary<Collider, Character>();


    public bool TryGetCharacter(Collider col, out Character character) => _characterCache.TryGetValue(col, out character);

    public void RegisterCharacter(Collider col, Character character)
    {
        if (col != null && !_characterCache.ContainsKey(col))
        {
            _characterCache.Add(col, character);
        }
    }
}
