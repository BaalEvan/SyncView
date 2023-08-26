using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), displayName: Name, id: ID)]
public class SceneViewSync : ToolbarOverlay
{
    private const string Name = "Sync UI";
    private const string ID = "Sync-UI";
    public const string SyncViewBoolFlag = "SyncViewEnabled";
    public const string Icon = "d_Refresh";
    public SceneViewSync() : base(SyncViewToggle.id)
    {
    }

}
[Icon(SceneViewSync.Icon)]

[EditorToolbarElement(id, typeof(SceneView))]
class SyncViewToggle : EditorToolbarToggle
{
    public const string id = "SyncView/Toggle";


    public SyncViewToggle()
    {
        text = "Toggle OFF";
        this.onIcon =  EditorGUIUtility.FindTexture(SceneViewSync.Icon);
        offIcon =  EditorGUIUtility.FindTexture(SceneViewSync.Icon);
        this.RegisterValueChangedCallback(Test);

        if (EditorPrefs.GetBool(SceneViewSync.SyncViewBoolFlag))
        {
            this.ToggleValue();
            EditorApplication.update += UpdatePosition;

        }
    }

    void Test(ChangeEvent<bool> evt)
    {

        if (evt.newValue)
        {
            Debug.Log("ON");
            text = "Toggle ON";
            EditorApplication.update += UpdatePosition;
            EditorPrefs.SetBool(SceneViewSync.SyncViewBoolFlag,true);
        }
        else
        {
            Debug.Log("OFF");
            text = "Toggle OFF";
            EditorApplication.update -= UpdatePosition;
            EditorPrefs.SetBool(SceneViewSync.SyncViewBoolFlag,false);

        }
    }

    void UpdatePosition()
    {
        var cam = Camera.main;
        var campos = cam.transform.position;
        var camrot = cam.transform.rotation;
        SceneView.lastActiveSceneView.FixNegativeSize();
        SceneView.lastActiveSceneView.pivot = campos + cam.transform.forward;
        SceneView.lastActiveSceneView.rotation = camrot;

    }
}