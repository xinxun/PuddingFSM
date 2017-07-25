using System;
using System.Collections.Generic;

//状态机数据
namespace VL.VLFSMSystem
{
    [Serializable]
    public class VLStateMachineData
    {
        public string stateMachineGUID;//状态机ID
        public string stateMachineName;      //状态名字   
        public List<string> StateList = new List<string>();//状态列表
        public List<string> ParamList = new List<string>();//参数列表
        public VLStateMachineData(string strStateMachineGUID, string strMachineName)
        {
            stateMachineGUID = strStateMachineGUID;
            stateMachineName = strMachineName;
        }
        
        public bool AddParam(string strParamGUID)
        {
            if (CheckParamById(strParamGUID))
            {
                return false;
            }
            ParamList.Add(strParamGUID);
            return true;

        }

        public bool DelParam(string strDelParamGUID)
        {
            foreach (string uParamId in StateList)
            {
                if (uParamId.Equals(strDelParamGUID))
                {
                    ParamList.Remove(uParamId);
                    return true;
                }
            }
            return false;
        }

        public int GetParamAmount()
        {
            return ParamList.Count;
        }

        public string GetParamByIndex(int iIndex)
        {
            if (iIndex >= ParamList.Count)
            {
                return "";
            }
            int iCurIndex = 0;
            foreach (string uParamId in ParamList)
            {
                if (iIndex == iCurIndex)
                {
                    return uParamId;
                }
                iCurIndex++;
            }
            return "";
        }

        private bool CheckParamById(string strCheckParamGUID)
        {
            foreach (string uParamId in ParamList)
            {
                if (strCheckParamGUID.Equals(uParamId))
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddState(string strStateGUID)
        {
            if(CheckStateById(strStateGUID))
            {
                return false;
            }
            StateList.Add(strStateGUID);
            return true;

        }

        public bool DelState(string strDelStateGUID)
        {
            foreach (string uStateId in StateList)
            {
                if (uStateId.Equals(strDelStateGUID))
                {
                    StateList.Remove(uStateId);
                    return true;
                }
            }
            return false;
        }

        public int GetStateAmount()
        {
            return StateList.Count;
        }

        public string GetStateByIndex(int iIndex)
        {
            if(iIndex >= StateList.Count)
            {
                return "";
            }
            int iCurIndex = 0;
            foreach(string uStateId in StateList)
            {
                if(iIndex == iCurIndex)
                {
                    return uStateId;
                }
                iCurIndex++;
            }
            return "";
        }

        private bool CheckStateById(string strCheckStateGUID)
        {
            foreach (string uStateId in StateList)
            {
                if (strCheckStateGUID.Equals(uStateId))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
