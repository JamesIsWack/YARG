using System;
using UnityEngine;

namespace YARG.Venue
{
    public class LightManager : MonoSingleton<LightManager>
    {
        public enum LightAnimation
        {
            None = 0,
            ManualCool,
            ManualWarm,
            Dischord,
            Stomp,
            LoopCool,
            LoopWarm,
            Harmony,
            Frenzy,
            Silhouettes,
            Searchlights,
            Sweep,
            StrobeFast,
            StrobeSlow,
            BlackoutFast,
            BlackoutSlow,
            FlareFast,
            FlareSlow,
            BRE,
            Verse,
            Chorus,
        }

        public class LightState
        {
            /// <summary>
            /// The intensity of the light between <c>0</c> and <c>1</c>. <c>1</c> is the default value.
            /// </summary>
            public float Intensity;

            /// <summary>
            /// The color of the light. <see cref="Intensity"/> should be taken into consideration.
            /// <c>null</c> indicates default.
            /// </summary>
            public Color? Color;
        }

        public LightAnimation Animation { get; private set; }
        public int AnimationFrame { get; private set; }

        public LightState MainLightState { get; private set; }

        private float _32NoteUpdate;
        private int _32NoteIndex;

        private void Start()
        {
            MainLightState = new();

            // TODO: FIX
            // VenueManager.OnEventReceive += VenueEvent;
        }

        protected override void SingletonDestroy()
        {
            // TODO: FIX
            // VenueManager.OnEventReceive -= VenueEvent;
        }

        private void Update()
        {
            // TODO: FIX
            // _32NoteUpdate += 1f / Play.Instance.CurrentBeatsPerSecond * Time.deltaTime;

            bool thirtySecondNote = false;
            if (_32NoteUpdate >= 1f / 32f)
            {
                _32NoteIndex++;
                _32NoteUpdate = 0f;
                thirtySecondNote = true;
            }

            UpdateLightAnimation(thirtySecondNote);
            UpdateLightStates();
        }

        private void UpdateLightAnimation(bool thirtySecondNote)
        {
            if (!thirtySecondNote)
            {
                return;
            }

            switch (Animation)
            {
                case LightAnimation.StrobeFast:
                case LightAnimation.Frenzy:
                case LightAnimation.FlareFast:
                case LightAnimation.BRE:
                    AnimationFrame++;
                    break;
                case LightAnimation.StrobeSlow:
                case LightAnimation.FlareSlow:
                    if (_32NoteIndex % 2 == 1)
                    {
                        AnimationFrame++;
                    }

                    break;
            }
        }

        private void UpdateLightStates()
        {
            MainLightState.Color = null;

            switch (Animation)
            {
                case LightAnimation.BlackoutFast:
                    MainLightState.Intensity = Mathf.Lerp(MainLightState.Intensity, 0f, Time.deltaTime * 15f);
                    break;
                case LightAnimation.BlackoutSlow:
                    MainLightState.Intensity = Mathf.Lerp(MainLightState.Intensity, 0f, Time.deltaTime * 10f);
                    break;
                case LightAnimation.Stomp:
                case LightAnimation.FlareSlow:
                case LightAnimation.Dischord:
                case LightAnimation.Searchlights:
                case LightAnimation.Sweep:
                case LightAnimation.Silhouettes:
                case LightAnimation.StrobeFast:
                case LightAnimation.Frenzy:
                case LightAnimation.FlareFast:
                case LightAnimation.BRE:
                case LightAnimation.StrobeSlow:
                    MainLightState.Intensity = AnimationFrame % 2 == 0 ? 1f : 0f;
                    break;
                default:
                    MainLightState.Intensity = 1f;
                    break;
            }
        }

        private void VenueEvent(string eventName)
        {
            if (eventName.StartsWith("venue_lightFrame_"))
            {
                eventName = eventName.Replace("venue_lightFrame_", "");
                switch (eventName)
                {
                    case "next":
                        AnimationFrame++;
                        break;
                    case "previous":
                        AnimationFrame--;
                        break;
                    case "first":
                        AnimationFrame = 0;
                        break;
                }
            }
            else if (eventName.StartsWith("venue_light_"))
            {
                eventName = eventName.Replace("venue_light_", "");
                Animation = eventName switch
                {
                    "manual_cool"   => LightAnimation.ManualCool,
                    "manual_warm"   => LightAnimation.ManualWarm,
                    "dischord"      => LightAnimation.Dischord,
                    "stomp"         => LightAnimation.Stomp,
                    "loop_cool"     => LightAnimation.LoopCool,
                    "loop_warm"     => LightAnimation.LoopWarm,
                    "harmony"       => LightAnimation.Harmony,
                    "frenzy"        => LightAnimation.Frenzy,
                    "silhouettes"   => LightAnimation.Silhouettes,
                    "searchlights"  => LightAnimation.Searchlights,
                    "sweep"         => LightAnimation.Sweep,
                    "strobe_fast"   => LightAnimation.StrobeFast,
                    "strobe_slow"   => LightAnimation.StrobeSlow,
                    "blackout_fast" => LightAnimation.BlackoutFast,
                    "blackout_slow" => LightAnimation.BlackoutSlow,
                    "flare_fast"    => LightAnimation.FlareFast,
                    "flare_slow"    => LightAnimation.FlareSlow,
                    "bre"           => LightAnimation.BRE,
                    "verse"         => LightAnimation.Verse,
                    "chorus"        => LightAnimation.Chorus,
                    _               => LightAnimation.None
                };

                AnimationFrame = 0;
            }
        }
    }
}