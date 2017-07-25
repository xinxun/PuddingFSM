
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace VL.VLFSMSystem
{//参数管理类
    public class VLSMParamMgr : MonoBehaviour
    {
        private string m_strStateMachineGUID = "";
        private List<VLSMParam> m_listParam = new List<VLSMParam>();
        
        //uSceneObjectId:所属SceneObjectId
        //uStateMachineId:所属状态机id
        public bool Init(string uStateMachineId)
        {
            m_strStateMachineGUID = uStateMachineId;
            if (m_listParam == null)
            {
                return false;
            }
            m_listParam.Clear();
            return true;
        }

        //加入参数
        public string AddParam(string strParamGUID, string strParamName , JsonData pJsonData)
        {
            if (gameObject == null)
            {
                return "";
            }

            GameObject pGameObject = new GameObject(strParamName + strParamGUID);
            if (pGameObject == null)
            {
                return "";
            }
            pGameObject.SetActive(false);
            pGameObject.transform.SetParent(gameObject.transform);
            pGameObject.transform.localPosition = Vector3.zero;
            pGameObject.transform.localRotation = Quaternion.identity;
            pGameObject.transform.localScale = Vector3.one;
            VLSMParam pVLParam = pGameObject.AddComponent<VLSMParam>();
            if (pVLParam == null)
            {
                GameObject.Destroy(pGameObject);
                return "";
            }
            pVLParam.Init(m_strStateMachineGUID, strParamGUID, pJsonData);
            m_listParam.Add(pVLParam);

            VLFSMManager.GetInstance().AddSMParam(pVLParam);

            return pVLParam.GetParamId();
        }
        //删除指定参数
        public bool DelParam(string strParamGUID)
        {
            foreach (VLSMParam pParam in m_listParam)
            {
                if (pParam == null)
                {
                    continue;
                }
                if (pParam.GetParamId().Equals(strParamGUID))
                {
                    VLFSMManager.GetInstance().DelSMParam(strParamGUID);
                    //删除组件
                    GameObject.Destroy(pParam);
                    //删除节点
                    GameObject.Destroy(pParam.gameObject);
                    m_listParam.Remove(pParam);
                    return true;
                }
            }
            return false;

        }
        public VLSMParam GetParamById(string strParamGUID)
        {
            foreach (VLSMParam pParam in m_listParam)
            {
                if (pParam == null)
                {
                    continue;
                }

                if (pParam.GetParamId().Equals(strParamGUID))
                {
                    return pParam;
                }

            }
            return null;
        }

        public VLSMParam GetParamByName(string strParamName)
        {
            foreach (VLSMParam pParam in m_listParam)
            {
                if (pParam == null)
                {
                    continue;
                }
                if (pParam.GetParamName().Equals(strParamName))
                {
                    return pParam;
                }
            }
            return null;
        }


        public int GetParamAmount()
        {
            if (m_listParam == null)
            {
                return 0;
            }
            return m_listParam.Count;
        }

        public VLSMParam GetParamByIndex(int iIndex)
        {
            if (iIndex >= GetParamAmount())
            {
                return null;
            }
            return m_listParam[iIndex];
        }
    }
}
