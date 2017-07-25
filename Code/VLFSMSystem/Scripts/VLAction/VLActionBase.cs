using LitJson;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace VL.VLFSMSystem
{//行为基类
    public abstract class VLActionBase : MonoBehaviour, IVLAction
    {
        public VLParamBase m_pParamBase = null;
        //参数基类
        [System.Serializable]
        public class VLParamBase : IVLActionParam
        {
            //            public string strName = "";
            public Dictionary<string, string> dictUseSMParam = new Dictionary<string, string>();
            public bool bSyncGoOn = false;   //依次播放的行为链：提供这个标志给用户，用于指定某个行为被执行后，是否同步地执行下一个行为（前提是该行为支持）
        }
        public static readonly string ParamDictName = "dictUseSMParam";
        public string actionGUID = "";
        public string stateMachineGUID = "";
        public string stateGUID = "";
        public JsonData m_JsonDataParam = null;//json参数
        public virtual VLActionType.EVLACTIONTYPE GetActionType()
        {
            return VLActionType.EVLACTIONTYPE.EVLACTIONTYPE_BASE;
        }

        public virtual bool Init(string strStateMachineGUID, string strStateGUID, string strActionGUID, JsonData pJsonData)
        {
            stateMachineGUID = strStateMachineGUID;
            stateGUID = strStateGUID;
            actionGUID = strActionGUID;
            m_JsonDataParam = pJsonData;
            m_pParamBase = GetParam<VLParamBase>();
            if (m_pParamBase == null)
            {
                m_pParamBase = new VLParamBase();
            }

            return true;
        }

        //必须重载
        public virtual JsonData GetParamData()
        {
            m_pParamBase = new VLParamBase();
            if (m_pParamBase == null)
            {
                return null;
            }
            return Param2Json<VLParamBase>(m_pParamBase);
        }

        public abstract IVLActionParam GetActionParam();

        public string GetActionId()
        {
            return actionGUID;
        }

        public virtual bool IsEnd()
        {
            return true;
        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnExit()
        {

        }

        void Update()
        {

        }

        object ParseTypeValue(Type type, string value)
        {
            object ret = null;
            if (type == typeof(bool))
            {
                ret = bool.Parse(value);
            }
            else if (type == typeof(int))
            {
                ret = int.Parse(value);
            }
            else if (type == typeof(float))
            {
                ret = float.Parse(value);
            }
            else if (type == typeof(long))
            {
                ret = long.Parse(value);
            }
            else if (type == typeof(string))
            {
                ret = value;
            }
            return ret;
        }

        protected T ModifyValueBySMParam<T>(T param) where T : new()
        {
            VLParamBase pmBase = param as VLParamBase;
            if (pmBase == null)
            {
                return param;
            }
            foreach (var pair in pmBase.dictUseSMParam)
            {
                FieldInfo fi = param.GetType().GetField(pair.Key);
                if (fi == null)
                {
                    continue;
                }
                VLStateMachine sm = VLFSMManager.GetInstance().GetStateMachineByGUID(stateMachineGUID);
                if (sm == null)
                {
                    continue;
                }
                var pmmgr = sm.GetParamMgr();
                if (pmmgr == null)
                {
                    continue;
                }
                VLSMParam pm = pmmgr.GetParamByName(pair.Value);
                if (pm == null  ||  pm.pParamBase == null)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(pm.pParamBase.strValue))
                {
                    continue;
                }
                object value = ParseTypeValue(fi.FieldType, pm.pParamBase.strValue);
                fi.SetValue(param, value);
            }
            return param;
        }

        protected T GetParam<T>() where T : new()
        {
            if (m_JsonDataParam != null)
            {
                T ret = JsonUtility.FromJson<T>(m_JsonDataParam.ToJson());
                VLParamBase pmBase = ret as VLParamBase;
                if (pmBase != null)
                {
                    ParamDictFromJson(ref pmBase, m_JsonDataParam);
                }
                ret = ModifyValueBySMParam(ret);
                return ret;
            }
            T obj = new T();
            return obj;
        }

        void ParamDictFromJson(ref VLParamBase _paramBase, JsonData jsonData)
        {
            if (_paramBase == null  ||  !jsonData.ContainsKey(ParamDictName))
            {
                return;
            }
            JsonData jdDict = jsonData[ParamDictName];
            if (jdDict.GetJsonType() != JsonType.Object)
            {
                return;
            }
            foreach (KeyValuePair<string, JsonData> pair in jdDict)
            {
                JsonData jd = pair.Value;
                _paramBase.dictUseSMParam[pair.Key] = jd.ToString();
            }
        }

        JsonData ParamDictToJson(VLParamBase paramBase, ref JsonData jsonData)
        {
            if (paramBase == null  || paramBase.dictUseSMParam == null)
            {
                return jsonData;
            }
            JsonData jdDict = JsonData.CreateEmptyObj();
            jdDict.SetJsonType(JsonType.Object);
            foreach (var pair in paramBase.dictUseSMParam)
            {
                jdDict.Add(pair.Key, pair.Value);
            }
            jsonData[ParamDictName] = jdDict;
            return jsonData;
        }

        protected JsonData Param2Json<T>(T pParamData)
        {
            if (m_pParamBase == null)
            {
                return "";
            }
            string strParamJson = JsonUtility.ToJson(pParamData);
            JsonData jd = JsonMapper.ToObject(strParamJson);
            m_pParamBase = pParamData as VLParamBase;
            if (m_pParamBase != null)
            {
                ParamDictToJson(m_pParamBase, ref jd);
            }
            return jd;
        }


        protected VLActionObject GetCurVLActionObject()
        {
            return gameObject.GetComponentInParent<VLActionObject>();
        }

    }
}
