using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace VL.VLFSMSystem
{
    //状态机数据管理类
    public class VLFSMDataMgr
    {
        public static readonly VLFSMDataMgr Instance = new VLFSMDataMgr();
        

        public VLStateMachineDataMgr StateMachineDataMgr
        {
            get { return m_oStateMachineDataMgr; }
        }
        VLStateMachineDataMgr m_oStateMachineDataMgr = new VLStateMachineDataMgr();

        public VLStateDataMgr StateDataMgr
        {
            get { return m_oStateDataMgr; }
        }
        VLStateDataMgr m_oStateDataMgr = new VLStateDataMgr();

        public VLParamDataMgr ParamDataMgr
        {
            get { return m_oParamDataMgr; }
        }
        VLParamDataMgr m_oParamDataMgr = new VLParamDataMgr();

        public VLActionDataMgr ActionDataMgr
        {
            get { return m_oActionDataMgr; }
        }
        VLActionDataMgr m_oActionDataMgr = new VLActionDataMgr();

        public VLEventDataMgr EventDataMgr
        {
            get { return m_oEventDataMgr; }
        }
        VLEventDataMgr m_oEventDataMgr = new VLEventDataMgr();

        public void Reset()
        {
            StateMachineDataMgr.Reset();
            StateDataMgr.Reset();
            ParamDataMgr.Reset();
            ActionDataMgr.Reset();
            EventDataMgr.Reset();
        }
       
    } //类末尾
} //命名空间末尾
