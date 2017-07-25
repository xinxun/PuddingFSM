
using LitJson;
using UnityEngine;

namespace VL.VLFSMSystem
{//行为基类
    public abstract class VLEventBase : MonoBehaviour, IVLEvent
    {
        [System.Serializable]
        public class VLParamBase
        {
            public string changeStateGUID = "";//状态的迁移ID 
        }
        public string eventGUID = "";
        public string stateMachineGUID = "";
        public string stateGUID = "";

        public JsonData m_JsonDataParam = null;//json参数

        public VLParamBase pParamBase = null;//从json转换过来的参数
        public virtual VLEventType.EVLEVENTTYPE GetEventType()
        {
            return VLEventType.EVLEVENTTYPE.EVLEVENTTYPE_BASE;
        }

        public virtual bool Init(string uStateMachineId, string uStateId, string uEventId, JsonData pJsonData)
        {
            stateMachineGUID = uStateMachineId;
            stateGUID = uStateId;
            eventGUID = uEventId;
            m_JsonDataParam = pJsonData;
            pParamBase = GetParam<VLParamBase>();
            return true;
        }

        public virtual JsonData GetParamData()
        {
            return Param2Json<VLParamBase>(pParamBase);
        }

        public virtual VLParamBase GetParamBase()
        {
            return pParamBase;
        }

        public string GetEventId()
        {
            return eventGUID;
        }

        public bool Play()
        {
            VLActionObject pVLActionObject = GetCurVLActionObject();
            if (pVLActionObject == null)
            {
                return false;
            }
            return pVLActionObject.PlayState(pParamBase.changeStateGUID);  
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

        protected VLActionObject GetCurVLActionObject()
        {
            return gameObject.GetComponentInParent<VLActionObject>();
        }
    }
}
