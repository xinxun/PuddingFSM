using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace VL.VLFSMSystem
{//行为管理类
    public class VLActionMgr : MonoBehaviour
    {
        public delegate IVLAction CreateCustomsAction(VLActionType.EVLACTIONTYPE eActionType, GameObject pGameObject);//用户Action

        [SerializeField]
        public bool m_bLoopProcess = false;//是否重复运行
        static public CreateCustomsAction createCustomsAction = null;//用户自定义Action

        //行为列表
        private List<VLActionBase> m_listAction = new List<VLActionBase>();
        private string m_uStateMachineId = "";
        private string m_uStateId = "";
        //private uint m_uIndexActionId = 0;

        private int m_iCurActionIndex = 0;//当前播放的ActionIndex
        private Dictionary<string, VLActionBase> mActiveAction = new Dictionary<string, VLActionBase>(); //当前使用的action列表
        private VLActionBase m_curAction = null;//当前使用的action 顺序
        private bool m_bEnd = false;        //当前Action是否运行完成

        public bool IsActive(string actionId)
        {
            return mActiveAction.ContainsKey(actionId);
        }

        public bool Init(string uStateMachineId, string uStateId)
        {
            m_uStateMachineId = uStateMachineId;
            m_uStateId = uStateId;
            if (m_listAction == null)
            {
                return false;
            }
            m_listAction.Clear();
            m_iCurActionIndex = 0;
            return true;
        }

        //播放当前的Action列表，逐个播放
        public void PlayAction()
        {
            if(m_iCurActionIndex != 0)
            {//停止当前在播放的Action
                StopActionByIndex(m_iCurActionIndex);
            }
            PlayActionByIndex(m_iCurActionIndex);
            m_iCurActionIndex = 0;//重新播放当前列表
            m_curAction = null;
            m_bEnd = false;      
        }

        //停止当前所有的行为，一次性全部停止 
        public void StopAction()
        {
            StopAllAction();
            m_iCurActionIndex = 0;
            m_curAction = null;
            mActiveAction.Clear();
            m_bEnd = true;
        }

        public bool IsEnd()
        {
            return m_bEnd;
        }

        public void SetLoop(bool bLoop)
        {
            m_bLoopProcess = bLoop;
        }

        //增加新的Action
        public string AddAction(VLActionType.EVLACTIONTYPE eActionType, string uActionId, JsonData pJsonData)
        {
            if (m_listAction == null)
            {
                return "";
            }

            //创建Action并挂接到当前Gameobject下
            VLActionBase pVLAction = CreateAction(eActionType, uActionId);
            if(pVLAction == null)
            {
                return "";
            }

            pVLAction.Init(m_uStateMachineId, m_uStateId, uActionId, pJsonData);
            //m_uIndexActionId++;
            m_listAction.Add(pVLAction);
            

            //状态机管理对象使用
            VLFSMManager.GetInstance().AddAction(pVLAction);
            return pVLAction.GetActionId();
        }

        //删除指定行为
        public bool DelAction(string uActionId)
        {
            foreach (VLActionBase pAction in m_listAction)
            {
                if (pAction == null)
                {
                    continue;
                }
                if (pAction.GetActionId().Equals(uActionId))
                {
                    //删除对应的事件（不在onDestroy删除）
                    VLFSMManager.GetInstance().DelAction(uActionId);
                    //删除组件
                    GameObject.Destroy(pAction);
                    //删除节点
                    GameObject.Destroy(pAction.gameObject);
                    m_listAction.Remove(pAction);
                    return true;
                }
            }
            return false;

        }

        public bool MoveAction(string targeId,string moveId)
        {
            if (targeId == null || moveId == null)
            {
                return false;
            }
            int moveIndex = -1;
            int targetIndex = -1;
            VLActionBase moveAction = null;
            foreach (VLActionBase pAction in m_listAction)
            {
                if (pAction == null)
                {
                    continue;
                }
                if (pAction.GetActionId().Equals(targeId))
                {
                    targetIndex = m_listAction.IndexOf(pAction);
                }
                if (pAction.GetActionId().Equals(moveId))
                {
                    moveAction = pAction;
                    moveIndex = m_listAction.IndexOf(pAction);
                }
                if (moveIndex != -1 && targetIndex != -1)
                {
                    m_listAction.Remove(moveAction);
                    if (moveIndex > targetIndex)
                    {
                        m_listAction.Insert(targetIndex,moveAction);
                    }
                    else
                    {
                        m_listAction.Insert(targetIndex-1,moveAction);
                    }
                    return true;
                }
            }
            return false;

        }

        public IVLAction GetActionById(string uActionId)
        {
            int iRetIndex = 0;
            foreach (VLActionBase pAction in m_listAction)
            {
                if (pAction == null)
                {
                    continue;
                }
                if (pAction.GetActionId().Equals(uActionId))
                {
                    return pAction;
                }
            }
            return null;
        }


        //创建Action
        private VLActionBase CreateAction(VLActionType.EVLACTIONTYPE eActionType, string uActionId)
        {
            //创建新的子对象挂接在statemachinemgr下
            GameObject pGameObject = new GameObject("" + eActionType);
            if (pGameObject == null)
            {
                return null;
            }
            
            pGameObject.transform.SetParent(gameObject.transform);
            pGameObject.transform.localPosition = Vector3.zero;
            pGameObject.transform.localRotation = Quaternion.identity;
            pGameObject.transform.localScale = Vector3.one;

            VLActionBase retAction = AddActionComponent(eActionType, pGameObject) as VLActionBase;
            if (retAction == null)
            {
                GameObject.Destroy(pGameObject);
                return null;
            }
            pGameObject.SetActive(false);

            return retAction;
        }


        //在此增加不同action对应不同的acitonbase
        private IVLAction AddActionComponent(VLActionType.EVLACTIONTYPE eActionType, GameObject pGameObject)
        {
            if(pGameObject == null)
            {
                return null;
            }
            IVLAction retAction = null;
            switch (eActionType)
            {
                case VLActionType.EVLACTIONTYPE.EVLACTIONTYPE_EFFECT:   //1 光效行为
                    break;
                case VLActionType.EVLACTIONTYPE.EVLACTIONTYPE_ANI://2 动作行为
                    break;
                case VLActionType.EVLACTIONTYPE.EVLACTIONTYPE_DELAY://5 延迟行为
                    retAction = pGameObject.AddComponent<VLActionDelay>();
                    break;
                case VLActionType.EVLACTIONTYPE.EVLACTIONTYPE_SENDEVENT:
                    retAction = pGameObject.AddComponent<VLActionSendEvent>();
                    break;
                default:
                    if(createCustomsAction != null)
                    {
                        retAction = createCustomsAction(eActionType, pGameObject);
                    }
                    else
                    { 
                        retAction = pGameObject.AddComponent<VLActionBase>();
                    }
                    break;
            }

            return retAction;
        }

        void Update()
        {
            if(m_bEnd)
            {
                return;
            }

            if(m_listAction == null)
            {
                return;
            }

            int iCount = m_listAction.Count;
            if (iCount <= 0)
            {
                return;
            }

            for (int iActionIndex = 0; iActionIndex < iCount; iActionIndex++)
            {
                VLActionBase pVlActionBase = GetActionByIndex(iActionIndex);
                if (pVlActionBase == null || pVlActionBase.GetActionParam() == null)
                {
                    continue;
                }
                if (mActiveAction.ContainsKey(pVlActionBase.actionGUID))
                {
                    continue;
                }
                VLActionBase.VLParamBase paramBase = pVlActionBase.GetActionParam() as VLActionBase.VLParamBase;
                if (paramBase == null)
                {
                    continue;
                }
                if (paramBase.bSyncGoOn)
                {
                    if (!pVlActionBase.gameObject.activeSelf)
                    {
                        pVlActionBase.gameObject.SetActive(true);
                        pVlActionBase.OnEnter();
                        mActiveAction.Add(pVlActionBase.actionGUID, pVlActionBase);
                    }
                }
            }

            if (m_curAction ==  null)
            {//第一次播放
                m_curAction = GetActionByIndex(m_iCurActionIndex);
                if(m_curAction == null)
                {
                    return;
                }
                mActiveAction.Add(m_curAction.actionGUID,m_curAction);
                if (!m_curAction.gameObject.activeSelf)
                {
                    PlayActionByIndex(m_iCurActionIndex);
                }
                return;
                
            }

            if (!m_curAction.IsEnd())
            {//当前状态没有结束
                return;
            }

            //切换acton
            StopActionByIndex(m_iCurActionIndex);

            bool isLoop;
            do
            {
                m_iCurActionIndex++;
                if (m_bLoopProcess)
                { //当前Action是否是loop
                    m_iCurActionIndex = m_iCurActionIndex % iCount;
                }

                //当前Action是否已经运行完成
                if (m_iCurActionIndex >= iCount)
                {
                    m_bEnd = true;
                    m_iCurActionIndex = 0;
                    m_curAction = null;
                    return;
                }
                VLActionBase pVlActionBase = GetActionByIndex(m_iCurActionIndex);
                if (pVlActionBase != null && pVlActionBase.GetActionParam() != null && pVlActionBase.m_pParamBase.bSyncGoOn)
                {
                    VLActionBase.VLParamBase paramBase = pVlActionBase.GetActionParam() as VLActionBase.VLParamBase;
                    if (paramBase != null && paramBase.bSyncGoOn)
                    {
                        isLoop = true;
                        continue;
                    }
                }
                isLoop = false;
            } while (isLoop);

            //当前Action开始运行
            m_curAction = GetActionByIndex(m_iCurActionIndex);
            mActiveAction.Add(m_curAction.actionGUID, m_curAction);
            PlayActionByIndex(m_iCurActionIndex);

        }

       

        private bool PlayActionByIndex(int iIndex)
        {
            IVLAction pAction = GetActionByIndex(iIndex);
            if(pAction == null)
            {
                return false;
            }

            VLActionBase pBaseAction = pAction as VLActionBase;
            if(pBaseAction == null)
            {
                return false;
            }
            pBaseAction.gameObject.SetActive(true);
            pBaseAction.OnEnter();
            
            return true;
        }

        private void StopAllAction()
        {
            int amount = GetActionAmount();
            for (int i = 0;i< amount;i++)
            {
                IVLAction pAction = m_listAction[i];
                if (pAction == null)
                {
                    continue;
                }
                VLActionBase pBaseAction = pAction as VLActionBase;
                if (pBaseAction.gameObject.activeSelf)
                {
                    pBaseAction.OnExit();
                    pBaseAction.gameObject.SetActive(false);
                }
            }
        }
        private bool StopActionByIndex(int iIndex)
        {
            IVLAction pAction = GetActionByIndex(iIndex);
            if (pAction == null)
            {
                return false;
            }

            VLActionBase pBaseAction = pAction as VLActionBase;
            if (pBaseAction == null)
            {
                return false;
            }
            
            pBaseAction.OnExit();
            pBaseAction.gameObject.SetActive(false);
            mActiveAction.Remove(pBaseAction.actionGUID);
            return true;
        }


        public int GetActionAmount()
        {
            if (m_listAction == null)
            {
                return 0;
            }
            return m_listAction.Count;
        }

        public VLActionBase GetActionByIndex(int iIndex)
        {
            if (iIndex >= GetActionAmount())
            {
                return null;
            }
            return m_listAction[iIndex];
        }
    }
}
