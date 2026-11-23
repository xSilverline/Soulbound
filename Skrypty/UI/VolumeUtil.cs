using UnityEngine;

namespace UI
{
    public class VolumeUtil : MonoBehaviour
    {
        public const float MIN_DECIBEL = -80f;

        public static float LinearToDecibel(float linear)
        {
            linear = Mathf.Clamp01(linear);
            if (linear <= 0.0001f) return MIN_DECIBEL;
            return Mathf.Log10(linear) * 20f;
        }
    }
}
