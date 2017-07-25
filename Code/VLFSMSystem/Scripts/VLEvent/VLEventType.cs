
using System;
using System.ComponentModel;

namespace VL.VLFSMSystem
{
    public class VLEventType
    {
        public enum EVLEVENTTYPE
        {
            [Description("BASE")]
            EVLEVENTTYPE_BASE =0,

            //<不同Event定义>===========begin

            [Description("NORMAL")]
            EVLEVENTTYPE_NORMAL,  //普通事件

            [Description("ONFOCUS")]
            EVLEVENTTYPE_FOCUS,//对象选定事件

            [Description("ONFINISH")]
            EVLEVENTTYPE_ONFINISH,//结束事件

            [Description("ONDETECT")]
            EVLEVENTTYPE_ONDETECT,//检测事件

            /// </不同Event定义>==========end
            [Description("NOT NAME")]
            EVLEVENTTYPE_NULL,
        }

        static public EVLEVENTTYPE GetEventByType(string strTypeName)
        {
            return (EVLEVENTTYPE)Enum.Parse(typeof(EVLEVENTTYPE), strTypeName);
        }

        static public string GetEventName(EVLEVENTTYPE eEventType)
        {
            string strInfo = eEventType.ToString();
            System.Reflection.FieldInfo field = eEventType.GetType().GetField(strInfo);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0)
            {
                return "NOT NAME";
            }

            DescriptionAttribute da = (DescriptionAttribute)objs[0];
            return da.Description;
        }
    }
}
