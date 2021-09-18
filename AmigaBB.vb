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
    Public Property SearchString As String

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
    Public Function RecogChar() As String()
        Dim items() As String = Recog.Split(",")
        Return items
    End Function
    Public Function BootColour(ByVal abb() As String, ByVal catcol() As String) As String
        Dim returncolor As String = "Black"
        For y = 0 To abb.Length - 1
            If BClass = abb(y) Then
                returncolor = catcol(y)
                Exit For
            Else
                returncolor = "Black"
                ColorName = "Black"
            End If
        Next
        Return returncolor
    End Function
    Public Function BootClass(ByVal abb() As String, ByVal catn() As String) As String
        Dim x As Integer
        Dim retclass As String = ""
        For x = 0 To abb.Length - 1
            If BClass = abb(x) Then
                retclass = catn(x)
                Return retclass
            Else
                retclass = ""
            End If
        Next
        Return retclass
    End Function
End Class
