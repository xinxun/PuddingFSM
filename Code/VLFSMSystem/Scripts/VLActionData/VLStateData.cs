using System;
using System.Collections.Generic;

//状态机数据
namespace VL.VLFSMSystem
{
    [Serializable]
    public class VLStateData
    {
        public string stateGUID;//状态ID
        public string stateName;      //状态名字   
        public List<string> ActionList = new List<string>();//行为列表
        public List<string> EventList = new List<string>();//事件列表
        public VLStateData(string strStateGUID, string strStateName)
        {
            stateGUID = strStateGUID;
            stateName = strStateName;
        }
        public bool AddAction(string strActionGUID)
        {
            if(CheckActionById(strActionGUID))
            {
                return false;
            }
            ActionList.Add(strActionGUID);
            return true;

        }

        public bool DelAction(string strDelActionGUID)
        {
            foreach (string strAction in ActionList)
            {
                if (strDelActionGUID.Equals(strAction))
                {
                    ActionList.Remove(strAction);
                    return true;
                }
            }
            return false;
        }

        public int GetActionAmount()
        {
            return ActionList.Count;
        }

        public string GetActionByIndex(int iIndex)
        {
            if(iIndex >= ActionList.Count)
            {
                return "";
            }
            int iCurIndex = 0;
            foreach(string uActionId in ActionList)
            {
                if(iIndex == iCurIndex)
                {
                    return uActionId;
                }
                iCurIndex++;
            }
            return "";
        }

        private bool CheckActionById(string strCheckActionGUID)
        {
            foreach (string strActionId in ActionList)
            {
                if (strCheckActionGUID.Equals(strActionId))
                {
                    return true;
                }
            }
            return false;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool AddEvent(string strEventGUID)
        {
            if (CheckEventById(strEventGUID))
            {
                return false;
            }
            EventList.Add(strEventGUID);
            return true;

        }

        public bool DelEvent(string strDelEventGUID)
        {
            foreach (string strEventId in EventList)
            {
                if (strEventId.Equals(strDelEventGUID))
                {
                    EventList.Remove(strEventId);
                    return true;
                }
            }
            return false;
        }

        public int GetEventAmount()
        {
            return EventList.Count;
        }

        public string GetEventByIndex(int iIndex)
        {
            if (iIndex >= EventList.Count)
            {
                return "";
            }
            int iCurIndex = 0;
            foreach (string uEventId in EventList)
            {
                if (iIndex == iCurIndex)
                {
                    return uEventId;
                }
                iCurIndex++;
            }
            return "";
        }

        private bool CheckEventById(string strCheckEventId)
        {
            foreach (string eventGUID in EventList)
            {
                if (eventGUID.Equals(strCheckEventId))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
