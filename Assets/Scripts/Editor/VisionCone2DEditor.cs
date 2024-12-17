using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy), true)]
[CanEditMultipleObjects]
public class VisionCone2DEditor : Editor
{
    private void OnSceneGUI()
    {
        Enemy cone = (Enemy)target;

        // Get the cone's transform
        Transform transform = cone.transform;
        Handles.color = Color.blue;
        // Draw a handle to adjust the radius
        Vector3 radiusHandlePosition = transform.position + (Vector3.right * cone.Radius);
        EditorGUI.BeginChangeCheck();
        Vector3 newRadiusHandlePosition = Handles.FreeMoveHandle(
            radiusHandlePosition,
            0.1f,
            Vector3.zero,
            Handles.SphereHandleCap
        );
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cone, "Change Vision Cone Radius");
            cone.Radius = Vector3.Distance(transform.position, newRadiusHandlePosition);
        }

        // Draw a handle to adjust the angle
        Vector3 startAngleDirection = Quaternion.Euler(0, 0, -cone.Angle / 2) * Vector3.right;
        Vector3 startAngleHandlePosition = transform.position + startAngleDirection * cone.Radius;
        EditorGUI.BeginChangeCheck();
        Vector3 newStartAngleHandlePosition = Handles.FreeMoveHandle(
            startAngleHandlePosition,
            0.1f,
            Vector3.zero,
            Handles.SphereHandleCap
        );
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(cone, "Change Vision Cone Angle");
            Vector3 adjustedDirection = (newStartAngleHandlePosition - transform.position).normalized;
            float newAngle = Vector3.SignedAngle(Vector3.right, adjustedDirection, Vector3.forward) * 2f;
            cone.Angle = Mathf.Clamp(newAngle, 0f, 360f);
        }

        // Draw the vision cone arc again for real-time feedback
        Handles.color = new Color(1f, 1f, 0f, 0.2f);
        Handles.DrawSolidArc(transform.position, Vector3.forward, startAngleDirection, cone.Angle, cone.Radius);

        // Force Scene view to repaint
        SceneView.RepaintAll();
    }
}
