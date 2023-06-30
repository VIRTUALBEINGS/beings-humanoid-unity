// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using VirtualBeings.ActiveCognition.Communication;
using VirtualBeings.Tech.BehaviorComposition;

namespace VirtualBeings.Beings.Humanoid
{
    public class HumanoidCommunicationClient : MonoBehaviour, ICommunicationClient
    {
        // ------------------------------
        // ICommunicationClient implementation

        AudioSource ICommunicationClient.SecondaryAudioSource => null;
        GameObject ICommunicationClient.OwningGO => gameObject;

        /// <summary>
        /// Called after the CommunicationClient has been parented to a being's head. Among other things,
        /// ensures that OVRLipSyncContextMorphTarget has access to the SkinnedMeshRenderer.
        /// </summary>
        void ICommunicationClient.Init(Being being)
        {
            _primaryAudioSource = GetComponent<AudioSource>();

            Debug.Assert(_primaryAudioSource != null);

            _primaryAudioSource.loop = false;
            _primaryAudioSource.playOnAwake = false;
            _primaryAudioSource.mute = false;
        }

        void ICommunicationClient.PrepareForPrimaryVocalization()
        {
        }

        void ICommunicationClient.DoPrimaryVocalization(bool loop, AudioClip clip, float volume, float pitch)
        {
            _primaryAudioSource.loop = loop;
            _primaryAudioSource.clip = clip;
            _primaryAudioSource.volume = volume;
            _primaryAudioSource.pitch = pitch;
            _primaryAudioSource.Play();
        }

        void ICommunicationClient.DoTextDisplay(string text, float duration)
        {
        }

        void ICommunicationClient.DoEmoteDisplay(int id, float duration)
        {
        }

        void ICommunicationClient.DoBodySound()
        {
        }

        void ICommunicationClient.DoSpeech(string text, float animationIntensity01, float volume01, float speed01, float pitch)
        {
        }

        bool ICommunicationClient.IsSpeaking => _isSpeaking;
        bool ICommunicationClient.IsSpeechReady => false;
        float ICommunicationClient.SpeechDuration => 0f;

        void ICommunicationClient.AutoEmitFootSteps()
        {
        }

        void ICommunicationClient.CreateChatCompletion(List<ChatMessage> messages, Action<ChatMessage> OnComplete)
        {

        }

        public void TerminateSpeech()
        {

        }

        public void TerminateChatCompletion()
        {

        }

        //////////////////////////////////////////////////////////////////////////////
        /// Private fields

        private AudioSource _primaryAudioSource;

        private bool _isSpeaking;
    }
}
