// Giao Dien Copy Grid -- .xaml

<Grid>
        <Label Content="Mã Sản Phẩm" HorizontalAlignment="Left" Margin="100,61,0,0" VerticalAlignment="Top"/>
        <TextBox x:Name="txtMaSP" HorizontalAlignment="Left" Margin="213,65,0,0" TextWrapping="Wrap" VerticalAlignment="Top" Width="152"/>
        <Label Content="Tên Sản Phẩm" HorizontalAlignment="Left" Margin="100,96,0,0" VerticalAlignment="Top"/>
        <TextBox x:Name="txtTenSP" HorizontalAlignment="Left" Margin="213,100,0,0" TextWrapping="Wrap" VerticalAlignment="Top" Width="234"/>
        <Label Content="Loại Sản Phẩm" HorizontalAlignment="Left" Margin="100,131,0,0" VerticalAlignment="Top"/>
        <ComboBox x:Name="cboLoai" HorizontalAlignment="Left" Margin="213,133,0,0" VerticalAlignment="Top" Width="172"/>
        <Label Content="Đơn Giá" HorizontalAlignment="Left" Margin="100,174,0,0" VerticalAlignment="Top"/>
        <TextBox x:Name="txtDonGia" HorizontalAlignment="Left" Margin="213,178,0,0" TextWrapping="Wrap" VerticalAlignment="Top" Width="152"/>
        <Label Content="Số Lượng" HorizontalAlignment="Left" Margin="100,220,0,0" VerticalAlignment="Top"/>
        <TextBox x:Name="txtSoLuong" HorizontalAlignment="Left" Margin="213,224,0,0" TextWrapping="Wrap" VerticalAlignment="Top" Width="152"/>
        <DataGrid x:Name="dgvSanPham" Margin="20,267,20,100" AutoGenerateColumns="False" CanUserAddRows="False" AlternatingRowBackground="AntiqueWhite" SelectedCellsChanged="dgvSanPham_SelectedCellsChanged">
            <DataGrid.Columns>
                <DataGridTextColumn Header="Mã Sản Phẩm" Binding="{Binding MaSp}"></DataGridTextColumn>
                <DataGridTextColumn Header="Tên Sản Phẩm" Binding="{Binding TenSp}"></DataGridTextColumn>
                <DataGridTextColumn Header="Loại Sản Phẩm" Binding="{Binding MaLoai}"></DataGridTextColumn>
                <DataGridTextColumn Header="Đơn Giá" Binding="{Binding DonGia}"></DataGridTextColumn>
                <DataGridTextColumn Header="Số Lượng" Binding="{Binding SoLuong}"></DataGridTextColumn>
                <DataGridTextColumn Header="Thành Tiền" Binding="{Binding ThanhTien}" Width="*"></DataGridTextColumn>
            </DataGrid.Columns>
        </DataGrid>
        <Button x:Name="btnThem" Content="Thêm" HorizontalAlignment="Left" Margin="128,456,0,0" VerticalAlignment="Top" Width="70" Height="28" Click="btnThem_Click"/>
        <Button x:Name="btnSua" Content="Sửa" HorizontalAlignment="Left" Margin="244,456,0,0" VerticalAlignment="Top" Width="70" Height="28" Click="btnSua_Click"/>
        <Button x:Name="btnXóa" Content="Xóa" HorizontalAlignment="Left" Margin="360,456,0,0" VerticalAlignment="Top" Width="70" Height="28" Click="btnXóa_Click"/>
        <Button x:Name="btnTimKiem" Content="Tìm Kiếm" HorizontalAlignment="Left" Margin="482,456,0,0" VerticalAlignment="Top" Width="70" Height="28"/>
        <Button x:Name="btnThongKe" Content="Thống Kê" HorizontalAlignment="Left" Margin="594,456,0,0" VerticalAlignment="Top" Width="70" Height="28" Click="btnThongKe_Click"/>

    </Grid>

//End-------------GD

//Hien Thi Du Lieu -- Window_Loaded

