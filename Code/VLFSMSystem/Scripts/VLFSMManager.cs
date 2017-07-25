
using System.Collections.Generic;

namespace VL.VLFSMSystem
{//状态机管理类(内部使用请不要public)
    public class VLFSMManager
    {//主要支持快速查找对象
        private static readonly VLFSMManager _instance = new VLFSMManager();
        public static VLFSMManager GetInstance()
        {
            return _instance;
        }

        private Dictionary<string, VLActionBase> m_dictAction = new Dictionary<string, VLActionBase>();
        private Dictionary<string, VLEventBase> m_dictEvent = new Dictionary<string, VLEventBase>();
        private Dictionary<string, VLState> m_dictState = new Dictionary<string, VLState>();
        private Dictionary<string, VLSMParam> m_dictSMParam = new Dictionary<string, VLSMParam>();
        private Dictionary<string, VLStateMachine> m_dictStateMachine = new Dictionary<string, VLStateMachine>();



        #region  外部调用接口

        public void Init()
        {
            m_dictAction.Clear();
            m_dictEvent.Clear();
            m_dictState.Clear();
            m_dictSMParam.Clear();
            m_dictStateMachine.Clear();

        }

        public VLActionBase GetActionByGUID(string strActionGUID)
        {
            if (m_dictAction == null)
            {
                return null;
            }
            if (m_dictAction.ContainsKey(strActionGUID))
            {
                return m_dictAction[strActionGUID];
            }
            return null;
        }

        public VLEventBase GetEventByGUID(string strEventGUID)
        {
            if (m_dictEvent == null)
            {
                return null;
            }
            if (m_dictEvent.ContainsKey(strEventGUID))
            {
                return m_dictEvent[strEventGUID];
            }
            return null;
        }

        public VLState GetStateByGUID(string strStateGUID)
        {
            if (m_dictState == null)
            {
                return null;
            }
            if (m_dictState.ContainsKey(strStateGUID))
            {
                return m_dictState[strStateGUID];
            }
            return null;
        }

        public VLSMParam GetSMParamByGUID(string strSMParamGUID)
        {
            if (m_dictSMParam == null)
            {
                return null;
            }
            if (m_dictSMParam.ContainsKey(strSMParamGUID))
            {
                return m_dictSMParam[strSMParamGUID];
            }
            return null;
        }


        public VLStateMachine GetStateMachineByGUID(string strStateMachineGUID)
        {
            if (m_dictStateMachine == null)
            {
                return null;
            }
            if (m_dictStateMachine.ContainsKey(strStateMachineGUID))
            {
                return m_dictStateMachine[strStateMachineGUID];
            }
            return null;
        }
        #endregion

        #region 内部调用接口
        public bool AddAction(VLActionBase pActionBase)
        {
            if (pActionBase == null)
            {
                return false;
            }
            if (m_dictAction.ContainsKey(pActionBase.GetActionId()))
            {
                return false;
            }

            m_dictAction.Add(pActionBase.actionGUID, pActionBase);
            return true;
        }

        public bool DelAction(string strActionGUID)
        {
            if (!m_dictAction.ContainsKey(strActionGUID))
            {
                return false;
            }

            m_dictAction.Remove(strActionGUID);
            return true;
        }



        public bool AddEvent(VLEventBase pEventBase)
        {
            if (pEventBase == null)
            {
                return false;
            }
            if (m_dictEvent.ContainsKey(pEventBase.GetEventId()))
            {
                return false;
            }

            m_dictEvent.Add(pEventBase.eventGUID, pEventBase);
            return true;
        }


        public bool DelEvent(string strEventGUID)
        {
            if (!m_dictEvent.ContainsKey(strEventGUID))
            {
                return false;
            }

            m_dictEvent.Remove(strEventGUID);
            return true;
        }

        public bool AddState(VLState pState)
        {
            if (pState == null)
            {
                return false;
            }
            if (m_dictState.ContainsKey(pState.GetStateId()))
            {
                return false;
            }

            m_dictState.Add(pState.GetStateId(), pState);
            return true;
        }

        public bool DelState(string strStateGUID)
        {
            if (!m_dictState.ContainsKey(strStateGUID))
            {
                return false;
            }

            //删除级联事件
            ClearStateAction(strStateGUID);
            //删除级联行为
            ClearStateEvent(strStateGUID);

            m_dictState.Remove(strStateGUID);

            return true;
        }

        public bool AddSMParam(VLSMParam pSMParam)
        {
            if (pSMParam == null)
            {
                return false;
            }
            if (m_dictSMParam.ContainsKey(pSMParam.GetParamId()))
            {
                return false;
            }

            m_dictSMParam.Add(pSMParam.GetParamId(), pSMParam);
            return true;
        }

