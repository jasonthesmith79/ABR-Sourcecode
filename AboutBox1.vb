Public NotInheritable Class AboutBox1
    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.Close()
    End Sub

    Private Sub AboutBox1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmdOK.Focus()
        Me.AcceptButton = cmdOK
    End Sub

    Private Sub LinkLabel5_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked
        Dim url As String = "http://aminet.net/package/dev/asm/ira"
        Process.Start(url)
    End Sub
End Class