QLBanHang3Context db = new QLBanHang3Context();

        private void HienThiDuLieu()
        {
            var query = from sp in db.SanPhams
                        orderby sp.DonGia
                        select new
                        {
                            sp.MaSp,
                            sp.TenSp,
                            sp.MaLoaiSp,
                            sp.DonGia,
                            sp.SoLuong,
                            ThanhTien = sp.SoLuong * sp.DonGia
                        };
            dgvSanPham.ItemsSource = query.ToList();
        }
        private void HienThiCB()
        {
            var query = from lsp in db.LoaiSanPhams
                        select lsp;
            cboLoai.ItemsSource = query.ToList();
            cboLoai.DisplayMemberPath = "TenLoai";
            cboLoai.SelectedValuePath = "MaLoai";
            cboLoai.SelectedIndex = 0;
        }

//End------------ Window_Loaded

//Check-Dl

private bool CheckDL()
        {
            string tb = "";
            if(txtMaSP.Text =="" || txtTenSP.Text =="" ||txtDonGia.Text =="" || txtSoLuong.Text == "")
            {
                tb += "\nBạn cần nhập đầy đủ dữ liệu";

            }
            if (!Regex.IsMatch(txtDonGia.Text, @"\d+"))
            {
                tb += "\nĐơn giá nhập phải là số!";
            }
            else
            {
                double dg = double.Parse(txtDonGia.Text);
                if (dg < 0)
                {
                    tb += "\nĐơn giá phải là số dương";
                }
            }
            if (!Regex.IsMatch(txtSoLuong.Text, @"\d+"))
            {
                tb += "\nSố lượng nhập phải là số!";
            }
            else
            {
                int sl = int.Parse(txtSoLuong.Text);
                if (sl < 0)
                {
                    tb += "\nSố lượng phải là số dương";
                }
            }
            if (tb != "")
            {
                MessageBox.Show(tb, "Thong Bao", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

//End -  CheckDl


//Them ---SP

try
            {

                if (CheckDL())
                {
                    var query = db.SanPhams.SingleOrDefault(t => t.MaSp.Equals(txtMaSP.Text));
                    if (query != null)
                    {
                        MessageBox.Show("Mã sản phẩm này đã tồn tại", "Thong Bao");
                        HienThiDuLieu();
                    }
                    else
                    {
                        SanPham spMoi = new SanPham();
                        spMoi.MaSp = int.Parse(txtMaSP.Text);
                        spMoi.TenSp = txtTenSP.Text;
                        spMoi.DonGia = double.Parse(txtDonGia.Text);
                        spMoi.SoLuong = int.Parse(txtSoLuong.Text);
                        LoaiSanPham selectedItem = (LoaiSanPham)cboLoai.SelectedItem;
                        spMoi.MaLoaiSp = selectedItem.MaLoaiSp;
                        db.SanPhams.Add(spMoi);
                        db.SaveChanges();
                        MessageBox.Show("Thêm thành công", "Thong Bao");
                        HienThiDuLieu();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi thêm: " + ex.Message, "Thong Bao");
            }

//End -- ThemSp

//Sua -- Sp

try
            {
                var spSua = db.SanPhams.SingleOrDefault(t => t.MaSp.Equals(txtMaSP.Text));
                if (spSua != null)
                {
                    if (CheckDL())
                    {
                        spSua.TenSp = txtTenSP.Text;
                        LoaiSanPham itemSelected = (LoaiSanPham)cboLoai.SelectedItem;
                        spSua.MaLoai = itemSelected.Maloai;
                        spSua.DonGia = double.Parse(txtDonGia.Text);
                        spSua.SoLuong = int.Parse(txtSoLuong.Text);
                        db.SaveChanges();
                        MessageBox.Show("Sửa thành công!", "Thong Bao");
                        HienThiDuLieu();
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm cần sửa!!!", "Thong Bao");
                }
            }
            catch(Exception ex1)
            {
                MessageBox.Show("Có lỗi khi sửa: " + ex1.Message, "Thong Bao");
            }

//End ---- Sua Sp

// xoa      


var spXoa = db.SanPhams.SingleOrDefault(t => t.MaSp.Equals(int.Parse(txtMaSP.Text)));
            if(spXoa != null)
            {
                MessageBoxResult rs = MessageBox.Show("ban co chac chan muon xoa","Thong Bao", MessageBoxButton.YesNo);
                if(rs == MessageBoxResult.Yes)
                {
                    db.SanPhams.Remove(spXoa);
                    db.SaveChanges();
                    HienThiDuLieu();
                }
            }
            else
            {
                MessageBox.Show("khong co anh nao de xoa");
            }

//End ---- xoa Sp






