using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace VL.VLFSMSystem
{
    //编辑器中的变量管理器
    public class VLStateDataMgr
    {
        private Dictionary<string, VLStateData> m_dictState = new Dictionary<string, VLStateData>();
        

        public void Reset()
        {
            m_dictState.Clear();
        }

        public VLStateData GetStateDataById(string strStateGUID)
        {
            VLStateData objResult = null;
            if (!m_dictState.TryGetValue(strStateGUID, out objResult))
            {
                return null;
            }

            return objResult;
        }

        public bool DelState(string strDelStateGUID)
        {
            if (!m_dictState.ContainsKey(strDelStateGUID))
            {
                return false;
            }

            return m_dictState.Remove(strDelStateGUID);
        }

        //添加状态机
        public bool AddState(string strStateGUID, string strStateName)
        {
            VLStateData pSateMachineData = GetStateDataById(strStateGUID);
            if(pSateMachineData != null)
            {
                return false;
            }
            VLStateData newState = new VLStateData(strStateGUID, strStateName);
            m_dictState.Add(newState.stateGUID, newState);
            return true;
        }

        public JsonData DataToJson(List<string> guids = null)
        {
            JsonData objResult = JsonData.CreateEmptyObj();
            foreach (KeyValuePair<string, VLStateData> pair in m_dictState)
            {
                VLStateData objStateData = pair.Value;
                if (null == objStateData)
                {
                    continue;
                }
                if (guids != null && !guids.Contains(pair.Key))
                {
                    continue;
                }
                JsonData objData = JsonData.CreateEmptyObj();
                string strParamJson = JsonUtility.ToJson(objStateData);
                objResult[pair.Key + ""] = JsonMapper.ToObject(strParamJson);
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
                JsonData objState = pairSateMachine.Value;
                if(objState == null || objData.IsEmpty())
                {
                    continue;
                }
                VLStateData pStateData = JsonUtility.FromJson(objState.ToJson(), typeof(VLStateData)) as VLStateData;
                if(pStateData != null)
                {
                    m_dictState[pStateData.stateGUID] = pStateData;
                }
                else
                {
                    Debug.LogAssertionFormat("StateData failed, param type not support:{0}"
                   , objState.GetType().FullName);
                }


            }
            return true;
        }

       
    } //类末尾
} //命名空间末尾
