using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class PemeliharaanSikatGigiManager : MonoBehaviour
{
    public VideoPlayer video;
    public VideoClip _home;
    public VideoClip _uploadSikat;
    public VideoClip _uploadDebris;

    public static PemeliharaanSikatGigiManager Instance;

    [SerializeField] private GameObject pageMilihFoto;
    [SerializeField] private GameObject pagePemantauan;
    [SerializeField] private GameObject PageKontrolSikatGigi;
    [SerializeField] private GameObject PageKontrolDebrisIndeks;
    [SerializeField] private GameObject PageKeluar;
    [SerializeField] private GameObject PageLoading;
    [SerializeField] private GameObject txtPrefab;
    [SerializeField] private Transform contentParentTxt;
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        Instance = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PageLoading.activeSelf)
                return;

            if (PageKontrolSikatGigi.activeSelf)
            {
                if (PageKontrolSikatGigi.GetComponent<PageFotoScript>().ImageShow.transform.parent.gameObject.activeSelf)
                {
                    PageKontrolSikatGigi.GetComponent<PageFotoScript>().ImageShow.transform.parent.gameObject.SetActive(false);
                    OrientationToPortrait();
                    return;
                }

                PageKontrolSikatGigi.SetActive(false);
                pagePemantauan.SetActive(true);
                PlayVideo(_home);
                return;
            }

            if (pageMilihFoto.activeSelf)
            {
                pageMilihFoto.SetActive(false);
                pagePemantauan.SetActive(true);
                PlayVideo(_home);
                return;
            }

            if (pagePemantauan.activeSelf)
            {
                //change scene
                Helper.GoToHomeMenu();
                return;
            }

            if (PageKontrolDebrisIndeks.activeSelf)
            {
                if (PageKontrolSikatGigi.GetComponent<PageFotoScript>().ImageShow.transform.parent.gameObject.activeSelf)
                {
                    PageKontrolSikatGigi.GetComponent<PageFotoScript>().ImageShow.transform.parent.gameObject.SetActive(false);
                    OrientationToPortrait();
                    return;
                }

                PageKontrolDebrisIndeks.SetActive(false);
                pagePemantauan.SetActive(true);
                PlayVideo(_home);
            }

            if (PageKeluar.activeSelf)
            {
                ExitApp();
                return;
            }
        }
    }

    public void OrientationToAuto() => StartCoroutine(ChangeOrienTation(false));


    public void OrientationToPortrait() => StartCoroutine(ChangeOrienTation(true));


    IEnumerator ChangeOrienTation(bool portrait)
    {
        yield return null;
        if (portrait)
            Screen.orientation = ScreenOrientation.Portrait;
        else
            Screen.orientation = ScreenOrientation.AutoRotation;
    }

    public void ShowLoading()
    {
        PageLoading.SetActive(true);
    }
    public void CloseLoading()
    {
        PageLoading.SetActive(false);
    }

    #region EXIT FUNC
    public void LogOut()
    {
        Helper.LogOut();
        pagePemantauan.SetActive(false);
        PageKontrolSikatGigi.SetActive(false);
        PageKontrolDebrisIndeks.SetActive(false);
        PageKeluar.SetActive(true);

    }

    public void ExitApp()
    {
        Application.Quit();
    }
    #endregion


    public void SetTextMessage(string _txt = "")
    {
        GameObject msg = Instantiate(txtPrefab, contentParentTxt);
        msg.GetComponent<PemantauanMessage>().SetText(_txt, canvas);
        msg.SetActive(true);
    }
    
    public void PlayVideo(VideoClip videoClip)
    {
        video.clip = videoClip;
    }
}
