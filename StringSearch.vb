

Public Class StringSearch
    Private Sub Dialog3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim x As Integer
        chkBoot.Checked = True
        For x = 0 To Main.CatList.Length - 1
            ComboBox1.Items.Add(Main.CatList(x))
        Next
        Me.Location = New Point(Main.Location.X + 650, Main.Location.Y + 100)
        txtString.Text = Main.rtBBDisplay.SelectedText
        ComboBox2.SelectedText = "1.3"
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        txtBBName.Text = cbxKnown.Text
    End Sub
End Class
