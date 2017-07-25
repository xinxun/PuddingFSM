
using LitJson;
namespace VL.VLFSMSystem
{
    //行为接口(提供外部接口)
    public interface IVLAction
    {
        VLActionType.EVLACTIONTYPE GetActionType();
        //初始化
        bool Init(string strStateMachineGUID, string strStateGUID, string strActionGUID, JsonData pJsonData);
        //得到当前Actionid
        string GetActionId();
        //进入Action时调用 
        void OnEnter();
        //退出Action时调用
        void OnExit();
        //当前Action是否结束
        bool IsEnd();
        //保存数据时使用
        JsonData GetParamData();

        IVLActionParam GetActionParam();
    }
}
