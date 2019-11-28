using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace HeadphoneMonitorApp
{
    public enum VolumeAction
    {
        Mute,
        Unmute,
        None,
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ProcessPriority
    {
        [Description("Low")]
        Low = ProcessPriorityClass.Idle,

        [Description("Below Normal")]
        BelowNormal = ProcessPriorityClass.BelowNormal,

        [Description("Normal")]
        Normal = ProcessPriorityClass.Normal,

        [Description("Above Normal")]
        AboveNormal = ProcessPriorityClass.AboveNormal,

        [Description("High")]
        High = ProcessPriorityClass.High,

        //[Description("Real Time")]
        //RealTime = ProcessPriorityClass.RealTime,
    }
}
