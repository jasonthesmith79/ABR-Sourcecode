Imports System.DirectoryServices
Imports System.Windows.Forms

Public Class BatchScan
    Public vlist As Boolean = False
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Directory.Exists(TextBox1.Text) Then
            Main.AddtoLog(" = Scanning " & TextBox1.Text & " =")
            Main.AddtoLog("")
            Main.fbdOpen.SelectedPath = TextBox1.Text
            Call Main.cmdBatch()
        Else
            MessageBox.Show("Path does not exist!!")
        End If
        My.Settings.LastDrawer = TextBox1.Text
        My.Settings.Save()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub BatchScan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim intADF As Integer = 0
        Dim intADZ As Integer = 0
        Dim intDMS As Integer = 0
        Dim intABB As Integer = 0
        Dim intzip As Integer = 0
        vlist = False
        TextBox1.Text = My.Settings.LastDrawer
        If Directory.Exists(TextBox1.Text) = False Then
            TextBox1.ForeColor = Color.Red
            MessageBox.Show("Path not found [" & TextBox1.Text & "]")
            Exit Sub
        Else
            TextBox1.ForeColor = Color.Black
        End If
        Dim dirs() As String
        Dim files() As String
        dirs = Directory.GetDirectories(TextBox1.Text)
        files = Directory.GetFiles(TextBox1.Text)
        Dim x As Integer
        For x = 0 To dirs.Length - 1
            ListBox1.Items.Add(dirs(x))
        Next
        For x = 0 To files.Length - 1
            ListBox2.Items.Add(Path.GetFileName(files(x)))
            If Path.GetExtension(files(x)) = ".adf" Then intADF += 1
            If Path.GetExtension(files(x)) = ".adz" Then intADZ += 1
            If Path.GetExtension(files(x)) = ".dms" Then intDMS += 1
            If Path.GetExtension(files(x)) = ".abb" Then intABB += 1
            If Path.GetExtension(files(x)) = ".zip" Then intZIP += 1
        Next
        txtADF.Text = intADF
        txtADZ.Text = intADZ
        txtABB.Text = intABB
        txtDMS.Text = intDMS
        txtZIP.Text = intzip
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles cmdParent.Click
        Dim parent As String = Path.GetDirectoryName(TextBox1.Text)
        If parent = Nothing Then
            cmdVolumes.PerformClick()
            Exit Sub
        End If
        TextBox1.Text = parent
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        txtADF.Text = 0
        txtADZ.Text = 0
        txtABB.Text = 0
        txtDMS.Text = 0
        txtZIP.Text = 0
        Dim dirs() As String
        Dim files() As String
        If Directory.Exists(TextBox1.Text) = False Then
            cmdVolumes.PerformClick()
            Exit Sub
        End If
        dirs = Directory.GetDirectories(TextBox1.Text)
        files = Directory.GetFiles(TextBox1.Text)
        Dim x As Integer
        For x = 0 To dirs.Length - 1
            ListBox1.Items.Add(dirs(x))
        Next
        For x = 0 To files.Length - 1
            ListBox2.Items.Add(Path.GetFileName(files(x)))
        Next

    End Sub

    Private Sub dgvdirs_CellContentClick() Handles ListBox1.SelectedIndexChanged
        Dim intADF As Integer = 0
        Dim intADZ As Integer = 0
        Dim intDMS As Integer = 0
        Dim intABB As Integer = 0
        Dim intzip As Integer = 0
        If ListBox1.SelectedItem.ToString.Contains("No drawers in") Then Exit Sub
        TextBox1.Text = ListBox1.SelectedItem.ToString
        If vlist = True Then
            TextBox1.Text = TextBox1.Text.Substring(0, 3)
        End If
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        Dim dirs() As String
        Dim files() As String
        Try
            dirs = Directory.GetDirectories(TextBox1.Text)
            files = Directory.GetFiles(TextBox1.Text)
        Catch
            MessageBox.Show("Error reading drive " & TextBox1.Text & "!!")
            Exit Sub
        End Try
        Dim x As Integer
        If dirs.Length = 0 Then
            ListBox1.Items.Add("No drawers in " & TextBox1.Text)
        Else
            For x = 0 To dirs.Length - 1
                ListBox1.Items.Add(dirs(x))
            Next
        End If
        For x = 0 To files.Length - 1
            ListBox2.Items.Add(Path.GetFileName(files(x)))
            If Path.GetExtension(files(x)) = ".adf" Then intADF += 1
            If Path.GetExtension(files(x)) = ".adz" Then intADZ += 1
            If Path.GetExtension(files(x)) = ".dms" Then intDMS += 1
            If Path.GetExtension(files(x)) = ".abb" Then intABB += 1
            If Path.GetExtension(files(x)) = ".zip" Then intzip += 1
        Next
        txtADF.Text = intADF
        txtADZ.Text = intADZ
        txtABB.Text = intABB
        txtDMS.Text = intDMS
        txtZIP.Text = intzip
        vlist = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles cmdVolumes.Click
        ListBox1.Items.Clear()
        TextBox1.ForeColor = Color.Black
        Dim drives() As DriveInfo = DriveInfo.GetDrives()
        Dim x As Integer
        For x = 0 To drives.Length - 1
            Try
                ListBox1.Items.Add(drives(x).Name & " " & drives(x).VolumeLabel)
            Catch
            End Try
        Next
        vlist = True
    End Sub

End Class
