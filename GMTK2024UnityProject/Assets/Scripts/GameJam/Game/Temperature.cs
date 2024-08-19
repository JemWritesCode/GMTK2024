namespace GameJam {
  public sealed class Temperature {
    public int Heat = 0;
    public int HeatLimit = 100;
    public int HeatTicks = 5;

    public float TemperatureThreshold = 0.8f;

    public float HeatPercent() {
      return Heat / (float) HeatLimit;
    }

    public bool HeatReachingCritical() {
      return (Heat / (float) HeatLimit) >= TemperatureThreshold;
    }

    public bool Overheated() {
      return Heat >= HeatLimit;
    }

    public void UpdateHeat() {
      Heat += HeatTicks;
    }

    public void LowerHeat(int heat) {
      Heat -= heat;
      if (Heat < 0) {
        Heat = 0;
      }
    }

    public void ResetHeat() {
      Heat = 0;
    }
  }
}
