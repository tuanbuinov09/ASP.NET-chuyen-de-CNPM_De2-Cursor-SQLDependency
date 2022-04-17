<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        *{
            font-family: Roboto;
        }
        td {
            margin:5px;
            padding:5px;
            font-size:12px;
            text-align:left;
            vertical-align:top;
        }

        .wrapper{
            width: 400px;
            padding: 20px;
            border: 1px solid #333;
            border-radius: 4px;
        }

        .input-wrapper{
            margin-bottom: 13px;
            display: flex;
            flex-direction: row;
            align-items:center;
        }

        .label{
            width: 80px;
            font-weight:600;
            font-size: 13px;
        }

        .input{
            outline:none;
            padding: 4px;
            flex:1;
             font-size: 13px;
        }
    </style>
</head>
<body>
    <form id="formDatLenh" runat="server">
    
    <div>
        <h1>Bảng Giá Trực Tuyến</h1>
        <br />
        <asp:GridView ID="gridViewBangGiaTT" runat="server" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="Both">
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" Font-Size="Small"/>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
        <br />
      <div class="wrapper">
            <div class="input-wrapper">
                <asp:Label runat="server" class="label" Text="Mã CP:"/>
                <asp:TextBox runat="server" class="input" ID="textBoxMaCP"></asp:TextBox>
            </div>
            <div class="input-wrapper">
                <asp:Label runat="server" class="label" Text="Loại lệnh:"/>
                <asp:DropDownList runat="server" class="input" ID="comboBoxLoaiLenh"></asp:DropDownList>
            </div>
            <div class="input-wrapper">
                <asp:Label runat="server" class="label" Text="Mua/Bán:"/>
               <asp:DropDownList runat="server" class="input" ID="comboBoxMuaBan" OnSelectedIndexChanged="comboBoxMuaBan_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <%--<div class="input-wrapper">
                <asp:Label runat="server" class="label" Text="Số lượng:"/>
                <asp:EditorZone runat="server" class="input" ID="spinEditSoLuong"></asp:EditorZone>
            </div>--%>
          <asp:Button
            ID="btnSubmit"
            Text="Mua"
            Runat="server" OnClick="btnSubmit_Click" />
        </div>

        
    </div>

      
    </form>
</body>
</html>