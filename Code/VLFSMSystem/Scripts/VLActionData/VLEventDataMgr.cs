using LitJson;
using System.Collections.Generic;
using UnityEngine;

//行为数据管理
namespace VL.VLFSMSystem
{
    public class VLEventDataMgr
    {
        private Dictionary<string, VLEventData> m_dictEvent = new Dictionary<string, VLEventData>();
        

        public void Reset()
        {
            m_dictEvent.Clear();
        }

        public VLEventData GetEventDataById(string strEventGUID)
        {
            VLEventData objResult = null;
            if (!m_dictEvent.TryGetValue(strEventGUID, out objResult))
            {
                return null;
            }

            return objResult;
        }

        public bool DelEvent(string uEventGUID)
        {
            if (!m_dictEvent.ContainsKey(uEventGUID))
            {
                return false;
            }

            return m_dictEvent.Remove(uEventGUID);
        }

        //添加事件
        public bool AddEvent(VLEventType.EVLEVENTTYPE uEventType, string strEventGUID, JsonData jsonDataParam)
        {
            VLEventData pEventData = GetEventDataById(strEventGUID);
            if(pEventData != null)
            {
                return false;
            }
            VLEventData newEvent = new VLEventData(uEventType, strEventGUID, "Event"+ strEventGUID, jsonDataParam);
            m_dictEvent.Add(newEvent.eventGUID, newEvent);
            return true;
        }

        public JsonData DataToJson(List<string> guids = null)
        {
            JsonData objResult = JsonData.CreateEmptyObj();
            foreach (KeyValuePair<string, VLEventData> pair in m_dictEvent)
            {
                VLEventData objEventData = pair.Value;
                if (null == objEventData)
                {
                    continue;
                }
                if (guids != null && !guids.Contains(pair.Key))
                {
                    continue;
                }
                string strJson = JsonUtility.ToJson(objEventData);
                JsonData objData = JsonMapper.ToObject(strJson);


                JsonData pJsonDataParam = objEventData.GetJsonDataParam();
                if (pJsonDataParam != null)
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
                JsonData objEvent = pairSateMachine.Value;
                if(objEvent == null || objEvent.IsEmpty())
                {
                    continue;
                }
                VLEventData pEventData = JsonUtility.FromJson(objEvent.ToJson(), typeof(VLEventData)) as VLEventData;
                if(pEventData == null)
                {
                    Debug.LogAssertionFormat("EventData failed, param type not support:{0}"
                  , objEvent.GetType().FullName);
                    continue;
                    
                }
                m_dictEvent[pEventData.eventGUID] = pEventData;

                //不同事件变参的数据读取
                JsonData objDataParam = objEvent["Param"];
                if (objDataParam.IsEmpty())
                {
                    continue;
                }
                pEventData.SetJsonDataParam(objDataParam);


            }
            return true;
        }

       
    } //类末尾
} //命名空间末尾
