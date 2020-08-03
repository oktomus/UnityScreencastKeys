
//
// This source file is part of UnityScreencastKeys.
// Visit https://github.com/oktomus/UnityScreencastKeys for additional information and resources.
//
// MIT License
// 
// Copyright(c) 2020 Kevin Masson
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

using UnityEditor;
using UnityEngine.UIElements;
using UnityRawInput;

public class ScreencastKeys : EditorWindow
{
    private Label KeycastLabel { get; set; }

    private bool IsCtrl { get; set; }
    private bool IsAlt { get; set; }
    private bool IsShift { get; set; }

    [MenuItem("Window/ScreencastKeys")]
    public static void ShowExample()
    {
        ScreencastKeys window = GetWindow<ScreencastKeys>();
        window.Show();
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(UXML_FILE_GUID));
        VisualElement labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);

        // Import USS
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(AssetDatabase.GUIDToAssetPath(USS_FILE_GUID));
        root.styleSheets.Add(styleSheet);

        KeycastLabel = new Label("...");
        root.Add(KeycastLabel);

        // Listen to keyboard events.
        RawKeyInput.Start(false);
        RawKeyInput.OnKeyDown += HandleKeyDown;
        RawKeyInput.OnKeyUp += HandleKeyUp;
    }

    private void OnDisable()
    {
        RawKeyInput.OnKeyDown -= HandleKeyDown;
        RawKeyInput.OnKeyUp -= HandleKeyUp;

        IsCtrl = IsAlt = IsShift = false;

        RawKeyInput.Stop();
    }

    private void HandleKeyDown(RawKey key)
    {
        // Activate modifiers.
        IsCtrl = IsCtrl || (key == RawKey.Control || key == RawKey.LeftControl || key == RawKey.RightControl);
        IsShift = IsShift || (key == RawKey.Shift || key == RawKey.LeftShift || key == RawKey.RightShift);
        IsAlt = IsAlt || (key == RawKey.Menu || key == RawKey.LeftMenu || key == RawKey.RightMenu);

        KeycastLabel.text = RawKeyToPrettyString(key);
    }

    private void HandleKeyUp(RawKey key)
    {
        // Deactivate modifiers.
        if (IsCtrl && (key == RawKey.Control || key == RawKey.LeftControl || key == RawKey.RightControl))
        { IsCtrl = false; }
        if (IsShift && (key == RawKey.Shift || key == RawKey.LeftShift || key == RawKey.RightShift))
        { IsShift = false; }
        if (IsAlt && (key == RawKey.Menu || key == RawKey.LeftMenu || key == RawKey.RightMenu))
        { IsAlt = false; }
    }

    private string RawKeyToPrettyString(RawKey key)
    {
        string keyString = key.ToString();

        // Special handle : numpads.
        if (key >= RawKey.Numpad0 && key <= RawKey.Numpad9)
        {
            keyString = keyString.Replace("Numpad", "");
        }

        // Special handle : maths.
        if (key == RawKey.Divide) { keyString = "/"; }
        else if (key == RawKey.Multiply) { keyString = "*"; }
        else if (key == RawKey.Subtract) { keyString = "-"; }
        else if (key == RawKey.Add) { keyString = "+"; }
        else if (key == RawKey.Decimal) { keyString = "."; }

        // Special handle : modifiers.
        if (key == RawKey.Control || key == RawKey.LeftControl || key == RawKey.RightControl
            || key == RawKey.Shift || key == RawKey.LeftShift || key == RawKey.RightShift
            || key == RawKey.Menu || key == RawKey.LeftMenu || key == RawKey.RightMenu)
        { keyString = ""; }

        // Prefix with modifiers.
        string prefix =
            IsCtrl && IsShift && IsAlt ? "Ctrl + Shift + Alt" :
            IsCtrl && IsAlt ? "Ctrl + Alt" :
            IsCtrl && IsShift ? "Ctrl + Shift" :
            IsCtrl ? "Ctrl" :
            IsShift && IsAlt ? "Shift + Alt" :
            IsShift ? "Shift" :
            IsAlt ? "Alt" :
            "";

        if (!string.IsNullOrEmpty(keyString) && !string.IsNullOrEmpty(prefix))
        { prefix += " + "; }

        return prefix + keyString;
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
