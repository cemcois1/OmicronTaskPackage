using UnityEngine;

public interface IHitableObject 
{
    void Hit(Vector3 hitPos);
    void Droped(Vector3 hitPos);
}