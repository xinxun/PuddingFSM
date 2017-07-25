
using System.Collections.Generic;
using UnityEngine;

namespace VL.VLFSMSystem
{//状态管理类
    public class VLStateMgr : MonoBehaviour
    {
        //private uint m_uIndexStateId = 0;
        private string m_strStateMachineGUID = "";
        private List<VLState> m_listState = new List<VLState>();

        //private uint m_uStartStateId = 0;//开始状态id
        private string m_uCurrentStateGUID = "";//当前状态机id

        public string CurrentStateGUID
        {
            get { return m_uCurrentStateGUID; }
        }

        //uSceneObjectId:所属SceneObjectId
        //uStateMachineId:所属状态机id
        public bool Init(string uStateMachineId)
        {
            m_strStateMachineGUID = uStateMachineId;
            if (m_listState == null)
            {
                return false;
            }
            
            m_listState.Clear();

            //加入默认开始状态
            //m_uStartStateId = this.AddState("StartState");
            return true;
        }

        //加入状态机
        public string AddState(string strStatGUID, string strStateName)
        {
            if (gameObject == null)
            {
                return "";
            }
            //m_uIndexStateId++;
            //创建新的子对象挂接在statemachinemgr下
            GameObject pGameObject = new GameObject(strStateName + strStatGUID);
            if (pGameObject == null)
            {
                return "";
            }
            
            pGameObject.transform.SetParent(gameObject.transform);
            pGameObject.transform.localPosition = Vector3.zero;
            pGameObject.transform.localRotation = Quaternion.identity;
            pGameObject.transform.localScale = Vector3.one;
            VLState pVLState = pGameObject.AddComponent<VLState>();        
            if (pVLState == null)
            {
                GameObject.Destroy(pGameObject);
                return "";
            }
            pVLState.Init(m_strStateMachineGUID, strStatGUID, strStateName);
            m_listState.Add(pVLState);

            //状态机管理对象使用
            VLFSMManager.GetInstance().AddState(pVLState);

            pGameObject.SetActive(false);
            SetStart();
            return pVLState.GetStateId();
        }
        //删除指定状态
        public bool DelState(string strStateGUID)
        {
            foreach (VLState pState in m_listState)
            {
                if (pState == null)
                {
                    continue;
                }
                if (pState.GetStateId().Equals(strStateGUID))
                {
                    //状态机管理对象使用
                    VLFSMManager.GetInstance().DelState(strStateGUID);
                    //删除组件
                    GameObject.Destroy(pState);
                    //删除节点
                    GameObject.Destroy(pState.gameObject);
                    m_listState.Remove(pState);
                    SetStart();
                    return true;
                }
            }
            return false;

        }

        private void SetStart()
        {
            if (m_listState.Count >= 1)
            {
                VLState mVLState = m_listState[0];
                mVLState.mIsStateStart = true;
            }
        }

        public bool PlayCurrentState()
        {
            int iCount = m_listState.Count;
            if(iCount<=0)
            {
                return false;
            }
            foreach (VLState pState in m_listState)
            {
                if(pState == null)
                {
                    continue;
                }
                bool bRet = PlayState(pState.GetStateId());
                m_uCurrentStateGUID = pState.GetStateId();
                return bRet;
            }
            
            
            return false;

        }

        public bool StopCurrentState()
        {
            bool bRet = StopState(m_uCurrentStateGUID);
            m_uCurrentStateGUID = "";
            return bRet;
        }

        //以打断的方式进行播放
        public bool PlayState(string strStateGUID)
        {
            /*if( m_uCurrentStateGUID.Equals(strStateGUID))
            {//相同状态不重复打断播放
                return false;
            }*/
            VLState pNextState = GetStateById(strStateGUID);
            if(pNextState == null)
            {
                return false;
            }

            VLState pCurrentState = GetStateById(m_uCurrentStateGUID);
            if(pCurrentState != null)
            {
                pCurrentState.StopState();
                m_uCurrentStateGUID = "";
            }

            if (pNextState != null)
            {
                pNextState.PlayState();
                m_uCurrentStateGUID = strStateGUID;
            }
            
            return true;
        }

        private bool StopState(string strStateGUID)
        {
            VLState pStopState = GetStateById(strStateGUID);
            if(pStopState != null)
            {
                pStopState.StopState();
            }
            return true;
        }

        public VLState GetStateById(string strStateGUID)
        {
            foreach (VLState pState in m_listState)
            {
                if (pState == null)
                {
                    continue;
                }

                if (pState.GetStateId().Equals(strStateGUID))
                {
                    return pState;
                }

            }
            return null;
        }


        public int GetStateAmount()
        {
            if (m_listState == null)
            {
                return 0;
            }
            return m_listState.Count;
        }

        public VLState GetStateByIndex(int iIndex)
        {
            if (iIndex >= GetStateAmount())
            {
                return null;
            }
            return m_listState[iIndex];
        }
    }
}
