using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IPSMART_CLOTHPOS;
using Oracle.ManagedDataAccess.Client;
using IPSMART_CLOTHPOS.Utils;
using IPSMART_CLOTHPOS.Forms;

namespace IPSMART_CLOTHPOS
{

    public partial class FrmLogin : Form
    {
        DBConnection Cn = new DBConnection();
        string sql = "";
        string comp_table = "";
        string SCHEMA = "IMPROVAR";
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            switch (Session.Modcd)
            {
                case "F":
                    comp_table = "FIN_COMPANY";
                    break;
                case "S":
                    comp_table = "SD_COMPANY";
                    break;
                case "P":
                    comp_table = "PAY_COMPANY";
                    break;
                case "I":
                    comp_table = "INV_COMPANY";
                    break;
            }
        }
        private void bindCompany()
        {

            sql = "SELECT DISTINCT COMPCD,COMPNM from " + SCHEMA + "." + comp_table;
            sql = sql + " where compcd in (select compcd from ms_musracs ";
            sql = sql + " where user_id='" + Session.UserId + "' and module_code like '" + Session.ModuleCode + "%') ";
            sql = sql + " order by COMPNM";
            //DataTable recordCompany = Cn.SQLquery(sql);
            //sql = "select distinct compnm,compcd from sd_company ";
            var dt = Cn.SQLquery(sql);
            cmbCompany.ValueMember = "compcd";
            cmbCompany.DisplayMember = "compnm";
            cmbCompany.DataSource = dt;
        }

