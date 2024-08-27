// Decompiled with JetBrains decompiler
// Type: ImageEditor.Program
// Assembly: ImageEditor, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC5FD-5783-4571-A04C-A506D4C08D16
// Assembly location: Z:\User\Desktop\repository\ImageEditor_v3.exe

using System;
using System.Windows.Forms;

#nullable disable
namespace ImageEditor
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new Form1());
    }
  }
}
