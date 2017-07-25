namespace VL.VLFSMSystem
{//行为基类
    public class VLEventOnFinished : VLEventBase
    {
        public override VLEventType.EVLEVENTTYPE GetEventType()
        {
            return VLEventType.EVLEVENTTYPE.EVLEVENTTYPE_ONFINISH;
        }

        #region unity接口函数

        void Update()
        {
            if (!IsEndAction())
            {
                return;
            }
            VLActionObject pVLActionObject = GetCurVLActionObject();
            if (pVLActionObject == null)
            {
                return;
            }
            pVLActionObject.PlayState(pParamBase.changeStateGUID);

        }
        #endregion

        //当前状态是不是都结束了
        private bool IsEndAction()
        {

            VLActionObject pVLActionObject = GetCurVLActionObject();
            if(pVLActionObject == null)
            {
                return false;
            }

            if (!pVLActionObject.IsActionAllEnd(stateGUID))
            {
                return false;
            }

            return true;
           
        }

      
    }
}
