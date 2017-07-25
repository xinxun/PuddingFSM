
using UnityEngine;

namespace VL.VLFSMSystem
{//状态
    public class VLState : MonoBehaviour
    {
        private string m_strStateMachineGUID = "";//所属状态机id
        private VLActionMgr m_pVLActionMgr = null;//状态管理器
        private VLEventMgr m_pVLEventMgr = null;//事件管理器
        private string m_strStateGUID = "";
        private string m_strStateName = "null";
        public bool mIsStateStart = false;

        //uStateMachineId状态的id
        //strStateMachineName状态名字
        public bool Init(string strStateMachineGUID, string uStateId, string strStateName)
        {
            m_strStateMachineGUID = strStateMachineGUID;
            if (m_strStateMachineGUID.Length<= 0)
            {
                return false;
            }
            m_strStateGUID = uStateId;
            m_strStateName = strStateName;

            //加入状态管理器
            m_pVLActionMgr = gameObject.GetComponent<VLActionMgr>();
            if (m_pVLActionMgr == null)
            {
                m_pVLActionMgr = gameObject.AddComponent<VLActionMgr>();
                if (m_pVLActionMgr != null)
                {
                    m_pVLActionMgr.Init(m_strStateMachineGUID, m_strStateGUID);
                }
            }
            m_pVLEventMgr = gameObject.GetComponent<VLEventMgr>();
            if(m_pVLEventMgr == null)
            {
                m_pVLEventMgr = gameObject.AddComponent<VLEventMgr>();
                if(m_pVLEventMgr != null)
                {
                    m_pVLEventMgr.Init(m_strStateMachineGUID, m_strStateGUID);
                }
            }
            return true;
        }

        public string GetStateId()
        {
            return m_strStateGUID;
        }

        public string GetStateName()
        {
            return m_strStateName;
        }

        public string GetStateMachineID()
        {
            return m_strStateMachineGUID;
        }

        public void SetStateName(string strStateName)
        {
            m_strStateName = strStateName;
        }

        public void PlayState()
        {
            gameObject.SetActive(true);
            if (m_pVLActionMgr != null)
            {
                m_pVLActionMgr.PlayAction();
            }
            if(m_pVLEventMgr != null)
            {
                m_pVLEventMgr.StartAllEvent();
            }
        }

        public void StopState()
        {    
            if (m_pVLActionMgr != null)
            {
                m_pVLActionMgr.StopAction();
            }
            if (m_pVLEventMgr != null)
            {
                m_pVLEventMgr.StopAllEvent();
            }
            gameObject.SetActive(false);
        }

        //是否是loop的action
        public void SetLoop(bool bLoop)
        {
            if (m_pVLActionMgr != null)
            {
                m_pVLActionMgr.SetLoop(bLoop);
            }
        }

        public bool IsActionListEnd()
        {
            if (!m_pVLActionMgr)
            {
                return true;
            }
            return m_pVLActionMgr.IsEnd();
        }

        //创建新的Actioin
        public string AddAction(VLActionType.EVLACTIONTYPE eActionType)
        {
            if(m_pVLActionMgr == null)
            {
                return "";
            }
            string strNewActionGUID = Func.GetGUID();
            return m_pVLActionMgr.AddAction(eActionType, strNewActionGUID, null);
        }

        //创建新的Event
        public string AddEvent(VLEventType.EVLEVENTTYPE eEventType)
        {
            if (m_pVLEventMgr == null)
            {
                return "";
            }
            string strNewEventGUID = Func.GetGUID();
            return m_pVLEventMgr.AddEvent(eEventType, strNewEventGUID, null);
        }

        public VLActionMgr GetActionMgr()
        {
            return m_pVLActionMgr;
        }

        public VLEventMgr GetEventMgr()
        {
            return m_pVLEventMgr;
        }

    }
}
