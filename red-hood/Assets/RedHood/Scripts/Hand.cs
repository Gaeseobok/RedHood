using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace RedHood
{
    // Hand class handles anything that the hand can do
    [RequireComponent(typeof(Animator))]
    public class Hand : MonoBehaviour
    {
        public float speed;

        private Animator animator;
        private float gripTarget;
        private float triggerTarget;
        private float gripCurrent;
        private float triggerCurrent;

        private const string ANIMATOR_PARAM_GRIP = "Grip";
        private const string ANIMATOR_PARAM_TRIGGER = "Trigger";

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            AnimateHand();
        }

        internal void SetGrip(float v)
        {
            gripTarget = v;
        }

        internal void SetTrigger(float v)
        {
            triggerTarget = v;
        }

        // Change animator parameter gradually across several frames -> Trigger the animation
        private void AnimateHand()
        {
            if (gripCurrent != gripTarget)
            {
                gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
                animator.SetFloat(ANIMATOR_PARAM_GRIP, gripCurrent);
            }
            if (triggerCurrent != triggerTarget)
            {
                triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
                animator.SetFloat(ANIMATOR_PARAM_TRIGGER, triggerCurrent);
            }
        }
    }
}