using CrystalDecisions.CrystalReports.Engine;
using IPSMART_CLOTHPOS.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPSMART_CLOTHPOS.Forms
{
    public partial class FrmCashMemo : Form
    {
        BillPrint billPrint = new BillPrint();
        public FrmCashMemo()
        {
            InitializeComponent();
        }

        private void FrmCashMemo_Load(object sender, EventArgs e)
        {

        }

        private void FrmCashMemo_SizeChanged(object sender, EventArgs e)
        {
            this.mainPannel.Location = new Point(
            this.ClientSize.Width / 2 - this.mainPannel.Size.Width / 2,
            this.ClientSize.Height / 2 - this.mainPannel.Size.Height / 2);
            this.mainPannel.Anchor = AnchorStyles.None;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            billPrint.printBill("");

        }
    }
}
