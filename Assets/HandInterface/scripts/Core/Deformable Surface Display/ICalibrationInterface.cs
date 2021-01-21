namespace HPUI.Core.DeformableSurfaceDisplay
{
    public interface ICalibrationInterface
    {
        bool isCalibrated();
        void Calibrate(); // expected to call the respective generatemesh from the PlaneMeshGenerator
    }
}
