Public Class BootCategory
    Public Property Name As String
    Public Property Abb As String
    Public Property ColorName As String
    Public Property Color As Color

    Public Sub New(ByVal Catname As String, ByVal CatAbb As String, ByVal CName As String, ByVal CatCol As Color)
        Name = Catname
        Abb = CatAbb
        ColorName = CName
        Color = CatCol
    End Sub
    Public Function CatNum(ByVal abb)

    End Function
End Class
