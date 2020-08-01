
//
// This source file is part of UnityRawInput.
// Visit https://github.com/Elringus/UnityRawInput for additional information and resources.
//
// MIT License
//
// Copyright(c) 2018 Artyom Sovetnikov
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

using System;
using System.Runtime.InteropServices;

namespace UnityRawInput
{
    public static class Win32API
    {
        public delegate int HookProc (int code, IntPtr wParam, IntPtr lParam);

        [DllImport("User32")]
        public static extern IntPtr SetWindowsHookEx (HookType code, HookProc func, IntPtr hInstance, int threadID);
        [DllImport("User32")]
        public static extern int UnhookWindowsHookEx (IntPtr hhook);
        [DllImport("User32")]
        public static extern int CallNextHookEx (IntPtr hhook, int code, IntPtr wParam, IntPtr lParam);
        [DllImport("Kernel32")]
        public static extern uint GetCurrentThreadId ();
        [DllImport("Kernel32")]
        public static extern IntPtr GetModuleHandle (string lpModuleName);
    }
}
