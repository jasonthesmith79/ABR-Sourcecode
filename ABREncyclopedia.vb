Public Class ABREncyclopedia
    Public Property Name As String
    Public Property ID As String
    Public Property Note As String
    Public Property BootBlocks As String

    Public Sub New(ByVal PName As String, ByVal PID As String, ByVal PNote As String)
        Name = PName
        ID = PID
        Note = PNote
    End Sub
End Class
