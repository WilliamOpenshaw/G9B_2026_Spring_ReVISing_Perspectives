using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class UIParticleFix : MonoBehaviour
{
    void Start()
    {
        // This forces the particles to render on top of UI elements
        ParticleSystem sys = GetComponent<ParticleSystem>();
        sys.GetComponent<Renderer>().sortingLayerName = "UI";
        sys.GetComponent<Renderer>().sortingOrder = 100; // High number so it stays on top!
    }
}