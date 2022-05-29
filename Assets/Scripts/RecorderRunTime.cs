using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using System.IO;
using UnityEditor;
using UnityEngine.Video;
using UnityEditor.Recorder.Timeline;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;

public class RecorderRunTime : MonoBehaviour
{
    public VideoClip[] allVideos;
    RecorderController m_RecorderController;
    public VideoPlayer videoPlayer;
    private VideoSource videoSource;


    
    public void StartRecordingFunc()
    {
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>(); // Recorder in ayarlar�na eri�ebilmek i�in bunu tan�ml�yoruz. 
        var TestRecorderController = new RecorderController(controllerSettings); // Recorderi ba�lat�p durdurabilmek i�in bunu tan�ml�yoruz.

        var videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>(); // Movie yani video �ekece�imiz i�in bunu da burada tan�ml�yoruz.
        videoRecorder.name = "My Video Recorder";  // ismini veriyoruz
        videoRecorder.Enabled = true; 
        videoRecorder.VideoBitRateMode = VideoBitrateMode.High; // video bit rate ini high yap�yoruz daha kaliteli bir g�r�nt� ��ks�n diye isterseniz low veya medium yapabilirisiniz.

        videoRecorder.ImageInputSettings = new GameViewInputSettings // ��z�n�rl�k ayarlar�n� yap�yoruz
        {
            OutputWidth = 640,
            OutputHeight = 480
        };

        videoRecorder.AudioInputSettings.PreserveAudio = true; // ses varsa onu korumas� ve kaydetmesi i�in true olarak kullan�yoruz

        controllerSettings.AddRecorderSettings(videoRecorder); 
        controllerSettings.SetRecordModeToFrameInterval(0, 300); // 5s @ 60 FPS 
        controllerSettings.FrameRate = 60; // saniyede ka� frame kullanaca�� 
        videoRecorder.OutputFile = "C:\\Users\\berkc\\Deneme\\Assets\\Resources\\vid\\video1"; // dosyan�n kay�t edilece�i konum
        RecorderOptions.VerboseMode = false; // ayr�nt�l� modu kapat�yoruz
        TestRecorderController.PrepareRecording(); // kayd� haz�rl�yoruz
        TestRecorderController.StartRecording(); // kayd� ba�lat�yoruz. 
        StartCoroutine(isVideoRecordFinish()); // kay�t bittikten sonra olacaklar
    }
    IEnumerator isVideoRecordFinish()
    {
        yield return new WaitForSeconds(7f);
        allVideos = Resources.LoadAll<VideoClip>("vid"); // resources klas�r�n�n vid adl� dosyas�n�n i�indekilerinin hepsini y�kl�yoruz
        videoPlayer.clip = allVideos[0]; // videoPlayeri vid dosyas�n�n alt�ndaki ilk klibe e�itliyoruz.
    }
}

