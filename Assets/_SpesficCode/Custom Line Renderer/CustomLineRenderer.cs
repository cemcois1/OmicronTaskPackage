using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomLineRenderer : MonoBehaviour
{
    [SerializeField] private List<Transform> Nodes;

    public void DrawLine(Vector3 beltPos, Vector3 TargetPos)
    {
        // Başlangıç noktasını listenin başına ekle
        Nodes[0].transform.position = beltPos;
        Nodes[0].gameObject.SetActive(false);
        // Başlangıç ve hedef noktaları arasındaki mesafeyi hesapla
        float distance = Vector3.Distance(beltPos, TargetPos);

        // Aralıkları belirlemek için mesafeyi nokta sayısına böl
        float interval = distance / (Nodes.Count - 1);

        // Her bir aralıkta bir nokta oluştur
        for (int i = 1; i < Nodes.Count - 1; i++)
        {
            // Yeni noktanın pozisyonunu hesapla
            Vector3 newPos = Vector3.Lerp(beltPos, TargetPos, interval * i / distance);
            
            Nodes[i].transform.position = newPos;
        }

        // Hedef noktayı listenin son elemanı yap
        Nodes[Nodes.Count - 1].position = TargetPos;
    }

}
