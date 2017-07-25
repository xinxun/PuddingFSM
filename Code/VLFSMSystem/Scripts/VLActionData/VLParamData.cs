using System;
using System.Collections.Generic;
using LitJson;

//状态机数据
namespace VL.VLFSMSystem
{
    [Serializable]
    public class VLParamData
    {
        public string paramGUID;//状态ID
        public string paramName;      //状态名字   
        private JsonData m_JsonDataParam;//相关参数配置(warning 这里有一个引用)
        public VLParamData(string strParamGUID, string strNameParam, JsonData jsonDataParam)
        {
            paramGUID = strParamGUID;
            paramName = strNameParam;
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
