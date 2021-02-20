using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MemberManagementSystem.Model.Service.Common
{
    public class Enum
    {
        /// <summary>
        /// 連線字串enum
        /// </summary>
        public enum ConnectionString
        {
            /// <summary>
            /// dbSystex
            /// </summary>
            [Description("dbSystex")]
            dbSystex,

            /// <summary>
            /// DBHQSavings
            /// </summary>
            [Description("DBHQSavings")]
            DBHQSavings,


            /// <summary>
            /// localdb
            /// </summary>
            [Description("localdb")]
            localdb,
        }
    }
}