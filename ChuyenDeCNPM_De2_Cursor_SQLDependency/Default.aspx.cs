using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class _Default : Page
{

    private const string TABLE_NAME = "BANG_GIA_TRUC_TUYEN";
    private SqlConnection _connection = null;
    private SqlCommand _command = null;

    private int _vitriRow;
    private int _vitriColumn;

    String loaiGiaoDich = "M";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            comboBoxMuaBan.Items.Add(new ListItem("Mua", "M"));
            comboBoxMuaBan.Items.Add(new ListItem("Bán", "B"));
            comboBoxMuaBan.SelectedIndex = 0;
            comboBoxMuaBan.SelectedValue = "M";

            comboBoxLoaiLenh.Items.Add(new ListItem("Lệnh khớp liên tục LO", "LO"));
            comboBoxLoaiLenh.SelectedIndex = 0;
            comboBoxLoaiLenh.SelectedValue = "LO";


            MessageBox.Show("Start", this, this.GetType());
            //nếu page render lần đầu hoặc mới reload
            if (CanRequestNotifications() == true)
                StartSQLDependency();
            else
                MessageBox.Show("Bạn chưa kích hoạt dịch vụ Broker", this, this.GetType());
            RefreshBangGia();
        }
        
    }

    private string GetSQL()
    {
        return " SELECT MACP AS[Mã CP],DM_GIA3 AS[Giá Mua 3], DM_SL3 AS[SL Mua 3], DM_GIA2 AS[Giá Mua 2], DM_SL2 AS[SL Mua 2], DM_GIA1 AS[Giá Mua 1], DM_SL1 AS[SL Mua 1], KL_GIA AS[Giá Khớp], "
                + " KL_SL AS[SL Khớp], DB_GIA1 AS[Giá Bán 1], DB_SL1 AS[SL Bán 1], DB_GIA2 AS[Giá Bán 2], DB_SL2 AS[SL Bán 2], DB_GIA3 AS[Giá Bán 3], DB_SL3 AS[SL Bán 3] " +
                "FROM dbo.BANG_GIA_TRUC_TUYEN ";
    }
    //private bool checkInput()
    //{
    //    if (textEditMaCP.Text.Trim().Equals(""))
    //    {
    //        MessageBox.Show("Mã cổ phiếu không được để trống!", "", MessageBoxButtons.OK);
    //        textEditMaCP.Focus();
    //        return false;
    //    }

    //    if (comboBoxLoaiLenh.SelectedIndex == -1)
    //    {
    //        MessageBox.Show("Bạn chưa điền số lượng cổ phiếu!", "", MessageBoxButtons.OK);
    //        comboBoxLoaiLenh.Focus();
    //        return false;
    //    }
    //    if (spinEditGia.Value == 0)
    //    {
    //        MessageBox.Show("Bạn chưa điền giá!", "", MessageBoxButtons.OK);
    //        spinEditGia.Focus();
    //        return false;
    //    }

    //    return true;

    //}
    private void RefreshBangGia()
    {

        // Make sure the command object does not already have
        // a notification object associated with it.
        _command.Notification = null;

        if (_command == null)
        {
            _command = new SqlCommand(GetSQL(), _connection);
            _command.CommandType = CommandType.Text;
            _command.CommandTimeout = 600;

        }

        // Create and bind the SqlDependency object
        // to the command object.

        SqlDependency dependency = new SqlDependency(_command);
        dependency.OnChange += dependency_OnChange;

        SqlDataAdapter da = new SqlDataAdapter(GetSQL(), GetConnectionString());
        DataSet ds = new DataSet();
        da.Fill(ds, TABLE_NAME);
        gridViewBangGiaTT.DataSource = ds.Tables[TABLE_NAME];
        gridViewBangGiaTT.DataBind();

        //DataTable dt = new DataTable();
        //dt.Load(_command.ExecuteReader());
        //gridViewBangGiaTT.DataSource = dt;

        // giữ vị trí cursor
        //try
        //{
        //    dataGridViewBangGiaTT.ClearSelection();
        //    dataGridViewBangGiaTT.Rows[_vitriRow].Cells[_vitriColumn].Selected = true;
        //}
        //catch (Exception ex)
        //{
        //    // neu co loi thi reset lai
        //    this.dataGridViewBangGiaTT.ClearSelection();
        //}

    }
    private bool CanRequestNotifications()
    {
        // In order to use the callback feature of the
        // SqlDependency, the application must have
        // the SqlClientPermission permission.
        try
        {
            SqlClientPermission perm = new SqlClientPermission(PermissionState.Unrestricted);
            perm.Demand();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    private void StartSQLDependency()
    {
        // Remove any existing dependency connection, then create a new one.
        SqlDependency.Stop(GetConnectionString());
        try
        {
            SqlDependency.Start(GetConnectionString());
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi khởi động sql dependency", this, this.GetType());
            return;
        }
        if (_connection == null)
        {
            _connection = new SqlConnection(GetConnectionString());
            _connection.Open();
        }

        if (_command == null)
        {
            _command = new SqlCommand(GetSQL(), _connection);
            _command.CommandType = CommandType.Text;
            _command.CommandTimeout = 600;
        }

        RefreshBangGia();
    }
    private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
    {
        // This event will occur on a thread pool thread.
        // It is illegal to update the UI from a worker thread
        // The following code checks to see if it is safe update the UI.
        ISynchronizeInvoke i = (ISynchronizeInvoke)this;

        // If InvokeRequired returns True, the code is executing on a worker thread.
        if (i.InvokeRequired)
        {
            // Create a delegate to perform the thread switch
            OnChangeEventHandler tempDelegate = new OnChangeEventHandler(dependency_OnChange);

            object[] args = new[] { sender, e };

            // Marshal the data from the worker thread
            // to the UI thread.
            i.BeginInvoke(tempDelegate, args);

            return;
        }
        // Remove the handler since it's only good
        // for a single notification
        SqlDependency dependency = (SqlDependency)sender;
        dependency.OnChange -= dependency_OnChange;
        // Reload the dataset that's bound to the grid.

        RefreshBangGia();

    }
    private String GetConnectionString()
    {
        return @"Data Source=TUANBUI-NOV09;Initial Catalog=CHUNGKHOAN;User ID=sa;Password=123";
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        //if (checkInput())
        if (true)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var command = new SqlCommand("SP_KHOPLENH_LO", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                DateTime time = DateTime.Now;
                string timeString = time.ToString("yyyy-MM-dd HH:mm:ss.mmm");

                command.Parameters.Add(new SqlParameter("@maCP", textBoxMaCP.Text.Trim()));
                command.Parameters.Add(new SqlParameter("@ngay", timeString));
                command.Parameters.Add(new SqlParameter("@loaiGD", loaiGiaoDich));
                command.Parameters.Add(new SqlParameter("@soLuongMB", btnSubmit));
                command.Parameters.Add(new SqlParameter("@giaDatMB", btnSubmit));

                conn.Open();
                command.ExecuteNonQuery();
            }

        }
        else
        {
            // lỗi return;
            return;
        }
    }

    protected void comboBoxMuaBan_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBoxMuaBan.SelectedIndex == 0)
        {
            btnSubmit.Text = "Mua";
            loaiGiaoDich = "M";
            return;
        }
        btnSubmit.Text = "Bán";
        loaiGiaoDich = "B";
    }
}