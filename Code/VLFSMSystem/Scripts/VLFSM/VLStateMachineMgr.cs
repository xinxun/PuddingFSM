

using System.Collections.Generic;
using UnityEngine;

namespace VL.VLFSMSystem
{//状态机管理类
    public class VLStateMachineMgr : MonoBehaviour
    {
        private List<VLStateMachine> m_listStateMachine = new List<VLStateMachine>();

        //pControlGameObject可操作的对象
        public bool Init()
        {
            if (m_listStateMachine == null)
            {
                return false;
            }
            //m_pControlObject = pControlGameObject;
            m_listStateMachine.Clear();
            return true;
        }

        public void PlayAllFSM()
        {
            foreach(VLStateMachine pStateMachine in m_listStateMachine)
            {
                if(pStateMachine == null)
                {
                    continue;
                }
                pStateMachine.PlayStateMachine();
            }
        }

        public void StopAllFSM()
        {
            foreach (VLStateMachine pStateMachine in m_listStateMachine)
            {
                if (pStateMachine == null)
                {
                    continue;
                }
                pStateMachine.StopStateMachine();
            }
        }

        //加入状态机
        public string AddStateMachine(string strStateMachineGUID, string strStateMachineName)
        {
            if(gameObject == null)
            {
                return "";
            }
            //m_uIndexStateMachineId++;
            //创建新的子对象挂接在statemachinemgr下
            GameObject pGameObject = new GameObject(strStateMachineName + strStateMachineGUID);
            if(pGameObject == null)
            {
                return "";
            }
            pGameObject.transform.SetParent(gameObject.transform);
            pGameObject.transform.localPosition = Vector3.zero;
            pGameObject.transform.localRotation = Quaternion.identity;
            pGameObject.transform.localScale = Vector3.one;
            VLStateMachine pVLStateMachine = pGameObject.AddComponent<VLStateMachine>();
            if(pVLStateMachine == null)
            {
                GameObject.Destroy(pGameObject);
                return "";
            }
            pVLStateMachine.Init(strStateMachineGUID, strStateMachineName);
            //加入管理列表
            m_listStateMachine.Add(pVLStateMachine);
            //状态机管理对象使用
            VLFSMManager.GetInstance().AddStateMachine(pVLStateMachine);
            return pVLStateMachine.GetStateMachinId();
        }

        //删除指定状态机
        public bool DelStateMachine(string uStateMachineId)
        {
            VLStateMachine vmsl = null;
            foreach(VLStateMachine pSm in m_listStateMachine)
            {
                if(pSm == null)
                {
                    continue;
                }
                if(pSm.GetStateMachinId().Equals(uStateMachineId))
                {
                    vmsl = pSm;
                    break;
                }
            }
            if (vmsl != null)
            {
                //状态机管理对象使用
                VLFSMManager.GetInstance().DelStateMachine(vmsl.GetStateMachinId());
                //删除组件
                GameObject.Destroy(vmsl);
                //删除节点
                GameObject.Destroy(vmsl.gameObject);
                m_listStateMachine.Remove(vmsl);
                return true;
            }
            return false;

        }

        public VLStateMachine GetStateMachine(string uStateMachineId)
        {
            foreach (VLStateMachine pSm in m_listStateMachine)
            {
                if (pSm == null)
                {
                    continue;
                }
                if (pSm.GetStateMachinId().Equals(uStateMachineId))
                {
                    //删除组件
                    return pSm;
                }
            }
            return null;
        }

        public int GetStateMachineAmount()
        {
            if(m_listStateMachine == null)
            {
                return 0;
            }
            return m_listStateMachine.Count;
        }

        public VLStateMachine GetStateMachineByIndex(int iIndex)
        {
            if (iIndex >= GetStateMachineAmount())
            {
                return null;
            }
            return m_listStateMachine[iIndex];
        }



    }
}
