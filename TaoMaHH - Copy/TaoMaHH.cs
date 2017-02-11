using System;
using System.Collections.Generic;
using System.Text;
using Plugins;
using CDTLib;
using System.Data;
using System.Globalization;


namespace TaoMaHH
{
    public class TaoMaHH : ICData
    {
        DataCustomData _data;
        InfoCustomData _info = new InfoCustomData(IDataType.MasterDetailDt);
        NumberFormatInfo nfi = new NumberFormatInfo();

        #region ICData Members

        public DataCustomData Data
        {
            set { _data = value; }
        }

        public void ExecuteAfter()
        {
          
        }

        public void ExecuteBefore()
        {
            
        }

        public InfoCustomData Info
        {
            get { return _info; }
        }

        #endregion
    }
}
