
using UnityEngine;

namespace VL.VLFSMSystem
{//行为编辑器对象的载体（在节点下创建一个行来对象）
    public class VLActionObject : MonoBehaviour
    {
        //状态机管理器
        private VLStateMachineMgr m_pVLStateMachineMgr = null;
        void Awake()
        {
            //加入状态机管理器
            m_pVLStateMachineMgr = gameObject.GetComponent<VLStateMachineMgr>();
            if (m_pVLStateMachineMgr == null)
            {
                m_pVLStateMachineMgr = gameObject.AddComponent<VLStateMachineMgr>();
                if (m_pVLStateMachineMgr != null)
                {
                    m_pVLStateMachineMgr.Init();
                }
            }


        }


        //播放所有状态机
        public void PlayAllFSM()
        {
            if (m_pVLStateMachineMgr == null)
            {
                return;
            }
            m_pVLStateMachineMgr.PlayAllFSM();
        }


        //停止所有状态机
        public void StopAllFSM()
        {
            if (m_pVLStateMachineMgr == null)
            {
                return;
            }
            m_pVLStateMachineMgr.StopAllFSM();
        }

       
        #region 得到管理器
        //得到状态机管理器
        private VLStateMachineMgr GetStateMachineMgr()
        {
            return m_pVLStateMachineMgr;
        }


        //得到uStateMachineId状态管理器
        private VLStateMgr GetStateMgr(string uStateMachineId)
        {
            VLStateMachine pStateMachine = m_pVLStateMachineMgr.GetStateMachine(uStateMachineId);
            if (pStateMachine == null)
            {
                return null;
            }
            return pStateMachine.GetStateMgr();
        }
        //得到uStateMachineId参数管理器
        private VLSMParamMgr GetSMParamMgr(string uStateMachineId)
        {
            VLStateMachine pStateMachine = m_pVLStateMachineMgr.GetStateMachine(uStateMachineId);
            if (pStateMachine == null)
            {
                return null;
            }
            return pStateMachine.GetParamMgr();
        }
        //得到uStateMachineId状态机下的uStateId状态的行为管理器
        private VLActionMgr GetActionMgr(string uStateMachineId, string uStateId)
        {
            VLStateMgr pStateMgr = GetStateMgr(uStateMachineId);
            if (pStateMgr == null)
            {
                return null;
            }

            VLState pState = pStateMgr.GetStateById(uStateId);
            if (pState == null)
            {
                return null;
            }
            return pState.GetActionMgr();
        }

        //得到uStateMachineId状态机下的uEventId的事件管理器
        private VLEventMgr GetEventMgr(string uStateMachineId, string uStateId)
        {
            VLStateMgr pStateMgr = GetStateMgr(uStateMachineId);
            if (pStateMgr == null)
            {
                return null;
            }

            VLState pState = pStateMgr.GetStateById(uStateId);
            if (pState == null)
            {
                return null;
            }
            return pState.GetEventMgr();
        }
        #endregion

        #region 状态机的操作增,删,查
        //得到uStateMachineId状态机
        public VLStateMachine GetStateMachine(string uStateMachineId)
        {
            if (m_pVLStateMachineMgr == null)
            {
                return null;
            }
            return m_pVLStateMachineMgr.GetStateMachine(uStateMachineId);
        }



        public VLStateMachine GetStateMachineByIndex(int iIndex)
        {
            if (m_pVLStateMachineMgr == null)
            {
                return null;
            }
            return m_pVLStateMachineMgr.GetStateMachineByIndex(iIndex);
        }

        public int GetStateMachineAmount()
        {
            if (m_pVLStateMachineMgr == null)
            {
                return 0;
            }
            return m_pVLStateMachineMgr.GetStateMachineAmount();
        }

        public string CreateStateMachine(string strFSMName)
        {
            if (m_pVLStateMachineMgr == null)
            {
                return "";
            }


            string strNewStateMachineGUID = Func.GetGUID();
            //加入状态机
            return m_pVLStateMachineMgr.AddStateMachine(strNewStateMachineGUID, strFSMName);
        }

