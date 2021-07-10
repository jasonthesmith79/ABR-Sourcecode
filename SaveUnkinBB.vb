
Public Class SaveUnkinBB

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If TextBox1.Text = String.Empty Then

        Else
            Try
                System.IO.File.WriteAllBytes(Main.txtSBoots.Text & "\" & TextBox1.Text, Main.Buffer)
                MessageBox.Show("Bootblock saved in " & Main.txtSBoots.Text & "\" & TextBox1.Text)
                Main.ReadBBlib(My.Settings.BBlocks)
            Catch ex As Exception
                MessageBox.Show("Cant write file!! " & Main.txtSBoots.Text & "\" & TextBox1.Text)
            End Try
        End If
        Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
