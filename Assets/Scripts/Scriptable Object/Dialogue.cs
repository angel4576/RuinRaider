using UnityEngine;

[CreateAssetMenu]
public class Dialogue : ScriptableObject
{
    public string dialogueName;

    [TextArea]
    public string[] sentences;
}
