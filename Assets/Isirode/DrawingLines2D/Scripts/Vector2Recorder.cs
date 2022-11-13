using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FIXME : is it useful ?
public class Vector2Recorder<T>
{
    private List<T> list = new List<T>();

    public void Add(T t)
    {
        list.Add(t);
    }

    public List<T> Get()
    {
        return this.list;
    }

    public void Clear()
    {
        list.Clear();
    }
}
