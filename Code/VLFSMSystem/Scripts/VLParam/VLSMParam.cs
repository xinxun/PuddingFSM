
using LitJson;
using UnityEngine;

namespace VL.VLFSMSystem
{//参数
    public class VLSMParam : MonoBehaviour
    {
        private string m_strStateMachineGUID = "";//所属状态机id
        private string m_strParamGUID = "";
        public VLParamInfo pParamBase = null;
        public JsonData m_JsonDataParam = null;//json参数

        [System.Serializable]
        public class VLParamInfo
        {
            public string strName = "";
            public string strType = "";
            public string strValue = "";
            public string strShowName = "";
            public string strDesc = string.Empty;
            public bool bShow = false;
        }

        public virtual bool Init(string strStateMachineGUID, string uParamId, JsonData pJsonData)
        {
            m_strStateMachineGUID = strStateMachineGUID;
            if (m_strStateMachineGUID.Length <= 0)
            {
                return false;
            }
            m_strParamGUID = uParamId;
            m_JsonDataParam = pJsonData;
            pParamBase = GetParam<VLParamInfo>();
            if (pParamBase == null)
            {
                pParamBase = new VLParamInfo();
            }
            return true;
        }

        public VLParamInfo GetParamInfo()
        {
            return pParamBase;
        }

        //必须重载
        public virtual JsonData GetParamData()
        {
            if (pParamBase == null)
            {
                return null;
            }
            return Param2Json<VLParamInfo>(pParamBase);
        }

        public string GetParamId()
        {
            return m_strParamGUID;
        }

        public string GetParamName()
        {
            if (pParamBase == null)
            {
                return string.Empty;
            }
            return pParamBase.strName;
        }

        public string GetStateMachineGUID()
        {
            return m_strStateMachineGUID;
        }
        
        protected T GetParam<T>() where T : new()
        {
            if (m_JsonDataParam != null)
            {
                return JsonUtility.FromJson<T>(m_JsonDataParam.ToJson());
            }
            T obj = new T();
            return obj;
        }

        protected JsonData Param2Json<T>(T pParamData)
        {
            if (pParamBase == null)
            {
                return "";
            }
            string strParamJson = JsonUtility.ToJson(pParamData);
            return JsonMapper.ToObject(strParamJson);
        }

    }
}
