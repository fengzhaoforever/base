using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
[RequireComponent(typeof(AudioSource))]
public class AudioTest : MonoBehaviour
{
    private AudioSource _audio;
    public AudioClip[] Clips = new AudioClip[3];
    private int _index;
    public string[] keywords = { "你好", "开始", "停止" };
    private KeywordRecognizer m_PhraseRecognizer;
    private ConfidenceLevel m_confidenceLevel;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.playOnAwake = false;
        m_confidenceLevel = ConfidenceLevel.Medium;
        //创建一个识别器
        m_PhraseRecognizer = new KeywordRecognizer(keywords, m_confidenceLevel);
        //通过注册监听的方法
        m_PhraseRecognizer.OnPhraseRecognized += M_PhraseRecognizer_OnPhraseRecognized;
        //开启识别器
        m_PhraseRecognizer.Start();
    }

    private void M_PhraseRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StartCoroutine(PlayAudio());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator  PlayAudio()
    {
        if (_audio.isPlaying)
            _audio.Stop();
        _audio.clip = Clips[_index];
        _audio.Play();
        yield return new WaitForSeconds(2f);
        _index++;
        if(_index<Clips.Length)
            StartCoroutine(PlayAudio());
    }

    private void OnDestroy()
    {
        if (m_PhraseRecognizer != null)
            m_PhraseRecognizer.Dispose();
    }

}
