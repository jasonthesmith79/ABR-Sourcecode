Imports System.IO

Public Class EditBrainfileEntry
    Public selindex As Integer
    Public bnode As Xml.XmlNode
    'Private Sub EditBrainfileEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '    Dim atype As String
    '    selindex = Main.BFind(Main.BrainTree.SelectedNode.Text)
    '    CheckBox1.Checked = False
    '    CheckBox2.Checked = False
    '    lblNum.Text &= selindex
    '    atype = Main.Isbtype(selindex)
    '    grpEBrain.Visible = True
    '    txtEName.Text = Main.StrBootblocks(selindex)
    '    Main.Oldpicname = txtEName.Text
    '    Dim x As Integer
    '    For x = 0 To Main.CatList.Length - 1
    '        ComboBox1.Items.Add(Main.CatList(x))
    '    Next
    '    For x = 0 To Main.CatList.Length - 1
    '        If atype = Main.CatAb(x) Then
    '            ComboBox1.SelectedText = Main.CatList(x)
    '        End If
    '    Next
    '    If Main.Boot(selindex) = "Y" Then CheckBox1.Checked = True
    '    If Main.DataNeeded(selindex) = "Y" Then CheckBox2.Checked = True
    '    'lbKS.SelectedText = Main.KSVer(selindex)
    '    txtNote.Text = Main.BNote(selindex)
    'End Sub

    Private Sub ECancel_Click(sender As Object, e As EventArgs) Handles cmdECancel.Click
        Me.Close()
    End Sub
End Class