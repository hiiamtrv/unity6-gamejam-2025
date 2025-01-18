using UnityEngine;

[CreateAssetMenu(fileName = "ColorConfig", menuName = "Scriptable Objects/ColorConfig")]
public class ColorConfig : ScriptableObject
{
    public Color[] colors = new Color[12];
}
