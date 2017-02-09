using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace DavidOchmann.Animation
{
    [System.Serializable]
    public class TweenComponentValuesEvent : UnityEvent { }

    [System.Serializable]
    public class PopupVO
    {
        public string name;
        public int index;
        public List<string> list;
    }

    [System.Serializable]
    public class PopupFloatFieldVO
    {
        public List<string> list;
        public float value;
        public int index;
    }

    [RequireComponent(typeof(Mutate))]
    public class TweenComponentValues : MonoBehaviour
    {
        public string id = "easeIn";
        public bool playOnStart = true;
        public bool allowOverwrite = true;
        public bool jumpToEnd = false;
        public float duration = .6f;

        public PopupVO tweenMethod;
        public PopupVO easeType;
        public PopupVO easeMethod;

        public TweenComponentValuesEvent onStart = new TweenComponentValuesEvent();
        public TweenComponentValuesEvent onUpdate = new TweenComponentValuesEvent();
        public TweenComponentValuesEvent onComplete = new TweenComponentValuesEvent();

        public bool showSetupList = false;
        public bool showOverwriteList = false;
        public bool showEasingList = true;
        public bool showPropertyList = true;
        public bool showEventList = false;

        public PopupVO popupComponentVO;
        public List<PopupFloatFieldVO> popupFloatFieldVOs = new List<PopupFloatFieldVO>();

        private static string EASING_NAMESPACE = "DavidOchmann.Animation.";

        private DTween dTween;
        private Component component;
        private Dictionary<string, object> dictionary;

        /**
		 * Getter / Setter
		 */

        public Tween.EaseDelegate GetDelegateFromSetup()
        {
            string typeName = easeType.list[easeType.index];
            string methodName = easeMethod.list[easeMethod.index];

            Type type = Type.GetType(EASING_NAMESPACE + typeName);
            MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);

            Tween.EaseDelegate easeDelegate = (Tween.EaseDelegate)Delegate.CreateDelegate(typeof(Tween.EaseDelegate), methodInfo);

            return easeDelegate;
        }

        /**
		 * Public
		 */

        /** MonoBehaviour interface. */

        public void Awake()
        {
            initVariables();
            initDictionary();
        }

        public void Start()
        {
            if (playOnStart)
            {
                Play();
                FixedUpdate();
            }
        }

        public void FixedUpdate()
        {
            dTween.Update();
        }

        public void Play(string id = null)
        {
            Kill();

            if (id != null)
            {
                TweenComponentValues[] tweenComponentValues = GetComponents<TweenComponentValues>();

                for (int i = 0; i < tweenComponentValues.Length; ++i)
                {
                    TweenComponentValues tweenGameObject = tweenComponentValues[i];

                    if (tweenGameObject.id == id)
                        tweenGameObject.Play();
                }
            }
            else
                initTween();
        }

        public void PlayAll()
        {
            TweenComponentValues[] tweenComponentValues = GetComponents<TweenComponentValues>();

            for (int i = 0; i < tweenComponentValues.Length; ++i)
            {
                TweenComponentValues tweenGameObject = tweenComponentValues[i];
                tweenGameObject.Play();
            }
        }

        public void Stop()
        {
            dTween.Kill();
        }

        public void StopAll()
        {
            TweenComponentValues[] tweenComponentValues = GetComponents<TweenComponentValues>();

            for (int i = 0; i < tweenComponentValues.Length; ++i)
            {
                TweenComponentValues tweenGameObject = tweenComponentValues[i];
                tweenGameObject.dTween.Kill();
            }
        }

        private void Kill(bool jumpToEnd = false)
        {
            if (dTween != null && allowOverwrite)
                dTween.Kill(jumpToEnd);
        }

        /**
		 * Private
		 */

        /** Init variables. */

        private void initVariables()
        {
            dTween = new DTween();
            component = GetComponent(popupComponentVO.list[popupComponentVO.index]);
            dictionary = new Dictionary<string, object>();
        }

        /** Create dictionary with attibutes elements. */

        private void initDictionary()
        {
            for (int i = 0; i < popupFloatFieldVOs.Count; ++i)
            {
                PopupFloatFieldVO popupFloatFieldVO = popupFloatFieldVOs[i];

                string property = popupFloatFieldVO.list[popupFloatFieldVO.index];
                float value = popupFloatFieldVO.value;

                dictionary.Add(property, value);
            }
        }

        /** Start Tween. */

        private void initTween()
        {
            Tween.EaseDelegate easeDelegate = GetDelegateFromSetup();
            Tween tween = null;

            string method = tweenMethod.list[tweenMethod.index];

            switch (method)
            {
                case "To":
                    tween = dTween.To(component, duration, dictionary, easeDelegate);
                    break;

                case "From":
                    tween = dTween.From(component, duration, dictionary, easeDelegate);
                    break;
            }

            tween.OnStart += dTweenOnStartHandler;
            tween.OnComplete += dTweenOnCompleteHandler;
            tween.OnUpdate += dTweenOnUpdateHandler;
        }

        private void dTweenOnStartHandler(Tween tween)
        {
            onStart.Invoke();
        }

        private void dTweenOnUpdateHandler(Tween tween)
        {
            onUpdate.Invoke();
        }

        private void dTweenOnCompleteHandler(Tween tween)
        {
            onComplete.Invoke();
        }
    }
}