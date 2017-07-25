using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace VL.VLFSMSystem
{
    //编辑器中的变量管理器
    public class VLParamDataMgr
    {
        private Dictionary<string, VLParamData> m_dictParam = new Dictionary<string, VLParamData>();

        public void Reset()
        {
            m_dictParam.Clear();
        }

        public VLParamData GetParamDataById(string strParamGUID)
        {
            VLParamData objResult = null;
            if (!m_dictParam.TryGetValue(strParamGUID, out objResult))
            {
                return null;
            }

            return objResult;
        }

        public bool DelParam(string strDelParamGUID)
        {
            if (!m_dictParam.ContainsKey(strDelParamGUID))
            {
                return false;
            }

            return m_dictParam.Remove(strDelParamGUID);
        }

        //添加状态机
        public bool AddParam(string strStateMachineGUID, string strParamGUID, JsonData jsonDataParam)
        {
            VLParamData pParamData = GetParamDataById(strParamGUID);
            if(pParamData != null)
            {
                return false;
            }
            VLParamData newParam = new VLParamData(strParamGUID, strParamGUID, jsonDataParam);
            m_dictParam.Add(newParam.paramGUID, newParam);
            return true;
        }

        public JsonData DataToJson(List<string> guids = null)
        {
            JsonData objResult = JsonData.CreateEmptyObj();
            foreach (KeyValuePair<string, VLParamData> pair in m_dictParam)
            {
                VLParamData objParamData = pair.Value;
                if (null == objParamData)
                {
                    continue;
                }
                if (guids != null  &&  !guids.Contains(pair.Key))
                {
                    continue;
                }
                string strParamJson = JsonUtility.ToJson(objParamData);
                JsonData objData = JsonMapper.ToObject(strParamJson);

                JsonData pJsonDataParam = objParamData.GetJsonDataParam();
                if (pJsonDataParam != null)
                {
                    objData["ParamInfo"] = pJsonDataParam;
                }

                objResult[pair.Key + ""] = objData;
            }
            return objResult;
        }

        public bool DataFromJson(JsonData objData)
        {
            if (null == objData
                || objData.IsEmpty())
            {
                return false;
            }

            if (!objData.IsObject)
            {
                return false;
            }

            
            foreach (KeyValuePair<string, JsonData> pairSateMachine in objData)
            {
                JsonData objParam = pairSateMachine.Value;
                if(objParam == null || objData.IsEmpty())
                {
                    continue;
                }
                VLParamData pParamData = JsonUtility.FromJson(objParam.ToJson(), typeof(VLParamData)) as VLParamData;
                if(pParamData != null)
                {
                    m_dictParam[pParamData.paramGUID] = pParamData;
                }
                else
                {
                    Debug.LogAssertionFormat("ParamData failed, param type not support:{0}"
                   , objParam.GetType().FullName);
                }

                //不同事件变参的数据读取
                JsonData objDataParam = objParam["ParamInfo"];
                if (objDataParam.IsEmpty())
                {
                    continue;
                }
                pParamData.SetJsonDataParam(objDataParam);
            }
            return true;
        }

       
    } //类末尾
} //命名空间末尾
