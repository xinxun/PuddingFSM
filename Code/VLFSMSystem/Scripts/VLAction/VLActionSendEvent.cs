using LitJson;
using UnityEngine;
using VL.Common;

namespace VL.VLFSMSystem
{//触发事件行为
    public class VLActionSendEvent : VLActionBase
    {
        private bool m_bEnd = false;
        private float m_fBeginTime = 0.0f;
        private float m_fDiffTime = 0.0f;//为了显示才加了你

        #region 参数配置
        /// <参数begin>
        public VLParam m_pVLParamBase = null;//从json转换过来的参数
        [System.Serializable]
        public class VLParam : VLParamBase
        {   
            public float fDelayTime = 0.0f;//延时相应

            [PIAttribut(PIADefine.EPIATYPE.PIATYPE_FSMEVENTLIST)]
            public string eventGUID = "";  //发送的EventGUID
        }
        /// <参数end>
        /// 
        #endregion

        #region 必须重载的函数
        public override bool Init(string strStateMachineGUID, string strStateGUID, string strActionGUID, JsonData pJsonData)
        {
            base.Init(strStateMachineGUID, strStateGUID, strActionGUID, pJsonData);
            m_pVLParamBase = GetParam<VLParam>();

            return true;
        }

        //必须重载
        public override JsonData GetParamData()
        {
            return Param2Json<VLParam>(m_pVLParamBase);
        }

        public override IVLActionParam GetActionParam()
        {
            return m_pVLParamBase;
        }
        #endregion

        #region 接口处理

        public override VLActionType.EVLACTIONTYPE GetActionType()
        {
            return VLActionType.EVLACTIONTYPE.EVLACTIONTYPE_SENDEVENT;
        }

        public override bool IsEnd()
        {
            return m_bEnd;
        }

        public override void OnEnter()
        {
            m_pVLParamBase = GetParam<VLParam>();
            m_bEnd = false;
            m_fBeginTime = Time.time;
            m_fDiffTime = 0.0f;

        }

        public override void OnExit()
        {
            m_bEnd = true;
            
        }
        #endregion

        #region unity基础接口
        void Awake()
        {

        }

        void Update()
        {
            if(m_bEnd)
            {
                return;
            }
            //得到结束时间
            float fDelayTime = 0.0f ;
            if(m_pVLParamBase != null)
            {
                fDelayTime = m_pVLParamBase.fDelayTime;
            }
            m_fDiffTime = Time.time - m_fBeginTime;
            if (m_fDiffTime >= fDelayTime)
            {
                m_bEnd = true;
            }
            else
            {
                return;
            }

            VLEventBase pEventBase = VLFSMManager.GetInstance().GetEventByGUID(m_pVLParamBase.eventGUID);
            if(pEventBase != null)
            {
                pEventBase.Play();
            }

        }
        #endregion

    }
}
