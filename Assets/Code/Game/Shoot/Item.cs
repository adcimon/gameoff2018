using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Custom/Item", order = 2001)]
public class Item : ScriptableObject
{
    public string gameObjectName = "";
    public Material material;
    public string sortingLayerName = "";
    public List<SpriteAnimationGroup> spriteAnimationGroups = new List<SpriteAnimationGroup>();
}