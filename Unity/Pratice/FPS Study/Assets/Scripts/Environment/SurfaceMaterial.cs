using UnityEngine;

public enum SurfaceType
{
    Concrete = 0,
    Dirt = 1,
    Wood = 2,
    Metal = 3,
    Water = 4,
}

/// <summary>
/// �ٴ�/�� �� Collider�� ���� ������Ʈ�� �����Ͽ�
/// '�� ǥ���� � �����ΰ�'�� �����ϴ� ������ �±� ������Ʈ.
/// </summary>
[DisallowMultipleComponent]
public class SurfaceMaterial : MonoBehaviour
{
    public SurfaceType surfaceType = SurfaceType.Concrete; // �� ������Ʈ�� ǥ�� Ÿ��.
}
