using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace VL.VLFSMSystem
{
    //编辑器中的变量管理器
    public class VLStateMachineDataMgr
    {
        private Dictionary<string, VLStateMachineData> m_dictStateMachine = new Dictionary<string, VLStateMachineData>();

        public void Reset()
        {
            m_dictStateMachine.Clear();
        }

        public VLStateMachineData GetStateMachineDataById(string uStateMachineId)
        {
            VLStateMachineData objResult = null;
            if (!m_dictStateMachine.TryGetValue(uStateMachineId, out objResult))
            {
                return null;
            }

            return objResult;
        }

        public bool DelStateMachine(string uStateMachine)
        {
            if (!m_dictStateMachine.ContainsKey(uStateMachine))
            {
                return false;
            }

            return m_dictStateMachine.Remove(uStateMachine);
        }

        //添加状态机
        public bool AddStateMachine(string strStateMachineGUID, string strStateMachineName)
        {
            VLStateMachineData pSateMachineData = GetStateMachineDataById(strStateMachineGUID);
            if(pSateMachineData != null)
            {
                return false;
            }
            VLStateMachineData newStateMachine = new VLStateMachineData(strStateMachineGUID, strStateMachineName);
            m_dictStateMachine.Add(newStateMachine.stateMachineGUID, newStateMachine);
            return true;
        }

        public JsonData DataToJson(List<string> guids = null)
        {
            JsonData objResult = JsonData.CreateEmptyObj();
            if(objResult == null)
            {
                return null;
            }
            foreach (KeyValuePair<string, VLStateMachineData> pair in m_dictStateMachine)
            {
                VLStateMachineData objStateMachineData = pair.Value;
                if (null == objStateMachineData)
                {
                    continue;
                }
                if (guids != null && !guids.Contains(pair.Key))
                {
                    continue;
                }
                string strParamJson = JsonUtility.ToJson(objStateMachineData);
                objResult[pair.Key + ""] = JsonMapper.ToObject(strParamJson);
            }
            return objResult;
        }

        public string DataFromJson(JsonData objData)
        {
            string guidRet = string.Empty;
            if (null == objData
                || objData.IsEmpty())
            {
                return guidRet;
            }

            if (!objData.IsObject)
            {
                return guidRet;
            }


            foreach (KeyValuePair<string, JsonData> pairSateMachine in objData)
            {
                JsonData objStateMachine = pairSateMachine.Value;
                if (objStateMachine == null || objStateMachine.IsEmpty())
                {
                    continue;
                }
                VLStateMachineData pStateMachineData = JsonUtility.FromJson(objStateMachine.ToJson(), typeof(VLStateMachineData)) as VLStateMachineData;
                if (pStateMachineData != null)
                {
                    guidRet = pStateMachineData.stateMachineGUID;
                    m_dictStateMachine[pStateMachineData.stateMachineGUID] = pStateMachineData;
                }
                else
                {
                    Debug.LogAssertionFormat("StateMachineData failed, param type not support:{0}"
                   , objStateMachine.GetType().FullName);
                }


            }
            return guidRet;
        }


    } //类末尾
} //命名空间末尾
