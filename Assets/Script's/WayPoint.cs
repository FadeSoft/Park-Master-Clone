using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WayPoint : MonoBehaviour
{
    public enum Tag { Red, Yellow };
    public Tag currentSel;
    [Range(1, 200)]
    public float movementSpeed, rotationSped;

    public List<Vector3> positions;
    public bool move;
    public Vector3 currentPos;
    public int y = 0;
    public Rigidbody rb;

    private void Start()
    {
        //Ayn� script farkl� iki objede bulunuyor. �arp��ma i�lemlerinde tag sorgusu yapmam gerekti.
        //�rne�in  if (collision.gameObject.CompareTag("Car2") desem iki arabada bu tag'� arayacakt�. �lk arabada "Car2" ikincide "Car" tag�n� sorgulamam gerekiyordu.
        //Onun yerine bi enum tan�mlay�p oyun ba�lamadan �nce inspectorden se�iyorum ve tag'� enum'dan yard�m alarak arat�yorum. Her script se�ilen enum'u ar�yor.

        switch (currentSel)
        {
            case Tag.Red:
                currentSel = Tag.Red;
                break;
            case Tag.Yellow:
                currentSel = Tag.Yellow;
                break;
        }
        currentPos = transform.position;
    }
    void Update()
    {
        #region Hareket ��lemleri 
        if (move)
        {
            Vector3 direction = currentPos - transform.position;
            transform.position = Vector3.Lerp(transform.position, currentPos, movementSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), rotationSped * Time.deltaTime);
            //Yukar�da klasik pozisyon ve rotasyon i�lemlerim var.

            //As�l �nemli k�s�m buras�. Araban�n bulundu�u pozisyon ile currentPos aras� mesafe 1f'den k���kse GetNextPosition fonksiyonunu �a��r�yorum.
            if (Vector3.Distance(transform.position, currentPos) < 1f)
            {
                GetNextPosition();
            }
        }
        #endregion
    }
    #region Fizik ��lemleri
    private void OnCollisionEnter(Collision collision)
    {
        //Etkile�ime girdi�i objeyi tespit edip bi �arp��ma fizi�i ekliyoruz.
        if (collision.gameObject.CompareTag("Car2") || collision.gameObject.CompareTag("Car"))
        {
            move = false;
            rb.AddExplosionForce(6f, transform.position, 2f, 4f, ForceMode.Impulse);
            StartCoroutine(ReloadScene());
            
        }

        if (collision.gameObject.CompareTag(currentSel.ToString()))
        {
            print(currentSel);
            //do your win-lose stuff
        }
    }
    #endregion
    #region Listeden Bir Sonraki Pozisyonu Alma
    private void GetNextPosition()
    {
        //Burada currentPos'a linerenderer'den gelen pozisyonlar�n oldu�u listedeki elemanlar� at�yorum.
        //Bu �ekilde kendi pozisyonu ile listedeki bir sonraki nokta aras�ndaki hareketi sa�l�yoruz.
        if (y >= positions.Count - 1)
        {
            move = false;
        }
        currentPos = positions[y];
        y++;
    }
    #endregion
    public void ClickToMove()
    {
        move = true;
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
