using LitJson;
using System.Collections.Generic;
using UnityEngine;

//行为数据管理
namespace VL.VLFSMSystem
{
    public class VLActionDataMgr
    {
        private Dictionary<string, VLActionData> m_dictAction = new Dictionary<string, VLActionData>();
        

        public void Reset()
        {
            m_dictAction.Clear();
        }

        public VLActionData GetActionDataById(string strActionGUID)
        {
            VLActionData objResult = null;
            if (!m_dictAction.TryGetValue(strActionGUID, out objResult))
            {
                return null;
            }

            return objResult;
        }

        public bool DelAction(string strActionGUID)
        {
            if (!m_dictAction.ContainsKey(strActionGUID))
            {
                return false;
            }

            return m_dictAction.Remove(strActionGUID);
        }

        //添加状态机
        public bool AddAction(VLActionType.EVLACTIONTYPE uActionType, string strActionGUID, JsonData jsonDataParam)
        {
            VLActionData pSateMachineData = GetActionDataById(strActionGUID);
            if(pSateMachineData != null)
            {
                return false;
            }
            VLActionData newAction = new VLActionData(uActionType, strActionGUID, "Action"+ strActionGUID, jsonDataParam);
            m_dictAction.Add(newAction.actionGUID, newAction);
            return true;
        }

        public JsonData DataToJson(List<string> guids = null)
        {
            JsonData objResult = JsonData.CreateEmptyObj();
            foreach (KeyValuePair<string, VLActionData> pair in m_dictAction)
            {
                VLActionData objActionData = pair.Value;
                if (null == objActionData)
                {
                    continue;
                }
                if (guids != null && !guids.Contains(pair.Key))
                {
                    continue;
                }
                string strJson = JsonUtility.ToJson(objActionData);
                JsonData objData = JsonMapper.ToObject(strJson);
                //objData["Action"] = objDataAction;
                //objData["uActionId"] = objActionData.uActionId;
                //objData["strName"] = objActionData.strName;
                // objData["uActionType"] = objActionData.uActionType;

                //参数
                JsonData pJsonDataParam = objActionData.GetJsonDataParam();
                if(pJsonDataParam != null)
                {
                    objData["Param"] = pJsonDataParam;
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
                JsonData objAction = pairSateMachine.Value;
                VLActionData pActionData = JsonUtility.FromJson(objAction.ToJson(), typeof(VLActionData)) as VLActionData;
                if(pActionData == null)
                {
                    Debug.LogAssertionFormat("ActionData failed, param type not support:{0}"
                  , objAction.GetType().FullName);
                    continue;
                    
                }
                m_dictAction[pActionData.actionGUID] = pActionData;

                //不同事件变参的数据读取
                JsonData objDataParam = objAction["Param"];
                if (objDataParam.IsEmpty())
                {
                    continue;
                }
                pActionData.SetJsonDataParam(objDataParam);


            }
            return true;
        }

       
    } //类末尾
} //命名空间末尾
