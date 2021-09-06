using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSMART_CLOTHPOS.Utils
{
  public  class BillPrint
    {
        public void printBill(string autono)
        {
            ReportDocument cryRpt = new ReportDocument();
            var GetDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var repPath = GetDirectory + "/" + "CrystalReport1.rpt";
            cryRpt.Load(repPath);
            //TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            //TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            //ConnectionInfo crConnectionInfo = new ConnectionInfo();
            //Tables CrTables;

            //crConnectionInfo.ServerName = "YOUR SERVERNAME";
            //crConnectionInfo.DatabaseName = "DATABASE NAME";
            //crConnectionInfo.UserID = "USERID";
            //crConnectionInfo.Password = "PASSWORD";

            //CrTables = cryRpt.Database.Tables;
            //foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            //{
            //    crtableLogoninfo = CrTable.LogOnInfo;
            //    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
            //    CrTable.ApplyLogOnInfo(crtableLogoninfo);
            //}

            cryRpt.Refresh();
            cryRpt.PrintToPrinter(2, true, 1, 2);


        }
    }
}
