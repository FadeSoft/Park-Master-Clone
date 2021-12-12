using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    [Range(1, 200)]
    public float rayDistance;
    public LayerMask layer;
    public LineRenderer line;
    private int lineIndex = 0;
    public GameObject waypointSc;
    void Start()
    {
        //Açýlýþta linerenderer'in pozisyon sayýsýný sýfýr yapýyorum ki hata yaþamayalým.
        line.positionCount = 0;
    }
    void Update()
    {
        #region Raycast
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit, rayDistance, layer))
        {
            if (Input.GetMouseButton(0))
            {
                //Mouse'a týklanýnca veya parmak ekrana deðince bu kýsým çalýþýr.
                //LineIndex adlý deðiþkenimi her seferinde bir artýrarak LineRenderer'in positionCount'una eþitliyorum.
                //Yani o an LineRenderer'in lineIndex kadar pozisyonu oluyor.
                lineIndex++;
                line.positionCount = lineIndex;
                //lineIndex deðerimi önceden artýrdýðým için burda pozisyonu lýneIndex-1'e eþitliyorum.
                //Eðer lineIndex-1 yazmazsam linerenderer'in son pozisyonu 0,0,0 olup çizgi çizmemize olanak vermiyor. 38. satýrý en üste çekerek deneyebilirsiniz.

                //O anki linerenderer'imin lýneIndex'nci pozsiyonunu raycast kullnarak hit point'e eþitliyorum.
                line.SetPosition(lineIndex - 1, new Vector3(hit.point.x, 0.1f, hit.point.z));

            }
            if (Input.GetMouseButtonUp(0))
            {
                //Týklama veya dokunma iþlemi bitince önce line objemizdeki konumlarý arabalarda kullanabilmek için arabalarýn scriptindeki listeye atýyoruz.
                //Sonra yeni bir linerenderer oluþturup çizme iþlemine devam ediyoruz.
                //lineIndex'i sýfýrlama nedenim ikinci lineRendererýn pozisyonunun sýfýrdan baþlamasýný istemem.
                lineIndex = 0;
                AddPointToList();
                CreateNewLine();
            }
        }
        #endregion
    }

    #region LineRenderer'deki noktalarý listeye atama
    private void AddPointToList()
    {
        //LineRenderer'imizin pozisyonu sayýsý kadar sürecek bir döngü kullanýyoruz.
        for (int i = 0; i < line.positionCount - 1; i++)
        {
            //LineRenderer'in o anki iterasyon sayýsýndaki pozsiyonunu alýp daha sonra kullanmak için bir vektör'e atýyoruz.
            Vector3 a = line.GetPosition(i);
            //En baþta waypointSc'ye birinci arabadaki scripti atadýk.
            //Bu scriptteki positions adlý listeye üstte oluþturduðumuz vektör'ü tek tek atýyoruz.
            waypointSc.GetComponent<WayPoint>().positions.Add(a);
            //Ve sonuçta arabanýn takip edeceði pozisyonlar listesi oluþuyor.
        }
    }
    #endregion 

    #region Yeni Çizgi Oluþturma
    private void CreateNewLine()
    {
        //Burada dokunma veya týklama iþlemi bitince yeni bir linerenderer oluþturup pozisyonlarýný arabaya veriyorum.
        LineRenderer newLine = new GameObject().AddComponent<LineRenderer>();
        newLine.material.color = Color.yellow;
        newLine.numCornerVertices = 90;
        newLine.numCapVertices = 90;
        newLine.startWidth = .15f;
        newLine.endWidth = .15f;

        newLine.positionCount = 0;
        //Tanýmladýðýmýz linerenderer objesine oluþturduðumuz newLine'ý atýyorum ki farklý bir line objemiz olsun ve öncekinin devamý olmasýn.Yoksa çizgi kaldýðý yerden devam eder.

        line = newLine;
        //Linerenderer pozisyonlarýný arabalarýn içindeki listeye atýyorduk. Yeni linerenderer oluþturunca pozisyonlarýný ikinci arabaya atamak için
        //waypointSc'nin konumunu ikinci araba olarak deðiþtiriyorum.
        waypointSc = GameObject.FindGameObjectWithTag("Car2");
    }
    #endregion
}