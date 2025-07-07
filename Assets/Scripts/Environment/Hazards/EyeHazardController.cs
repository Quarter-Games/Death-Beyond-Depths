using UnityEditor;
using UnityEngine;

public class EyeHazardController : MonoBehaviour
{
    [SerializeField] readonly float TimeToOpen = 1f;
    [SerializeField] readonly float TimeToClose = 1f;
    [SerializeField] readonly float TimeStaysOpen = 1f;
    [SerializeField] readonly float TimeStaysClosed = 2f;

    [SerializeField] bool IsLightMoving = false;
    [SerializeField] GameObject LightObject;
    [SerializeField] float LightMoveSpeed = 1f;
    [SerializeField] float LightMoveDistance = 1f;
    [SerializeField] float LightStopTime = 1f;
    [SerializeField] Direction LightAxis = Direction.Horizontal;

    private void OnValidate()
    {
        if(LightObject == null)
        {
            LightObject = gameObject.GetComponentInChildren<Light>().gameObject;
        }
    }

    private void Start()
    {
        if (IsLightMoving)
        {
            // lets us rotate the light horizontally or vertically based on the LightAxis enum
            if (LightAxis == Direction.Horizontal)
            {
                LightObject.transform.rotation =
                    new Quaternion(LightObject.transform.rotation.x, -90f, LightObject.transform.rotation.z, LightObject.transform.rotation.w);
                return;
            }
            LightObject.transform.rotation =
                    new Quaternion(LightObject.transform.rotation.x, 0f, LightObject.transform.rotation.z, LightObject.transform.rotation.w);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(!IsLightMoving)
        {
            return;
        }
    }
}

[CustomEditor(typeof(EyeHazardController))]
public class EyeHazardControllerEditor : Editor
{
    SerializedProperty isLightMovingProp;
    SerializedProperty lightObjectProp;
    SerializedProperty lightMoveSpeedProp;
    SerializedProperty lightMoveDistanceProp;
    SerializedProperty lightStopTimeProp;
    SerializedProperty lightAxisProp;

    void OnEnable()
    {
        isLightMovingProp = serializedObject.FindProperty("IsLightMoving");
        lightObjectProp = serializedObject.FindProperty("LightObject");
        lightMoveSpeedProp = serializedObject.FindProperty("LightMoveSpeed");
        lightMoveDistanceProp = serializedObject.FindProperty("LightMoveDistance");
        lightStopTimeProp = serializedObject.FindProperty("LightStopTime");
        lightAxisProp = serializedObject.FindProperty("LightAxis");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw all fields except the movement ones
        DrawPropertiesExcluding(serializedObject, "LightObject", "LightMoveSpeed", "LightMoveDistance", "LightStopTime", "LightAxis");

        // Draw movement fields only if IsLightMoving is true
        if (isLightMovingProp.boolValue)
        {
            EditorGUILayout.PropertyField(lightObjectProp);
            EditorGUILayout.PropertyField(lightMoveSpeedProp);
            EditorGUILayout.PropertyField(lightMoveDistanceProp);
            EditorGUILayout.PropertyField(lightStopTimeProp);
            EditorGUILayout.PropertyField(lightAxisProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}


enum Direction
{
    Horizontal,
    Vertical
}
