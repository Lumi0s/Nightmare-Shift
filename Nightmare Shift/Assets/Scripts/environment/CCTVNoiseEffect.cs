using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CCTVNoiseEffect : MonoBehaviour
{
    public Shader noiseShader; // Reference to the noise shader
    private Material _material; // The material used to apply the shader
    public float noiseAmount = 0.5f; // Amount of noise to apply
    public Texture2D noiseTexture; // Reference to the noise texture

    private Camera _camera;
    private float _timer;
    private Vector2 _noiseOffset;

    void Start()
    {
        // Get the camera component
        _camera = GetComponent<Camera>();

        // Create a material with the custom shader
        if (noiseShader != null)
        {
            _material = new Material(noiseShader);
        }
        else
        {
            Debug.Log("Noise Shader not assigned.");
            enabled = false;
            return;
        }

        // Set the initial noise parameters
        _material.SetFloat("_NoiseAmount", noiseAmount);

        // Assign the noise texture
        if (noiseTexture != null)
        {
            _material.SetTexture("_NoiseTex", noiseTexture);
        }
        else
        {
            Debug.LogError("Noise Texture not assigned.");
            enabled = false;
            return;
        }

        // Initialize the noise offset
        _noiseOffset = Vector2.zero;
        _material.SetVector("_NoiseOffset", new Vector4(_noiseOffset.x, _noiseOffset.y, 0, 0));
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Apply the noise shader to the camera feed
        if (_material != null)
        {
            // Pass the camera feed (source texture) and apply the noise effect
            Graphics.Blit(source, destination, _material);
        }
        else
        {
            // Just pass the source to destination without any effect if no material is set
            Graphics.Blit(source, destination);
        }
    }

    // Update noise parameters at runtime if needed
    void Update()
    {
        if (_material != null)
        {
            _material.SetFloat("_NoiseAmount", noiseAmount);

            // Update the noise offset every 0.1 seconds
            _timer += Time.deltaTime;
            if (_timer >= 0.1f)
            {
                _timer = 0f;
                _noiseOffset = new Vector2(Random.value, Random.value);
                _material.SetVector("_NoiseOffset", new Vector4(_noiseOffset.x, _noiseOffset.y, 0, 0));
            }
        }
    }

    // Cleanup when the script is destroyed
    void OnDestroy()
    {
        if (_material != null)
        {
            Destroy(_material);
        }
    }
}