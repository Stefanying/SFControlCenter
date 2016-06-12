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
        private string[] _operationDatas= new string[2];
        public string[] OperationDatas
        {
            get { return _operationDatas; }
            set { _operationDatas = value; }
        }
    }

    //继电器模块设置
   public class UserRelaySetting
    {
       //单个继电器模块名称
        private string _relayName;
        public string RelayName
        {
            get { return _relayName; }
            set { _relayName = value; }
        }


        private RelayOperationDataList[] _relayDatas;

        public UserRelaySetting(string p_name, int p_approachCout)
        {
            _relayDatas = new RelayOperationDataList[p_approachCout];
            _relayName = p_name;
        }

       //获取对应序号的对应状态的继电器数据
        public string GetOperationData(int p_relayIndex, RelayOperationType p_type)
        {
            if (p_relayIndex < 0 || p_relayIndex >= _relayDatas.Length)
                return null;
            return _relayDatas[p_relayIndex].OperationDatas[(int)p_type];
        }

        public void SetOperationData(int p_relayIndex, RelayOperationType p_type,string p_data)
        {
            if (p_relayIndex < 0 || p_relayIndex >= _relayDatas.Length)
                return;
            _relayDatas[p_relayIndex].OperationDatas[(int)p_type]= p_data;
        }
    }

   public class UserRelay
   {
       string _name;
       public string Name
       {
           get { return _name; }
           set { _name = value; }
       }

       //每个继电器操作数据
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


       //
       private int _approachCout;
       public int ApproachCout
       {
           get { return _approachCout; }
           set { _approachCout = value; }
       }

       public UserRelay(string p_name,ComSetting relaycom,int p_approachCount)
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
