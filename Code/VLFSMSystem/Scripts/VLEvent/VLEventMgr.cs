using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace VL.VLFSMSystem
{//行为管理类
    public class VLEventMgr : MonoBehaviour
    {
        public delegate IVLEvent CreateCustomsEvent(VLEventType.EVLEVENTTYPE eEventType, GameObject pGameObject);//用户Action
        //行为列表
        private List<VLEventBase> m_listEvent = new List<VLEventBase>();
        private string m_uStateMachineGUID = "";
        private string m_uStateGUID = "";
        static public CreateCustomsEvent createCustomsEvent = null;//用户自定义Evnet回调
        //private uint m_uIndexEventId = 0;

        public bool Init(string uStateMachineId, string uStateId)
        {
            m_uStateMachineGUID = uStateMachineId;
            m_uStateGUID = uStateId;
            if (m_listEvent == null)
            {
                return false;
            }
            m_listEvent.Clear();
            return true;
        }

        //启动所有事件
        public void StartAllEvent()
        {
            foreach(VLEventBase pEvent in m_listEvent)
            {
                if(pEvent == null)
                {
                    continue;
                }
                pEvent.gameObject.SetActive(true);
            }
        }

        //停止所有事件
        public void StopAllEvent()
        {
            foreach (VLEventBase pEvent in m_listEvent)
            {
                if (pEvent == null)
                {
                    continue;
                }
                pEvent.gameObject.SetActive(false);
            }
        }

        //增加新的Event
        public string AddEvent(VLEventType.EVLEVENTTYPE eEventType, string uEventId, JsonData jsonData)
        {
            if (m_listEvent == null)
            {
                return "";
            }

            //创建Event并挂接到当前Gameobject下
            VLEventBase pVLEvent = CreateEvent(eEventType, uEventId);
            if(pVLEvent == null)
            {
                return "";
            }

            pVLEvent.Init(m_uStateMachineGUID, m_uStateGUID, uEventId, jsonData);
            //m_uIndexEventId++;
            m_listEvent.Add(pVLEvent);
            
            //状态机管理对象使用
            VLFSMManager.GetInstance().AddEvent(pVLEvent);
            return pVLEvent.GetEventId();
        }

        //删除指定行为
        public bool DelEvent(string uEventId)
        {
            foreach (VLEventBase pEvent in m_listEvent)
            {
                if (pEvent == null)
                {
                    continue;
                }
                if (pEvent.GetEventId().Equals(uEventId))
                {
                    //状态机管理对象使用
                    VLFSMManager.GetInstance().DelEvent(uEventId);
                    //删除组件
                    GameObject.Destroy(pEvent);
                    //删除节点
                    GameObject.Destroy(pEvent.gameObject);
                    m_listEvent.Remove(pEvent);
                    return true;
                }
            }
            return false;

        }

        public int GetEventAmount()
        {
            if(m_listEvent == null)
            {
                return 0;
            }
            return m_listEvent.Count;
        }

        public VLEventBase GetEventByIndex(int iIndex)
        {
            if (iIndex >= GetEventAmount())
            {
                return null;
            }
            return m_listEvent[iIndex];
        }

        public VLEventBase GetEventById(string uEventId)
        {
            foreach (VLEventBase pEvent in m_listEvent)
            {
                if (pEvent == null)
                {
                    continue;
                }
                if (pEvent.GetEventId().Equals(uEventId))
                {
                    return pEvent;
                }
            }
            return null;
        }

        void Update()
        {

        }


        //创建Event
        private VLEventBase CreateEvent(VLEventType.EVLEVENTTYPE eEventType, string uEventId)
        {
            //创建新的子对象挂接在statemachinemgr下
            GameObject pGameObject = new GameObject("" + eEventType);
            if (pGameObject == null)
            {
                return null;
            }
            
            pGameObject.transform.SetParent(gameObject.transform);
            pGameObject.transform.localPosition = Vector3.zero;
            pGameObject.transform.localRotation = Quaternion.identity;
            pGameObject.transform.localScale = Vector3.one;

            VLEventBase retEvent = AddEventComponent(eEventType, pGameObject) as VLEventBase;
            if (retEvent == null)
            {
                GameObject.Destroy(pGameObject);
                return null;
            }
            pGameObject.SetActive(false);

            return retEvent;
        }


        //在此增加不同Event对应不同的acitonbase
        private IVLEvent AddEventComponent(VLEventType.EVLEVENTTYPE eEventType, GameObject pGameObject)
        {
            if(pGameObject == null)
            {
                return null;
            }
            IVLEvent retEvent = null;
            switch (eEventType)
            {
                case VLEventType.EVLEVENTTYPE.EVLEVENTTYPE_NORMAL:
                    retEvent = pGameObject.AddComponent<VLEventNormal>();
                    break;
                case VLEventType.EVLEVENTTYPE.EVLEVENTTYPE_ONFINISH://2 onFinished调用 
                    retEvent = pGameObject.AddComponent <VLEventOnFinished>();
                    break;
                default:
                    if (createCustomsEvent != null)
                    {
                        retEvent = createCustomsEvent(eEventType, pGameObject);
                    }
                    else
                    {
                        retEvent = pGameObject.AddComponent<VLEventBase>();
                    }
                    break;
            }

            return retEvent;
        }

    }
}
