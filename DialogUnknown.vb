Public Class DialogUnknown

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Main.rtBBDisplay.Visible = True
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgUnknown_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim chars() As Char
        chars = Main.ReadCP.GetChars(Main.Buffer)
        lstASClist.Items.Clear()
        lstChar.Items.Clear()
        For x = 0 To Main.BootLen
            lstASClist.Items.Add(x & ", " & Main.ASCIIInt(x))
            lstChar.Items.Add(chars(x))
        Next
    End Sub
End Class