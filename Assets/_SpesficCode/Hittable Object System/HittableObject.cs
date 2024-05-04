using UnityEngine;

public class HittableObject : MonoBehaviour,IHitableObject
{
    public void Hit(Vector3 hitPos)
    {
        Debug.Log("Hit");
    }
}