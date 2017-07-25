
//状态机对外接口
using LitJson;

namespace VL.VLFSMSystem
{
    public class VLFSMSystemInterface
    {//对外的全局接口
        private static readonly VLFSMSystemInterface _instance = new VLFSMSystemInterface();

        public static VLFSMSystemInterface GetInstance()
        {
            return _instance;
        }

        //初始化调用
        public void Init()
        {

        }

        //进入地图调用
        public void OnEnterCopyMap()
        {
            VLFSMManager.GetInstance().Init();
        }

        public void ResetData()
        {
            //清空数据
            VLFSMDataMgr.Instance.Reset();
        }   

    }
}
