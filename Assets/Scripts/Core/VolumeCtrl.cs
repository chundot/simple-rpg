using RPG.Resx;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RPG.Core
{
  public class VolumeCtrl : MonoBehaviour
  {
    Volume _volume;
    ColorAdjustments _colorAdjustment;
    ChromaticAberration _chromaticAberration;
    void Awake()
    {
      _volume = GetComponent<Volume>();
      _volume.profile.TryGet(out _colorAdjustment);
      _volume.profile.TryGet(out _chromaticAberration);
    }

    public void AdjustVolumeByHealthPercentage(float healthPercentage)
    {
      if (healthPercentage < 60)
      {
        _colorAdjustment.saturation.SetValue(new FloatParameter(healthPercentage - 100, true));
        _chromaticAberration.intensity.SetValue(new FloatParameter(1 - healthPercentage / 100, true));
      }
      else
      {
        _colorAdjustment.saturation.SetValue(new FloatParameter(0, true));
        _chromaticAberration.intensity.SetValue(new FloatParameter(0, true));
      }
    }
  }

}