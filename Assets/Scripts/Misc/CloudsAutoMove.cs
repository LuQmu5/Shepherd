using UnityEngine;

public class CloudsAutoMove : MonoBehaviour
{
    [Header("Target Material")]
    [SerializeField] private Material cloudMaterial;

    [Header("Cloud movement speed")]
    [SerializeField] private Vector3 moveSpeed = new Vector3(0.01f, 0f, 0f);

    private float xShift, yShift, zShift;

    void Start()
    {
        if (cloudMaterial == null)
        {
            Renderer rend = GetComponent<Renderer>();
            if (rend != null)
                cloudMaterial = rend.material;
        }
    }

    void Update()
    {
        if (cloudMaterial == null) return;

        // накапливаем сдвиги
        xShift += moveSpeed.x * Time.deltaTime;
        yShift += moveSpeed.y * Time.deltaTime;
        zShift += moveSpeed.z * Time.deltaTime;

        // циклим значения (0..1)
        if (xShift > 1f) xShift -= 1f;
        if (yShift > 1f) yShift -= 1f;
        if (zShift > 1f) zShift -= 1f;

        // передаем в шейдер
        cloudMaterial.SetFloat("_XShift", xShift);
        cloudMaterial.SetFloat("_YShift", yShift);
        cloudMaterial.SetFloat("_ZShift", zShift);
    }
}