        public bool DelStateMachine(string strStateMachineGUID)
        {
            if (m_pVLStateMachineMgr == null)
            {
                return false;
            }
            return m_pVLStateMachineMgr.DelStateMachine(strStateMachineGUID);
        }

        //通过strStateMachineGUID播放单个状态机
        public bool PlayStateMachineByID(string strStateMachineGUID)
        {
            if(m_pVLStateMachineMgr == null)
            {
                return false;
            }
            VLStateMachine pStateMachine = m_pVLStateMachineMgr.GetStateMachine(strStateMachineGUID);
            if (pStateMachine == null)
            {
                return false;
            }
            pStateMachine.PlayStateMachine();
            return true;
        }

        //得到当前状态机运行的状态
        public string GetCurPlayState(string strStateMachine)
        {
            VLStateMgr pStateMgr = this.GetStateMgr(strStateMachine);
            if(pStateMgr == null)
            {
                return "";
            }
            return pStateMgr.CurrentStateGUID;
        }

        #endregion

        #region 状态的增删查
        //得到uStateMachineId状态机下uSatetId的状态
        public VLState GetState(string uStateMachineId, string uStateId)
        {
            VLStateMgr pStateMgr = GetStateMgr(uStateMachineId);
            if (pStateMgr == null)
            {
                return null;
            }

            return pStateMgr.GetStateById(uStateId);
        }

        public int GetStateAmount(string uStateMachineId)
        {
            VLStateMgr pStateMgr = GetStateMgr(uStateMachineId);
            if (pStateMgr == null)
            {
                return 0;
            }
            return pStateMgr.GetStateAmount();
        }

        public VLState GetStateByIndex(string uStateMachineId, int iIndex)
        {
            VLStateMgr pStateMgr = GetStateMgr(uStateMachineId);
            if (pStateMgr == null)
            {
                return null;
            }
            return pStateMgr.GetStateByIndex(iIndex);
        }


        public string CreateState(string strStateMachineGUID, string strStateName)
        {
            VLStateMgr pVLStateMgr = GetStateMgr(strStateMachineGUID);
            if (pVLStateMgr == null)
            {
                return "";
            }

            string strNewStateGUID = Func.GetGUID();
            return pVLStateMgr.AddState(strNewStateGUID, strStateName);
        }

        public bool DelState(string strStateGUID)
        {
            VLState pState = VLFSMManager.GetInstance().GetStateByGUID(strStateGUID);
            if (pState == null)
            {
                return false;
            }

            VLStateMachine pStateMachine = VLFSMManager.GetInstance().GetStateMachineByGUID(pState.GetStateMachineID());
            if (pStateMachine == null)
            {
                return false;
            }

            VLStateMgr pStateMgr = pStateMachine.GetStateMgr();
            if(pStateMgr == null)
            {
                return false;
            }
            return pStateMgr.DelState(strStateGUID);
        }

        //播放状态
        public bool PlayState(string strStateGUID)
        {
            VLState pState = VLFSMManager.GetInstance().GetStateByGUID(strStateGUID);
            if (pState == null)
            {
                return false;
            }
            VLStateMachine pStateMachine = VLFSMManager.GetInstance().GetStateMachineByGUID(pState.GetStateMachineID());
            if (pStateMachine == null)
            {
                return false;
            }

            VLStateMgr pStateMgr = pStateMachine.GetStateMgr();
            if (pStateMgr == null)
            {
                return false;
            }

            pStateMgr.PlayState(strStateGUID);
            return true;
        }
        #endregion

        #region 参数的增删查
        //得到uStateMachineId状态机下uSatetId的状态
        public VLSMParam GetSMParam(string uStateMachineId, string uSMParamId)
        {
            VLSMParamMgr pSMParamMgr = GetSMParamMgr(uStateMachineId);
            if (pSMParamMgr == null)
            {
                return null;
            }

            return pSMParamMgr.GetParamById(uSMParamId);
        }

        public int GetSMParamAmount(string uStateMachineId)
        {
            VLSMParamMgr pSMParamMgr = GetSMParamMgr(uStateMachineId);
            if (pSMParamMgr == null)
            {
                return 0;
            }
            return pSMParamMgr.GetParamAmount();
        }

