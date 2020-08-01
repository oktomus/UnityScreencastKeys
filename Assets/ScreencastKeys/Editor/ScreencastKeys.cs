using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityRawInput;

public class ScreencastKeys : EditorWindow
{
    [MenuItem("Window/UIElements/ScreencastKeys")]
    public static void ShowExample()
    {
        ScreencastKeys window = GetWindow<ScreencastKeys>();
        window.titleContent = new GUIContent("ScreencastKeys");
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(UXML_FILE_GUID));
        VisualElement labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(AssetDatabase.GUIDToAssetPath(USS_FILE_GUID));
        VisualElement labelWithStyle = new Label("Hello World! With Style");
        labelWithStyle.styleSheets.Add(styleSheet);
        root.Add(labelWithStyle);

        // Listen to keyboard events.
        RawKeyInput.Start(false);
        RawKeyInput.OnKeyDown += HandleKeyDown;
    }

    private void OnDisable()
    {
        RawKeyInput.OnKeyDown -= HandleKeyDown;
        RawKeyInput.Stop();
    }

    private void HandleKeyDown(RawKey key)
    {
        Debug.Log("Keydown : " + key.ToString());
    }

    //
    // Assets GUID.
    //
    // There is only 2 ways that I know of to load an asset:
    // - Using its path
    // - Using its GUID to get its path
    //
    // Because we are not sure of where people will place the plugin in their
    // project, it's safer to use GUID.
    //

    /// <summary>
    /// Assets\ScreencastKeys\Editor\ScreencastKeys.uss
    /// </summary>
    private const string USS_FILE_GUID = "45355e9042d77824d8830b3440d77ed0";
    /// <summary>
    /// Assets\ScreencastKeys\Editor\ScreencastKeys.uxml
    /// </summary>
    private const string UXML_FILE_GUID = "cc1491a61f073744f8ec098421246ef6";
}
