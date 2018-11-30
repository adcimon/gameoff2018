using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sprite Animation Group", menuName = "Custom/Sprite Animation Group", order = 2000)]
public class SpriteAnimationGroup : ScriptableObject
{
    public new string name;
    public Vector3 eulerAngles = Vector3.zero;
    public List<SpriteAnimation> spriteAnimations = new List<SpriteAnimation>();
}