        private void bindLocation(string compcd)
        {
            sql = "select distinct loccd,locnm  from " + SCHEMA + "." + comp_table + "  where  ";
            sql = sql + " loccd in (select loccd from ms_musracs where user_id='" + Session.UserId + "' and compcd='" + compcd + "' and module_code like '" + Session.ModuleCode + "%') ";
            sql = sql + " order by locnm";
            var dt = Cn.SQLquery(sql);
            cmbLocation.ValueMember = "loccd";
            cmbLocation.DisplayMember = "locnm";
            cmbLocation.DataSource = dt;

            //var dt = Cn.SQLquery(sql); string loccd = "";
            //if (dt.Rows.Count > 0)
            //{
            //    schemaname = dt.Rows[0]["schema_name"].ToString();
            //}


            //sql = "select distinct schema_name from sd_company a  where compcd='" + compcd + "'"+ " ";
            //var dt = Cn.SQLquery(sql);string schemaname = "";
            //if (dt.Rows.Count > 0)
            //{
            //    schemaname = dt.Rows[0]["schema_name"].ToString();
            //}

            //sql = "select distinct locnm,loccd from"
            //    + "" + schemaname + ".M_loca where compcd='" + compcd + "'";
            //    +" ";
            //var dt = Cn.SQLquery(sql);
            //cmbCompany.DataSource = dt;
            //cmbCompany.ValueMember = "compnm";
            //cmbCompany.DisplayMember = "compcd";
        }
        private void bindFinyr(string compcd, string loccd)
        {
            sql = "select DISTINCT compcd,loccd, TO_CHAR(from_date,'DD/MM/YYYY') ||' - '|| TO_CHAR(upto_date,'DD/MM/YYYY') as FINYR, from_date ";
            sql += " from " + SCHEMA + "." + comp_table + " where ";
            sql += " schema_name in (select schema_name from ms_musracs where user_id='" + Session.UserId + "'  and compcd='" + compcd + "'  and loccd='" + loccd + "'  and module_code like '" + Session.ModuleCode + "%') ";
            sql += " order by from_date desc ";

            //sql = "select distinct year_code,from_date from sd_company where compcd='" + compcd + "' and loccd='" + loccd + "' ";
            var dt = Cn.SQLquery(sql);
            cmbFinyr.ValueMember = "year_code";
            cmbFinyr.DisplayMember = "FINYR";
            cmbFinyr.DataSource = dt;
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserId.Text))
            {
                MessageBox.Show("Please enter UserID", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter Password", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            var msg = Login(txtUserId.Text, txtPassword.Text);
            bindCompany();
            if (msg == "ok")
            {
                GroupBoxLogin.Visible = false;
                groupBoxCompany.Visible = true;
            }
            else
            {
                MessageBox.Show(msg, "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cmbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindLocation(cmbCompany.SelectedValue.ToString());
        }
        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindFinyr(cmbCompany.SelectedValue.ToString(), cmbLocation.SelectedValue.ToString());
        }

        private void btnSelectCompany_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbCompany.Text))
            {
                MessageBox.Show("Please select  Company", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(cmbLocation.Text))
            {
                MessageBox.Show("Please select  Location", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(cmbFinyr.Text))
            {
                MessageBox.Show("Please select year", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            menuStrip1.Visible = true;
            groupBoxCompany.Visible = false;


        }
        public string Login(string uid, string pass)
        {
            try
            {
                string SCHEMA = "IMPROVAR";
                sql = "select pswrd, user_type from " + SCHEMA + ".user_appl where user_id='" + uid + "'";
                var DT = Cn.SQLquery(sql);
                var CS = Cn.GetConnectionString();
                string MWORD = "";
                string PWD = "";
                string userType = "";
                if (DT.Rows.Count > 0)
                {
                    string encPswrd = DT.Rows[0]["pswrd"].ToString();
                    userType = DT.Rows[0]["user_type"].ToString(); ;
                    MWORD = Cn.Decrypt(encPswrd);
                    string user = uid + "56";
                    string sub = MWORD.Substring(user.Length);
                    MWORD = sub;
                    if (MWORD.Trim() != pass)
                    {
                        PWD = "NO";
                    }
                    else
                    {
                        PWD = "YES";
                    }
                }
                OracleConnection con = new OracleConnection(CS);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                var com1 = new OracleCommand(SCHEMA + ".SP_USER_VALID", con);
                com1.CommandType = CommandType.StoredProcedure;
                com1.Parameters.Add("USR", uid);
                com1.Parameters.Add("PWD", PWD);
                com1.Parameters.Add("ACS", Session.ModuleCode);
                com1.Parameters.Add("ATM", "0");
                com1.Parameters.Add("USR_IP", "");
                com1.Parameters.Add("USR_SIP", "");
                OracleParameter pm = new OracleParameter("C1", OracleDbType.Int32);
                pm.Direction = ParameterDirection.Output;
                com1.Parameters.Add(pm);
                OracleParameter pm1 = new OracleParameter("USER_NAME", OracleDbType.Varchar2);
                pm1.Direction = ParameterDirection.Output;
                pm1.Size = 100;
                com1.Parameters.Add(pm1);
                OracleParameter pm2 = new OracleParameter("C3", OracleDbType.Varchar2);
                pm2.Size = 100;
                pm2.Direction = ParameterDirection.Output;
                com1.Parameters.Add(pm2);
                OracleParameter pm3 = new OracleParameter("CR", OracleDbType.Varchar2);
                pm3.Direction = ParameterDirection.Output;
                pm3.Size = 100;
                com1.Parameters.Add(pm3);
                OracleParameter pm4 = new OracleParameter("TIME_OUT", OracleDbType.Int32);
                pm4.Direction = ParameterDirection.Output;
                com1.Parameters.Add(pm4);
                OracleParameter pm5 = new OracleParameter("REMOT_CONFIG", OracleDbType.Varchar2);
                pm5.Direction = ParameterDirection.Output;
                pm5.Size = 1;
                com1.Parameters.Add(pm5);
                OracleParameter pm6 = new OracleParameter("REM1", OracleDbType.Varchar2);
                pm6.Direction = ParameterDirection.Output;
                pm6.Size = 500;
                com1.Parameters.Add(pm6);
                com1.ExecuteNonQuery();
                con.Close();
                string outp = com1.Parameters["C1"].Value.ToString();
                string msg = com1.Parameters["CR"].Value.ToString();
                string USER_NAME = com1.Parameters["USER_NAME"].Value.ToString();
                string[] session_no = com1.Parameters["CR"].Value.ToString().Split(',');
                string dt = com1.Parameters["C3"].Value.ToString();
                if (outp == "0")
                {

                    Session.UserName = USER_NAME;
                    Session.UserId = uid;
                    Session.UserType = userType;
                }
                else
                {
                    return msg;
                }
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void FrmLogin_SizeChanged(object sender, EventArgs e)
        {
            // this.groupBoxLogin.Left = this.Width / 3;
            this.GroupBoxLogin.Location = new Point(
            this.ClientSize.Width / 2 - this.GroupBoxLogin.Size.Width / 2,
            this.ClientSize.Height / 2 - this.GroupBoxLogin.Size.Height / 2);
            this.GroupBoxLogin.Anchor = AnchorStyles.None;

            this.groupBoxCompany.Location = new Point(
            this.ClientSize.Width / 2 - this.groupBoxCompany.Size.Width / 2,
            this.ClientSize.Height / 2 - this.groupBoxCompany.Size.Height / 2);
            this.groupBoxCompany.Anchor = AnchorStyles.None;
        }

        private void cashMemoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCashMemo frm = new FrmCashMemo();
            frm.Show();
        }
    }
}