        public VLSMParam GetSMParamByIndex(string uStateMachineId, int iIndex)
        {
            VLSMParamMgr pSMParamMgr = GetSMParamMgr(uStateMachineId);
            if (pSMParamMgr == null)
            {
                return null;
            }
            return pSMParamMgr.GetParamByIndex(iIndex);
        }


        public string CreateSMParam(string strStateMachineGUID, string strSMParamName)
        {
            VLSMParamMgr pVLSMParamMgr = GetSMParamMgr(strStateMachineGUID);
            if (pVLSMParamMgr == null)
            {
                return "";
            }

            string strNewSMParamGUID = Func.GetGUID();
            return pVLSMParamMgr.AddParam(strNewSMParamGUID, strSMParamName, null);
        }

        public bool DelSMParam(string strSMParamGUID)
        {
            VLSMParam pSMParam = VLFSMManager.GetInstance().GetSMParamByGUID(strSMParamGUID);
            if (pSMParam == null)
            {
                return false;
            }

            VLStateMachine pStateMachine = VLFSMManager.GetInstance().GetStateMachineByGUID(pSMParam.GetStateMachineGUID());
            if (pStateMachine == null)
            {
                return false;
            }

            VLSMParamMgr pSMParamMgr = pStateMachine.GetParamMgr();
            if (pSMParamMgr == null)
            {
                return false;
            }
            return pSMParamMgr.DelParam(strSMParamGUID);
        }

       
        #endregion

        #region Action的增，删，查
        //得到uStateMachineId下的uSatteId状态下的uActionId的行为
        public IVLAction GetAction(string uActionId)
        {
            return VLFSMManager.GetInstance().GetActionByGUID(uActionId);
        }

        //得到stateId下的所有Action个数
        public int GetActionAmount(string uStateId)
        {
            VLState pVLState = VLFSMManager.GetInstance().GetStateByGUID(uStateId);
            if (pVLState == null)
            {
                return 0;
            }
            VLActionMgr pActionMgr = pVLState.GetActionMgr();
            if (pActionMgr == null)
            {
                return 0;
            }
            return pActionMgr.GetActionAmount();
        }

        public IVLAction GetActionByIndex(string uStateId, int iIndex)
        {
            VLState pVLState = VLFSMManager.GetInstance().GetStateByGUID(uStateId);
            if (pVLState == null)
            {
                return null;
            }
            VLActionMgr pActionMgr = pVLState.GetActionMgr();
            if (pActionMgr == null)
            {
                return null;
            }

            return pActionMgr.GetActionByIndex(iIndex);
        }


        //创建Action
        public string CreateAction(string strStateGUID, VLActionType.EVLACTIONTYPE eActionType)
        {
            //得到状态
            VLState pVLState = VLFSMManager.GetInstance().GetStateByGUID(strStateGUID);
            if (pVLState == null)
            {
                return "";
            }
            return pVLState.AddAction(eActionType);
        }

        //通过actionguid删除Action
        public bool DelAction(string strActionGUID)
        {
            VLActionBase pVLAction = VLFSMManager.GetInstance().GetActionByGUID(strActionGUID);
            if(pVLAction == null)
            {
                return false;
            }
            VLState pState = VLFSMManager.GetInstance().GetStateByGUID(pVLAction.stateGUID);
            if(pState == null)
            {
                return false;
            }
            
            VLActionMgr pVLActionMgr = pState.GetActionMgr();
            if (pVLActionMgr == null)
            {
                return false;
            }
            return pVLActionMgr.DelAction(strActionGUID);
        }

        //当前状态Action是否都运行结束
        public bool IsActionAllEnd(string struStateGUID)
        {
            VLState pVLState = VLFSMManager.GetInstance().GetStateByGUID(struStateGUID);
            if(pVLState == null)
            {
                return true;
            }
            VLActionMgr pVLActionMgr = pVLState.GetActionMgr();
            if(pVLActionMgr == null)
            {
                return true;
            }
            if (!pVLActionMgr.IsEnd())
            {
                return false;
            }
            return true;
        }

        #endregion

        #region 事件增，删，查
        public VLEventBase GetEvent(string uEventId)
        {
            return VLFSMManager.GetInstance().GetEventByGUID(uEventId);
        }