        public bool DelSMParam(string strSMParamGUID)
        {
            if (!m_dictSMParam.ContainsKey(strSMParamGUID))
            {
                return false;
            }

            m_dictSMParam.Remove(strSMParamGUID);

            return true;
        }

        public bool AddStateMachine(VLStateMachine pStateMachine)
        {
            if (pStateMachine == null)
            {
                return false;
            }
            if (m_dictStateMachine.ContainsKey(pStateMachine.GetStateMachinId()))
            {
                return false;
            }

            m_dictStateMachine.Add(pStateMachine.GetStateMachinId(), pStateMachine);
            return true;
        }

        public bool DelStateMachine(string strStateMachineGUID)
        {
            if (!m_dictStateMachine.ContainsKey(strStateMachineGUID))
            {
                return false;
            }
            //清空级联的状态
            ClearStateMachineState(strStateMachineGUID);
            //清空级联的参数
            ClearStateMachineSMParam(strStateMachineGUID);

            m_dictStateMachine.Remove(strStateMachineGUID);
            return true;
        }

        #endregion


        //清空状态机中的action
        private int ClearStateAction(string strStateGUID)
        {
            if (m_dictState == null)
            {
                return 0;
            }
            if (!m_dictState.ContainsKey(strStateGUID))
            {
                return 0;
            }

            VLState pState = m_dictState[strStateGUID];
            if (pState == null)
            {
                return 0;
            }
            VLActionMgr pActionMgr = pState.GetActionMgr();
            if (pActionMgr == null)
            {
                return 0;
            }

            if (m_dictAction == null)
            {
                return 0;
            }
            int iRet = 0;
            int iAmount = pActionMgr.GetActionAmount();
            for (int i = 0; i < iAmount; i++)
            {
                VLActionBase pAction = pActionMgr.GetActionByIndex(i);
                if (pAction == null)
                {
                    continue;
                }
                m_dictAction.Remove(pAction.GetActionId());
                iRet++;
            }
            return iRet;
        }

        private int ClearStateEvent(string strStateGUID)
        {
            if (m_dictState == null)
            {
                return 0;
            }
            if (!m_dictState.ContainsKey(strStateGUID))
            {
                return 0;
            }

            VLState pState = m_dictState[strStateGUID];
            if (pState == null)
            {
                return 0;
            }
            VLEventMgr pEventMgr = pState.GetEventMgr();
            if (pEventMgr == null)
            {
                return 0;
            }

            if (m_dictEvent == null)
            {
                return 0;
            }
            int iRet = 0;
            int iAmount = pEventMgr.GetEventAmount();
            for (int i = 0; i < iAmount; i++)
            {
                VLEventBase pEvent = pEventMgr.GetEventByIndex(i);
                if (pEvent == null)
                {
                    continue;
                }
                m_dictEvent.Remove(pEvent.GetEventId());
                iRet++;
            }
            return iRet;
        }

        private int ClearStateMachineState(string strStateMachineGUID)
        {
            if (m_dictStateMachine == null)
            {
                return 0;
            }
            if (!m_dictStateMachine.ContainsKey(strStateMachineGUID))
            {
                return 0;
            }

            VLStateMachine pStateMachine = m_dictStateMachine[strStateMachineGUID];
            if (pStateMachine == null)
            {
                return 0;
            }
            VLStateMgr pStateMgr = pStateMachine.GetStateMgr();
            if (pStateMgr == null)
            {
                return 0;
            }

            if (m_dictState == null)
            {
                return 0;
            }
            int iRet = 0;
            int iAmount = pStateMgr.GetStateAmount();
            for (int i = 0; i < iAmount; i++)
            {
                VLState pState = pStateMgr.GetStateByIndex(i);
                if (pState == null)
                {
                    continue;
                }
                DelState(pState.GetStateId());
                iRet++;
            }
            return iRet;
        }

        private int ClearStateMachineSMParam(string strStateMachineGUID)
        {
            if (m_dictStateMachine == null)
            {
                return 0;
            }
            if (!m_dictStateMachine.ContainsKey(strStateMachineGUID))
            {
                return 0;
            }

            VLStateMachine pStateMachine = m_dictStateMachine[strStateMachineGUID];
            if (pStateMachine == null)
            {
                return 0;
            }
            VLSMParamMgr pSMParamMgr = pStateMachine.GetParamMgr();
            if (pSMParamMgr == null)
            {
                return 0;
            }

            if (m_dictSMParam == null)
            {
                return 0;
            }
            int iRet = 0;
            int iAmount = pSMParamMgr.GetParamAmount();
            for (int i = 0; i < iAmount; i++)
            {
                VLSMParam pSMParam = pSMParamMgr.GetParamByIndex(i);
                if (pSMParam == null)
                {
                    continue;
                }
                DelSMParam(pSMParam.GetParamId());
                iRet++;
            }
            return iRet;
        }

    }
}
