// Decompiled with JetBrains decompiler
// Type: TempAR.Properties.Resources
// Assembly: TempAR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C3F8A34-0260-4D18-8C4B-520ED4C1EC88
// Assembly location: D:\ROMs & ISOs\Vita Schtuff\pointer_searcher\TempAR.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace TempAR.Properties
{
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [CompilerGenerated]
    [DebuggerNonUserCode]
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
                if (object.ReferenceEquals((object)TempAR.Properties.Resources.resourceMan, (object)null))
                    TempAR.Properties.Resources.resourceMan = new ResourceManager("TempAR.Properties.Resources", typeof(TempAR.Properties.Resources).Assembly);
                return TempAR.Properties.Resources.resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return TempAR.Properties.Resources.resourceCulture;
            }
            set
            {
                TempAR.Properties.Resources.resourceCulture = value;
            }
        }
    }
}