        public int GetEventAmount(string uStateId)
        {
            VLState pVLState = VLFSMManager.GetInstance().GetStateByGUID(uStateId);
            if(pVLState == null)
            {
                return 0;
            }
            VLEventMgr pEvnetMgr = pVLState.GetEventMgr();
            if(pEvnetMgr == null)
            {
                return 0;
            }
            return pEvnetMgr.GetEventAmount();
        }

        public VLEventBase GetEventByIndex(string uStateId, int iIndex)
        {
            VLState pVLState = VLFSMManager.GetInstance().GetStateByGUID(uStateId);
            if (pVLState == null)
            {
                return null;
            }
            VLEventMgr pEvnetMgr = pVLState.GetEventMgr();
            if (pEvnetMgr == null)
            {
                return null;
            }

            return pEvnetMgr.GetEventByIndex(iIndex);
        }

        public string CreateEvent(string strStateGUID, VLEventType.EVLEVENTTYPE eEventType)
        {

            //得到状态
            VLState pVLState = VLFSMManager.GetInstance().GetStateByGUID(strStateGUID);
            if (pVLState == null)
            {
                return "";
            }
            return pVLState.AddEvent(eEventType);
        }


        public bool DelEvent(string strEventGUID)
        {
            VLEventBase pVLEvent = VLFSMManager.GetInstance().GetEventByGUID(strEventGUID);
            if (pVLEvent == null)
            {
                return false;
            }
            VLState pState = VLFSMManager.GetInstance().GetStateByGUID(pVLEvent.stateGUID);
            if (pState == null)
            {
                return false;
            }

            VLEventMgr pVLEventMgr = pState.GetEventMgr();
            if (pVLEventMgr == null)
            {
                return false;
            }
            return pVLEventMgr.DelEvent(strEventGUID);
        }
        #endregion

        //===============================================================================================
        //重新保存状态机的所有数据
        public bool SaveStateMahcine()
        {
            if(m_pVLStateMachineMgr == null)
            {
                return false;
            }

            int iAmount = m_pVLStateMachineMgr.GetStateMachineAmount();
            for (int i = 0; i < iAmount; i++)
            {
                VLStateMachine pVLStateMachine = m_pVLStateMachineMgr.GetStateMachineByIndex(i);
                if (pVLStateMachine == null)
                {
                    continue;
                }
                VLFSMDataMgr.Instance.StateMachineDataMgr.AddStateMachine(pVLStateMachine.GetStateMachinId(), pVLStateMachine.GetStateMachineName());
                SaveParam(pVLStateMachine);
                SaveState(pVLStateMachine);
            }
            return true;
        }
        //加入状态机模板里的状态机
        public bool AddStateMachineByTemplateId(string strStateMachineTempGuid)
        {
            if (m_pVLStateMachineMgr == null)
            {
                return false;
            }

            VLStateMachineData pStateMachineInfo = VLFSMTemplateDataMgr.Instance.StateMachineDataMgr.GetStateMachineDataById(strStateMachineTempGuid);
            if (pStateMachineInfo == null)
            {
                return false;
            }

            string strStateMachineName = pStateMachineInfo.stateMachineName;

            //加入状态机
            strStateMachineTempGuid = m_pVLStateMachineMgr.AddStateMachine(strStateMachineTempGuid, strStateMachineName);
            if (strStateMachineTempGuid.Length <= 0)
            {
                return false;
            }

            int iAmountState = pStateMachineInfo.GetStateAmount();
            for (int i = 0; i < iAmountState; i++)
            {
                string strStateGUID = pStateMachineInfo.GetStateByIndex(i);
                //加入状态
                AddStateById(strStateMachineTempGuid, strStateGUID);
            }
            int iAmountParam = pStateMachineInfo.GetParamAmount();
            for (int i = 0; i < iAmountParam; i++)
            {
                string strParamGUID = pStateMachineInfo.GetParamByIndex(i);
                //加入参数
                AddParamById(strStateMachineTempGuid, strParamGUID);
            }
            return true;
        }

