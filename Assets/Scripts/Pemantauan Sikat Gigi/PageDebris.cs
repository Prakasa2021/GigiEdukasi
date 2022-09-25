using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class PageDebris : MonoBehaviour
{
    [System.Serializable]
    public class DataGigi
    {
        public string namaGigi;
        public bool status;
        public string stringFoto;
        public string pathFoto;

        //UI
        public TextMeshProUGUI txtButtonFoto;
        public Button btnUploadFoto;
        public Button btnAda;
        public Sprite sprData;

        public void Setup()
        {
            foreach (var a in btnAda.GetComponentsInChildren<Transform>())
            {
                if (a == btnAda.transform)
                    continue;
                else
                    a.gameObject.SetActive(false);
            }
            ChangeStatusButtonGambar();

            btnAda.onClick.AddListener(() =>
            {
                foreach (var a in btnAda.GetComponentsInChildren<Transform>())
                {
                    if (a == btnAda.transform)
                        continue;
                    else
                        a.gameObject.SetActive(false);
                }

                status = !status;

                ChangeStatusButtonGambar();
            });

            btnUploadFoto.onClick.AddListener(() =>
            {
                if (!string.IsNullOrEmpty(stringFoto))
                {
                    Instance.PanelImage.SetActive(true);
                    Instance.PanelImage.transform.GetChild(0).GetComponent<Image>().sprite = sprData;
                }
                else
                    OpenGallery();
            });
        }

        public void OpenGallery()
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
            {
                Debug.Log("Image path: " + path);
                if (path != null)
                {
                    // Create Texture from selected image
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, -1);
                    if (texture == null)
                    {
                        print("Couldn't load texture from " + path);
                        return;
                    }

                    ImagePicked(texture, path);
                }
            });
        }

        public void ImagePicked(Texture2D _tex, string _path = "")
        {
            print($"Creating image from {_path}");
            pathFoto = _path;
            stringFoto = Helper.TextureToBase64(_tex);
            CreateSprite(_tex);
        }

        private void CreateSprite(Texture2D _tex)
        {
            //create sprite
            txtButtonFoto.text = "Lihat Foto";
            Sprite spriteFoto = Helper.TextureToSprite(_tex);
            sprData = spriteFoto;
        }

        public void ChangeStatusButtonGambar()
        {
            if (status)
            {
                btnAda.transform.Find("Ada").gameObject.SetActive(true);
            }
            else
            {
                btnAda.transform.Find("TidakAda").gameObject.SetActive(true);
            }
        }
    }

    public List<DataGigi> listDataGigiDebris = new List<DataGigi>();
    public Button btnSimpan;
    public GameObject PanelImage;
    public static PageDebris Instance;
    private void Awake()
    {
        Instance = this;

        LoadData();
        foreach (var a in listDataGigiDebris)
            a.Setup();

    }

    public void LoadData()
    {
        if (!RespondenData.Instance)
            return;

        if (RespondenData.Instance.currentDataSelected.statusDebris == "1")
        {
            //tampilkan data
            for (int i = 0; i < RespondenData.Instance.dataDebris.debris.listDebris.Count; i++)
            {
                listDataGigiDebris[i].namaGigi = RespondenData.Instance.dataDebris.debris.listDebris[i].namaGigi;
                listDataGigiDebris[i].status = RespondenData.Instance.dataDebris.debris.listDebris[i].status;
                listDataGigiDebris[i].stringFoto = RespondenData.Instance.dataDebris.debris.listDebris[i].stringFoto;
                listDataGigiDebris[i].pathFoto = RespondenData.Instance.dataDebris.debris.listDebris[i].pathFoto;

                //create sprite
                Texture2D tex = Helper.Base64ToTexture(listDataGigiDebris[i].stringFoto);
                listDataGigiDebris[i].ImagePicked(tex);
            }
        }
    }

    public void SaveData()
    {
        //cek dulu datanya
        foreach (var dataGigi in listDataGigiDebris)
        {
            if (string.IsNullOrEmpty(dataGigi.stringFoto))
            {
                print("DATA BELUM LENGKAP");
                return;
            }
        }

        RespondenData.Instance.dataDebris = new RespondenData.DebrisFile();
        RespondenData.Instance.dataDebris.debris.listDebris = new List<RespondenData.DebrisData>();

        foreach (var a in listDataGigiDebris)
        {
            RespondenData.DebrisData d = new RespondenData.DebrisData();
            d.namaGigi = a.namaGigi;
            d.status = a.status;
            d.stringFoto = a.stringFoto;
            d.pathFoto = a.pathFoto;
            RespondenData.Instance.dataDebris.debris.listDebris.Add(d);
        }
        string json = JsonUtility.ToJson(RespondenData.Instance.dataDebris);
        RespondenData.Instance.SaveDebris(json);
    }
}
