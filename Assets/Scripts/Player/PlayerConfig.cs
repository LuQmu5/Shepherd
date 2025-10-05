using UnityEngine;

[CreateAssetMenu(menuName = "Static Data/Configs/Player", order = 54, fileName = "Player Config")]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement Settings")]
    [field: SerializeField] public float MovementSpeed = 10f;
    [field: SerializeField] public float Acceleration = 15f;
    [field: SerializeField] public float Deceleration = 15f;

    [Header("Jump Settings")]
    [field: SerializeField] public float JumpForce = 7f;
    [field: SerializeField] public LayerMask JumpLayers;

    [Header("Mouse Settings")]
    [field: SerializeField] public float VerticalLookLimit = 85f;
    [field: SerializeField] public float MouseSensitivity = 100f;

    [Header("Combat Settings")]
    [field: SerializeField] public float ShootPower = 10f;
    [field: SerializeField] public Vector3 RecoilForce = new Vector3(0, 3, -10);
}
