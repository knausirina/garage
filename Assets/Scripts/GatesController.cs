using UnityEngine;

public class GatesController: MonoBehaviour
{
    [SerializeField] private Gate[] _gates;

    public void OpenGates()
    {
        foreach (var gate in _gates)
        {
            gate.Open();
        }
    }
}
