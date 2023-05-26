// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using UnityEngine;
using VirtualBeings.Tech.BehaviorComposition;

namespace VirtualBeings.Beings.Humanoid
{
    public class HumanoidEmitter : MonoBehaviour, IEmitter
    {
        // ------------------------------
        // IEmitter implementation

        AudioSource IEmitter.SecondaryAudioSource => null;
        GameObject IEmitter.OwningGO => gameObject;

        /// <summary>
        /// Called after the Emitter has been parented to a being's head. Among other things,
        /// ensures that OVRLipSyncContextMorphTarget has access to the SkinnedMeshRenderer.
        /// </summary>
        void IEmitter.Init(Being being)
        {
            _primaryAudioSource = GetComponent<AudioSource>();

            Debug.Assert(_primaryAudioSource != null);

            _primaryAudioSource.loop = false;
            _primaryAudioSource.playOnAwake = false;
            _primaryAudioSource.mute = false;
        }

        void IEmitter.PrepareForPrimaryVocalization()
        {
        }

        void IEmitter.EmitPrimaryVocalization(bool loop, AudioClip clip, float volume, float pitch)
        {
            _primaryAudioSource.loop = loop;
            _primaryAudioSource.clip = clip;
            _primaryAudioSource.volume = volume;
            _primaryAudioSource.pitch = pitch;
            _primaryAudioSource.Play();
        }

        void IEmitter.EmitText(string text, float duration)
        {
        }

        void IEmitter.EmitEmote(int id, float duration)
        {
        }

        void IEmitter.EmitBodySound()
        {
        }

        void IEmitter.EmitSpeech(string text, float animationIntensity01, float volume01, float speed01, float pitch)
        {
        }

        bool IEmitter.IsEmittingSpeech => _isSpeaking;

        void IEmitter.AutoEmitFootSteps()
        {
        }

        private void Update()
        {
            if (_isSpeaking)
            {

            }
        }

        //////////////////////////////////////////////////////////////////////////////
        /// Private fields

        private AudioSource _primaryAudioSource;

        private bool _isSpeaking;
    }
}
