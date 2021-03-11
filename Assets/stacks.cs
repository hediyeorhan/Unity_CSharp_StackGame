using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stacks : MonoBehaviour
{
    int y = 1; //unity'de görülen 'y' değerinin bir fazlasını yazdık üste koyması için.
    int stack_sayisi;
    GameObject[] stack_dizi;
    int stack_deger;
    bool stack_durum = false;
    const float pozitif = 7f;
    const float hiz_deger = 0.18f;
    float hiz = hiz_deger;
    const float boyut = 4f;
    Vector2 stack_boyut = new Vector2(boyut, boyut);
    Vector3 kamera_pozisyonu; // kamera eklenen bloklar ile birlikte hareket etsin diye bunu oluşturduk.
    bool eksen_x;
    Vector3 eski_alan;
    float hareket;
    bool bitti_mi = false;
    float hata_payi = 0.1f;
    int combo;
    Color32 renk;
    public Color32 renk1;
    public Color32 renk2;
    public Color32 renk3;
    public Color32 renk4;
    int sayac = 0;
    Camera kamera;
    public Text yazi;
    int skor = 0;
    public GameObject panel;
    int yüksek_skor;
    public Text yüksek_skor_yazisi;
   

    void Start()
    {
        yüksek_skor = PlayerPrefs.GetInt("highscore"); // yüksek skoru tutmak için.
        yüksek_skor_yazisi.text = yüksek_skor.ToString();
        yazi.text = skor.ToString();
        kamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); // kamera pozisyonu
        kamera.backgroundColor = renk3;
        renk = renk1;
        stack_sayisi = transform.GetChildCount();  //Küp sayısını bulmak için kullandık.
        stack_dizi = new GameObject[stack_sayisi];
        for (int i = 0; i < stack_sayisi; i++)
        {
            stack_dizi[i] = transform.GetChild(i).gameObject; // Bütün küpleri bir diziye aktardık bu şekilde.
            stack_dizi[i].GetComponent<Renderer>().material.color = renk;
        }
        stack_deger = stack_sayisi - 1; // Bu şekilde küpleri en alttan çekip en üste koymamız sağlanacak.
    }

    void fazlalik(Vector3 konum, Vector3 pozisyon, Color32 renkk) // Fazla parçaların düşmesi için bu fonksiyonu yazıyoruz.
    {
        GameObject obje = GameObject.CreatePrimitive(PrimitiveType.Cube); // Küp şeklinde bir game object nesnesi oluşturduk.
        obje.transform.localScale = pozisyon;
        obje.transform.position = konum;
        obje.GetComponent<Renderer>().material.color = renkk;
        obje.AddComponent < Rigidbody > (); //Kesilen kenarların aşağı düşmesini sağlıyor.

    }

    void Update()
    {
        if (!bitti_mi)
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                if (alan_kontrol())
                {
                    stack_al_koy();
                    y++;
                    skor++;
                    yazi.text = skor.ToString(); 
                    if(skor > yüksek_skor)
                    {
                        yüksek_skor = skor;
                        
                        
                    }
                    byte degisken = 25;
                    renk = new Color32((byte)(renk.r + degisken), (byte)(renk.g + degisken), (byte)(renk.b + degisken), renk.a);
                    renk2 = new Color32((byte)(renk2.r + degisken), (byte)(renk2.g + degisken), (byte)(renk2.b + degisken), renk2.a);
                    if (sayac > 4)
                    {
                        sayac = 0;
                        renk1 = renk2;
                        renk2 = renk3;
                        renk3 = renk4;
                        renk4 = renk;
                        renk = renk1;
                    }
                    sayac++;
                }
                else
                {
                    son();
                }
            }
            
            sag_sol_hareket();
            transform.position = Vector3.Lerp(transform.position, kamera_pozisyonu, 0.1f);
        }
        
    }
    void stack_al_koy()
    {
        eski_alan = stack_dizi[stack_deger].transform.localPosition;
        if (stack_deger <= 0)
        {
            stack_deger = stack_sayisi;
        }
        stack_durum = false;
        stack_deger--;
        eksen_x = !eksen_x;
        kamera_pozisyonu = new Vector3(0, -y, 0);
        stack_dizi[stack_deger].transform.localScale = new Vector3(stack_boyut.x, 1, stack_boyut.y);
        stack_dizi[stack_deger].GetComponent<Renderer>().material.color = Color32.Lerp(stack_dizi[stack_deger].GetComponent<Renderer>().material.color,renk,0.3f);
        
       
        kamera.backgroundColor = Color32.Lerp(kamera.backgroundColor, renk2, 0.1f); 
    }

    void sag_sol_hareket()
    {
        if (eksen_x)
        {
            if (!stack_durum)
            {
                stack_dizi[stack_deger].transform.localPosition = new Vector3(pozitif, y, hareket);
                stack_durum = true;
            }
            if (stack_dizi[stack_deger].transform.localPosition.x > pozitif) //eksende tamamen kaybolmasın diye sınır belirledik gel git hareketleri yapması için.
            {
                hiz = hiz_deger * (-1);
            }
            else if (stack_dizi[stack_deger].transform.localPosition.x < -pozitif)
            {
                hiz = hiz_deger;
            }
            stack_dizi[stack_deger].transform.localPosition += new Vector3(hiz, 0, 0); //z ve y ekseninde hız degeri olmayacagi için 0 dedik.
        }
        else
        {
            if (!stack_durum)
            {
                stack_dizi[stack_deger].transform.localPosition = new Vector3(hareket, y, pozitif);
                stack_durum = true;
            }
            if (stack_dizi[stack_deger].transform.localPosition.z > pozitif) //eksende tamamen kaybolmasın diye sınır belirledik gel git hareketleri yapması için.
            {
                hiz = hiz_deger * (-1);
            }
            else if (stack_dizi[stack_deger].transform.localPosition.z < -pozitif)
            {
                hiz = hiz_deger;
            }
            stack_dizi[stack_deger].transform.localPosition += new Vector3(0, 0, hiz); //x ve y ekseninde hız degeri olmayacagi için 0 dedik.
        }
    }
    bool alan_kontrol()
    {
        if (eksen_x)
        {
            float fark = eski_alan.x - stack_dizi[stack_deger].transform.localPosition.x; // x ekseninde hareket ettiğimiz için '.x' kullandık.
            if (Mathf.Abs(fark) > hata_payi)
            {
                combo = 0;
                Vector3 konum;
                if (stack_dizi[stack_deger].transform.localPosition.x > eski_alan.x)
                {
                    konum = new Vector3(stack_dizi[stack_deger].transform.position.x + stack_dizi[stack_deger].transform.localScale.x / 2, stack_dizi[stack_deger].transform.position.y, stack_dizi[stack_deger].transform.position.z);
                }
                else
                {
                    konum = new Vector3(stack_dizi[stack_deger].transform.position.x - stack_dizi[stack_deger].transform.localScale.x / 2, stack_dizi[stack_deger].transform.position.y, stack_dizi[stack_deger].transform.position.z);
                }
                Vector3 büyüklük = new Vector3(fark, 1, stack_boyut.y);
                stack_boyut.x -= Mathf.Abs(fark); // yaptığımız her işlem x ekseninde..
                if (stack_boyut.x < 0)
                {
                    return false; //küp boşluğa gelirse false döner ve oyun biter.
                }
                stack_dizi[stack_deger].transform.localScale = new Vector3(stack_boyut.x, 1, stack_boyut.y);
                float orta = stack_dizi[stack_deger].transform.localPosition.x / 2 + eski_alan.x / 2;// yeni gelen objeleri tam ortalaması için böyle bir şey yaptık.
                stack_dizi[stack_deger].transform.localPosition = new Vector3(orta, y, eski_alan.z);
                hareket = stack_dizi[stack_deger].transform.localPosition.x;
                fazlalik(konum, büyüklük, stack_dizi[stack_deger].GetComponent<Renderer>().material.color);
            }
            else
            {
                combo++;
                if(combo > 1)
                {
                    stack_boyut.x += 0.3f;
                    if(stack_boyut.x > boyut)
                    {
                        stack_boyut.x = boyut;
                        
                    }
                    stack_dizi[stack_deger].transform.localScale = new Vector3(stack_boyut.x, 1, stack_boyut.y);
                    stack_dizi[stack_deger].transform.localPosition = new Vector3(eski_alan.x, y, eski_alan.z);
                }
                else
                {
                    stack_dizi[stack_deger].transform.localPosition = new Vector3(eski_alan.x, y, eski_alan.z);
                }
                hareket = stack_dizi[stack_deger].transform.localPosition.x;
            }
        }
        else
        {
            float fark = eski_alan.z - stack_dizi[stack_deger].transform.localPosition.z; // x ekseninde hareket ettiğimiz için '.x' kullandık.
            if (Mathf.Abs(fark) > hata_payi)
            {
                combo = 0; // her seferinde 0'lıyoruz ki üstüne tam geldiğinde büyüme olmasın diye.
                Vector3 konum;
                if (stack_dizi[stack_deger].transform.localPosition.z > eski_alan.z)
                {
                    konum = new Vector3(stack_dizi[stack_deger].transform.position.x, stack_dizi[stack_deger].transform.position.y, stack_dizi[stack_deger].transform.position.z + stack_dizi[stack_deger].transform.localScale.z / 2);
                }
                else
                {
                    konum = new Vector3(stack_dizi[stack_deger].transform.position.x, stack_dizi[stack_deger].transform.position.y, stack_dizi[stack_deger].transform.position.z - stack_dizi[stack_deger].transform.localScale.z / 2);
                }
                Vector3 büyüklük = new Vector3(stack_boyut.x, 1, fark);
                stack_boyut.y -= Mathf.Abs(fark); // yaptığımız her işlem x ekseninde..
                if (stack_boyut.y < 0)
                {
                    return false;
                }
                stack_dizi[stack_deger].transform.localScale = new Vector3(stack_boyut.x, 1, stack_boyut.y);
                float orta = stack_dizi[stack_deger].transform.localPosition.z / 2 + eski_alan.z / 2;
                stack_dizi[stack_deger].transform.localPosition = new Vector3(eski_alan.x, y, orta);
                hareket = stack_dizi[stack_deger].transform.localPosition.z;
                fazlalik(konum, büyüklük, stack_dizi[stack_deger].GetComponent<Renderer>().material.color);
                
            }
            else
            {
                combo++;
                if (combo > 3)
                {
                    stack_boyut.y += 0.3f;
                    if (stack_boyut.y > boyut)
                    {
                        stack_boyut.y = boyut;
                    }
                    stack_dizi[stack_deger].transform.localScale = new Vector3(stack_boyut.x, 1, stack_boyut.y);
                    stack_dizi[stack_deger].transform.localPosition = new Vector3(eski_alan.x, y, eski_alan.z);

                }
                else
                {
                    stack_dizi[stack_deger].transform.localPosition = new Vector3(eski_alan.x, y, eski_alan.z);
                }
                hareket = stack_dizi[stack_deger].transform.localPosition.z;
            }
        }
        return true;
    }

    void son()
    {
        
        bitti_mi = true;
        stack_dizi[stack_deger].AddComponent< Rigidbody >();
        panel.SetActive(true);
        PlayerPrefs.SetInt("highscore", yüksek_skor); // En yüksek skoru bu şekilde kaydediyoruz.
        yüksek_skor_yazisi.text = yüksek_skor.ToString(); 
        yazi.text = "";
    }
   public void yeni_oyun()
    {
        Application.LoadLevel(Application.loadedLevel); // yeni oyuna başlamak için yazdık .Unity kısmında butona tıkladığımızda yeni oyun fonksiyonun çalıştırması için seçim yaptık.
    }

}

