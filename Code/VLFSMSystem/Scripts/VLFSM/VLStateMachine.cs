
using UnityEngine;

namespace VL.VLFSMSystem
{//状态机
    public class VLStateMachine : MonoBehaviour
    {
        private VLStateMgr m_pVLStateMgr = null;//状态管理器
        private VLSMParamMgr m_pVLParameMgr = null;//参数管理器
        private string m_StateMachineGUID = "";
        private string m_StateMachineName = "null";



        //uStateMachineId状态机的id
        //strStateMachineName状态机名字
        public bool Init(string strStateMachineGUID, string strStateMachineName)
        {
            m_StateMachineGUID = strStateMachineGUID;
            m_StateMachineName = strStateMachineName;
            //加入状态管理器
            m_pVLStateMgr = gameObject.GetComponent<VLStateMgr>();
            if (m_pVLStateMgr == null)
            {
                m_pVLStateMgr = gameObject.AddComponent<VLStateMgr>();
                if (m_pVLStateMgr != null)
                {
                    m_pVLStateMgr.Init(m_StateMachineGUID);
                }
            }
            //加入参数管理器
            m_pVLParameMgr = gameObject.GetComponent<VLSMParamMgr>();
            if (m_pVLParameMgr == null)
            {
                m_pVLParameMgr = gameObject.AddComponent<VLSMParamMgr>();
                if (m_pVLParameMgr != null)
                {
                    m_pVLParameMgr.Init(m_StateMachineGUID);
                }
            }
            return true;
        }


        public string GetStateMachinId()
        {
            return m_StateMachineGUID;
        }

        public string GetStateMachineName()
        {
            return m_StateMachineName;
        }


        //从开始状态运行当前状态机
        public void PlayStateMachine()
        {
            if (m_pVLStateMgr != null)
            {
                m_pVLStateMgr.PlayCurrentState();
            }
        }


        //停止当前状态机
        public void StopStateMachine()
        {
            if (m_pVLStateMgr != null)
            {
                m_pVLStateMgr.StopCurrentState();
            }
        }

        //播放状态机下的指定状态
        public void PlayState(string uStateId)
        {
            if(m_pVLStateMgr != null)
            {
                //开始新状态
                m_pVLStateMgr.PlayState(uStateId);
            }
        }

        public VLStateMgr GetStateMgr()
        {
            return m_pVLStateMgr;
        }

        public VLSMParamMgr GetParamMgr()
        {
            return m_pVLParameMgr;
        }
    }
}
