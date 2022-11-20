using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace NK.Animate
{
    public class NKAnimationState
    {
        public struct AnimationEvents
        {
            public Action OnAnimationEnd;
            public Action OnAnimationLoopEnd;
        }

        public AnimationClip AnimationClip { get; private set; }
        public AnimationClipPlayable AnimationClipPlayable { get; private set; }
        public AnimationEvents Events;
        public bool Looping => AnimationClip.isLooping;
        public float Time => (float)AnimationClipPlayable.GetTime();
        public float LoopTime
        {
            get
            {
                float time = Mathf.Clamp(Time - startTime, 0.0f, AnimationClip.length);
                return GetValueInNewRange(time, AnimationClip.length, 1.0f);
            }
            set
            {
                float loopTime = Mathf.Clamp01(value);
                float time = GetValueInNewRange(loopTime, 1.0f, AnimationClip.length);
                AnimationClipPlayable.SetTime(time);
            }
        }

        private float startTime;

        public NKAnimationState(PlayableGraph playableGraph, AnimationClip animation)
        {
            AnimationClipPlayable = AnimationClipPlayable.Create(playableGraph, animation);
            AnimationClip = animation;
        }

        public void Update()
        {
            if (IsLoopDone())
                Events.OnAnimationLoopEnd?.Invoke();

            if (IsDone())
                Events.OnAnimationEnd?.Invoke();

            if (Looping)
            {
                if (Time - startTime >= AnimationClip.length)
                    startTime = Time;
            }
        }

        public bool IsDone() => AnimationClipPlayable.IsDone();
        public bool IsLoopDone() => LoopTime >= 1.0f;

        private float GetValueInNewRange(float value, float oldMax, float newMax)
        {
            return value * newMax / oldMax;
        }
    }
}