Public Class AmigaBB
    Public Property Name As String
    Public Property BClass As String
    Public Property CRC As String
    Public Property Recog As String
    Public Property DataNeeded As Boolean
    Public Property KS As String
    Public Property Bootable As Boolean
    Public Property Note As String
    Public Property ColorName As String

    Public Sub New(ByVal BN As String, ByVal BootClass As String, ByVal BR As String, ByVal DN As Boolean, ByVal BKS As String, ByVal Boot As Boolean, ByVal BCRC As String)
        Name = BN
        Recog = BR
        DataNeeded = DN
        KS = BKS
        Bootable = Boot
        CRC = BCRC
        BClass = BootClass
    End Sub
    Public Sub New(ByVal BN As String, ByVal BootClass As String, ByVal BR As String, ByVal DN As Boolean, ByVal BKS As String, ByVal Boot As Boolean, ByVal BCRC As String, ByVal BNote As String)
        Name = BN
        Recog = BR
        DataNeeded = DN
        KS = BKS
        Bootable = Boot
        CRC = BCRC
        Note = BNote
        BClass = BootClass
    End Sub
    Public Function BootColour() As Color
        Dim returncolor As Color = Color.Black
        For y = 0 To Main.CatList.Length - 2
            If BClass = Main.CatList(y).Abb Then
                returncolor = Main.CatList(y).Color
                ColorName = Main.CatList(y).ColorName
                Exit For
            Else
                returncolor = Color.Black
                ColorName = "Black"
            End If
        Next
        Return returncolor
    End Function
    Public Function BootClass() As String
        Dim x As Integer
        Dim retclass As String
        For x = 0 To Main.CatList.Length - 1
            If BClass = Main.CatList(x).Abb Then
                retclass = Main.CatList(x).Name
                Return retclass
            Else
                retclass = ""
            End If
        Next
        Return retclass
    End Function
End Class
