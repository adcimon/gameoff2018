using System;
using UnityEngine;

[Serializable]
public class SpriteAnimation
{
    public string gameObjectName;
    public Vector3 position = Vector3.zero;
    public Vector3 rotation = Vector3.zero;
    public Vector3 scale = Vector3.one;
    public Sprite sprite;
    public int sortingOrder = 0;
    public bool ignoreOrder = false;
}