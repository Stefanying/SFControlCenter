using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Configuring.Business
{
    //继电器动作类型
   public enum RelayOperationType
    {
        吸合=0,
        断开
    }

   public class RelayOperationDataList
    {
        private string[] _operationDatas = new string[2];
        public void SetOperationData(RelayOperationType p_type, string data)
        {
            _operationDatas[(int)p_type] = data;
        }

        public string GetOperationData(RelayOperationType p_type)
        {
            return _operationDatas[(int)p_type];
        }

    }

    //继电器模块设置
   public class UserRelaySetting
    {
     
       //继电器序号
        private int _relayId;
        public int RelayId
        {
            get { return _relayId; }
            set { _relayId = value; }
        }
        private static int _approachCount =0;

        List<RelayOperationDataList> _relayOperationDatas;
        public List<RelayOperationDataList> RelayOperationDatas
        {
            get { return _relayOperationDatas; }
            set { _relayOperationDatas = value; }
        }
        

        public UserRelaySetting( int id,int p_approachCout)
        {
            _relayId = id;
            _approachCount = p_approachCout;
            _relayOperationDatas = new List<RelayOperationDataList>();
        }

        public void AddRelayOperationData(RelayOperationDataList p_relayOperationData)
        {
            _relayOperationDatas.Add(p_relayOperationData);
        }

        public void RemoveRelayOperationData(RelayOperationDataList p_relayOperationData)
        {
            _relayOperationDatas.Remove(p_relayOperationData);
        }     
    }

    //继电器模块管理（包含多个继电器模块）
   public class UserRelayArray
   {
       string _name;
       public string Name
       {
           get { return _name; }
           set { _name = value; }
       }

       //每个继电器模块
       List<UserRelaySetting> _relayOperationDatas;
       public List<UserRelaySetting> RelayOperationDatas
       {
           get { return _relayOperationDatas; }
           set { _relayOperationDatas = value; }
       }

       //单个继电器模块串口数据
       private ComSetting _relayCom;
       public ComSetting RelayCom
       {
           get { return _relayCom; }
           set { _relayCom = value; }
       }


       //路数继电器拥有多少路
       private int _approachCout;
       public int ApproachCout
       {
           get { return _approachCout; }
           set { _approachCout = value; }
       }

       public UserRelayArray(string p_name,ComSetting relaycom,int p_approachCount)
       {
           _name = p_name;
           _relayCom = relaycom;
           _approachCout = p_approachCount;
           _relayOperationDatas = new List<UserRelaySetting>();
       }

       public void AddRelayData(UserRelaySetting p_userelaySetting)
       {
           _relayOperationDatas.Add(p_userelaySetting);
       }

       public void RemoveRelayData(UserRelaySetting p_userelaySetting)
       {
           _relayOperationDatas.Remove(p_userelaySetting);
       }
 
   }
}
