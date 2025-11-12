using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScrewColorData", menuName = "NutColorData/ScrewColorData")]
public class ScrewColorData : ScriptableObject
{
    public List<PipeColorEntry> ScrewColor= new List<PipeColorEntry>();
    public Color GetColor(ScrewColors pipeColor)
    {
        foreach (var entry in ScrewColor)
        {
            if (entry.ColorName == pipeColor)
                return entry.ColorValue;
        }
        Debug.LogWarning("PipeColor not found! Using white.");
        return Color.white;
    }
}
[System.Serializable]
public class PipeColorEntry
{
    public ScrewColors ColorName;
    public Color ColorValue;
}
public enum ScrewColors
    {
        Red, Green, Blue, Orange, Pink, Brown, Yellow, Purple,White
    }
