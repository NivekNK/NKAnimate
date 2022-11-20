using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace NK.Animate
{
    [DefaultExecutionOrder(-1)]
    [RequireComponent(typeof(Animator))]
    public class NKAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private PlayableGraph playableGraph;
        private AnimationPlayableOutput playableOutput;

        public NKAnimationState CurrentAnimation { get; private set; }

        private void Awake()
        {
            playableGraph = PlayableGraph.Create();
            playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            playableOutput = AnimationPlayableOutput.Create(playableGraph, "CurrentAnimation", animator);
        }

        private void Update()
        {
            CurrentAnimation?.Update();
        }

        public NKAnimationState Play(AnimationClip animation)
        {
            // Checks if is playing a animation clip
            // If it is, then checks if the animator wants to play the same clip
            if (CurrentAnimation != null && CurrentAnimation.AnimationClip == animation)
                return CurrentAnimation;

            CurrentAnimation = new NKAnimationState(playableGraph, animation);
            playableOutput.SetSourcePlayable(CurrentAnimation.AnimationClipPlayable);
            playableGraph.Play();

            return CurrentAnimation;
        }

        private void OnDisable()
        {
            playableGraph.Destroy();
        }
    }
}