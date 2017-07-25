using LitJson;
using System;

//状态机数据
namespace VL.VLFSMSystem
{
    [Serializable]
    public class VLActionData
    {
        public string actionGUID;//行为ID
        public string actionName;//行为名字
        public string actionType;//行为类型
        private JsonData m_JsonDataParam;//相关参数配置(warning 这里有一个引用)
        public VLActionData(VLActionType.EVLACTIONTYPE uActionType, string strActionGUID, string strNameParam, JsonData jsonDataParam)
        {
            actionType = uActionType.ToString();
            actionGUID = strActionGUID;
            actionName = strNameParam;
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
