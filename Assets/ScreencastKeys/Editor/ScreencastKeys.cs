
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
using UnityEngine;
using UnityEngine.UIElements;
using UnityRawInput;

public class ScreencastKeys : EditorWindow
{
    private Label KeycastLabel { get; set; }

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
    }

    private void OnDisable()
    {
        RawKeyInput.OnKeyDown -= HandleKeyDown;
        RawKeyInput.Stop();
    }

    private void HandleKeyDown(RawKey key)
    {
        KeycastLabel.text = key.ToString();
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
