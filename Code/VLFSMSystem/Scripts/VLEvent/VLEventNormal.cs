
using LitJson;

namespace VL.VLFSMSystem
{//行为基类
    public class VLEventNormal : VLEventBase
    {
        public override VLEventType.EVLEVENTTYPE GetEventType()
        {
            return VLEventType.EVLEVENTTYPE.EVLEVENTTYPE_NORMAL;
        } 

    }
}
