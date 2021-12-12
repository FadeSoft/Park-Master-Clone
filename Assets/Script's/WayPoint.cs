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
        //Ayný script farklý iki objede bulunuyor. Çarpýþma iþlemlerinde tag sorgusu yapmam gerekti.
        //Örneðin  if (collision.gameObject.CompareTag("Car2") desem iki arabada bu tag'ý arayacaktý. Ýlk arabada "Car2" ikincide "Car" tagýný sorgulamam gerekiyordu.
        //Onun yerine bi enum tanýmlayýp oyun baþlamadan önce inspectorden seçiyorum ve tag'ý enum'dan yardým alarak aratýyorum. Her script seçilen enum'u arýyor.

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
        #region Hareket Ýþlemleri 
        if (move)
        {
            Vector3 direction = currentPos - transform.position;
            transform.position = Vector3.Lerp(transform.position, currentPos, movementSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), rotationSped * Time.deltaTime);
            //Yukarýda klasik pozisyon ve rotasyon iþlemlerim var.

            //Asýl önemli kýsým burasý. Arabanýn bulunduðu pozisyon ile currentPos arasý mesafe 1f'den küçükse GetNextPosition fonksiyonunu çaðýrýyorum.
            if (Vector3.Distance(transform.position, currentPos) < 1f)
            {
                GetNextPosition();
            }
        }
        #endregion
    }
    #region Fizik Ýþlemleri
    private void OnCollisionEnter(Collision collision)
    {
        //Etkileþime girdiði objeyi tespit edip bi çarpýþma fiziði ekliyoruz.
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
        //Burada currentPos'a linerenderer'den gelen pozisyonlarýn olduðu listedeki elemanlarý atýyorum.
        //Bu þekilde kendi pozisyonu ile listedeki bir sonraki nokta arasýndaki hareketi saðlýyoruz.
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
