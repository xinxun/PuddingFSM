
using LitJson;

namespace VL.VLFSMSystem
{
    //行为接口(提供外部接口)
    public interface IVLEvent
    {
        VLEventType.EVLEVENTTYPE GetEventType();
        //初始化
        bool Init(string uStateMachineId, string uStateId, string uEventId, JsonData pJsonData);
        //得到当前Eventid
        string GetEventId();
        //事件运行
        bool Play();
        //保存数据时使用
        JsonData GetParamData();
    }
}
