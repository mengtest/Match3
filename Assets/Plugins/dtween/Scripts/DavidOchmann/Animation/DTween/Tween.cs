using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DavidOchmann.Animation
{
    public class Tween
    {
        public delegate float EaseDelegate(float t, float b, float c, float d);

        public static string WARNING_MESSAGE_PROPERTY = "has no property \"";
        public static string KEY_DELAY = "delay";
        public static string KEY_PARAMETER = "parameter";
        public static string[] IGNORED_PROPERTIES = { KEY_DELAY, KEY_PARAMETER };
        public static int FPS = (int)Math.Floor(1 / Time.fixedDeltaTime);

        public object target;
        public float duration;
        public Dictionary<string, object> setup;
        public bool hasCompleted;
        public bool hasStarted;

        private int frame = 0;
        private float delay;
        private EaseDelegate ease;
        private Dictionary<string, float> beginValues;

        public Tween(object target, float duration, Dictionary<string, object> setup, EaseDelegate ease)
        {
            this.target = target;
            this.duration = duration;
            this.setup = setup;

            this.frame = 0;
            this.delay = setup.ContainsKey(KEY_DELAY) ? Convert.ToSingle(setup[KEY_DELAY]) : 0;
            this.ease = ease != null ? ease : Quad.EaseOut;

            Reset();
        }

        /**
		 * Event interface.
		 */

        public delegate void OnStartEventHandler(Tween tween);

        public event OnStartEventHandler OnStart;

        public virtual void InvokeStart()
        {
            if (OnStart != null) OnStart(this);
        }

        public delegate void OnUpdateEventHandler(Tween tween);

        public event OnUpdateEventHandler OnUpdate;

        public virtual void InvokeUpdate()
        {
            if (OnUpdate != null) OnUpdate(this);
        }

        public delegate void OnCompleteEventHandler(Tween tween);

        public event OnCompleteEventHandler OnComplete;

        public virtual void InvokeComplete()
        {
            if (OnComplete != null) OnComplete(this);
        }

        /**
		 * Static interface.
		 */

        public static object GetObjectValue(object target, string property)
        {
            object value = null;
            Type type = target.GetType();

            PropertyInfo propertyInfo = type.GetProperty(property);

            if (propertyInfo != null)
            {
                value = propertyInfo.GetValue(target, null);
            }
            else
            {
                FieldInfo fieldInfo = type.GetField(property);

                if (fieldInfo != null)
                    value = fieldInfo.GetValue(target);
            }

            return value;
        }

        public static object SetObjectValue(object target, string property, object value)
        {
            Type type = target.GetType();
            PropertyInfo propertyInfo = type.GetProperty(property);

            if (propertyInfo != null)
            {
                propertyInfo.SetValue(target, value, null);
            }
            else
            {
                FieldInfo fieldInfo = type.GetField(property);

                if (fieldInfo != null)
                    fieldInfo.SetValue(target, value);
                else
                    Debug.LogWarning(target + WARNING_MESSAGE_PROPERTY + property + "\".");
            }

            return target;
        }

        public static bool GetIsIgnoredProperty(string property)
        {
            for (int i = 0; i < IGNORED_PROPERTIES.Length; ++i)
            {
                if (property == IGNORED_PROPERTIES[i])
                    return true;
            }

            return false;
        }

        /**
		 * Getter / Setter.
		 */

        public bool isFirstFrame
        {
            get { return frame == 0; }
        }

        public bool complete
        {
            get { return frame >= totalFrames; }
        }

        public bool start
        {
            get { return frame >= delayFrames; }
        }

        public int totalFrames
        {
            get { return durationFrames + delayFrames; }
        }

        public int durationFrames
        {
            get { return GeSecondsToFrames(duration); }
        }

        public int delayFrames
        {
            get { return GeSecondsToFrames(delay); }
        }

        public float timescale
        {
            get { return duration / durationFrames; }
        }

        public int GeSecondsToFrames(float seconds)
        {
            return (int)Math.Ceiling(FPS * seconds);
        }

        public Dictionary<string, float> GetDictionaryAtFrame(int frame)
        {
            Dictionary<string, float> dictionary = null;

            if (start && beginValues != null)
            {
                dictionary = new Dictionary<string, float>();

                for (int i = 0; i < setup.Count; ++i)
                {
                    KeyValuePair<string, object> pair = setup.ElementAt(i);

                    string property = pair.Key;

                    if (!GetIsIgnoredProperty(property))
                    {
                        float value = Convert.ToSingle(pair.Value);
                        int durationFrame = frame - delayFrames;

                        dictionary[property] = float.NaN;

                        float t = durationFrame * timescale;
                        float b = beginValues[property];
                        float c = value - b;

                        if (durationFrame < durationFrames - 1)
                            dictionary[property] = this.ease(t, b, c, duration);
                        else
                            dictionary[property] = value;
                    }
                }
            }

            return dictionary;
        }

        /**
		 * Public interface.
		 */

        public void Reset()
        {
            frame = 0;
            hasCompleted = false;
            beginValues = null;
        }

        public void Kill()
        {
            frame = this.totalFrames - 1;
            updateCurrentFrameProperties();
        }

        public void Update()
        {
            updateFrame();
            updateEventsAndBeginValues();
            updateCurrentFrameProperties();
        }

        /**
		 * Private interface.
		 */

        private void updateEventsAndBeginValues()
        {
            if (start && beginValues == null)
            {
                initBeginValues();
                InvokeStart();
            }
            else
            if (beginValues != null)
                InvokeUpdate();

            if (complete)
                InvokeComplete();
        }

        private void initBeginValues()
        {
            beginValues = new Dictionary<string, float>();

            for (int i = 0; i < setup.Count; ++i)
            {
                KeyValuePair<string, object> pair = setup.ElementAt(i);
                string name = pair.Key;

                if (!GetIsIgnoredProperty(name))
                {
                    object value = GetObjectValue(target, name);
                    beginValues.Add(name, Convert.ToSingle(value));
                }
            }
        }

        private void updateCurrentFrameProperties()
        {
            Dictionary<string, float> dictionary = GetDictionaryAtFrame(frame);

            if (dictionary != null)
            {
                for (int i = 0; i < dictionary.Count; ++i)
                {
                    KeyValuePair<string, float> pair = dictionary.ElementAt(i);
                    SetObjectValue(target, pair.Key, pair.Value);
                }
            }
        }

        private void updateFrame()
        {
            frame++;
        }
    }
}