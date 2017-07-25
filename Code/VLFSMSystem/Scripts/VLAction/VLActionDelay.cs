using LitJson;
using UnityEngine;


namespace VL.VLFSMSystem
{//行为基类
    public class VLActionDelay : VLActionBase
    {
        private bool m_bEnd = false;
        private float m_fBeginTime = 0.0f;
        private float m_fDiffTime = 0.0f;//为了显示才加了你


        /// <参数begin>
        public VLParam m_pVLParamBase = null;//从json转换过来的参数
        [System.Serializable]
        public class VLParam : VLParamBase
        {
            public VLParam()
            {
                bSyncGoOn = false;
            }
            
            public float fDelayTime = 0.0f;
        }
        /// <参数end>
        /// 

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



        public override VLActionType.EVLACTIONTYPE GetActionType()
        {
            return VLActionType.EVLACTIONTYPE.EVLACTIONTYPE_DELAY;
        }

        void Awake()
        {

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

        void Update()
        {
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
        }
        public override IVLActionParam GetActionParam()
        {
            return m_pVLParamBase;
        }
    }
}
