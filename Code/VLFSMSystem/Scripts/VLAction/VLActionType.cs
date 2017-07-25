
using System;

namespace VL.VLFSMSystem
{
    public class VLActionType
    {
//        private static readonly VLActionType _instance = new VLActionType();
//        public static VLActionType GetInstance()
//        {
//            return _instance;
//        }
//
//        //保存所有界面对应的prefabs的位置
//        private Dictionary<EVLACTIONTYPE, string> dicTypeName = new Dictionary<EVLACTIONTYPE, string>();

        public enum EVLACTIONTYPE
        {
            EVLACTIONTYPE_BASE=0,
            //<不同Action定义>===========begin
            EVLACTIONTYPE_EFFECT =1,     //光效行为
            EVLACTIONTYPE_ANI =2,        //动作行为
            EVLACTIONTYPE_MOVE =3,       //移动行为
            EVLACTIONTYPE_MOVETOPOS =4,  //移动到位置
            EVLACTIONTYPE_DELAY =5,      //延迟行为
            EVLACTIONTYPE_HIGHTLIGHTEDGE = 6,//高亮行为
            EVLACTIONTYPE_RANDOM_MOVE = 7,//随机移动行为
            EVLACTIONTYPE_FACE_TO_OBJECT = 8,//面向对象行为
            EVLACTIONTYPE_BACK_TO_OBJECT = 9,//背对对象行为
            EVLACTIONTYPE_TAKE_A_WALK = 10,//散步
            EVLACTIONTYPE_ESCAPE = 11,//逃跑
            EVLACTIONTYPE_FOLLOW = 12,//跟随
            EVLACTIONTYPE_SET_TAG = 13,//设置tag
            EVLACTIONTYPE_Add_360_MOVIE,//添加360电影
            EVLACTIONTYPE_INVOKE_LUA,    //调用LUA行为
            EVLACTIONTYPE_SENDEVENT, //发送事件行为
            EVLACTIONTYPE_PLAY_ANIM, //做动作行为

            /// </不同Action定义>==========end
            EVLACTIONTYPE_NULL,
        }

//        public void init()
//        {
//            for (VLActionType.EVLACTIONTYPE eType = VLActionType.EVLACTIONTYPE.EVLACTIONTYPE_BASE; eType < VLActionType.EVLACTIONTYPE.EVLACTIONTYPE_NULL; ++eType)
//            {
//                string strInfo = eType.ToString();
//                System.Reflection.FieldInfo field = eType.GetType().GetField(strInfo);
//                object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
//                if (objs == null || objs.Length == 0)
//                {
//                    continue;
//                }
//
//                System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
//
//            }
//        }
//        

        static public EVLACTIONTYPE GetActionByType(string strTypeName)
        {
            return (EVLACTIONTYPE)Enum.Parse(typeof(EVLACTIONTYPE), strTypeName);
        }
    }
}
