using UnityEngine;

public static class Util
{
    public static bool Approximately(Quaternion quatA, Quaternion value, float acceptableRange)
    {
        return 1 - Mathf.Abs(Quaternion.Dot(quatA, value)) < acceptableRange;
    }
}
