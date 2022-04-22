using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Helpers
{
    /// <summary> 設定檔讀取器 </summary>
    public class ConfigHelper
    {
        private const string _mainDBName = "MainDB";

        /// <summary> 讀取設定檔中的連線字串 </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            return GetConnectionString(_mainDBName);
        }

        /// <summary> 讀取設定檔中的連線字串 </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConnectionString(string name)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings[name].ConnectionString;

            return connectionString;
        }
    }
}