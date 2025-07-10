using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class EyeHazardController : MonoBehaviour
{
    [SerializeField] float TimeToOpen = 1f;
    [SerializeField] float TimeToClose = 1f;
    [SerializeField] float TimeStaysOpen = 1f;
    [SerializeField] float TimeStaysClosed = 2f;
    
    [Space(10)]
    [SerializeField] bool IsLightMoving = false;
    [Space(5)]
    [SerializeField] GameObject LightObject;
    [SerializeField] GameObject FollowObject;
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

    // Update is called once per frame
    void Update()
    {
        if(!IsLightMoving)
        {
            return;
        }

        LightObject.transform.LookAt(FollowObject.transform, Vector3.up);

        //if(LightAxis == Direction.Horizontal)
        //{
        //    //LightObject.transform.Rotate(Vector3.right * LightMoveSpeed * Time.deltaTime);
        //}
        //else
        //{
        //    LightObject.transform.Rotate(Vector3.up * LightMoveSpeed * Time.deltaTime);
        //}
    }
}

[CustomEditor(typeof(EyeHazardController))]
public class EyeHazardControllerEditor : Editor
{
    SerializedProperty isLightMovingProp;
    SerializedProperty lightObjectProp;
    SerializedProperty followObjectProp;
    SerializedProperty lightMoveSpeedProp;
    SerializedProperty lightMoveDistanceProp;
    SerializedProperty lightStopTimeProp;
    SerializedProperty lightAxisProp;

    void OnEnable()
    {
        isLightMovingProp = serializedObject.FindProperty("IsLightMoving");
        lightObjectProp = serializedObject.FindProperty("LightObject");
        followObjectProp = serializedObject.FindProperty("FollowObject");
        lightMoveSpeedProp = serializedObject.FindProperty("LightMoveSpeed");
        lightMoveDistanceProp = serializedObject.FindProperty("LightMoveDistance");
        lightStopTimeProp = serializedObject.FindProperty("LightStopTime");
        lightAxisProp = serializedObject.FindProperty("LightAxis");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw all fields except the movement ones
        DrawPropertiesExcluding(serializedObject, "LightObject", "FollowObject", "LightMoveSpeed", "LightMoveDistance", "LightStopTime", "LightAxis");

        // Draw movement fields only if IsLightMoving is true
        if (isLightMovingProp.boolValue)
        {
            EditorGUILayout.PropertyField(lightObjectProp);
            EditorGUILayout.PropertyField(followObjectProp);
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