        //通过配置文件加入数据
        public bool AddStateMachineById(string strStateMachineGUID)
        {
            if (m_pVLStateMachineMgr == null)
            {
                return false;
            }

            VLStateMachineData pStateMachineInfo = VLFSMDataMgr.Instance.StateMachineDataMgr.GetStateMachineDataById(strStateMachineGUID);
            if (pStateMachineInfo == null)
            {
                return false;
            }

            string strStateMachineName = pStateMachineInfo.stateMachineName;

            //加入状态机
            strStateMachineGUID = m_pVLStateMachineMgr.AddStateMachine(strStateMachineGUID, strStateMachineName);
            if (strStateMachineGUID.Length <= 0)
            {
                return false;
            }

            int iAmountParam = pStateMachineInfo.GetParamAmount();
            for (int i = 0; i < iAmountParam; i++)
            {
                string strParamGUID = pStateMachineInfo.GetParamByIndex(i);
                //加入参数
                AddParamById(strStateMachineGUID, strParamGUID);
            }
            int iAmountState = pStateMachineInfo.GetStateAmount();
            for (int i = 0; i < iAmountState; i++)
            {
                string strStateGUID = pStateMachineInfo.GetStateByIndex(i);
                //加入状态
                AddStateById(strStateMachineGUID, strStateGUID);
            }
            return true;
        }


        //写入状态机
        private void SaveState(VLStateMachine pVLStateMachine)
        {
            if (pVLStateMachine == null)
            {
                return;
            }
            VLStateMgr pVLStateMgr = pVLStateMachine.GetStateMgr();
            if (pVLStateMgr != null)
            {
                int iAmount = pVLStateMgr.GetStateAmount();
                for (int i = 0; i < iAmount; i++)
                {
                    VLState pVLState = pVLStateMgr.GetStateByIndex(i);
                    if (pVLState == null)
                    {
                        continue;
                    }
                    string strFSMGUID = pVLStateMachine.GetStateMachinId();
                    VLStateMachineData pSMD = VLFSMDataMgr.Instance.StateMachineDataMgr.GetStateMachineDataById(strFSMGUID);
                    if (pSMD != null)
                    {
                        pSMD.AddState(pVLState.GetStateId());
                    }

                    VLFSMDataMgr.Instance.StateDataMgr.AddState(pVLState.GetStateId(), pVLState.GetStateName());
                    SaveAction(pVLState);
                    SaveEvent(pVLState);
                }
            }
        }
        private void SaveParam(VLStateMachine pVLStateMachine)
        {
            if (pVLStateMachine == null)
            {
                return;
            }
            VLSMParamMgr pVLParamMgr = pVLStateMachine.GetParamMgr();
            if (pVLParamMgr != null)
            {
                int iAmount = pVLParamMgr.GetParamAmount();
                for (int i = 0; i < iAmount; i++)
                {
                    VLSMParam pVLParam = pVLParamMgr.GetParamByIndex(i);
                    if (pVLParam == null)
                    {
                        continue;
                    }
                    string strFSMGUID = pVLStateMachine.GetStateMachinId();
                    VLStateMachineData pSMD = VLFSMDataMgr.Instance.StateMachineDataMgr.GetStateMachineDataById(strFSMGUID);
                    if (pSMD != null)
                    {
                        pSMD.AddParam(pVLParam.GetParamId());
                    }
                    VLFSMDataMgr.Instance.ParamDataMgr.AddParam(strFSMGUID,pVLParam.GetParamId(), pVLParam.GetParamData());
                }
            }
        }

