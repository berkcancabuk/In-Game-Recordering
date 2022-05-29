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
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>(); // Recorder in ayarlarýna eriþebilmek için bunu tanýmlýyoruz. 
        var TestRecorderController = new RecorderController(controllerSettings); // Recorderi baþlatýp durdurabilmek için bunu tanýmlýyoruz.

        var videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>(); // Movie yani video çekeceðimiz için bunu da burada tanýmlýyoruz.
        videoRecorder.name = "My Video Recorder";  // ismini veriyoruz
        videoRecorder.Enabled = true; 
        videoRecorder.VideoBitRateMode = VideoBitrateMode.High; // video bit rate ini high yapýyoruz daha kaliteli bir görüntü çýksýn diye isterseniz low veya medium yapabilirisiniz.

        videoRecorder.ImageInputSettings = new GameViewInputSettings // Çözünürlük ayarlarýný yapýyoruz
        {
            OutputWidth = 640,
            OutputHeight = 480
        };

        videoRecorder.AudioInputSettings.PreserveAudio = true; // ses varsa onu korumasý ve kaydetmesi için true olarak kullanýyoruz

        controllerSettings.AddRecorderSettings(videoRecorder); 
        controllerSettings.SetRecordModeToFrameInterval(0, 300); // 5s @ 60 FPS 
        controllerSettings.FrameRate = 60; // saniyede kaç frame kullanacaðý 
        videoRecorder.OutputFile = "C:\\Users\\berkc\\Deneme\\Assets\\Resources\\vid\\video1"; // dosyanýn kayýt edileceði konum
        RecorderOptions.VerboseMode = false; // ayrýntýlý modu kapatýyoruz
        TestRecorderController.PrepareRecording(); // kaydý hazýrlýyoruz
        TestRecorderController.StartRecording(); // kaydý baþlatýyoruz. 
        StartCoroutine(isVideoRecordFinish()); // kayýt bittikten sonra olacaklar
    }
    IEnumerator isVideoRecordFinish()
    {
        yield return new WaitForSeconds(7f);
        allVideos = Resources.LoadAll<VideoClip>("vid"); // resources klasörünün vid adlý dosyasýnýn içindekilerinin hepsini yüklüyoruz
        videoPlayer.clip = allVideos[0]; // videoPlayeri vid dosyasýnýn altýndaki ilk klibe eþitliyoruz.
    }
}

