// Decompiled with JetBrains decompiler
// Type: ImageEditor.Properties.Resources
// Assembly: ImageEditor, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC5FD-5783-4571-A04C-A506D4C08D16
// Assembly location: Z:\User\Desktop\repository\ImageEditor_v3.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace ImageEditor.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (ImageEditor.Properties.Resources.resourceMan == null)
          ImageEditor.Properties.Resources.resourceMan = new ResourceManager("ImageEditor.Properties.Resources", typeof (ImageEditor.Properties.Resources).Assembly);
        return ImageEditor.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => ImageEditor.Properties.Resources.resourceCulture;
      set => ImageEditor.Properties.Resources.resourceCulture = value;
    }
  }
}
