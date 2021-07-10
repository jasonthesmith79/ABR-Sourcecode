Imports System.Windows.Forms
Public Class EditSearchBB
    '    Dim sclass As String
    '    Dim loc As Integer = Main.SFind(Main.trvSearch.SelectedNode.Text)
    '    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
    '        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    '        Me.Close()
    '    End Sub

    'Private Sub EditSearchBB_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '    Dim x As Integer
    '    TextBox1.Text = Main.Searchname(loc)
    '    TextBox2.Text = Main.Searchtext(loc)
    '    lblWarn.Visible = False
    '    Main.Origsearch = TextBox2.Text
    '    Dim atype As String = Main.Searchclass(loc)
    '    For x = 0 To Main.CatList.Length - 1
    '        If Main.CatAb(x) = atype Then ComboBox1.SelectedText = Main.CatList(x)
    '    Next
    'End Sub

    'Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    '    Main.Searchname(loc) = TextBox1.Text
    '    Main.Searchtext(loc) = TextBox2.Text
    '    Main.Searchclass(loc) = sclass
    '    Main.SaveSearchBB()
    '    Main.RefreshSearchBB()
    '    Me.Close()
    'End Sub

    'Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
    '        If TextBox2.Text = Main.Origsearch Then
    '            lblWarn.Visible = False
    '        Else
    '            lblWarn.Visible = True
    '        End If
    '    End Sub

    'Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '    Dim result As DialogResult
    '    result = MessageBox.Show("Are you sure you wish to delete " & Main.Searchname(Main.EditNode).ToString & "?")
    '    If result = DialogResult.OK Then
    '        Main.BBDelete = True
    '        Main.Searchclass(Main.EditNode) = ""
    '        Main.Searchname(Main.EditNode) = ""
    '        Main.Searchtext(Main.EditNode) = ""
    '        Main.SaveSearchBB()
    '    Else
    '    End If
    '    Me.Close()
    'End Sub
End Class
