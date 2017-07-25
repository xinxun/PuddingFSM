using LitJson;
using System;

//状态机数据
namespace VL.VLFSMSystem
{
    [Serializable]
    public class VLEventData
    {
        public string eventGUID;//行为ID
        public string eventName;//行为名字
        public string eventType;//行为类型
        private JsonData m_JsonDataParam;//相关参数配置(warning 这里有一个引用)
        public VLEventData(VLEventType.EVLEVENTTYPE eEventType, string strEventGUID, string strNameParam, JsonData jsonDataParam)
        {
            eventType = eEventType.ToString();
            eventGUID = strEventGUID;
            eventName = strNameParam;
            m_JsonDataParam = jsonDataParam;
        }

        public void SetJsonDataParam(JsonData pJsonData)
        {
            m_JsonDataParam = pJsonData;
        }

        public JsonData GetJsonDataParam()
        {
            return m_JsonDataParam;
        }
    }
}
