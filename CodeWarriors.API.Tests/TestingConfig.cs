using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarriors.API.Tests
{
    public class TestingConfig
    {
        public static bool XUnit
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["TestingFramework"].Equals("xUnit",
                        StringComparison.CurrentCultureIgnoreCase);
                }
                catch (Exception)
                {
                }
                return false;
            }
        }

        public static bool MsUnit
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["TestingFramework"].Equals("MSUnit",
                        StringComparison.CurrentCultureIgnoreCase);
                }
                catch (Exception)
                {
                }
                return false;
            }
        }
    }
}
