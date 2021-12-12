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
        //A��l��ta linerenderer'in pozisyon say�s�n� s�f�r yap�yorum ki hata ya�amayal�m.
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
                //Mouse'a t�klan�nca veya parmak ekrana de�ince bu k�s�m �al���r.
                //LineIndex adl� de�i�kenimi her seferinde bir art�rarak LineRenderer'in positionCount'una e�itliyorum.
                //Yani o an LineRenderer'in lineIndex kadar pozisyonu oluyor.
                lineIndex++;
                line.positionCount = lineIndex;
                //lineIndex de�erimi �nceden art�rd���m i�in burda pozisyonu l�neIndex-1'e e�itliyorum.
                //E�er lineIndex-1 yazmazsam linerenderer'in son pozisyonu 0,0,0 olup �izgi �izmemize olanak vermiyor. 38. sat�r� en �ste �ekerek deneyebilirsiniz.

                //O anki linerenderer'imin l�neIndex'nci pozsiyonunu raycast kullnarak hit point'e e�itliyorum.
                line.SetPosition(lineIndex - 1, new Vector3(hit.point.x, 0.1f, hit.point.z));

            }
            if (Input.GetMouseButtonUp(0))
            {
                //T�klama veya dokunma i�lemi bitince �nce line objemizdeki konumlar� arabalarda kullanabilmek i�in arabalar�n scriptindeki listeye at�yoruz.
                //Sonra yeni bir linerenderer olu�turup �izme i�lemine devam ediyoruz.
                //lineIndex'i s�f�rlama nedenim ikinci lineRenderer�n pozisyonunun s�f�rdan ba�lamas�n� istemem.
                lineIndex = 0;
                AddPointToList();
                CreateNewLine();
            }
        }
        #endregion
    }

    #region LineRenderer'deki noktalar� listeye atama
    private void AddPointToList()
    {
        //LineRenderer'imizin pozisyonu say�s� kadar s�recek bir d�ng� kullan�yoruz.
        for (int i = 0; i < line.positionCount - 1; i++)
        {
            //LineRenderer'in o anki iterasyon say�s�ndaki pozsiyonunu al�p daha sonra kullanmak i�in bir vekt�r'e at�yoruz.
            Vector3 a = line.GetPosition(i);
            //En ba�ta waypointSc'ye birinci arabadaki scripti atad�k.
            //Bu scriptteki positions adl� listeye �stte olu�turdu�umuz vekt�r'� tek tek at�yoruz.
            waypointSc.GetComponent<WayPoint>().positions.Add(a);
            //Ve sonu�ta araban�n takip edece�i pozisyonlar listesi olu�uyor.
        }
    }
    #endregion 

    #region Yeni �izgi Olu�turma
    private void CreateNewLine()
    {
        //Burada dokunma veya t�klama i�lemi bitince yeni bir linerenderer olu�turup pozisyonlar�n� arabaya veriyorum.
        LineRenderer newLine = new GameObject().AddComponent<LineRenderer>();
        newLine.material.color = Color.yellow;
        newLine.numCornerVertices = 90;
        newLine.numCapVertices = 90;
        newLine.startWidth = .15f;
        newLine.endWidth = .15f;

        newLine.positionCount = 0;
        //Tan�mlad���m�z linerenderer objesine olu�turdu�umuz newLine'� at�yorum ki farkl� bir line objemiz olsun ve �ncekinin devam� olmas�n.Yoksa �izgi kald��� yerden devam eder.

        line = newLine;
        //Linerenderer pozisyonlar�n� arabalar�n i�indeki listeye at�yorduk. Yeni linerenderer olu�turunca pozisyonlar�n� ikinci arabaya atamak i�in
        //waypointSc'nin konumunu ikinci araba olarak de�i�tiriyorum.
        waypointSc = GameObject.FindGameObjectWithTag("Car2");
    }
    #endregion
}