        //写入行为
        private void SaveAction(VLState pVLState)
        {
            if (pVLState == null)
            {
                return;
            }
            VLActionMgr pVLActionMgr = pVLState.GetActionMgr();
            if (pVLActionMgr == null)
            {
                return;
            }

            int iAmount = pVLActionMgr.GetActionAmount();
            for (int i = 0; i < iAmount; i++)
            {
                VLActionBase pVLAction = pVLActionMgr.GetActionByIndex(i);
                if (pVLAction == null)
                {
                    continue;
                }
                string strStateGUID = pVLState.GetStateId();
                VLStateData pSD = VLFSMDataMgr.Instance.StateDataMgr.GetStateDataById(strStateGUID);
                if (pSD != null)
                {
                    pSD.AddAction(pVLAction.GetActionId());
                }
                VLFSMDataMgr.Instance.ActionDataMgr.AddAction(pVLAction.GetActionType(), pVLAction.GetActionId(), pVLAction.GetParamData());
            }
        }
        //写入行来
        private void SaveEvent(VLState pVLState)
        {
            if (pVLState == null)
            {
                return;
            }
            VLEventMgr pVLEventMgr = pVLState.GetEventMgr();
            if (pVLEventMgr == null)
            {
                return;
            }

            int iAmount = pVLEventMgr.GetEventAmount();
            for (int i = 0; i < iAmount; i++)
            {
                VLEventBase pVLEvent = pVLEventMgr.GetEventByIndex(i);
                if (pVLEvent == null)
                {
                    continue;
                }
                string strStateGUID = pVLState.GetStateId();
                VLStateData pSD = VLFSMDataMgr.Instance.StateDataMgr.GetStateDataById(strStateGUID);
                if (pSD != null)
                {
                    pSD.AddEvent(pVLEvent.GetEventId());
                }
                VLFSMDataMgr.Instance.EventDataMgr.AddEvent(pVLEvent.GetEventType(), pVLEvent.GetEventId(), pVLEvent.GetParamData());
            }
        }

        private bool AddParamById(string ustrStateMachineGUID, string strParamGUID)
        {
            VLParamData pParamData = VLFSMDataMgr.Instance.ParamDataMgr.GetParamDataById(strParamGUID);
            if (pParamData == null)
            {
                return false;
            }
            VLSMParamMgr pVLParamMgr = GetSMParamMgr(ustrStateMachineGUID);
            if (pVLParamMgr == null)
            {
                return false;
            }

            pVLParamMgr.AddParam(pParamData.paramGUID, pParamData.paramName,pParamData.GetJsonDataParam());
            
            return true;
        }

        private bool AddStateById(string ustrStateMachineGUID, string strStateGUID)
        {
            VLStateData pStateData = VLFSMDataMgr.Instance.StateDataMgr.GetStateDataById(strStateGUID);
            if (pStateData == null)
            {
                return false;
            }
            VLStateMgr pVLStateMgr = GetStateMgr(ustrStateMachineGUID);
            if (pVLStateMgr == null)
            {
                return false;
            }

            pVLStateMgr.AddState(pStateData.stateGUID, pStateData.stateName);

            int iAmountAction = pStateData.GetActionAmount();
            for (int i = 0; i < iAmountAction; i++)
            {
                string uActionId = pStateData.GetActionByIndex(i);
                AddActionById(ustrStateMachineGUID, strStateGUID, uActionId);
            }


            int iAmountEvent = pStateData.GetEventAmount();
            for (int i = 0; i < iAmountEvent; i++)
            {
                string uEventId = pStateData.GetEventByIndex(i);
                AddEventById(ustrStateMachineGUID, strStateGUID, uEventId);
            }
            return true;
        }

        private bool AddActionById(string uStateMachineId, string uStateId, string uActionId)
        {
            VLActionData pActionData = VLFSMDataMgr.Instance.ActionDataMgr.GetActionDataById(uActionId);
            if (pActionData == null)
            {
                return false;
            }
            VLActionMgr pVLActionMgr = GetActionMgr(uStateMachineId, uStateId);
            if (pVLActionMgr == null)
            {
                return false;
            }

            VLActionType.EVLACTIONTYPE eType = VLActionType.GetActionByType(pActionData.actionType);
            pVLActionMgr.AddAction(eType, uActionId, pActionData.GetJsonDataParam());
            return true;
        }

        private bool AddEventById(string uStateMachineId, string uStateId, string uEventId)
        {
            VLEventData pEventData = VLFSMDataMgr.Instance.EventDataMgr.GetEventDataById(uEventId);
            if (pEventData == null)
            {
                return false;
            }
            VLEventMgr pVLEventMgr = GetEventMgr(uStateMachineId, uStateId);
            if (pVLEventMgr == null)
            {
                return false;
            }

            VLEventType.EVLEVENTTYPE eType = VLEventType.GetEventByType(pEventData.eventType);
            pVLEventMgr.AddEvent(eType, uEventId, pEventData.GetJsonDataParam());
            return true;
        }


    }
}
