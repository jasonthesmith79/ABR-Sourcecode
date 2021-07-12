Imports System.ComponentModel
Imports System.Text
Imports System.Threading
Imports System.Windows
Imports System.Xml
Imports ICSharpCode.SharpZipLib.Core
Imports ICSharpCode.SharpZipLib.GZip
Imports ICSharpCode.SharpZipLib.Zip

Public Class Main

    ' Global variables used in more than one place in the program
    Public objentry As ZipEntry
    Public bootpos As Integer = 0
    Public FoundbySearch As Boolean = False
    Public zipperfile As String = ""
    Public OpenBootFile As String
    Public ASMLoc As Integer
    Public filtered As Boolean
    Public ASMFile() As String
    Public filtercount As Integer = 0
    Public Recogtoggle As Boolean = False
    Public DMSInfo(11) As String
    Public AddDir As Boolean
    Public drawers() As String
    Public BackCRC() As String
    Public dbox As String
    Public CompVirus As Boolean = False
    Public zfile As String
    Public opadd(1) As String
    Public EQUName(20) As String
    Public EQURef(20) As String
    Public EQUCount As Integer = 0
    Public opnames(1) As String
    Public opdesc(1) As String

    Public CatList(20) As BootCategory
    Public sbfiles() As String
    Public IntMainLoc(2, 2) As Integer
    Public zipper As New FastZip
    Public CList() As Color


    Public newsaved As Boolean = False
    Public oldbrain() As String
    Public logfile As New StringBuilder

    Public OBootNum As Integer
    Public initialPath As String
    Public BBDelete As Boolean = False
    Dim Formloaded As Boolean = False
    Public learnoverride As Boolean
    Public Cancelprog As Boolean = False

    Public Origsearch As String
    Dim BootNum As Integer
    Public IsKnown As Boolean = False
    Dim Iszip As Boolean = False
    Dim Isadf As Boolean = False
    Dim isdms As Boolean = False
    Dim isadz As Boolean = False
    Public BrainXML As XmlDocument
    Public dmsfile As String
    Public adzfile As String
    Public Finput() As String
    Public catselect As Boolean = False
    Public EditNode As Integer
    Public BNum As Integer
    Public BootASCIIIntegers(1024) As Integer
    Dim DefaultH As Integer
    Dim DefaultW As Integer
    Dim Editbuffer() As Byte
    Dim Counter As Integer
    Dim Rchars(1024) As Char
    Public Oldpicname As String
    Dim Redobuffer() As String
    Dim Curboot(1024) As Byte
    Dim Savecount As Integer = 0
    Public Boots As Integer
    Public IntClicked As Integer
    Dim Adfbuffer(901120) As Byte
    Dim Tempdrawer As String
    Dim Batchlist() As String
    Public Foundno As Integer
    Public DisplayType As Integer = 2
    Public Files(20000) As String
    Public Bootno As Integer
    Public BootLen As Integer
    Public IntBytePos(6) As Integer
    Dim Fileclass(500) As String
    Dim Filebname(500) As String
    Dim Filebnum(500) As Integer
    Public filecolor(500) As String
    Public Buffer(1024) As Byte
    Dim Strc As New StringBuilder
    Dim Bootname As String
    Public Bootclass As String
    Public bootcolor As String
    Dim Fbuffer(1024) As Byte
    Dim FoundLoc(500) As Integer
    Dim WithEvents Worker As New BackgroundWorker
    Dim WithEvents SWorker As New BackgroundWorker
    Dim Zipname As String
    Dim BType As Integer
    Public ORMessage As String
    Public Recogline(3000) As String
    Public ORecogline(3000) As String
    Dim Nopic As Boolean
    Dim Ziprealname As String
    Public ReadCP As Encoding
    ReadOnly Win As Encoding = Encoding.GetEncoding(1251)
    ReadOnly ISO As Encoding = Encoding.GetEncoding("iso8859-1")
    ReadOnly utf7 As Encoding = Encoding.UTF7
    ReadOnly utf8 As Encoding = Encoding.UTF8
    ReadOnly Ascii As ASCIIEncoding = New ASCIIEncoding()
    Public StartPath As String = Forms.Application.StartupPath
    Public DefaultBrainPath As String = StartPath & "\brainfile.xml"
    Public DefaultSBrainPath As String = StartPath & "\searchbrain.xml"
    Public Brainpath As String
    Public Fread As Boolean
    Public BBAmt As Integer
    Public searchnodes As Integer
    Public LastDrawer As String = My.Settings.LastDrawer

    Public bblist() As String

    'Class definitions
    Public BBDatabase(2) As AmigaBB
    Public SearchDatabase(2) As AmigaBB
    Public BBE(2) As ABREncyclopedia
    Public catnames() As String
    Public abbnames() As String

    'Display types
    Public Scanner As Integer = 1
    Public DisplayBoot As Integer = 2

    Private Sub ofdBootOpen_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ofdBootOpen.FileOk
        tabBrain.SelectTab(0)
        Isadf = False
        'Setting default open drawer to ADF drawer set on paths tab
        OpenBootFile = ofdBootOpen.FileName

        If File.Exists(OpenBootFile) = True Then
            Dim FileProps As FileInfo = New FileInfo(OpenBootFile)
            'Checking size and extension of the file opened and enabling and disabling appropriate buttons and passing on to 'disptype' sub
            If FileProps.Length = 1024 Then
                TestBootblockInBufferToolStripMenuItem.Enabled = True
            End If
            If InStr(OpenBootFile.ToUpper, ".ADF") Then
                BootDiskToolStripMenuItem.Enabled = True
                SaveADFFileToolStripMenuItem.Enabled = True
                Isadf = True
                Call Disptype(DisplayType, OpenBootFile)
                TestBootblockInBufferToolStripMenuItem.Enabled = True
            Else
                Disptype(DisplayType, OpenBootFile)
            End If
            If FileProps.Length > 2000000 Then
                MessageBox.Show("File is too LARGE to be an Amiga disk")
            End If
        End If
    End Sub
    Public Sub UnzipToFile(ByVal OpenFileName, ByVal outputdrawer)
        Dim strmZipInputStream As ZipInputStream
        Zipname = OpenFileName
        '' Attempt to open the Zip file
        Try
            strmZipInputStream = New ZipInputStream(File.OpenRead(OpenFileName))
        Catch
            AddtoLog("*Cant open file")
            If DisplayType = Scanner Then
                Bootname = "Cant open file"
            Else
                txtBootName.Text = "Invalid file"
            End If
            Exit Sub
        End Try
        Iszip = True
        Dim found As Boolean = False
        objentry = Nothing
        '' Attempt to obtain first ZIP file entry
        Try
            If strmZipInputStream.CanRead = True Then objentry = strmZipInputStream.GetNextEntry
        Catch
            AddtoLog("*Cant open ZIP File")
            If DisplayType = Scanner Then
                Bootname = "Invalid ZIP File"
            Else
                txtBootName.Text = "Invalid ZIP file"
            End If
        End Try
        ' Find .ADF file in ZIP
        Do While IsNothing(objentry) = False
            If InStr(objentry.Name.ToUpper, ".ADF") Then
                Try
                    zipper.ExtractZip(OpenFileName, outputdrawer, "")
                Catch
                    AddtoLog("*Invalid ZIP File")
                    If DisplayType = Scanner Then
                        Bootname = "Invalid ZIP File"
                    Else
                        txtBootName.Text = "Invalid ZIP File"
                    End If
                    Exit Sub
                End Try
                zfile = objentry.Name
                Exit Do
            End If
            strmZipInputStream.CloseEntry()
            Try
                If strmZipInputStream.CanRead = True Then objentry = strmZipInputStream.GetNextEntry
            Catch
            End Try
        Loop
        strmZipInputStream.Close()
        If zfile = "" Then
            AddtoLog("*No ADFs found in zipfile")
            If DisplayType = Scanner Then
                Bootname = "No .ADF files in zipfile"
                Bootclass = "BAD"
            Else
                txtBootName.Text = "No .ADF files in zipfile"
                txtClass.Text = ""
            End If
            Exit Sub
        End If
        OpenBootFile = Tempdrawer & "\" & zfile
        bootpos = txtReadOffset.Text
        If Bootclass = "BAD" Then
            Exit Sub
        End If

        '' Load file into memory
    End Sub
    Public Sub ViewDMSInfo(ByVal dmsfile As String)
        Dim p As New Process
        Dim runner As New ProcessStartInfo
        runner.FileName = My.Settings.XDMS
        runner.UseShellExecute = False
        runner.RedirectStandardOutput = True
        runner.RedirectStandardError = True
        runner.CreateNoWindow = True
        runner.Arguments = "v """ & dmsfile & ""
        If runner.FileName = "" Then
            Exit Sub
        End If
        runner.CreateNoWindow = True
        p.StartInfo = runner
        p.Start()
        p.WaitForExit()
        Dim std_err As StreamReader = p.StandardOutput
        For i = 0 To DMSInfo.Length - 1
            DMSInfo(i) = std_err.ReadLine
        Next
        std_err.Close()
    End Sub
    Public Function UnDMStoFile(ByVal OpenFileName As String, ByVal Destdrawer As String) As String
        Dim up As String
        isdms = True
        dmsfile = OpenFileName
        Dim p As New Process
        Dim runner As New ProcessStartInfo
        runner.FileName = My.Settings.XDMS
        runner.UseShellExecute = False
        runner.RedirectStandardOutput = True
        runner.RedirectStandardError = True
        runner.CreateNoWindow = True
        runner.Arguments = "-d """ & Destdrawer & """ u """ & OpenFileName & ""
        If runner.FileName = "" Then
            MessageBox.Show("Please Set XDMS path To use this feature")
            Exit Function
        End If
        runner.CreateNoWindow = True
        p.StartInfo = runner
        p.Start()
        p.WaitForExit()
        Dim newfile As String = Destdrawer & "\" & Path.GetFileName(OpenFileName).ToLower.Replace(".dms", ".adf")
        OpenBootFile = newfile
        Iszip = True
        Dim std_err As StreamReader = p.StandardError
        up = std_err.ReadToEnd
        Dim FileProps As FileInfo = New FileInfo(OpenBootFile)
        Dim err As Boolean = False
        If up.Contains("Is Not a") Then
            AddtoLog("*Error unpacking - invalid file " & OpenFileName)
            Bootname = "**Invalid DMS File"
            Bootclass = "BAD"
            err = True
        ElseIf up.Contains("encrypted") Then
            AddtoLog("*Error unpacking " & OpenFileName)
            Bootname = "**Error unpacking DMS File - file is encrypted"
            Bootclass = "BAD"
            err = True
        ElseIf up.Contains("Error In file") Then
            AddtoLog("*Error unpacking " & OpenFileName)
            Bootname = "**Error unpacking DMS File - Data CRC Error"
            Bootclass = "BAD"
            err = True
        ElseIf up.Contains("checksum Error after unpacking") Then
            AddtoLog("*Checksum Error unpacking " & OpenFileName)
            Bootname = "**Error unpacking DMS File - File seems ok, checksum Error after unpacking"
            Bootclass = "BAD"
            err = True
        ElseIf up.Contains("Error In file") Then
            AddtoLog("*Error unpacking " & OpenFileName)
            Bootname = "**Error unpacking DMS File - Data CRC Error"
            Bootclass = "BAD"
            err = True
        ElseIf up.ToLower.Contains("crc error") Then
            AddtoLog("*Error unpacking " & OpenFileName)
            Bootname = "**Error unpacking DMS File - CRC Error"
            Bootclass = "BAD"
            err = True
        ElseIf up.Contains("is not a DMS") Then
            AddtoLog("*Error unpacking " & OpenFileName)
            Bootname = "**Error unpacking DMS File - Invalid DMS File"
            Bootclass = "BAD"
            err = True
        End If
        If err = False Then
            If File.Exists(newfile) = False Then
                MessageBox.Show(OpenFileName & ": Unhandled DMS error!!" & up)
                Exit Function
            End If
            If FileProps.Length = 0 Then
                AddtoLog("*Error unpacking " & OpenFileName)
                AddtoLog("File reported unpacking correctly however unpacked ADF Is 0 bytes")
                Bootname = "**Error unpacking DMS File"
                Bootclass = "BAD"
                err = True
            End If
        Else
            If chkSDMS.Checked = True Then
                Exit Function
            Else
                MessageBox.Show(Path.GetFileName(dmsfile) & ":" & up)
                Exit Function
            End If
        End If
        std_err.Close()
        Return up
    End Function
    Public Sub Disptype(ByVal DisplayType As Integer, ByVal OpenFileName As String)
        Dim a As Integer = 0
        Dim zipcont(50) As String
        IsKnown = False
        If OpenFileName = Nothing Then Exit Sub
        If picPreview.Image IsNot Nothing Then picPreview.Image = Nothing
        '' File Name and Type checking 
        ''
        ''
        If chkSkip.Checked = True Then
            Bootname = "Scanning skipped - open to scan file"
            Exit Sub
        End If
        If chkErrorOnly.Checked = False Then AddtoLog("Scanning file " & OpenFileName)
        IsKnown = False
        isdms = False
        Iszip = False
        Bootname = ""
        Bootclass = ""
        isadz = False
        Dim ext As String
        ext = Path.GetExtension(OpenFileName).ToUpper
        OpenBootFile = OpenFileName
        If OpenBootFile = "" Then Exit Sub
        Dim i As Integer = 0
        Dim tryadzaszip As Boolean = False
        'Dim objEntry As ZipArchiveEntry 'OLD .NET 4.5 ZIP CODE
        Dim chars As Char()  'Char array to store bootblock to print to boot display textbox
        chars = New Char(1024) {}
        Dim FileProps As FileInfo = New FileInfo(OpenBootFile)
        'Invalid File types DMS and LHA etc
        If ext = ".LHA" Then
            Bootname = "LHA Archive file - not supported"
            Bootclass = "BAD"
            Exit Sub
        ElseIf ext = ".RAR" Then
            Bootname = "RAR Archive currently not supported"
            Bootclass = "BAD"
            Exit Sub
        ElseIf ext = ".EXE" Then
            Bootname = "Windows / DOS executable file (Not scanned)"
            Bootclass = "BAD"
            Exit Sub
        ElseIf ext = ".TXT" Then
            Bootname = "Text file (Not scanned)"
            Bootclass = "BAD"
            Exit Sub
        ElseIf ext = ".PNG" Then
            Bootname = "PNG Picture file (not scanned)"
            Bootclass = "BAD"
            Exit Sub
        ElseIf ext = ".JPG" Then
            Bootname = "JPEG Picture file (not scanned)"
            Bootclass = "BAD"
            Exit Sub
        End If
        'File size error
        Try
            If FileProps.Length > 1000000 Then
                Bootname = "File exceeds 1mb - invalid file"
                Bootclass = "BAD"
                Exit Sub
            End If
        Catch
        End Try
        If ext = ".DMS" Then
            isdms = True
            If chkDMS.Checked = True Then
                If File.Exists(StartPath & "\xdms.exe") Then
                    UnDMStoFile(OpenFileName, Tempdrawer)
                End If
            Else
                Bootname = "'Scan .DMS Files' off - DMS not scanned"
                Bootclass = "BAD"
            End If
        Else
        End If
        If ext = ".ADZ" Then
            isadz = True
            Dim fslength As Integer
            Isadf = True
            adzfile = OpenFileName
            Dim dataBuffer As Byte() = New Byte(4095) {}
            Try
                Using fs As Stream = New FileStream(OpenFileName, FileMode.Open, FileAccess.Read)
                    Using gzipstream As New GZipInputStream(fs)
                        Dim fnOut As String = Path.Combine(Tempdrawer, Path.GetFileNameWithoutExtension(OpenFileName) & ".adf")
                        Using fsOut As FileStream = File.Create(fnOut)
                            StreamUtils.Copy(gzipstream, fsOut, dataBuffer)
                            fslength = fsOut.Length
                        End Using
                        OpenBootFile = fnOut

                    End Using
                End Using
            Catch
                AddtoLog("*Error unpacking ADZ File " & OpenFileName)
                If DisplayType = Scanner Then
                    Bootname = "Invalid ADZ File"
                Else
                    txtBootName.Text = "Invalid ADZ File"
                End If
                If chkTZIP.Checked = True Then
                    If fslength = 0 Then
                        tryadzaszip = True
                        AddtoLog("Treating ADZ file as a zipped ADF file")
                    Else
                        tryadzaszip = False
                    End If
                End If
            End Try
        End If
        zfile = ""
        If ext = ".ZIP" OrElse tryadzaszip = True Then
            UnzipToFile(OpenFileName, Tempdrawer)
        Else
            Iszip = False
        End If
        bootpos = txtReadOffset.Text
        If Bootclass = "BAD" Then Exit Sub
        If File.Exists(OpenBootFile) Or File.Exists(ofdReader.FileName) Then
            Try
                Using fs As New FileStream(OpenBootFile, FileMode.Open, FileAccess.Read)
                    Dim dinput As New BinaryReader(fs)
                    If bootpos > fs.Length Then
                        MessageBox.Show("End of file reached")
                        Exit Sub
                    End If

                    If InStr(OpenBootFile.ToUpper, ".ADF") Then
                        Isadf = True
                        If DisplayType > 1 Then
                            Adfbuffer = dinput.ReadBytes(fs.Length - 1)
                            BootDiskToolStripMenuItem.Enabled = True
                            SaveADFFileToolStripMenuItem.Enabled = True
                        End If
                    End If
                    If bootpos > 0 Then
                        fs.Position = bootpos
                    Else
                        fs.Position = 0
                    End If
                    Buffer = dinput.ReadBytes(1024)
                    If Iszip = True Then
                        txtCFile.Text = Zipname
                    ElseIf isdms = True Then
                        txtCFile.Text = dmsfile

                    ElseIf isadz = True Then
                        txtCFile.Text = adzfile
                    Else
                        txtCFile.Text = fs.Name
                    End If
                End Using
            Catch
                If DisplayType = Scanner Then
                    Bootname = "Cant Open file!!!"
                Else
                    txtBootName.Text = "Cant Open File!!!!"
                End If
            End Try
        End If
        If Buffer.Length = 0 Then Exit Sub 'check for empty file
        If DisplayType > 1 Then
            txtLen.Text = chars.Length
            Select Case Buffer(3).ToString 'Checking for boot type value at byte 3
                Case 0
                    txtFS.Text = "OldFileSystem"
                Case 1
                    txtFS.Text = "FastFileSystem"
                Case 2
                    txtFS.Text = "OFS-International"
                Case 3
                    txtFS.Text = "FFS International"
                Case 4
                    txtFS.Text = "OFS DC Int"
                Case 5
                    txtFS.Text = "FFS DC Int"
                Case Else
                    txtFS.Text = "Unknown FS Flag"
            End Select
        End If
        If DisplayType = 7 Then
            bootpos = Val(txtReadOffset.Text)
            DisplayType = DisplayBoot
        End If
        Try
            If Iszip = True Or isdms = True Then
                File.Delete(OpenBootFile)
            End If
        Catch
        End Try
        chars = ReadCP.GetChars(Buffer)
        Rchars = chars
        BootLen = chars.Length - 1
        If DisplayType > 1 Then
            If Isadf = True Then
                BootDiskToolStripMenuItem.Enabled = True
                SaveADFFileToolStripMenuItem.Enabled = True
                InstallBootblockToADFToolStripMenuItem.Enabled = True
            Else
                BootDiskToolStripMenuItem.Enabled = False
                SaveADFFileToolStripMenuItem.Enabled = False
                InstallBootblockToADFToolStripMenuItem.Enabled = False
            End If
        End If
        BType = Buffer(3).ToString

        'Storing bootblock into another buffer for use in installing
        Curboot = Buffer
        BootLen = chars.Length - 1
        'Display type for batch scanning - do not display
        If DisplayType = Scanner Then
            Dim x As Integer
            For x = 0 To BootLen
                BootASCIIIntegers(x) = Buffer(x)
            Next
            'Default display type
            ''
        ElseIf DisplayType = DisplayBoot Then

            If isdms = True Then
                txtLoad.Text = Path.GetFileName(dmsfile)
            ElseIf isadz = True Then
                txtLoad.Text = Path.GetFileName(adzfile)
            ElseIf Iszip Then
                txtLoad.Text = Path.GetFileName(zipperfile)
            Else
                txtLoad.Text = Path.GetFileName(OpenBootFile)
            End If
            Dim bootline As String = ""
            Dim x As Integer
            Dim count As Integer = 0
            Dim str As New StringBuilder
            Dim str2 As New StringBuilder
            Dim isDone As Boolean = False
            For x = 0 To BootLen
                BootASCIIIntegers(x) = Buffer(x).ToString
            Next
            For i = 0 To 16
                bootline = ""
                If isDone = True Then Exit For
                For x = 0 To 63
                    If (count + x) > (BootLen) Then
                        bootline &= "."
                    ElseIf Asc(chars(count + x)) = 0 Then
                        bootline &= "."
                    ElseIf Asc(chars(count + x)) = 3 Then
                        bootline &= "."
                    ElseIf Asc(chars(count + x)) = 141 Then
                        bootline &= "?"
                    ElseIf Asc(chars(count + x)) = 10 Then
                        bootline &= "?"
                    ElseIf Asc(chars(count + x)) = 9 Then
                        bootline &= "?"
                    ElseIf Asc(chars(count + x)) = 12 Then
                        bootline &= "?"
                    ElseIf Asc(chars(count + x)) = 11 Then
                        bootline &= "?"
                    ElseIf Asc(chars(count + x)) = 13 Then
                        bootline &= "?"

                    Else
                        bootline &= chars(count + x)
                    End If
                Next
                str.AppendLine(bootline)
                count += 64
            Next

            BBDisplay.Text = str.ToString
            rtBBDisplay.Text = str.ToString
            BBDisplay.SelectionStart = 0
            BBDisplay.SelectionLength = 0
        ElseIf DisplayType = 4 Then
            If isdms = True Then
                txtLoad.Text = Path.GetFileName(dmsfile)
            ElseIf isadz = True Then
                txtLoad.Text = Path.GetFileName(adzfile)
            ElseIf Iszip Then
                txtLoad.Text = Path.GetFileName(zipperfile)
            Else
                txtLoad.Text = Path.GetFileName(OpenBootFile)
            End If
            Dim bootline As String = ""
            Dim x As Integer
            Dim count As Integer = 0
            Dim str As New StringBuilder
            Dim str2 As New StringBuilder
            Dim isDone As Boolean = False
            For x = 0 To BootLen
                BootASCIIIntegers(x) = Buffer(x).ToString

            Next
            For i = 0 To 16
                bootline = ""
                If isDone = True Then Exit For
                For x = 0 To 63
                    If (count + x) > (BootLen) Then
                        bootline &= "."
                    ElseIf Asc(chars(count + x)) > 64 AndAlso Asc(chars(count + x)) < 127 Then
                        bootline &= chars(count + x)
                    Else
                        bootline &= "."
                    End If
                Next
                str.AppendLine(bootline)
                count += 64
            Next
            If IsKnown Then
                cmdGotoBrain.ForeColor = Color.Green

            Else
                cmdGotoBrain.ForeColor = Color.Black
            End If
            cmdGotoBrain.Enabled = IsKnown

            BBDisplay.Text = str.ToString
            rtBBDisplay.Text = str.ToString
            BBDisplay.SelectionStart = 0
            BBDisplay.SelectionLength = 0

            If Foundno = -1 Then Else txtNote.Text = BBDatabase(Foundno).Note
        ElseIf DisplayType = 3 Then  'Hex display type
            Dim bootline As String = ""
            Dim x As Integer
            Dim count As Integer = 0
            Dim str As New StringBuilder
            For x = 0 To BootLen
                BootASCIIIntegers(x) = Buffer(x).ToString
            Next
            For i = 0 To 63
                bootline = ""
                For x = 0 To 15
                    If (count + x) > (BootLen - 1) Then
                        bootline &= "00"
                        If count + x > BootLen Then
                            bootline &= "."
                        Else
                            bootline &= Buffer(count + x).ToString("x2").ToUpper & " "
                        End If
                    Else
                        bootline &= Buffer(count + x).ToString("x2").ToUpper & " "
                    End If
                Next
                bootline &= "  "
                For x = 0 To 15
                    If (count + x) > (BootLen) Then
                        bootline &= "."
                    ElseIf Asc(chars(count + x)) = 0 Then
                        bootline &= "."
                    ElseIf Asc(chars(count + x)) = 9 Then
                        bootline &= " "
                        'ElseIf Asc(chars(count + x)) = 13 Then
                        '    bootline &= "."
                    Else
                        bootline &= chars(count + x)
                    End If
                Next
                If count < 256 Then
                    If count = 0 Then
                        str.AppendLine("00" & Hex(count) & ": " & bootline)
                    Else
                        str.AppendLine("0" & Hex(count) & ": " & bootline)
                    End If
                Else
                    str.AppendLine(Hex(count) & ": " & bootline)
                End If
                count += 16
            Next
            BBDisplay.Text = str.ToString
        ElseIf DisplayType = 5 Then
            Dim str As New StringBuilder
            Dim x As Integer
            Dim tester As Char
            Dim number As String
            Dim newline As String
            str.AppendLine("Position     Character     HEX Value     ASCII Value")
            str.AppendLine("--------     ---------     ---------     -----------")
            BBDisplay.Text = str.ToString
            For x = 0 To BootLen
                Select Case Asc(chars(x))
                    Case 0
                        tester = "."
                    Case 3
                        tester = "."
                    Case 141
                        tester = "."
                    Case 10
                        tester = "."
                    Case 9
                        tester = "."
                    Case 11
                        tester = "?"
                    Case 12
                        tester = "?"
                    Case 13
                        tester = "?"
                    Case Else
                        tester = chars(x)
                End Select
                number = String.Format("{0:000#}", x)
                newline = String.Format("{0,-15} {1,-15} {2, -10} {3, -18}", number, tester, Hex(Buffer(x)), Buffer(x).ToString)
                str.AppendLine(newline)
            Next
            BBDisplay.Text = str.ToString
        End If
        'Pass on to checkboot sub for brainfile indentification
        Dim bcrc As String
        FoundbySearch = False
        bcrc = Hex(Crc32.ComputeChecksum(Buffer))
        CheckBootByCRC(bcrc)
        If Bootname = "" Then
            CheckBoot(BootASCIIIntegers)
        End If
        Dim dossig As String = BootASCIIIntegers(0) & BootASCIIIntegers(1) & BootASCIIIntegers(2)
        If DisplayType > 1 Then
            txtBootName.Text = Bootname
            txtClass.Text = Bootclass
            txtCRC.Text = Hex(Crc32.ComputeChecksum(Buffer))
            If IsKnown Then
                cmdGotoBrain.ForeColor = Color.Green
                If Foundno > -1 Then
                    If bcrc = BBDatabase(Foundno).CRC Then txtCRC.Text = bcrc & " (Known CRC)"
                End If
            Else
                cmdGotoBrain.ForeColor = Color.Black
            End If
            cmdGotoBrain.Enabled = IsKnown
        End If
        If Iszip = True Then
            If File.Exists(Tempdrawer & "\" & zfile) = True Then
                File.Delete(Tempdrawer & "\" & zfile)
            End If
        End If
        If IsKnown = False Then
            'If chkSearch.Checked = True Then
            '    For i = 0 To Searchtext.Length - 1
            '        SearchBoot(Searchtext(i), Searchname(i), Searchclass(i))
            '    Next
            'End If
            If chkSearch.Checked = True Then
                For i = 0 To SearchDatabase.Length - 2
                    SearchBoot(SearchDatabase(i).Recog, SearchDatabase(i).Name, SearchDatabase(i).BClass)
                Next
            End If
        End If
        If IsKnown = False Then
            bootcolor = "Gray"
            If dossig = "687983" Then
                Bootname = "Unknown Bootblock"
                Bootclass = "UNK"

            Else
                Bootname = "Unknown File"
                Bootclass = "UNK"
                If Isadf = True Then Bootname = "Unknown Bootblock (Non-DOS bootblock)"
                If isdms = True Then Bootname = "Unknown DMS Bootblock (Non-DOS bootblock)"
                If isadz = True Then Bootname = "Unknown ADZ Bootblock (Non-DOS bootblock)"

            End If
            Dim savename As String
            If chkSUnk.Checked = True Then
                If OpenBootFile.Contains(".abb") Then
                    savename = Path.GetFileName(OpenBootFile)
                Else
                    savename = Path.GetFileName(OpenBootFile) & ".abb"
                End If

                If Directory.Exists(StartPath & "\Unknown") = False Then
                    Try
                        Directory.CreateDirectory(StartPath & "\Unknown")
                    Catch
                        MessageBox.Show("Cant create 'Unknown' directory in ABR path - insufficient privileges!", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)
                        chkSUnk.Checked = False
                    End Try
                End If
                Try
                    File.WriteAllBytes(StartPath & "\Unknown\" & savename, Buffer)
                Catch
                    Dim result As Integer
                    result = MessageBox.Show("Cant write bootblock file " + savename, "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)
                    If result = DialogResult.Cancel Then Exit Sub
                End Try
            End If
        End If
        bootpos = 0
        txtReadOffset.Text = 0
    End Sub
    Private Sub CheckBootByCRC(ByVal bCRC As String)
        If IsKnown = True Then Exit Sub
        Dim x As Integer
        For x = 0 To BBDatabase.Length - 2
            If bCRC = BBDatabase(x).CRC Then
                Boots = x
                Bootname = BBDatabase(x).Name
                Bootclass = BBDatabase(x).BootClass(abbnames, catnames)
                Foundno = Boots
                If DisplayType = DisplayBoot Then
                    txtNote.Text = BBDatabase(Boots).Note
                    txtKS.Text = BBDatabase(Boots).KS
                End If
                If DisplayType > 1 Then
                    chkBootable.Checked = BBDatabase(Boots).Bootable
                    chkDDR.Checked = BBDatabase(Boots).DataNeeded
                End If
                If chkCreate.Checked = True Then
                    SaveInBBLib(BBDatabase(Boots).Name, BBDatabase(Boots).BClass)
                End If
                If DisplayType = Scanner Then
                Else
                    cmdGotoBrain.Enabled = True
                    If DisplayType = Scanner Then
                    Else
                        SetTextBox_ThreadSafe(txtBootName, BBDatabase(Boots).Name)
                    End If
                    Dim intpictype As Integer = 0
                    If File.Exists(StartPath & "\bootpic\" & txtBootName.Text & ".png") Then intpictype = 1
                    If File.Exists(StartPath & "\bootpic\" & txtBootName.Text & ".gif") Then intpictype = 2
                    If DisplayType = DisplayBoot Then
                        Select Case intpictype
                            Case 1
                                picPreview.ImageLocation = StartPath & "\bootpic\" & txtBootName.Text & ".png"
                                picPreview.Load()
                            Case 2
                                picPreview.ImageLocation = StartPath & "\bootpic\" & txtBootName.Text & ".gif"
                                picPreview.Load()
                            Case 0
                                picPreview.Image = My.Resources.Nopic
                        End Select

                    Else
                        picPreview.Image = My.Resources.Nopic
                    End If
                End If
                IsKnown = True
                Dim bootimg As String = BBDatabase(Boots).Name

                If DisplayType > 1 Then
                    If InStr(bootimg, "/") Then
                        bootimg = bootimg.Replace("/", "-")
                    End If
                    txtBootName.ForeColor = BBDatabase(Boots).BootColour
                End If

                Exit For
                Exit Sub
            End If
        Next
    End Sub

    Private Sub SearchBoot(searchtm As String, name As String, bclass As String)
        If IsKnown = True Then Exit Sub
        If searchtm = "" Then Exit Sub
        Dim x As Integer
        Dim a As Integer
        FoundbySearch = False
        Dim i As Integer
        Dim endfile As Boolean
        Dim found As Boolean = False
        Dim bytestring(searchtm.Length - 1) As Byte
        For x = 0 To searchtm.Length - 1
            bytestring(x) = Convert.ToByte(searchtm(x))
        Next
        For x = 0 To Buffer.Length - 1
            a = 0
            If (x + bytestring.Length) > Buffer.Length - 1 Then
                endfile = True
                Exit For
            End If
            For i = 0 To bytestring.Length - 1
                If Buffer(x + i) = bytestring(i) Then a += 1
            Next
            If a = bytestring.Length Then found = True
            If found = True Then
                If chkCreate.Checked = True Then
                    SaveInBBLib(name, bclass)
                End If
                IsKnown = True
                FoundbySearch = True
                Dim catcol As Integer
                For i = 0 To CatList.Length - 1
                    If bclass = CatList(i).Abb Then
                        Bootclass = CatList(i).Name
                        bootcolor = CatList(i).ColorName
                        catcol = i
                        Exit For
                    End If
                Next
                Bootname = name
                If DisplayType > 1 Then
                    txtBootName.Text = name
                    txtBootName.ForeColor = CatList(catcol).Color
                    txtClass.Text = CatList(catcol).Name
                End If
                Exit Sub
            End If
            If endfile = True Then Exit For
        Next
    End Sub

    Private Sub CheckBoot(ByVal BootASCIIIntegers As Integer())
        Dim intPOS As Integer
        Dim intVal As Integer
        Dim i As Integer
        Bootno = 0
        'Count the number of bootblocks in memory
        Bootno = BBDatabase.Length - 2
        Dim x As Integer = 0
        Dim z As Integer = 0
        Dim y As Integer = 0
        Foundno = -1
        Dim hit As Integer = 0
        IsKnown = False
        Dim dossig As String = BootASCIIIntegers(0) & BootASCIIIntegers(1) & BootASCIIIntegers(2)
        For Boots = 0 To Bootno - 1
            If BBDatabase(Boots).Name = "DELETED" OrElse BBDatabase(Boots).Name = "" Then
            Else
                x = 0
                y = 1
                For i = 0 To 6
                    intPOS = IntMainLoc(Boots, x)
                    intVal = IntMainLoc(Boots, y)
                    If BootASCIIIntegers(intPOS) = intVal Then hit += 1
                    x += 2
                    y += 2
                Next
            End If
            'If bootblock found...
            If hit = 7 Then
                Foundno = Boots
                Bootclass = BBDatabase(Foundno).BootClass(abbnames, catnames)
                If DisplayType = DisplayBoot Then
                    txtNote.Text = BBDatabase(Boots).Note
                    txtKS.Text = BBDatabase(Boots).KS
                    txtClass.Text = Bootclass
                    chkBootable.Checked = BBDatabase(Foundno).Bootable
                    chkDDR.Checked = BBDatabase(Foundno).DataNeeded
                End If
                If chkCreate.Checked = True Then
                    SaveInBBLib(BBDatabase(Boots).Name, BBDatabase(Boots).BClass)
                End If
                If DisplayType = Scanner Then
                    Bootname = BBDatabase(Boots).Name
                Else
                    cmdGotoBrain.Enabled = True
                    If DisplayType = Scanner Then
                    Else
                        SetTextBox_ThreadSafe(txtBootName, BBDatabase(Boots).Name)
                    End If
                    Bootname = BBDatabase(Boots).Name
                    If DisplayType = DisplayBoot Then
                        If File.Exists(StartPath & "\bootpic\" & txtBootName.Text & ".png") Then
                            picPreview.ImageLocation = StartPath & "\bootpic\" & txtBootName.Text & ".png"
                            picPreview.Load()
                        Else
                            picPreview.Image = My.Resources.Nopic
                        End If
                    End If
                End If
                IsKnown = True
                'Store CRC information if missing
                Dim bootimg As String = BBDatabase(Boots).Name
                'Display classification

                If DisplayType > 1 Then
                    If InStr(bootimg, "/") Then
                        bootimg = bootimg.Replace("/", "-")
                    End If
                End If

                Exit For
            Else
                IsKnown = False
            End If
            hit = 0
        Next
        If IsKnown = False Then
            Bootname = "Unknown Bootblock"
            bootcolor = "Black"
        End If
    End Sub

    Private Sub SaveInBBLib(bootname As String, bootclass As String)
        If CheckBox1.Checked = True Then
            If bootclass = "v" Then Exit Sub
        End If
        If bootname = "" Then Exit Sub
        Dim savename As String = bootname
        newsaved = False
        If Directory.Exists(My.Settings.BBlocks) = False Then
            Try
                Directory.CreateDirectory(My.Settings.BBlocks)
            Catch
                MessageBox.Show("Cant create drawer in ABR directory - insufficient privileges", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                My.Settings.CreateBoots = False
                chkCreate.Checked = False
            End Try
        End If

        If InStr(bootname, "/") Then
            savename = bootname.Replace("/", "-")
        End If
        If InStr(bootname, "?") Then
            savename = bootname.Replace("?", "-")
        End If
        Dim writefile As String = ""
        Try
            writefile = My.Settings.BBlocks & "\" & savename & ".abb"
            If File.Exists(writefile) Then
            Else
                If chkErrorOnly.Checked = False Then AddtoLog("New bootblock '" & savename & "' added to bootblock collection")
                File.WriteAllBytes(writefile, Buffer)
                newsaved = True
            End If
            Savecount += 1
        Catch
            Dim result As Integer
            result = MessageBox.Show("Cant write file - " + savename, "Cant write file", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)
            If result = DialogResult.Cancel Then Exit Sub
        End Try
    End Sub

    Private Sub RefreshDisplay()
        rtBBDisplay.Visible = True
        DisplayType = DisplayBoot
        Using f As Font = BBDisplay.Font
            Select Case ListBox2.SelectedIndex
                Case 0
                    rtBBDisplay.Visible = True
                    BBDisplay.ScrollBars = ScrollBars.None
                    ReadCP = Win
                    DisplayType = DisplayBoot
                Case 1
                    BBDisplay.ScrollBars = ScrollBars.None
                    ReadCP = ISO
                    DisplayType = DisplayBoot
                Case 2
                    ReadCP = utf7
                    BBDisplay.ScrollBars = ScrollBars.None
                    DisplayType = DisplayBoot
                Case 3
                    ReadCP = Encoding.Default
                    BBDisplay.ScrollBars = ScrollBars.None
                    DisplayType = DisplayBoot
                Case 4
                    rtBBDisplay.Visible = False
                    ReadCP = Ascii
                    BBDisplay.ScrollBars = ScrollBars.None
                    DisplayType = 4
                Case 5
                    rtBBDisplay.Visible = False
                    DisplayType = 5
                    BBDisplay.ScrollBars = ScrollBars.Vertical
                Case 6
                    rtBBDisplay.Visible = False
                    BBDisplay.ScrollBars = ScrollBars.Vertical
                    DisplayType = 3
            End Select
        End Using
        Call Disptype(DisplayType, OpenBootFile)
    End Sub

    Public Sub ReadBrain()
        Dim bnode As XmlNode
        Dim i As Integer = 0, x As Integer
        Dim rline As String
        Dim BootCRC As String
        Dim items(16) As String
        Dim BrainNodelist As XmlNodeList
        BrainXML = New XmlDocument
        BrainXML.Load(DefaultBrainPath)
        BrainNodelist = BrainXML.GetElementsByTagName("Bootblock")
        ReDim IntMainLoc(BrainNodelist.Count, 17)
        ReDim BBDatabase(BrainNodelist.Count)
        BBAmt = BrainNodelist.Count - 1
        For Each bnode In BrainNodelist
            Dim Note As String
            rline = bnode.Item("Recog").InnerText
            If Not bnode.Item("Notes") Is Nothing AndAlso
            Len(bnode.Item("Notes").InnerText) > 0 Then
                Note = bnode.Item("Notes").InnerText
            Else Note = ""
            End If
            If Not bnode.Item("CRC") Is Nothing AndAlso
            Len(bnode.Item("CRC").InnerText) > 0 Then
                BootCRC = bnode.Item("CRC").InnerText
            Else BootCRC = ""
            End If
            Dim data As Boolean
            Dim boot As Boolean
            If bnode.Item("Bootable").InnerText = "N" Then boot = False Else boot = True
            If bnode.Item("Data").InnerText = "Y" Then data = True Else data = False
            items = Split(rline, ",")
            For x = 0 To 13
                IntMainLoc(i, x) = Val(items(x))
            Next
            BBDatabase(i) = New AmigaBB(bnode.Item("Name").InnerText, bnode.Item("Class").InnerText, bnode.Item("Recog").InnerText, data,
                           bnode.Item("KS").InnerText, boot, BootCRC, Note)
            i += 1
        Next
        PopulateBFBox()
        BrainTree.Sort()
        TextBox5.Text = BrainTree.Nodes.Count & " bootblocks in main brainfile"
        AddtoLog(BrainTree.Nodes.Count & " main bootblock nodes found")

    End Sub
    Public Shared Function FormatFileSize(ByVal FileSizeBytes As Long) As String
        Dim sizeTypes() As String = {"b", "Kb", "Mb", "Gb"}
        Dim Len As Decimal = FileSizeBytes
        Dim sizeType As Integer = 0
        Do While Len > 1024
            Len = Decimal.Round(Len / 1024, 2)
            sizeType += 1
            If sizeType >= sizeTypes.Length - 1 Then Exit Do
        Loop

        Dim Resp As String = Len.ToString & " " & sizeTypes(sizeType)
        Return Resp
    End Function
    Public Sub RefreshSearchBB()
        trvSearch.Nodes.Clear()
        Dim xmld As XmlDocument
        Dim nodelist As XmlNodeList
        Dim bnode As XmlNode
        Dim i As Integer, x As Integer
        Dim items(16) As String
        Dim data As Boolean
        Dim boot As Boolean
        xmld = New XmlDocument
        xmld.Load("searchbrain.xml")
        nodelist = xmld.GetElementsByTagName("Bootblock")
        ReDim SearchDatabase(nodelist.Count)
        searchnodes = nodelist.Count - 1
        For Each bnode In nodelist
            If bnode.Item("Data").InnerText = "Y" Then data = True Else data = False
            If bnode.Item("Bootable").InnerText = "Y" Then boot = True Else boot = False
            SearchDatabase(i) = New AmigaBB(bnode.Item("Name").InnerText, bnode.Item("Class").InnerText, bnode.Item("Recog").InnerText,
                                            data, bnode.Item("KS").InnerText, boot, "---")
            trvSearch.Nodes.Add(SearchDatabase(i).Name)
            trvSearch.Nodes(i).ForeColor = SearchDatabase(i).BootColour
            trvSearch.Nodes(i).Nodes.Add("Bootblock Type: " & SearchDatabase(i).BootClass(abbnames, catnames))
            trvSearch.Nodes(i).Nodes.Add("KickStart req:" & SearchDatabase(i).KS)
            trvSearch.Nodes(i).Nodes.Add("Disk Data Required:" & SearchDatabase(i).DataNeeded)
            trvSearch.Nodes(i).Nodes.Add("Bootable:" & SearchDatabase(i).Bootable)
            i += 1
        Next
        TextBox11.Text = trvSearch.Nodes.Count & " bootblocks in search brainfile"
        AddtoLog(trvSearch.Nodes.Count & " search bootblock nodes found")
    End Sub
    Public Sub LoadEncyclopedia()
        Dim xmld As XmlDocument
        Dim nodelist As XmlNodeList
        Dim bnode As XmlNode
        Dim i As Integer = 0
        xmld = New XmlDocument
        xmld.Load(StartPath & "\encyclopedia.xml")
        TreeView1.Nodes.Clear()
        nodelist = xmld.GetElementsByTagName("Program")
        ReDim BBE(nodelist.Count)
        For Each bnode In nodelist
            BBE(i) = New ABREncyclopedia(bnode.Item("Name").InnerText, bnode.Item("ProgramID").InnerText, bnode.Item("Note").InnerText)
            TreeView1.Nodes.Add(BBE(i).Name)
            i += 1
        Next
        AddtoLog(nodelist.Count & " entries found")
    End Sub
    Public Sub LoadCategoryList()
        Dim xmld As XmlDocument
        Dim nodelist As XmlNodeList
        Dim bnode As XmlNode
        Dim i As Integer = 0

        xmld = New XmlDocument
        xmld.Load("catlist.xml")
        trvCategories.Nodes.Clear()
        nodelist = xmld.GetElementsByTagName("Category")
        ReDim CatList(nodelist.Count - 1)
        ReDim catnames(CatList.Length - 1)
        ReDim abbnames(CatList.Length - 1)
        For Each bnode In nodelist
            CatList(i) = New BootCategory(bnode.Item("Name").InnerText, bnode.Item("abbrev").InnerText, bnode.Item("Colour").InnerText, Color.FromName(bnode.Item("Colour").InnerText))
            trvCategories.Nodes.Add(CatList(i).Name & " / " & CatList(i).Abb)
            trvCategories.Nodes(i).ForeColor = CatList(i).Color
            catnames(i) = CatList(i).Name
            abbnames(i) = CatList(i).Abb
            i += 1
        Next
        AddtoLog(nodelist.Count & " category nodes found")
    End Sub

    Public Sub SaveCategoryList()
        AddtoLog("Saving category list")
        Dim i As Integer
        Dim writer As New XmlTextWriter("catlist.xml", utf8)
        writer.Formatting = Formatting.Indented
        writer.Indentation = 2
        writer.WriteStartDocument(True)
        writer.WriteStartElement("Categories")
        For i = 0 To CatList.Length - 1
            CreateCat(CatList(i).Name, CatList(i).ColorName, CatList(i).Abb, writer)
        Next
        writer.WriteEndElement()
        writer.WriteEndDocument()
        writer.Close()

    End Sub
    Private Sub Main_Keypress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F5 Then
            cmdView.Enabled = False
            cmdOpenFR.Enabled = False
            Worker.CancelAsync()
            DisplayType = Scanner
            ToolStripProgressBar1.Value = 0
            dgvBatch.Rows.Clear()
            Worker.RunWorkerAsync()
        ElseIf e.KeyCode = Keys.Space Then
            cmdStop.PerformClick()
        ElseIf e.KeyCode = Keys.Tab Then
            Dim x As Integer
            x = tabBrain.SelectedIndex
            If tabBrain.SelectedIndex < 8 Then
                tabBrain.SelectTab(x + 1)
            Else
                tabBrain.SelectTab(0)
            End If
        End If
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If File.Exists(My.Settings.XDMS) = False Then
            My.Settings.XDMS = ""
            My.Settings.ProcessDMS = False
            MessageBox.Show("Please set correct XDMS path to process DMS Files")
        End If
        Try
            sbfiles = Directory.GetFiles(My.Settings.BBlocks, "*.*", SearchOption.AllDirectories)
        Catch
            If Directory.Exists(My.Settings.BBlocks) Then
            Else
                Directory.CreateDirectory(My.Settings.BBlocks)
                sbfiles = Directory.GetFiles(My.Settings.BBlocks, "*.*", SearchOption.AllDirectories)
            End If
        End Try
        Dim flog As Boolean
        Me.KeyPreview = True
        If chkLog.Checked = False Then
            flog = True
            chkLog.Checked = True
        End If
        If File.Exists(StartPath & "\xdms.exe") Then
            Label2.Text = "XDMS executable found"
            Label2.ForeColor = Color.Green
        Else
            Label2.Text = "XDMS executable not found in ABR Path"
            Label2.ForeColor = Color.Red
        End If

        If File.Exists(StartPath & "\ira.exe") Then
            Label16.Text = "IRA executable found"
            Label16.ForeColor = Color.Green
        Else
            Label16.Text = "IRA executable not found in ABR Path"
            Label16.ForeColor = Color.Red
        End If
        AddtoLog("----------------------------------------------------------------")
        AddtoLog("| Amiga Bootblock Reader v4.2 (C) 2021 by Jason and Jordan Smith |")
        AddtoLog("----------------------------------------------------------------")
        AddtoLog("")
        AddtoLog("Date / Time: " & Date.Now)
        AddtoLog("")
        If flog = True Then
            chkLog.Checked = False
        End If
        AddtoLog("Logfile created at " & StartPath & "\ABRLogfile.txt")
        AddtoLog("")
        Control.CheckForIllegalCrossThreadCalls = False
        AddtoLog("Loading Category list from Category.xml")
        LoadCategoryList()
        AddtoLog("Loading Encyclopedia Listing from Encyclopedia.xml")
        LoadEncyclopedia()
        Dim ttAll As New ToolTip()
        Dim ttUnk As New ToolTip()
        Dim ttVir As New ToolTip()
        Dim arg As String = ""
        Dim i As Integer = 0

        DefaultH = Me.Size.Height
        DefaultW = Me.Size.Width
        ListBox2.SelectedIndex = 0
        ReadCP = Win
        If My.Settings.BrainPath = "\" Then
            My.Settings.BrainPath = StartPath
        End If
        Brainpath = My.Settings.BrainPath & "\brainfile.xml"
        Worker.WorkerReportsProgress = True
        Worker.WorkerSupportsCancellation = True
        SWorker.WorkerReportsProgress = True
        SWorker.WorkerSupportsCancellation = True
        ofdBootOpen.InitialDirectory = My.Settings.ADFs
        Tempdrawer = Environment.GetEnvironmentVariable("Temp")
        TextBox2.Text = Tempdrawer
        'Set up display of various parts of the main form
        cmdView.Enabled = False
        cmdOpenFR.Enabled = False
        AddtoLog("Reading brainfile from brainfile.xml")
        Call ReadBrain()
        Call RefreshSearchBB()
        PopInfo()
        txtSBoots.Text = My.Settings.BBlocks
        If txtSBoots.Text = "" Then
            My.Settings.BBlocks = StartPath & "\Bootblocks"
            txtSBoots.Text = My.Settings.BBlocks
        End If
        txtADFs.Text = My.Settings.ADFs
        txtUAE.Text = My.Settings.UAEPath
        If My.Settings.ProcessZips Then chkZip.Checked = True
        If My.Settings.CreateBoots Then chkCreate.Checked = True
        If My.Settings.SCRC Then chkSCRC.Checked = True
        If My.Settings.SScan Then chkBMT.Checked = True
        If My.Settings.SSize Then chkFS.Checked = True
        If My.Settings.ProcessDMS Then chkDMS.Checked = True
        If My.Settings.SupDMS Then chkSDMS.Checked = True
        If My.Settings.ADZZIP Then chkTZIP.Checked = True
        If My.Settings.EnableLog Then chkLog.Checked = True
        If My.Settings.ScanALL Then chkIFE.Checked = True
        dgvBatch.Columns(5).Visible = chkBMT.Checked
        dgvBatch.Columns(4).Visible = chkSCRC.Checked
        dgvBatch.Columns(2).Visible = chkFS.Checked
        filtered = False
        lbxFilter.SelectedIndex = 0

        If File.Exists(StartPath & "\xdms.exe") = False Then
            chkDMS.Checked = False
        End If
        Dim snip As String
        snip = StartPath.Length + 7
        AddtoLog("Reading Bootblock collection from " & My.Settings.BBlocks)
        ReadBBlib(My.Settings.BBlocks)
        Formloaded = True
        Dim starttext As String = "Unknown Emulator"
        If InStr(txtUAE.Text.ToUpper, "FS-UAE") Then starttext = "FS-UAE"
        If InStr(txtUAE.Text.ToUpper, "WINUAE") Then starttext = "WinUAE (32/64)"
        If InStr(txtUAE.Text.ToUpper, ".EXE") Then
            Label71.Text = starttext & " " & GetFileVersionInfo(txtUAE.Text).ToString & " used"
        End If
        tssBFDate.Text = File.GetLastWriteTime(Brainpath)
        tssSBDate.Text = File.GetLastWriteTime(DefaultSBrainPath)
        TextBox8.Text = logfile.ToString
        AddtoLog("")
    End Sub

    Public Sub ReadBBlib(ByVal CurBBDraw As String)
        Dim subdirs() As String
        Dim pardirs() As String
        Dim x As Integer
        Dim abfiles() As String = sbfiles
        trvBBCollection.Nodes.Clear()
        TextBox7.Text = CurBBDraw
        If Directory.Exists(CurBBDraw) Then
            abfiles = Directory.GetFiles(CurBBDraw, "*.*", SearchOption.TopDirectoryOnly)
            If CurBBDraw = My.Settings.BBlocks Then
                pardirs = Directory.GetFiles(CurBBDraw, "*.*", SearchOption.AllDirectories)
                AddtoLog(pardirs.Length & " Bootblocks found in " & CurBBDraw)
                TextBox4.Text = pardirs.Length & " bootblocks found in library"
                ReDim pardirs(10)
            End If
            subdirs = Directory.GetDirectories(CurBBDraw)
            If subdirs.Length = 0 Then
                If abfiles.Length = 0 Then
                    Exit Sub
                End If
            End If

            If subdirs.Length > 0 Then
                For x = 0 To subdirs.Length - 1
                    trvBBCollection.Nodes.Add(subdirs(x).Replace(".abb", ""))
                Next
            End If
            For x = 0 To abfiles.Length - 1
                trvBBCollection.Nodes.Add(Path.GetFileName(abfiles(x).Replace(".abb", "")))
            Next
        Else
            Directory.CreateDirectory(My.Settings.BBlocks)
        End If

        If abfiles.Length = Nothing Then
            Exit Sub
        End If
        trvBBCollection.Sort()
        Dim y As Integer
        For x = 0 To trvBBCollection.Nodes.Count - 1
            For y = 0 To BrainTree.Nodes.Count - 1
                If trvBBCollection.Nodes(x).Text = BrainTree.Nodes(y).Text Then
                    trvBBCollection.Nodes(x).ForeColor = BrainTree.Nodes(y).ForeColor
                End If
            Next
        Next
        For x = 0 To trvBBCollection.Nodes.Count - 1
            For y = 0 To trvSearch.Nodes.Count - 1
                If trvBBCollection.Nodes(x).Text = trvSearch.Nodes(y).Text Then
                    trvBBCollection.Nodes(x).ForeColor = trvSearch.Nodes(y).ForeColor
                End If
            Next
        Next
    End Sub

    Private Shared Sub AboutBoxShow() Handles Me.HelpButtonClicked
        AboutBox1.Show()
    End Sub

    Public Sub ResetForm() 'Reset all back to default
        Array.Clear(IntBytePos, 0, 6)
        txtTrail.Text = ""
        IntClicked = 0
        lblMaxel.Visible = False
        txtFS.Text = ""
        txtLen.Text = ""
        txtReadOffset.Text = 0
    End Sub

    Public Sub CreateXMLRec(ByVal BName As String, ByVal BRec As String, ByVal BClass As String, ByVal ks As String, ByVal Boot As String, ByVal DN As String, ByVal BNotes As String, ByVal BCRC As String, ByVal writer As XmlTextWriter)
        writer.WriteStartElement("Bootblock")
        writer.WriteElementString("Name", BName)
        writer.WriteElementString("Recog", BRec)
        writer.WriteElementString("Class", BClass)
        If BNotes = "" Then Else writer.WriteElementString("Notes", BNotes)
        writer.WriteElementString("KS", ks)
        writer.WriteElementString("Bootable", Boot)
        writer.WriteElementString("Data", DN)
        If BCRC = "" Then Else writer.WriteElementString("CRC", BCRC)
        writer.WriteEndElement()
    End Sub

    Public Sub CreateCat(ByVal CatName As String, ByVal CatColour As String, ByVal CatAbbrev As String, ByVal writer As XmlTextWriter)
        writer.WriteStartElement("Category")
        writer.WriteElementString("Name", CatName)
        writer.WriteElementString("abbrev", CatAbbrev)
        writer.WriteElementString("Colour", CatColour)
        writer.WriteEndElement()

    End Sub

    Public Sub SaveBrain()
        Dim i As Integer, x As Integer
        Dim recstring As String = ""
        Dim settings As XmlWriterSettings = New XmlWriterSettings
        settings.Indent = True
        Dim writer As New XmlTextWriter("brainfile.xml", utf8)
        writer.Formatting = Formatting.Indented
        writer.Indentation = 2
        writer.WriteStartDocument(True)
        writer.WriteStartElement("Bootblocks")
        For i = 0 To BBDatabase.Length - 2
            recstring = ""
            If BBDatabase(i).Name = "DELETED" OrElse BBDatabase(i).BClass = "" Then
            Else
                For x = 0 To 13
                    recstring &= IntMainLoc(i, x)
                    If x < 13 Then recstring &= ","
                Next
                CreateXMLRec(BBDatabase(i).Name, recstring, BBDatabase(i).BClass, BBDatabase(i).KS, BBDatabase(i).Bootable, BBDatabase(i).DataNeeded,
                             BBDatabase(i).Note, BBDatabase(i).CRC, writer)
            End If
        Next
        writer.WriteEndElement()
        writer.WriteEndDocument()
        writer.Close()

    End Sub

    'Public Sub SaveSearchBB()
    '    Dim i As Integer
    '    Dim writer As New XmlTextWriter("searchbrain.xml", utf8)
    '    writer.WriteStartDocument(True)
    '    writer.WriteStartElement("SBBootblocks")
    '    writer.Formatting = Formatting.Indented
    '    writer.Indentation = 2
    '    For i = 0 To Searchname(i).Length - 1
    '        If Searchname(i) = "DELETED" OrElse Searchclass(i) = "" Then
    '        Else
    '            CreateXMLRec(Searchname(i), Searchtext(i), Searchclass(i), SearchKS(i), SearchBootable(i), SearchDR(i), Searchnote(i), "---", writer)
    '        End If
    '    Next
    '    writer.WriteEndElement()
    '    writer.WriteEndDocument()
    '    writer.Close()

    'End Sub

    'Public Sub SaveSearchBBEntry(ByVal newSS As String, ByVal newClass As String, ByVal newText As String, ByVal newKS As String, ByVal newDR As String, ByVal newBoot As String)
    '    Dim i As Integer
    '    Dim writer As New XmlTextWriter("searchbrain.xml", utf8)
    '    writer.WriteStartDocument(True)
    '    writer.WriteStartElement("SBBootblocks")
    '    For i = 0 To Searchname(i).Length - 1
    '        If Searchname(i) = "DELETED" OrElse Isbtype(i) = "" Then
    '        Else
    '            CreateXMLRec(Searchname(i), Searchtext(i), Searchclass(i), SearchKS(i), SearchBootable(i), SearchBootable(i), Searchnote(i), "---", writer)
    '        End If
    '    Next
    '    CreateXMLRec(newSS, newText, newClass, newKS, newDR, newBoot, "---", "---", writer)
    '    writer.WriteEndElement()
    '    writer.WriteEndDocument()
    '    writer.Close()
    '    RefreshSearchBB()
    'End Sub

    Private Sub cmdSaveFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveBufferToFileToolStripMenuItem.Click
        If OpenBootFile = "" Then
            MessageBox.Show("No Bootblock / Disk file loaded")
            Exit Sub
        End If
        Dim intresult As Integer
        sfdBootSave.FileName = txtBootName.Text.Replace("/", "-").Replace("??", "")
        intresult = sfdBootSave.ShowDialog()
        If intresult = DialogResult.OK Then MessageBox.Show("Bootblock saved")
    End Sub

    Private Sub cmdInstallBoot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InstallBootblockFromFileToolStripMenuItem.Click
        If OpenBootFile = "" Then Exit Sub
        ofdInstall.InitialDirectory = My.Settings.BBlocks
        ofdInstall.ShowDialog()
    End Sub
    Public Sub AddtoLog(ByVal s As String)
        If chkLog.Checked = True Then
            logfile.AppendLine(s)
            Try
                File.WriteAllText(StartPath & "\ABRLogfile " & Date.Now & ".txt", logfile.ToString)
            Catch ex As Exception
            End Try
            If chkReal.Checked = True Then
                TextBox8.Text = logfile.ToString
            End If
        Else
        End If
    End Sub

    Private Sub cmdBBSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBBSet.Click
        fbdOpen.SelectedPath = My.Settings.BBlocks
        fbdOpen.ShowDialog()
        My.Settings.BBlocks = fbdOpen.SelectedPath
        txtSBoots.Text = My.Settings.BBlocks
    End Sub

    Private Sub cmdADF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdADF.Click
        fbdOpen.SelectedPath = My.Settings.ADFs
        fbdOpen.ShowDialog()
        My.Settings.ADFs = fbdOpen.SelectedPath
        txtADFs.Text = My.Settings.ADFs
    End Sub

    Private Sub ofdInstall_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ofdInstall.FileOk
        Dim i As Integer
        Dim boot(1024) As Byte
        Dim adf As New BinaryReader(File.OpenRead(OpenBootFile))
        If File.Exists(ofdInstall.FileName) Then
            Dim input As New BinaryReader(File.OpenRead(ofdInstall.FileName))
            Dim FileProps As FileInfo = New FileInfo(ofdInstall.FileName)
            If FileProps.Length > 1024 Then
                MessageBox.Show("The file you wish to install doesn't appear to be a bootblock file")
                Exit Sub
            End If
            boot = input.ReadBytes(1024)
            For i = 0 To 1024
                Adfbuffer(i) = 0
            Next
            For i = 0 To boot.Length - 1
                Adfbuffer(i) = boot(i)
            Next
            input.Close()
            adf.Close()
            Try
                File.WriteAllBytes(OpenBootFile, Adfbuffer)
            Catch
                MessageBox.Show("Error writing bootblock - ADF file is in use / Write protected")
                Exit Sub
            End Try
        End If
        Disptype(2, OpenBootFile)
        MessageBox.Show("Bootblock " & ofdInstall.SafeFileName & " Installed on " & ofdBootOpen.SafeFileName & " disk")
    End Sub



    Public Sub cmdBatch()
        If Worker.IsBusy Then Exit Sub
        tabBrain.SelectTab(1)
        txtReadOffset.Text = 0
        If fbdOpen.SelectedPath IsNot Nothing Then
            DisplayType = Scanner
            ToolStripProgressBar1.Value = 0
            My.Settings.LastPath = fbdOpen.SelectedPath
            My.Settings.Save()
            cmdView.Enabled = False
            cmdOpenFR.Enabled = False
            FileToolStripMenuItem.Enabled = False
            TestBootblockInBufferToolStripMenuItem.Enabled = False
            DirectoryScannerToolStripMenuItem.Enabled = False
            BootblockBufferToolStripMenuItem.Enabled = True
            FileToolStripMenuItem.Enabled = False
            TextBox10.Text = fbdOpen.SelectedPath
            Worker.RunWorkerAsync()
        End If

    End Sub

    Private Sub worker_done(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles Worker.RunWorkerCompleted
        lbxFilter.Enabled = True
        ListBox4.Enabled = True
        If chkSkip.Checked Then chkSkip.Checked = False
        If Files Is Nothing Then Exit Sub
        If Files.Length = 0 Then
            If chkIFE.Checked = True Then
                dgvBatch.Rows.Add("= No files found in drawer =")
            Else
                dgvBatch.Rows.Add("= No ADF/ADZ/DMS/ZIP/ABB/ABK Files found - Use 'Scan All Files' to ignore extension if needed =")
            End If
        End If
        'Enable controls which will cause errors if used during scan
        TestBootblockInBufferToolStripMenuItem.Enabled = True
        DirectoryScannerToolStripMenuItem.Enabled = True
        BootblockBufferToolStripMenuItem.Enabled = True
        FileToolStripMenuItem.Enabled = True
        cmdView.Enabled = True
        ' End
        txtBootName.Text = ""
        ToolStripProgressBar1.Value = ToolStripProgressBar1.Maximum
        cmdOpenFR.Enabled = True
        cmdStop.Enabled = False
        txtSel.Text = 1 & "/" & dgvBatch.Rows.Count
        If e.Cancelled Then
            tssCount.Text = dgvBatch.Rows.Count - 1 & " / " & dgvBatch.Rows.Count - 1
            txtSel.Text = 1 & "/" & Counter - 1
        End If
        OpenBootFile = ""
        txtCFile.Text = ""
        ToolStripProgressBar1.Visible = True
        txtCFile.Visible = True
        If newsaved = True Then ReadBBlib(My.Settings.BBlocks)
        TextBox8.Text = logfile.ToString
        Dim x As Integer
        If filtered = True Then
        Else
            For x = 0 To dgvBatch.Rows.Count - 2
                If filecolor(x) IsNot Nothing Then
                    dgvBatch.Rows(x).Cells(1).Style.ForeColor = Color.FromName(filecolor(x))
                    dgvBatch.Rows(x).Cells(3).Style.ForeColor = Color.FromName(filecolor(x))
                End If
            Next
        End If
    End Sub
    Public Function GetAllKnownFiles(ByVal SO As SearchOption) As String()
        dgvBatch.Select()
        txtCFile.Text = "Parsing all known files....."
        Dim BFiles() As String
        Try
            BFiles = Directory.GetFiles(fbdOpen.SelectedPath, "*.*", SO)
        Catch
            MessageBox.Show("Error parsing directory - avoid scanning root drawers")
            Exit Function
        End Try
        Dim ALLFiles(BFiles.Length - 1) As String
        Dim x As Integer
        Dim ext As String
        Dim bc As Integer = 0
        If chkIFE.Checked = True Then
            txtCFile.Text = "Parsing ALL Files......."
            ALLFiles = Directory.GetFiles(fbdOpen.SelectedPath, "*.*", SO)
        Else
            For x = 0 To BFiles.Length - 1
                ext = Path.GetExtension(BFiles(x))
                Select Case ext.ToUpper
                    Case ".ADF"
                        ALLFiles(bc) = BFiles(x)
                        bc += 1
                    Case ".DMS"
                        ALLFiles(bc) = BFiles(x)
                        bc += 1
                    Case ".ABB"
                        ALLFiles(bc) = BFiles(x)
                        bc += 1
                    Case ".ABK"
                        ALLFiles(bc) = BFiles(x)
                        bc += 1
                    Case ".ZIP"
                        ALLFiles(bc) = BFiles(x)
                        bc += 1
                    Case ".ADZ"
                        ALLFiles(bc) = BFiles(x)
                        bc += 1
                End Select
            Next
            ReDim Preserve ALLFiles(bc - 1)
        End If
        Return ALLFiles
    End Function
    Private Sub worker_dowork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles Worker.DoWork
        If fbdOpen.SelectedPath = "" Then Exit Sub
        ListBox4.Enabled = False
        dgvBatch.Rows.Clear()
        Dim worker As BackgroundWorker = DirectCast(sender, BackgroundWorker)
        Counter = 0
        filtercount = 0
        ListBox4.Items.Clear()
        Dim x As Integer = 0
        dgvBatch.Columns(5).Visible = chkBMT.Checked
        dgvBatch.Columns(4).Visible = chkSCRC.Checked
        dgvBatch.Columns(2).Visible = chkFS.Checked
        drawers = Directory.GetDirectories(fbdOpen.SelectedPath)
        Dim i As Integer
        If drawers.Length = 0 Then ListBox4.Items.Add("No Subdirectories in " & fbdOpen.SelectedPath)
        For i = 0 To drawers.Length - 1
            ListBox4.Items.Add(drawers(i))
        Next
        lbxFilter.Enabled = False
        If fbdOpen.SelectedPath = "" Then Exit Sub
        Dim str As New StringBuilder
        If Not Files Is Nothing Then
            Array.Clear(Files, 0, Files.Length)
        End If
        'Process files in subdirs if Check subdirectories is checked
        If BatchScan.chkSub.Checked = True Then
            Files = GetAllKnownFiles(SearchOption.AllDirectories)
        Else
            Files = GetAllKnownFiles(SearchOption.TopDirectoryOnly)
        End If
        If Not Files Is Nothing Then
        Else
            Exit Sub
        End If
        If Files.Length > 0 Then ToolStripProgressBar1.Maximum = Files.Length - 1
        initialPath = fbdOpen.SelectedPath
        Array.Sort(Files)
        If Files.Length = 0 Then
            Bootname = "No ADF Files in drawer"
            worker.CancelAsync()
        End If
        'Disable controls which will cause errors if used during scan
        ReDim Batchlist(Files.Length - 1)
        ReDim CList(Files.Length)
        ReDim Filebname(Files.Length)
        ReDim Fileclass(Files.Length)
        ReDim filecolor(Files.Length)
        ReDim Filebnum(Files.Length)
        Dim bfile As String
        Dim offset As Integer = 1

        For x = 0 To Files.Length - 1
            If lbxFilter.SelectedIndex > 0 Then Thread.Sleep(5)
            If chkNoZip.Checked = True Then
                If InStr(Files(x), ".zip") Then
                    Batchlist(x) = "Skipped - Please use Open/View"
                Else
                    Disptype(1, Files(x))
                End If
            Else
                Disptype(1, Files(x))
            End If
            bfile = Files(x).Substring(fbdOpen.SelectedPath.Length + 1)
            Fileclass(x) = Bootclass
            Filebname(x) = Bootname
            Filebnum(x) = Boots
            Batchlist(x) = bfile
            If Boots < BBDatabase.Length - 1 Then
                filecolor(x) = BBDatabase(Boots).ColorName
            ElseIf FoundbySearch = True Then
                filecolor(x) = SearchDatabase(SearchNo(Bootname)).ColorName
            End If

            worker.ReportProgress(x)
            If worker.CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If
        Next
    End Sub

    Public Function Truncate(value As String, length As Integer) As String
        ' If argument is too big, return the original string.
        ' ... Otherwise take a substring from the string's start index.
        If length > value.Length Then
            Return value
        Else
            Return value.Substring(0, length)
        End If
    End Function

    Public Function SearchNo(ByVal Bootname As String) As Integer
        Dim x As Integer
        For x = 0 To SearchDatabase.Length - 2
            If SearchDatabase(x).Name = Bootname Then
                Return x
            Else
                Return 0
            End If
        Next
    End Function

    Private Sub worker_progress(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles Worker.ProgressChanged
        cmdStop.Enabled = True
        Dim tval As Integer = 97
        Dim fi As New FileInfo(Files(Counter))
        Dim flength As Long = fi.Length
        Dim fltext As String = FormatFileSize(flength)
        Dim CRCsum = Hex(Crc32.ComputeChecksum(Buffer))
        Dim truncfile As String = Truncate(Files(Counter).Replace(initialPath & "\", ""), tval)
        Dim kcrc As String = ""
        filtered = False
        Select Case lbxFilter.SelectedIndex
            Case 0
                Select Case Foundno
                    Case -1
                        kcrc = "---"
                    Case > -1
                        If Foundno > -1 Then
                            If BBDatabase(Foundno).CRC = CRCsum Then
                                kcrc = "Known CRC"
                            ElseIf BBDatabase(Foundno).CRC = "---" Then
                                kcrc = "---"
                            Else
                                If Bootclass = "Utility" Then
                                    kcrc = "Byte Match (!)"
                                Else
                                    kcrc = "Byte Match"
                                End If

                            End If
                        End If
                End Select
                If FoundbySearch = True Then kcrc = "Text Match"

                dgvBatch.Rows.Add(Counter, truncfile, fltext, Filebname(Counter), CRCsum, kcrc)
                If chkCD.Checked = True Then
                    If Foundno > -1 Then dgvBatch.Rows(Counter).Cells(3).Style.ForeColor = BBDatabase(Foundno).BootColour
                End If
            Case 1
                filtered = True
                If Bootclass = "Virus" Then
                    dgvBatch.Rows.Add(Counter, truncfile, fltext, Filebname(Counter), CRCsum, kcrc)
                    dgvBatch.Rows(dgvBatch.Rows.Count - 1).Cells(3).Style.ForeColor = Color.Red
                End If
            Case 2
                filtered = True
                If Bootclass = "UNK" Then
                    dgvBatch.Rows.Add(Counter, truncfile, CRCsum, kcrc)

                End If
        End Select
        Try
            ToolStripProgressBar1.Value = Counter
        Catch f As NullReferenceException
        End Try
        Counter += 1
        tssCount.Text = (Counter & " / " & Files.Length)
    End Sub

    Delegate Sub SetLabelText_Delegate(ByVal [Label] As Label, ByVal [text] As String)

    ' The delegates subroutine.
    Private Sub SetLabelText_ThreadSafe(ByVal [Label] As Label, ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If [Label].InvokeRequired Then
            Dim MyDelegate As New SetLabelText_Delegate(AddressOf SetLabelText_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[Label], [text]})
        Else
            [Label].Text = [text]
        End If
    End Sub

    Delegate Sub SetListBox_Delegate(ByVal [ListBox] As ListBox, ByVal [text] As String)

    ' The delegates subroutine
    Private Sub SetListBox_ThreadSafe(ByVal [ListBox] As ListBox, ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If [ListBox].InvokeRequired Then
            Dim MyDelegate As New SetListBox_Delegate(AddressOf SetListBox_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[ListBox], [text]})
        Else
            [ListBox].Items.Add([text])
        End If
    End Sub

    Delegate Sub SetTreeView_Delegate(ByVal [TreeView] As TreeView, ByVal [text] As String)

    ' The delegates subroutine
    Private Sub SetTreeView_ThreadSafe(ByVal [TreeView] As TreeView, ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If [TreeView].InvokeRequired Then
            Dim MyDelegate As New SetTreeView_Delegate(AddressOf SetTreeView_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[TreeView], [text]})
        Else
            [TreeView].Nodes.Add([text])
        End If
    End Sub

    Delegate Sub SetTreeNode_Delegate(ByVal [TreeView] As TreeView, ByVal [Node] As Integer, ByVal [text] As String)

    ' The delegates subroutine
    Private Sub SetTreeNode_ThreadSafe(ByVal [TreeView] As TreeView, ByVal [Node] As Integer, ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If [TreeView].InvokeRequired Then
            Dim MyDelegate As New SetTreeNode_Delegate(AddressOf SetTreeNode_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[TreeView], [text]})
        Else
            [TreeView].Nodes([Node]).Nodes.Add([text])
        End If
    End Sub

    Delegate Sub SetTextBox_Delegate(ByVal [TextBox] As TextBox, ByVal [text] As String)

    ' The delegates subroutine.
    Private Sub SetTextBox_ThreadSafe(ByVal [TextBox] As TextBox, ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If [TextBox].InvokeRequired Then
            Dim MyDelegate As New SetTextBox_Delegate(AddressOf SetTextBox_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[TextBox], [text]})
        Else
            [TextBox].Text = [text]
        End If
    End Sub

    Delegate Sub SetLabel_Delegate(ByVal [Label] As Label, ByVal [text] As String)

    Private Sub SetLabel_ThreadSafe(ByVal [Label] As Label, ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If [Label].InvokeRequired Then
            Dim MyDelegate As New SetLabel_Delegate(AddressOf SetLabel_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[Label], [text]})
        Else
            [Label].Text = [text]
        End If
    End Sub

    Private Sub stopit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStop.Click
        AddtoLog("= STOP button pressed =")
        If Worker.IsBusy Then Worker.CancelAsync()
    End Sub

    Private Sub ViewSelectedFile()
        Dim snum As Integer
        Dim filext As String
        DisplayType = DisplayBoot
        Bootclass = ""
        txtBootName.ForeColor = Color.Black
        txtCFile.Text = OpenBootFile
        txtSel.Text = dgvBatch.CurrentCell.RowIndex & "/" & dgvBatch.Rows.Count
        snum = dgvBatch.SelectedRows(0).Cells(0).Value
        Array.Clear(IntBytePos, 0, 6)
        txtTrail.Text = ""
        ListBox2.TopIndex = ListBox2.SelectedIndex
        OpenBootFile = Files(snum)
        Ziprealname = OpenBootFile
        If File.Exists(OpenBootFile) = True Then
            Dim FileProps As FileInfo = New FileInfo(OpenBootFile)
            Call Disptype(DisplayType, OpenBootFile)
            filext = Path.GetExtension(OpenBootFile).ToUpper
            If filext = ".ADF" OrElse filext = ".ADZ" OrElse filext = ".DMS" Then
                BootDiskToolStripMenuItem.Enabled = True
                SaveADFFileToolStripMenuItem.Enabled = True
                Isadf = True
                InstallBootblockFromFileToolStripMenuItem.Enabled = True
            Else
                BootDiskToolStripMenuItem.Enabled = False
                SaveADFFileToolStripMenuItem.Enabled = False
                Isadf = False
                InstallBootblockFromFileToolStripMenuItem.Enabled = False
            End If
            tabBrain.SelectedTab = TabPage9
        End If
    End Sub

    Private Sub cmdView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdView.Click
        If Files.Length > 0 Then grpscan.Enabled = True
        picPreview.Image = Nothing
        If Worker.IsBusy Then Exit Sub
        ResetForm()
        ViewSelectedFile()
    End Sub

    'Public Sub PopulateBFBox()
    '    BrainTree.Nodes.Clear()
    '    Dim bnode As Xml.XmlNode
    '    Dim count As Integer = 0
    '    Dim y As Integer = 0
    '    Dim mycolor As Color
    '    Dim BrainNodelist As XmlNodeList = BrainXML.GetElementsByTagName("Bootblock")
    '    For Each bnode In BrainNodelist
    '        BrainTree.Nodes.Add(bnode.Item("Name").InnerText)
    '        BrainTree.Nodes(count).Nodes.Add("Bootable: " & bnode.Item("Bootable").InnerText)
    '        If BootCRC(count) = "" Then Else BrainTree.Nodes(count).Nodes.Add("CRC: " & bnode.Item("CRC").InnerText)
    '        If BNote(count) = "" Then Else BrainTree.Nodes(count).Nodes.Add("Notes: " & bnode.Item("Notes").InnerText)
    '        BrainTree.Nodes(count).Nodes.Add("KickStart Required: " & bnode.Item("KS").InnerText)
    '        BrainTree.Nodes(count).Nodes.Add("Disk Data Required: " & bnode.Item("Data").InnerText)
    '        For y = 0 To CatList.Length - 1
    '            If Isbtype(count).ToUpper = CatAb(y).ToUpper Then
    '                BrainTree.Nodes(count).Nodes.Add("Bootblock Type: " & CatList(y))
    '                mycolor = Color.FromName(catcolours(y))
    '            End If
    '        Next
    '        BrainTree.Nodes(count).ForeColor = mycolor
    '        count += 1
    '    Next

    'End Sub
    Public Sub PopulateBFBox()
        BrainTree.Nodes.Clear()
        Dim bnode As Xml.XmlNode
        Dim count As Integer = 0
        Dim y As Integer = 0
        Dim BrainNodelist As XmlNodeList = BrainXML.GetElementsByTagName("Bootblock")
        For Each bnode In BrainNodelist
            BrainTree.Nodes.Add(BBDatabase(count).Name)
            BrainTree.Nodes(count).Nodes.Add("Bootable: " & BBDatabase(count).Bootable)
            If BBDatabase(count).CRC = "" Then Else BrainTree.Nodes(count).Nodes.Add("CRC: " & BBDatabase(count).CRC)
            If BBDatabase(count).Note = "" Then Else BrainTree.Nodes(count).Nodes.Add("Notes: " & BBDatabase(count).Note)
            BrainTree.Nodes(count).Nodes.Add("Bootblock Type:" & BBDatabase(count).BootClass(abbnames, catnames))
            BrainTree.Nodes(count).Nodes.Add("KickStart Required: " & BBDatabase(count).KS)
            BrainTree.Nodes(count).Nodes.Add("Disk Data Required: " & BBDatabase(count).DataNeeded)
            BrainTree.Nodes(count).ForeColor = BBDatabase(count).BootColour()
            count += 1
        Next
    End Sub

    Private Sub cmdBSNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBSNext.Click
        Dim currrow As Integer
        Try
            currrow = dgvBatch.SelectedRows(0).Index
        Catch
            MessageBox.Show("At last entry")
            Exit Sub
        End Try

        If dgvBatch.CurrentCell.RowIndex = -1 Then Exit Sub
        If currrow = dgvBatch.Rows.Count - 1 Then Exit Sub
        dgvBatch.ClearSelection()
        If currrow + 1 > dgvBatch.Rows.Count - 1 Then Exit Sub
        dgvBatch.Rows(currrow + 1).Selected = True

        BBDisplay.ScrollBars = ScrollBars.None
        ViewSelectedFile()
    End Sub

    Private Sub cmdBSPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBSPrev.Click
        Dim currrow As Integer = dgvBatch.SelectedRows(0).Index
        If currrow = 0 Then Exit Sub
        If dgvBatch.Rows.Count = 0 Then Exit Sub
        If dgvBatch.CurrentCell.RowIndex = -1 Then Exit Sub
        dgvBatch.ClearSelection()
        dgvBatch.Rows(currrow - 1).Selected = True
        BBDisplay.ScrollBars = ScrollBars.None
        ViewSelectedFile()
    End Sub
    Private Sub Button1_Click_2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AsOriginalToolStripMenuItem.Click
        Dim runner As String
        If OpenBootFile = "" Then Exit Sub
        If File.Exists(OpenBootFile) = False Then
            If isdms OrElse isadz Then
                runner = dmsfile
            Else
                runner = Zipname
            End If
        Else
            runner = OpenBootFile
        End If
        rundisk(runner)
    End Sub

    Private Sub cmdTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestBootblockInBufferToolStripMenuItem.Click
        If OpenBootFile = "" Then Exit Sub
        Dim tempfile As String = Tempdrawer & "\abrtemp.adf"
        Dim tempadf(901120) As Byte
        Dim message As New StringBuilder
        tempadf = My.Resources.temp
        If BType = 1 Then tempadf = My.Resources.tempffs
        Array.Copy(Buffer, tempadf, 1024)
        message.AppendLine("ABR will now write the bootblock to a blank ADF")
        message.AppendLine("And will attempt to boot it in your amiga emulator")
        MessageBox.Show(message.ToString, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Try
            If File.Exists(tempfile) Then File.Delete(tempfile)
        Catch
            MessageBox.Show("Cant delete temp file")
        End Try
        AddtoLog("Writing " & tempfile)
        File.WriteAllBytes(tempfile, tempadf)
        rundisk(tempfile)
    End Sub

    Private Sub AttachPic(ByVal newfile As String)
        If Directory.Exists(My.Settings.UAEPath & "\screenshots") Then ofdPic.InitialDirectory = My.Settings.UAEPath & "\screenshots" Else ofdPic.InitialDirectory = ""
        Dim result As DialogResult
        result = ofdPic.ShowDialog()
        If result = DialogResult.Cancel Then Exit Sub
        If ofdPic.FileName = "" Then Exit Sub
        If File.Exists(ofdPic.FileName) Then
            newfile = newfile.Replace("/", "-")
            File.Copy(ofdPic.FileName, StartPath & "\bootpic\" & newfile & ofdPic.FileName, True)
        End If
        If BrainTree.SelectedNode.Index < 0 Then Exit Sub
        If File.Exists(StartPath & "\bootpic\" & newfile & ".png") Then
            bootpic.Visible = True
            bootpic.ImageLocation = StartPath & "\bootpic\" & newfile & ".png"
            bootpic.Load()
        Else
            bootpic.Visible = False
            bootpic.Image = Nothing
        End If
    End Sub

    Private Sub rundisk(bootdisk As String)
        Dim p As New Process
        Dim runner As New System.Diagnostics.ProcessStartInfo
        runner.FileName = txtUAE.Text
        If runner.FileName = "" Then
            MessageBox.Show("Please set Amiga emulator path to use this feature")
            Exit Sub
        End If
        If InStr(runner.FileName.ToUpper, "WINUAE") Then
            runner.Arguments = "-s use_gui=no -0 """ & bootdisk & ""
        Else
            runner.Arguments = bootdisk
        End If
        p.StartInfo = runner
        Try
            AddtoLog("Starting " & runner.FileName & " with arguments [" & runner.Arguments & "]")
            p.Start()
        Catch
            MessageBox.Show("An error occured running amiga emulator!")
        End Try
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRescan.Click
        If Worker.IsBusy = True Then Exit Sub
        cmdView.Enabled = False
        cmdOpenFR.Enabled = False
        Worker.CancelAsync()
        DisplayType = Scanner
        ToolStripProgressBar1.Value = 0
        dgvBatch.Rows.Clear()

        Worker.RunWorkerAsync()
    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles BrainTree.AfterSelect
        Dim bootimg As String
        Dim test(13) As String
        If BrainTree.SelectedNode.Index < 0 Then Exit Sub
        Dim loc As Integer
        Dim bloc As Integer = BFind(BrainTree.SelectedNode.Text)
        loc = bloc
        If loc = -1 Then Exit Sub
        bootimg = BBDatabase(loc).Name
        Dim x As Integer
        Dim ClassN As String = ""
        For x = 0 To CatList.Length - 1
            If BBDatabase(loc).BClass = CatList(x).Abb Then ClassN = CatList(x).Name
        Next
        Dim bootloc As String
        bootloc = FindinBBLib(BrainTree.SelectedNode.Text)
        If bootloc = "" Then
            Button7.Enabled = False
        Else
            Button7.Enabled = True
        End If
        Try
            If BrainTree.SelectedNode.Text.Contains(bootimg) Then
                'Show boot picture
                If InStr(bootimg, "/") Then
                    bootimg = bootimg.Replace("/", "-")
                End If
                Dim intpictype As Integer = 0
                If File.Exists(StartPath & "\bootpic\" & bootimg & ".png") Then intpictype = 1
                If File.Exists(StartPath & "\bootpic\" & bootimg & ".gif") Then intpictype = 2
                Select Case intpictype
                    Case 1
                        bootpic.ImageLocation = StartPath & "\bootpic\" & bootimg & ".png"
                        bootpic.Load()
                    Case 2
                        bootpic.ImageLocation = StartPath & "\bootpic\" & bootimg & ".gif"
                        bootpic.Load()
                    Case 0
                        bootpic.Image = My.Resources.Nopic
                End Select
                grpRec.Visible = False
                'Populte recog boxes and show selections
                If Recogline(loc).Contains("0,0,0,0") Then Exit Sub
                test = Split(Recogline(loc), ",")
                Pos1.Text = test(0)
                BVal1.Text = Chr(test(1))
                Pos2.Text = test(2)
                BVal2.Text = Chr(test(3))
                Pos3.Text = test(4)
                BVal3.Text = Chr(test(5))
                Pos4.Text = test(6)
                BVal4.Text = Chr(test(7))
                Pos5.Text = test(8)
                BVal5.Text = Chr(test(9))
                Pos6.Text = test(10)
                BVal6.Text = Chr(test(11))
                Pos7.Text = test(12)
                BVal7.Text = Chr(test(13))
            End If
        Catch
            'If you Then just deleted something this will occur
        End Try

    End Sub

    Private Sub sfdInstall_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles sfdInstall.FileOk
        Try
            File.WriteAllBytes(sfdInstall.FileName, Editbuffer)
        Catch
            MessageBox.Show("Cant write file!!!!", "Write failure")
        End Try
    End Sub
    Public Function FindinBBLib(ByVal s As String) As String
        Dim x As Integer
        Dim fnd As String = ""
        For x = 0 To sbfiles.Length - 1
            If InStr(sbfiles(x), s.Replace(" [S]", "")) Then
                fnd = sbfiles(x)
                Exit For
            End If
        Next
        Return fnd
    End Function

    Private Sub cmdGotoBrain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGotoBrain.Click
        tabBrain.SelectTab(3)
        BrainTree.SelectedNode = FindinBFPage(BBDatabase(Boots).Name)
    End Sub

    Private Sub OpenBrain() Handles ofdNewBrain.FileOk
        Brainpath = ofdNewBrain.FileName
        ReadBrain()

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles cmdSaveSettings.Click
        My.Settings.BBlocks = txtSBoots.Text
        My.Settings.ADFs = txtADFs.Text
        My.Settings.Save()
    End Sub

    Private Sub txtADFs_TextChanged(sender As Object, e As EventArgs) Handles txtADFs.TextChanged
        cmdSaveSettings.ForeColor = Color.Red
    End Sub


    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles OpenNewFileToolStripMenuItem.Click
        ofdBootOpen.InitialDirectory = LastDrawer
        ofdBootOpen.ShowDialog()
        LastDrawer = ofdBootOpen.FileName.Substring(0, ofdBootOpen.FileName.Length - ofdBootOpen.SafeFileName.Length)
        My.Settings.LastDrawer = LastDrawer
        My.Settings.Save()
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button31.Click
        tabBrain.SelectTab(0)
        Disptype(2, ofdReader.FileName)
    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        txtReadOffset.Text = Val(txtReadOffset.Text) + 100
        FReader()
    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        txtReadOffset.Text = Val(txtReadOffset.Text) - 100
        FReader()
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkZip.CheckedChanged
        If chkZip.Checked = True Then chkNoZip.Checked = False
        If chkZip.Checked = False Then chkNoZip.Checked = True
    End Sub

    Private Sub FReader()
        If Iszip Then
            MessageBox.Show("File is a ZIP file - invalid file for reading")
            Exit Sub
        End If
        If isdms Then
            MessageBox.Show("File is a DMS File - invalid file for reading")
            Exit Sub
        End If
        Dim fbdopen As String
        Dim OpenFileName As String = ""
        If Fread Then
            fbdopen = OpenBootFile
        Else
            fbdopen = ofdReader.FileName

        End If
        TextBox1.Text = OpenBootFile
        Dim i As Integer = 0
        Dim chars As Char()  'Char array to store bootblock to print to boot display textbox
        chars = New Char(1024) {}

        Dim a As Integer = 0
        If Val(txtReadOffset.Text) < 0 Then txtReadOffset.Text = 0
        Dim bootpos As Integer = Val(txtReadOffset.Text)

        Dim x As Integer
        If File.Exists(fbdopen) Then
            TextBox1.Text = fbdopen
            Try
                Using fs As New FileStream(fbdopen, FileMode.Open, FileAccess.Read)
                    Dim dinput As New BinaryReader(fs)
                    If bootpos > fs.Length Then
                        MessageBox.Show("End of file reached")
                        Exit Sub
                    End If
                    fs.Position = bootpos
                    txtBytePos.Text = bootpos & "/" & fs.Length
                    If fs.Length < 1025 Then
                        HScrollBar1.Enabled = False
                    Else
                        HScrollBar1.Enabled = True
                    End If
                    HScrollBar1.Maximum = fs.Length
                    lblFSize.Text = fs.Length & " bytes"
                    Fbuffer = dinput.ReadBytes(1024)
                End Using
            Catch
                MessageBox.Show("Cant Open File!!!!")
            End Try
        End If

        Dim bufflen As Integer
        chars = ReadCP.GetChars(Fbuffer)
        bufflen = chars.Length - 1
        Dim bootline As String
        Dim count As Integer = 0
        Dim str As New StringBuilder
        Dim isDone As Boolean = False
        For i = 0 To 16
            bootline = ""
            If isDone = True Then Exit For
            For x = 0 To 63
                If (count + x) > (bufflen) Then
                    bootline &= "."
                ElseIf Asc(chars(count + x)) = 0 Then
                    bootline &= "."
                ElseIf Asc(chars(count + x)) = 9 Then
                    bootline &= " "
                ElseIf Asc(chars(count + x)) = 10 Then
                    bootline &= "?"
                ElseIf Asc(chars(count + x)) = 7 Then
                    bootline &= "?"
                ElseIf Asc(chars(count + x)) = 13 Then
                    bootline &= "?"
                Else
                    bootline &= chars(count + x)
                End If
            Next
            str.AppendLine(bootline)
            count += 64
        Next
        txtFDisp.Text = str.ToString
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button27.Click
        txtReadOffset.Text = 0
        If txtCSearch.Text = "" Then
            MessageBox.Show("Nothing in custom search box!!")
            Exit Sub
        End If
        SearchFile(ofdReader.FileName, txtCSearch.Text)
    End Sub

    Private Sub SearchFile(ByVal bbfile As String, ByVal searchword As String)
        cbxLoc.Items.Clear()
        Dim filebuffer() As Byte
        Try
            Dim fs As New FileStream(bbfile, FileMode.Open, FileAccess.Read)
            Dim dinput As New BinaryReader(fs)
            fs.Position = 0
            filebuffer = dinput.ReadBytes(fs.Length)
            dinput.Close()
        Catch ex As Exception
            MessageBox.Show("Cant open file!")
            Exit Sub
        End Try
        Dim parsestring() As Char
        parsestring = searchword.ToCharArray
        Dim bytestring(searchword.Length - 1) As Byte
        Dim x As Integer
        Dim i As Integer = 0
        Dim a As Integer = 0
        lblFSize.Text = filebuffer.Length
        HScrollBar1.Maximum = filebuffer.Length
        Dim stloc As Integer = 0
        Dim found As Boolean = False
        Dim exitnow As Boolean = False
        x = 0
        For x = 0 To parsestring.Length - 1
            bytestring(x) = Convert.ToByte(parsestring(x))
        Next
        Dim endfile As Boolean
        For x = 0 To filebuffer.Length - 1
            a = 0
            If (x + bytestring.Length) > filebuffer.Length - 1 Then
                endfile = True
                Exit For
            End If
            For i = 0 To bytestring.Length - 1
                If filebuffer(x + i) = bytestring(i) Then a += 1
            Next
            If a = bytestring.Length Then found = True
            If found = True Then
                cbxLoc.Items.Add(x + i - bytestring.Length & "-" & x + i)
                FoundLoc(stloc) = x + i - bytestring.Length
                stloc += 1
                found = False
                a = 0
                IsKnown = True
            End If
            If endfile = True Then Exit For
        Next
        If cbxLoc.Items.Count = 0 Then
            txtReadOffset.Text = 0
            MessageBox.Show("Search string not found!")
            Exit Sub
        End If
        txtReadOffset.Text = FoundLoc(0)
        cbxLoc.SelectedIndex = 0
        FReader()
    End Sub

    Private Sub cmdFOpen_Click_1(sender As Object, e As EventArgs) Handles OpenFileInBootblockRipperToolStripMenuItem.Click
        Dim result As DialogResult
        txtReadOffset.Text = 0
        result = ofdReader.ShowDialog()
        If result = DialogResult.OK Then
            Fread = False
            TextBox1.Text = ofdReader.FileName
            FReader()
            tabBrain.SelectTab(3)
        Else
        End If
    End Sub

    Private Sub cmdAddTen_Click(sender As Object, e As EventArgs) Handles cmdAddTen.Click
        txtReadOffset.Text = Val(txtReadOffset.Text) + 10
        FReader()
    End Sub

    Private Sub cmdTakeTen_Click(sender As Object, e As EventArgs) Handles cmdTakeTen.Click
        txtReadOffset.Text = Val(txtReadOffset.Text) - 10
        FReader()
    End Sub

    Private Sub cmdDOSSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDOSSearch.Click
        If ofdReader.FileName = "" Then
            SearchFile(OpenBootFile, "DOS")
        Else
            SearchFile(ofdReader.FileName, "DOS")
        End If

    End Sub

    Private Sub cmdFS2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFS2.Click
        If cbxLoc.SelectedIndex < 1 Then Exit Sub
        cbxLoc.SelectedIndex = cbxLoc.SelectedIndex - 1
    End Sub

    Private Sub CMDfs3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMDfs3.Click
        If cbxLoc.SelectedIndex = cbxLoc.Items.Count - 1 Then Exit Sub
        cbxLoc.SelectedIndex = cbxLoc.SelectedIndex + 1
    End Sub

    Private Sub cmdFS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFS.Click
        If cbxLoc.Items.Count = 0 Then Exit Sub
        cbxLoc.SelectedIndex = 0
    End Sub

    Private Sub cmdFS4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFS4.Click
        If cbxLoc.Items.Count = 0 Then Exit Sub
        cbxLoc.SelectedIndex = cbxLoc.Items.Count - 1
    End Sub

    Private Sub cmdLibSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLibSearch.Click
        SearchFile(ofdReader.FileName, "dos.library")
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        txtReadOffset.Text = Val(txtReadOffset.Text) - 1
        FReader()
    End Sub

    Private Sub Button16_Click_1(sender As Object, e As EventArgs) Handles Button16.Click
        txtReadOffset.Text = Val(txtReadOffset.Text) + 1
        FReader()
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        txtReadOffset.Text = Val(txtReadOffset.Text) + 1000
        FReader()
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        txtReadOffset.Text = Val(txtReadOffset.Text) - 1000
        FReader()
    End Sub

    Private Sub Button30_Click(sender As Object, e As EventArgs) Handles Button30.Click
        txtReadOffset.Text = 0
        FReader()

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxLoc.SelectedIndexChanged
        txtReadOffset.Text = FoundLoc(cbxLoc.SelectedIndex)
        FReader()
    End Sub

    Private Sub cmdReRead_Click(sender As Object, e As EventArgs) Handles cmdReRead.Click
        FReader()
    End Sub

    Private Sub cmdOpenFR_Click(sender As Object, e As EventArgs) Handles cmdOpenFR.Click
        Dim snum As Integer
        Fread = True
        txtSel.Text = dgvBatch.CurrentRow.Cells(0).Value & "/" & dgvBatch.Rows.Count
        snum = dgvBatch.CurrentRow.Cells(0).Value
        txtReadOffset.Text = 0
        OpenBootFile = Files(snum)
        ofdReader.FileName = OpenBootFile
        FReader()
        tabBrain.SelectedIndex = 4
    End Sub
    Private Sub cmdButton5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ofdReader.FileName = OpenBootFile
        FReader()
        tabBrain.SelectedIndex = 3
    End Sub

    Private Sub trvBatch_AfterSelect() Handles dgvBatch.CellContentDoubleClick
        Dim bbfile As String
        If Files.Length = 0 Then Exit Sub
        bbfile = Files(dgvBatch.CurrentRow.Cells(0).Value)
        If Worker.IsBusy = False Then
            cmdView.Enabled = True
            cmdOpenFR.Enabled = True
            grpscan.Enabled = True
            txtCFile.Text = bbfile
        Else
            cmdView.Enabled = False
            cmdOpenFR.Enabled = False
            grpscan.Enabled = False
        End If

        If File.Exists(bbfile) = True Then
            Dim FileProps As FileInfo = New FileInfo(bbfile)
            Dim AType As String = FileProps.Extension
            Dim ASize As String = FileProps.Length
            Select Case FileProps.Extension.ToUpper
                Case ".ADF"
                    AType = "Amiga Disk File (.ADF)"
                Case ".DMS"
                    AType = "Amiga Disk Masher System (.DMS)"
                Case ".ADZ"
                    AType = "GZipped Amiga Disk File (.ADZ)"
                Case ".ZIP"
                    AType = "Zip archive file"
                Case ".EXE"
                    AType = "DOS / Windows Executable File"
                Case ".ABB"
                    AType = "Amiga Bootblock File"
                Case ".ABK"
                    AType = "Amiga Bootblock File"
                Case ".INFO"
                    AType = "Amiga Icon file"
            End Select
            If FileProps.Length = 901120 Then
                ASize = "901120 bytes (880KB)"
            ElseIf FileProps.Length = 1024 Then
                ASize = "1024 bytes (1KB)"
            Else
                ASize = FileProps.Length & " bytes"
            End If
            Dim note As New StringBuilder
            note.AppendLine("File extension: " & AType)
            note.AppendLine("File size: " & ASize)
            If FileProps.IsReadOnly Then note.AppendLine("File is read-only")
            If AType = "Amiga Disk Masher System (.DMS)" Then
                ViewDMSInfo(bbfile)
                note.AppendLine("DMS Information:")
                If My.Settings.XDMS = "" Or chkDMS.Checked = False Then
                    note.AppendLine("DMS scanning off / XDMS not configured")
                Else
                    Dim x As Integer
                    For x = 1 To DMSInfo.Length - 1
                        note.AppendLine(DMSInfo(x))
                    Next
                End If
            End If
            MessageBox.Show(note.ToString)
        End If

    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        Dim bblist() As String
        Dim i As Integer
        Try
            bblist = Directory.GetFileSystemEntries(StartPath & "\Unknown")
            For i = 0 To bblist.Length - 1
                File.Delete(bblist(i))
            Next
        Catch
            Exit Sub
        End Try
        MessageBox.Show(bblist.Length & " bootblocks in Unknown drawer deleted")
    End Sub

    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles ScanUnknownDrawerToolStripMenuItem.Click
        dgvBatch.Columns(3).Visible = False
        dgvBatch.Columns(5).Visible = False
        chkSUnk.Checked = False
        If Worker.IsBusy Then Exit Sub
        tabBrain.SelectTab(1)
        If Directory.Exists(StartPath & "\Unknown") Then

            fbdOpen.SelectedPath = StartPath & "\Unknown"
            If Worker.IsBusy Then Worker.CancelAsync()
            DisplayType = Scanner
            ToolStripProgressBar1.Value = 0
            Worker.RunWorkerAsync()
        Else
            Directory.CreateDirectory(StartPath & "\Unknown")
            MessageBox.Show("Click 'Create Unknown drawer' in options to populate this drawer")
        End If
    End Sub

    Private Sub HScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles HScrollBar1.Scroll
        txtReadOffset.Text = HScrollBar1.Value
        FReader()
    End Sub

    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        If IsKnown = True Then
            Try
                If File.Exists(txtSBoots.Text & "\" & txtBootName.Text) Then
                    MessageBox.Show("Bootblock already saved")
                    Exit Sub
                End If
                File.WriteAllBytes(txtSBoots.Text & "\" & txtBootName.Text, Buffer)
                MessageBox.Show("Bootblock saved in " & txtSBoots.Text & "\" & txtBootName.Text)
                ReadBBlib(My.Settings.BBlocks)
            Catch ex As Exception
                MessageBox.Show("Cant write file!! " & txtSBoots.Text & "\" & txtBootName.Text)
            End Try
        Else
            SaveUnkinBB.Show()
        End If
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        Dim delfile As String = TextBox7.Text & "\" & trvBBCollection.SelectedNode.Text
        Dim result As DialogResult
        If File.Exists(delfile) Then
            Try
                result = MessageBox.Show("Really delete bootblock? " & vbCr & delfile, "Delete bootblock?", MessageBoxButtons.OKCancel)
                If result = DialogResult.OK Then
                    File.Delete(delfile)
                Else
                    Exit Sub
                End If
            Catch ex As Exception
                MessageBox.Show("Could not delete file!! " & delfile)
            End Try
        End If
        PictureBox5.Image = Nothing
        ReadBBlib(TextBox7.Text)
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles ScanBootblockCollectionToolStripMenuItem.Click
        dgvBatch.Columns(3).Visible = True
        If chkBMT.Checked = True Then dgvBatch.Columns(5).Visible = True
        If Worker.IsBusy Then Exit Sub
        tabBrain.SelectTab(1)
        If Directory.Exists(txtSBoots.Text) Then
            chkCreate.Checked = False
            fbdOpen.SelectedPath = txtSBoots.Text
            If Worker.IsBusy Then Worker.CancelAsync()
            DisplayType = Scanner
            ToolStripProgressBar1.Value = 0
            Worker.RunWorkerAsync()
        Else
            Directory.CreateDirectory(txtSBoots.Text)
            MessageBox.Show("Tick 'Create / Update Bootblock Drawer' to populate this drawer")
        End If
    End Sub




    Private Function FindinBrain(ByVal s As String) As Boolean
        Dim x As Integer
        Dim f As Boolean = False
        For x = 0 To BrainTree.Nodes.Count - 1
            If BrainTree.Nodes(x).Text = s Then f = True
        Next
        Return f
    End Function

    Public Function BFind(ByVal s As String) As Integer
        Dim x As Integer
        Dim f As Integer
        For x = 0 To BBDatabase.Length - 1
            If BBDatabase(x).Name = s Then
                f = x
                Exit For
            Else
                f = -1
            End If
        Next
        Return f
    End Function

    Public Function FindinBFPage(ByVal s As String) As TreeNode
        Dim i As Integer
        Dim tr As TreeNode = Nothing
        For i = 0 To BrainTree.Nodes.Count - 1
            If BrainTree.Nodes(i).Text = s Then tr = BrainTree.Nodes(i)

        Next
        Return tr
    End Function

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        If Formloaded = False Then Exit Sub
        If txtBootName.Text = "" Then Exit Sub
        RefreshDisplay()
    End Sub

    Private Sub bootpic_Click(sender As Object, e As EventArgs) Handles bootpic.Click
        If Nopic = True Then Call AttachPic(BrainTree.SelectedNode.Text)
    End Sub

    Private Sub RunBB(ByVal s As String)
        Disptype(DisplayType, s)
        If OpenBootFile = "" Then Exit Sub
        Dim tempfile As String = Tempdrawer & "\abrtemp.adf"
        Dim tempadf(901120) As Byte
        Dim message As New StringBuilder
        tempadf = My.Resources.temp
        If BType = 1 Then tempadf = My.Resources.tempffs
        Dim tempboot(901120) As Byte
        tempboot = tempadf
        Dim i As Integer
        For i = 0 To Curboot.Length - 1
            tempboot(i) = Curboot(i)
        Next
        message.AppendLine("ABR will now write the bootblock to a blank ADF")
        message.AppendLine("And attempt to boot it in your chosen Amiga emulator")
        MessageBox.Show(message.ToString, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Try
            If File.Exists(tempfile) Then File.Delete(tempfile)
        Catch
            MessageBox.Show("Cant delete temp file")
        End Try
        File.WriteAllBytes(tempfile, tempboot)
        rundisk(tempfile)
    End Sub

    Private Sub cmdUAE_Click(sender As Object, e As EventArgs) Handles cmdUAE.Click
        Dim result As DialogResult
        ofdUAE.FileName = ""
        result = ofdUAE.ShowDialog()
        If result = DialogResult.OK AndAlso ofdUAE.FileName IsNot Nothing Then
            Dim starttext As String = "Unknown Emulator"
            If InStr(ofdUAE.FileName.ToUpper, ".EXE") Then txtUAE.Text = ofdUAE.FileName
            My.Settings.UAEPath = txtUAE.Text
            If InStr(txtUAE.Text.ToUpper, "FS-UAE") Then starttext = "FS-UAE"
            If InStr(txtUAE.Text.ToUpper, "WINUAE") Then starttext = "WinUAE (32/64)"
            If InStr(txtUAE.Text.ToUpper, "WINFELLOW") Then
                MessageBox.Show("WinFellow doesnt have a command line interface to make ABR function work")
                Exit Sub
            End If
            Label71.Text = starttext & " " & GetFileVersionInfo(txtUAE.Text).ToString & " used"
        End If
    End Sub

    Private Function GetFileVersionInfo(ByVal filename As String) As Version
        Return Version.Parse(FileVersionInfo.GetVersionInfo(filename).FileVersion)
    End Function
    Private Sub Button7_Click_1(sender As Object, e As EventArgs) Handles Button7.Click
        Dim bootloc As String
        bootloc = FindinBBLib(BrainTree.SelectedNode.Text)
        RunBB(bootloc)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        RunBB(TextBox7.Text & "\" & trvBBCollection.SelectedNode.Text)
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        txtUAE.Text = ""
        My.Settings.UAEPath = txtUAE.Text
        Label71.Text = "No emulator set"
    End Sub

    Private Sub AboutABRToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutABRToolStripMenuItem.Click
        AboutBox1.Show()
    End Sub

    Private Sub BootblockBufferToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BootblockBufferToolStripMenuItem.Click
        tabBrain.SelectTab(0)
    End Sub

    Private Sub ExitABRToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitABRToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub cmdCycle_Click(sender As Object, e As EventArgs) Handles cmdCycle.Click
        Dim maxlimit As Integer = ListBox2.Items.Count - 1
        If ListBox2.SelectedIndex = maxlimit Then
            ListBox2.SelectedIndex = 0
        Else
            ListBox2.SelectedIndex += 1
            Exit Sub
        End If

    End Sub

    Private Sub DirectoryScannerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DirectoryScannerToolStripMenuItem.Click
        tabBrain.SelectTab(1)
    End Sub

    Private Sub chkZip_Click(sender As Object, e As EventArgs) Handles chkZip.Click
        My.Settings.ProcessZips = chkZip.Checked
    End Sub

    Private Sub trvCategories_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles trvCategories.AfterSelect
        ListBox3.Items.Clear()
        catselect = True
        TextBox3.Text = trvCategories.SelectedNode.Index
        txtCat.Text = CatList(trvCategories.SelectedNode.Index).Name
        txtCCol.Text = CatList(trvCategories.SelectedNode.Index).ColorName
        txtCCol.BackColor = CatList(trvCategories.SelectedNode.Index).Color
        txtAbbrev.Text = CatList(trvCategories.SelectedNode.Index).Abb
        Dim x As Integer
    End Sub

    Private Sub ScanNewDrawerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScanNewDrawerToolStripMenuItem.Click
        dgvBatch.Columns(3).Visible = True
        If chkBMT.Checked = True Then dgvBatch.Columns(5).Visible = True
        BatchScan.Show()
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click

        ZoomBoot.Show()
    End Sub

    Private Sub chkCreate_Click_1(sender As Object, e As EventArgs) Handles chkCreate.Click
        My.Settings.CreateBoots = chkCreate.Checked
        My.Settings.Save()
    End Sub

    Private Sub chkDMS_Click(sender As Object, e As EventArgs) Handles chkDMS.Click
        My.Settings.ProcessDMS = chkDMS.Checked
    End Sub

    Private Sub chkFast_Click(sender As Object, e As EventArgs)
        Dim strm As New StringBuilder
        strm.AppendLine("WARNING")
        strm.AppendLine("Enabling this option will disable the Thread sleep function")
        strm.AppendLine("needed for correct gui updating (Batch scanner colours)")
        strm.AppendLine("The result will be faster scanning, but the colours *MAY* ")
        strm.AppendLine("not be present in the scanner window")
    End Sub


    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        TestBootblockInBufferToolStripMenuItem.Enabled = True
        Disptype(2, TextBox7.Text & "\" & trvBBCollection.SelectedNode.Text & ".abb")
        tabBrain.SelectTab(0)
    End Sub

    Private Sub InstallBootToDisk(ByVal s As String)
        If isadz Then
            MessageBox.Show("Cant install bootblock onto .ADZ compressed disk")
            Exit Sub
        ElseIf Iszip Then
            MessageBox.Show("Cant install bootblock onto Zipped disk, unpack disk first")
            Exit Sub
        ElseIf isdms Then
            MessageBox.Show("Cant write bootblock to DMS File - unzip disk first")
            Exit Sub
        End If

        Dim boot(1024) As Byte
        Select Case s
            Case "std"
                boot = My.Resources.stdboot1x
            Case "ffs"
                boot = My.Resources.stdboot20ffs
            Case "ofs"
                boot = My.Resources.stdboot20ofs
            Case "ffsint"
                boot = My.Resources.stdboot20ffsintl
            Case "ffsdc"
                boot = My.Resources.stdboot30ffsdir
            Case "clear"
                boot = My.Resources.stdclear
            Case Else
                boot = File.ReadAllBytes(s)
        End Select
        Dim i As Integer
        Try
            Dim adf As New BinaryReader(File.OpenRead(OpenBootFile))
            Adfbuffer = adf.ReadBytes(901120)
            adf.Close()
        Catch
        End Try
        For i = 0 To 1024
            Adfbuffer(i) = 0
        Next
        For i = 0 To boot.Length - 1
            Adfbuffer(i) = boot(i)
        Next

        Try
            File.WriteAllBytes(OpenBootFile, Adfbuffer)

        Catch
            Dim fi As New FileInfo(OpenBootFile)
            If fi.IsReadOnly = True Then
                MessageBox.Show("Cant write to disk - file is read-only")
            Else
                MessageBox.Show("ADF file is in use / Cannot be accessed")
            End If
            Exit Sub
        End Try
        Disptype(2, OpenBootFile)
        MessageBox.Show("Standard Bootblock installed on " & OpenBootFile)
    End Sub

    Private Sub Button13_Click_1(sender As Object, e As EventArgs) Handles Standard13BootblockToolStripMenuItem.Click
        InstallBootToDisk("std")
    End Sub

    Private Sub Button24_Click_1(sender As Object, e As EventArgs) Handles Standard20FFSBootblockToolStripMenuItem.Click
        InstallBootToDisk("ffs")
    End Sub

    Private Sub Button29_Click(sender As Object, e As EventArgs) Handles Standard20FFSBootblockToolStripMenuItem.Click
        InstallBootToDisk("ffsint")
    End Sub

    Private Sub Button32_Click(sender As Object, e As EventArgs) Handles Standard30FFSIntDirCacheToolStripMenuItem.Click
        InstallBootToDisk("ffsdc")
    End Sub

    Private Sub Button34_Click(sender As Object, e As EventArgs) Handles ClearBootblocknonbootableToolStripMenuItem.Click
        InstallBootToDisk("clear")
    End Sub

    Private Sub chkSDMS_Click(sender As Object, e As EventArgs) Handles chkSDMS.CheckedChanged
        My.Settings.SupDMS = chkSDMS.Checked
    End Sub

    Private Sub TreeView1_AfterSelect_1(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        Dim BBNum As Integer
        Dim y As Integer
        For y = 0 To BBE.Length - 2
            If BBE(y).Name = TreeView1.SelectedNode.Text Then BBNum = y
        Next
        TextBox6.Text = BBE(BBNum).Note
        If File.Exists(StartPath & "\encpic\" & BBE(BBNum).ID & ".png") Then
            PictureBox2.ImageLocation = StartPath & "\encpic\" & BBE(BBNum).ID & ".png"
            PictureBox2.Load()
        Else
            PictureBox2.Image = Nothing
        End If
    End Sub

    Private Sub PopInfo()
        trvinfo.Nodes.Clear()
        Dim x As Integer
        Dim y As Integer
        Dim catcount As Integer = 0
        trvinfo.Nodes.Add("")
        trvinfo.Nodes.Add("  Main Brainfile information")
        trvinfo.Nodes.Add("  ~~~~~~~~~~~~~~~~~~~~~~~~~~")
        For x = 0 To CatList.Length - 1
            catcount = 0
            For y = 0 To BBDatabase.Length - 2
                If BBDatabase(y).BClass = CatList(x).Abb Then catcount += 1
            Next
            trvinfo.Nodes.Add(String.Format("{0,-40} {1,-30}", "  " & CatList(x).Name, catcount & " bootblocks"))
            trvinfo.Nodes(trvinfo.Nodes.Count - 1).ForeColor = CatList(x).Color
        Next
        trvinfo.Nodes.Add("")
        trvinfo.Nodes.Add("  Search Brainfile information")
        trvinfo.Nodes.Add("  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
        For x = 0 To CatList.Length - 1
            catcount = 0
            For y = 0 To SearchDatabase.Length - 2
                If SearchDatabase(y).BClass = CatList(x).Abb Then catcount += 1
            Next
            If catcount > 0 Then
                trvinfo.Nodes.Add(String.Format("{0,-40} {1,-30}", "  " & CatList(x).Name, catcount & " bootblocks"))
                trvinfo.Nodes(trvinfo.Nodes.Count - 1).ForeColor = CatList(x).Color
            End If
        Next
    End Sub

    Private Sub chkTZIP_CheckedChanged(sender As Object, e As EventArgs) Handles chkTZIP.CheckedChanged
        My.Settings.ADZZIP = chkTZIP.Checked
    End Sub

    Private Sub trvBBCollection_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles trvBBCollection.AfterSelect
        If Directory.Exists(trvBBCollection.SelectedNode.Text) Then
            ReadBBlib(trvBBCollection.SelectedNode.Text)
        Else
            If File.Exists(StartPath & "\bootpic\" & trvBBCollection.SelectedNode.Text & ".png") Then
                PictureBox5.ImageLocation = StartPath & "\bootpic\" & trvBBCollection.SelectedNode.Text & ".png"
                PictureBox5.Load()

            Else
                PictureBox5.ImageLocation = Nothing

            End If
        End If
    End Sub

    Private Sub Button28_Click(sender As Object, e As EventArgs) Handles Button28.Click
        ReadBBlib(My.Settings.BBlocks)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim result As DialogResult
        result = sfdLog.ShowDialog
        If result = DialogResult.OK Then
            Try
                File.WriteAllText(sfdLog.FileName, logfile.ToString)
            Catch ex As Exception
                MessageBox.Show("Cant write file!! Please select another path")
                Exit Sub
            End Try
            MessageBox.Show("Log file saved")
        End If
    End Sub

    Private Sub chkReal_CheckedChanged(sender As Object, e As EventArgs) Handles chkReal.CheckedChanged
        My.Settings.RealLog = chkReal.Checked
    End Sub

    Private Sub chkLog_Click(sender As Object, e As EventArgs) Handles chkLog.Click
        My.Settings.EnableLog = chkLog.Checked
        My.Settings.Save()
    End Sub



    Private Sub chkIFE_CheckedChanged(sender As Object, e As EventArgs) Handles chkIFE.CheckedChanged
        My.Settings.ScanALL = chkIFE.Checked
        My.Settings.Save()
    End Sub

    Private Sub sfdBootSave_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles sfdBootSave.FileOk
        Try
            File.WriteAllBytes(sfdBootSave.FileName, Curboot)
        Catch
            MessageBox.Show("Cant write file!!")
        End Try
    End Sub




    Private Sub SaveADFFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveADFFileToolStripMenuItem.Click
        Dim intresult As DialogResult
        sfdADF.DefaultExt = "adf"
        If isdms Then
            If chkDMS.Checked = False Then
                MessageBox.Show("DMS Processing is off - saving copy of .dms")
                sfdADF.DefaultExt = "dms"
            End If
            sfdADF.FileName = Path.GetFileNameWithoutExtension(dmsfile)
        ElseIf isadz Then
            sfdADF.FileName = Path.GetFileNameWithoutExtension(adzfile)
        Else
            sfdADF.FileName = Path.GetFileName(OpenBootFile)
        End If
        Adfbuffer = File.ReadAllBytes(OpenBootFile)
        intresult = sfdADF.ShowDialog()
        If intresult = DialogResult.OK Then
            File.WriteAllBytes(sfdADF.FileName, Adfbuffer)
        End If
    End Sub

    Private Sub WithKS13OFSBootblockToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WithKS13OFSBootblockToolStripMenuItem.Click
        If OpenBootFile = "" Then Exit Sub
        Dim tempfile As String = Tempdrawer & "\abrtemp.adf"
        Dim tempadf(901120) As Byte
        tempadf = Adfbuffer
        Dim ofsboot() As Byte = My.Resources.stdboot1x
        Dim message As New StringBuilder
        Array.Copy(ofsboot, tempadf, 1024)
        message.AppendLine("ABR will now write the bootblock to the ADF in buffer")
        message.AppendLine("And will attempt to boot it in your amiga emulator")
        MessageBox.Show(message.ToString, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Try
            If File.Exists(tempfile) Then File.Delete(tempfile)
        Catch
            MessageBox.Show("Can't delete temp file")
        End Try
        AddtoLog("Writing " & tempfile)
        File.WriteAllBytes(tempfile, tempadf)
        rundisk(tempfile)
    End Sub

    Private Sub WithKS20FFSBootblockToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WithKS20FFSBootblockToolStripMenuItem.Click
        If OpenBootFile = "" Then Exit Sub
        Dim tempfile As String = Tempdrawer & "\abrtemp.adf"
        Dim tempadf(901120) As Byte
        Dim ofsboot() As Byte = My.Resources.stdboot20ffs
        Dim message As New StringBuilder

        tempadf = Adfbuffer
        Array.Copy(ofsboot, tempadf, 1024)
        message.AppendLine("ABR will now write the bootblock to the ADF in buffer")
        message.AppendLine("And will attempt to boot it in your amiga emulator")
        MessageBox.Show(message.ToString, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Try
            If File.Exists(tempfile) Then File.Delete(tempfile)
        Catch
            MessageBox.Show("Can't delete temp file")
        End Try
        AddtoLog("Writing " & tempfile)
        File.WriteAllBytes(tempfile, tempadf)
        rundisk(tempfile)
    End Sub

    Private Sub lbxFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbxFilter.SelectedIndexChanged
        cmdRescan.PerformClick()
    End Sub

    Private Sub Button13_Click_2(sender As Object, e As EventArgs) Handles Button13.Click
        If TextBox10.Text = "" Then Exit Sub
        Dim parentpath As String = Path.GetDirectoryName(TextBox10.Text)
        If parentpath = Environment.GetEnvironmentVariable("SystemDrive") & "\" Then Exit Sub
        fbdOpen.SelectedPath = parentpath
        TextBox10.Text = parentpath
        cmdRescan.PerformClick()
    End Sub

    Private Sub BrowseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BrowseToolStripMenuItem.Click
        Dim result As DialogResult
        result = ofdExtBrain.ShowDialog()
        If result = DialogResult.OK Then
            If File.Exists(ofdExtBrain.FileName) = False Then Exit Sub
            DefaultBrainPath = ofdExtBrain.FileName
            TextBox12.Text = "Using external brainfile " & DefaultBrainPath
            ReadBrain()
            MessageBox.Show("External brainfile " & DefaultBrainPath & " loaded into memory")
            OverwriteDefaultBrainfileToolStripMenuItem.Enabled = True
            UseDefaultToolStripMenuItem.Enabled = True
        End If
        PopInfo()
    End Sub

    Private Sub UseExternalBrainfileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UseExternalBrainfileToolStripMenuItem.Click

    End Sub

    Private Sub UseDefaultToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UseDefaultToolStripMenuItem.Click
        DefaultBrainPath = StartPath & "\brainfile.xml"
        OverwriteDefaultBrainfileToolStripMenuItem.Enabled = False
        TextBox12.Text = ""
        ReadBrain()
    End Sub

    Private Sub OverwriteDefaultBrainfileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OverwriteDefaultBrainfileToolStripMenuItem.Click
        Dim result As DialogResult
        result = MessageBox.Show("Really overwrite default brainfile with " & DefaultBrainPath & "?", "Overwrite default brainfile", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Try
                File.Copy(DefaultBrainPath, StartPath & "\brainfile.xml", True)
                MessageBox.Show("File copied successfully")
            Catch
                MessageBox.Show("Can't overwrite default brainfile!!")
            End Try
        End If
        DefaultBrainPath = StartPath & "\brainfile.xml"
        ReadBrain()
    End Sub

    Private Sub BrainfileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BrainfileToolStripMenuItem.Click
        tabBrain.SelectTab(2)
    End Sub

    Private Sub ListBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox4.SelectedIndexChanged
        If ListBox4.SelectedItem.ToString.Contains("No Subdirectories in") Then Exit Sub
        Dim newpath As String = ListBox4.SelectedItem.ToString
        fbdOpen.SelectedPath = newpath
        TextBox10.Text = newpath
        cmdRescan.PerformClick()
    End Sub

    Private Sub Button34_Click_1(sender As Object, e As EventArgs) Handles Button34.Click
        ASMLoc = 0
        ListBox5.Items.Clear()
        EQUCount = 0
        TextBox13.Text = txtBootName.Text
        TreeView3.Nodes.Clear()
        If OpenBootFile = "" Then Exit Sub
        TreeView3.Nodes.Clear()
        Dim up As String
        Dim p As New Process
        Dim runner As New ProcessStartInfo
        Dim quote As Char = Chr(34)
        File.WriteAllBytes(Tempdrawer & "\boot.abb", Buffer)
        Dim bsarg As String = "-M68000 -BINARY -OLDSTYLE"
        If CheckBox5.Checked Then bsarg &= " -PREPROC"
        If File.Exists(StartPath & "\ira.exe") Then runner.FileName = StartPath & "\ira.exe"
        If runner.FileName = "" Then
            MessageBox.Show("Please copy IRA.exe into ABR drawer")
            Exit Sub
        End If
        runner.WorkingDirectory = Tempdrawer
        runner.UseShellExecute = False
        runner.RedirectStandardOutput = True
        runner.RedirectStandardError = True
        runner.CreateNoWindow = True
        runner.Arguments = bsarg & " " & quote & Tempdrawer & "\boot.abb" & quote & " " & quote & Tempdrawer & "\boot.asm" & quote
        runner.CreateNoWindow = True
        p.StartInfo = runner
        p.Start()
        p.WaitForExit()
        Dim std_err As StreamReader = p.StandardError
        Dim std_out As StreamReader = p.StandardOutput
        up = std_err.ReadToEnd
        If CheckBox2.Checked = True Then
            MessageBox.Show(up)
        End If
        ASMFile = File.ReadAllLines(Tempdrawer & "\boot.asm")
        'Read EQU lines into memory
        Dim x As Integer
        EQUCount = 0
        Dim segs(4) As String
        'Check each line
        For x = 0 To ASMFile.Length - 1
            segs = ASMFile(x).Split(vbTab)
            If segs.Length = 3 Then
                If segs(1) = "EQU" Then
                    EQUName(EQUCount) = segs(0)
                    EQURef(EQUCount) = segs(2)
                    EQUCount += 1
                End If
            End If
        Next
        Dim opfound As Boolean = False
        ProgressBar1.Maximum = ASMFile.Length - 1
        TreeView3.Nodes.Add("")
        For x = 0 To ASMFile.Length - 1
            ASMLoc = x
            TreeView3.Nodes.Add(CheckAsmLine(ASMFile(x)))
        Next
        TreeView3.Nodes.Add("")
        File.Delete(Tempdrawer & "\boot.abb")
        File.Delete(Tempdrawer & "\boot.asm")
        If File.Exists(Tempdrawer & "\boot.cnf") Then File.Delete(Tempdrawer & "\boot.cnf")
    End Sub

    Private Function CheckAsmLine(ByVal s As String) As String
        Dim segs() As String
        Dim asmop As String
        Dim asmval() As String
        Dim sout As String
        Dim desc As String = ""
        segs = s.Split(vbTab)
        If segs(0) = "" Then sout = "    " Else sout = ""
        Select Case segs.Length
            Case 0
                Return Format(ASMLoc, "0000") & " : " & ""
            Case 1
                asmop = segs(0).Replace(";", "")
                Return Format(ASMLoc, "0000") & " : " & asmop
            Case 2
                asmop = segs(0)
                Return Format(ASMLoc, "0000") & " : " & segs(1)
            Case > 2

                asmval = Split(segs(2), ",")
                asmop = segs(1)
                'Testing of 1st value
                If asmop = "TST.L" And asmval(0) = "42(A6)" Then desc = "Testing of the Cold capture vector"
                If asmop = "CLR.L" And asmval(0) = "42(A6)" Then desc = "Clearing of the Cold capture vector"
                If asmop = "TST.L" And asmval(0) = "46(A6)" Then desc = "Testing of the Cool capture vector"
                If asmop = "CLR.L" And asmval(0) = "46(A6)" Then desc = "Clearing of the Cool capture vector"
                If asmop = "CLR.L" And asmval(0) = "550(A6)" Then desc = "Clearing of Kicktag vector"
                If asmop = "TST.L" And asmval(0) = "550(A6)" Then desc = "Testing of the Kicktag vector"
                'testing of 2nd value
                If asmval.Length > 1 Then
                    If asmop = "MOVE.L" And asmval(0).Contains("$47555255") Then
                        desc = "'GURU' written to memory " & asmval(1) & " (Random Access Virus)"
                    End If
                    If asmop = "MOVE.L" And asmval(1) = "550(A6)" Then desc = "Copying " & asmval(0) & " into the Kicktag vector"
                    If asmop = "MOVE.L" And asmval(0) = "550(A6)" Then desc = "Copying Kicktag vector to " & asmval(1)
                    If asmop = "CMPI.L" And asmval(1) = "550(A6)" Then desc = "Comparing " & asmval(0) & " to Kicktag vector"
                    If asmop = "MOVE.L" And asmval(1) = "554(A6)" Then desc = "Copying " & asmval(0) & " into the KickCheckSum vector"
                    If asmop = "MOVE.L" And asmval(0) = "554(A6)" Then desc = "Copying KickCheckSum vector to " & asmval(1)
                    If asmop = "CMPI.L" And asmval(1) = "554(A6)" Then desc = "Comparing " & asmval(0) & " to KickCheckSum vector"
                    If asmop = "MOVE.L" And asmval(1) = "42(A6)" Then desc = "Moving " & asmval(0) & " to the Cold capture vector"
                    If asmop = "MOVE.L" And asmval(1) = "46(A6)" Then desc = "Moving " & asmval(0) & " to the Cool capture vector"
                    If asmop = "BTST" And asmval(0) = "#6" AndAlso asmval(1) = "CIAA_PRA" Then desc = "Check if Joystick 1 / Mouse button pressed"
                    If asmop = "BTST" And asmval(0) = "#7" AndAlso asmval(1) = "CIAA_PRA" Then desc = "Check if Joystick 2 / Mouse button pressed"
                    If asmop = "BTST" And asmval(0) = "#3" AndAlso asmval(1) = "CIAA_PRA" Then desc = "Check if Disk is write protected"
                    If asmop = "BCLR" And asmval(0) = "#1" AndAlso asmval(1) = "CIAA_PRA" Then desc = "Brighten LED (Clear filter)"
                    If asmop = "BCLR" And asmval(0) = "#1" AndAlso asmval(1) = "$BFE001" Then desc = "Brighten LED (Clear filter)"
                    If asmop = "BCHG" And asmval(0) = "#1" AndAlso asmval(1) = "CIAA_PRA" Then desc = "Toggle LED (Filter on or off)"
                    If asmop = "BCHG" And asmval(0) = "#1" AndAlso asmval(1) = "$BFE001" Then desc = "Toggle LED (Filter on or off)"
                    If asmop = "BCHG" And asmval(0) = "#1" AndAlso asmval(1) = "CIAA_PRA" Then desc = "Toggle LED (Filter on or off)"
                    If asmop = "BCHG" And asmval(0) = "#1" AndAlso asmval(1) = "$BFE001" Then desc = "Toggle LED (Filter on or off)"
                    If asmop = "BSET" And asmval(0) = "#1" AndAlso asmval(1) = "CIAA_PRA" Then desc = "Disable Filter LED"
                    If asmop = "BSET" And asmval(0) = "#1" AndAlso asmval(1) = "$BFE001" Then desc = "Disable Filter LED"
                    If desc = "" Then Else ListBox5.Items.Add(Format(ASMLoc, "0000") & " : " & desc)
                End If
                If segs.Length = 4 Then
                    Return Format(ASMLoc, "0000") & " : " & segs(0) & " " & String.Format("{0,-10} {1,-30} {2,-20}{3,-20}", asmop, segs(2), segs(3), desc)
                Else
                    Return Format(ASMLoc, "0000") & " : " & segs(0) & " " & String.Format("{0,-10} {1,-30} {2,-20}", asmop, segs(2), desc)
                End If

        End Select
    End Function
    Private Sub SWork_dowork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles SWorker.DoWork
        Dim x As Integer, y As Integer
        Dim sim As Integer
        Dim pre As String = ""
        Dim compboot(1024) As Byte
        Dim perc As Double = 0
        Dim bclass As Integer
        Dim aclass As String = ""
        Dim ssw As Boolean
        ReDim bblist(sbfiles.Length - 1)
        For x = 0 To sbfiles.Length - 1
            ssw = False
            If x Mod 20 = 0 Then Thread.Sleep(2)
            perc = 0
            sim = 0
            If CompVirus = True Then
                bclass = BFind(Path.GetFileNameWithoutExtension(sbfiles(x)))
                If bclass > -1 Then
                    aclass = BBDatabase(bclass).BClass
                Else aclass = "unk"
                End If
                If aclass = "v" Then
                    compboot = File.ReadAllBytes(sbfiles(x))
                    For y = 0 To compboot.Length - 1
                        If Buffer(y) = compboot(y) Then sim += 1
                    Next
                    ssw = True
                End If
            Else
                compboot = File.ReadAllBytes(sbfiles(x))
                For y = 0 To compboot.Length - 1
                    If Buffer(y) = compboot(y) Then sim += 1
                Next
                ssw = True
            End If
            If ssw = True Then
                If sim < 1000 Then pre = "0"
                If sim < 100 Then pre = "00"
                If sim < 10 Then pre = "000"
                If sim = 1024 Then pre = ""
                perc = (sim / 1024) * 100
                bblist(x) = "- " & pre & sim & "/1024 Char match (" & Format(perc, "00") & "%)" & " | " & Path.GetFileNameWithoutExtension(sbfiles(x))
                SetTextBox_ThreadSafe(TextBox14, Path.GetFileNameWithoutExtension(sbfiles(x)))
                SetTreeView_ThreadSafe(TreeView3, bblist(x))
                SWorker.ReportProgress(x)

            End If
            If SWorker.CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If
        Next

    End Sub

    Private Sub SWork_progress(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles SWorker.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
        Label44.Text = "Bootblocks Scanned: " & e.ProgressPercentage.ToString
    End Sub

    Private Sub SWork_end(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles SWorker.RunWorkerCompleted
        SetTextBox_ThreadSafe(TextBox14, "Sorting.....")
        Array.Sort(bblist)
        TextBox14.Text = "Reprinting...."
        TreeView3.Nodes.Clear()
        TreeView3.Nodes.Add("")
        TreeView3.Nodes.Add("    Similarity score       /     Bootblock name")
        TreeView3.Nodes.Add("-------------------------------------------")
        For x = bblist.Length - 1 To 0 Step -1
            If bblist(x) = "" Then

            Else
                TreeView3.Nodes.Add(bblist(x))
            End If
        Next
        SetTextBox_ThreadSafe(TextBox14, "OK")
        Button38.Enabled = True
    End Sub

    Private Sub Button36_Click(sender As Object, e As EventArgs) Handles Button36.Click
        CompVirus = False
        TreeView3.Nodes.Clear()
        ProgressBar1.Maximum = sbfiles.Length - 1
        SWorker.RunWorkerAsync()
    End Sub

    Private Sub Button35_Click(sender As Object, e As EventArgs) Handles Button35.Click
        ProgressBar1.Value = ProgressBar1.Maximum
        If SWorker.IsBusy Then SWorker.CancelAsync()
    End Sub

    Private Sub Button37_Click(sender As Object, e As EventArgs) Handles Button37.Click
        CompVirus = True
        TreeView3.Nodes.Clear()
        ProgressBar1.Maximum = sbfiles.Length
        SWorker.RunWorkerAsync()
    End Sub

    Private Sub Button38_Click(sender As Object, e As EventArgs) Handles Button38.Click

        Dim result As DialogResult
        sfdASM.InitialDirectory = Environment.GetEnvironmentVariable("USERPROFILE") & "\Desktop"
        sfdASM.FileName = txtBootName.Text
        result = sfdASM.ShowDialog
        If result = DialogResult.OK Then
            Try
                File.WriteAllLines(sfdASM.FileName, ASMFile)
            Catch
                MessageBox.Show("Cant write file!!")
                Exit Sub
            End Try
            MessageBox.Show("ASM File saved to " & sfdASM.FileName)
        End If
    End Sub

    Private Sub Button41_Click(sender As Object, e As EventArgs) Handles Button41.Click
        BatchScan.Show()
    End Sub

    Private Sub Label17_Click(sender As Object, e As EventArgs) Handles Label17.Click

    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        If dgvBatch.CurrentCell.RowIndex = -1 Then Exit Sub
        If dgvBatch.CurrentCell.RowIndex = dgvBatch.Rows.Count - 1 Then Exit Sub
        dgvBatch.ClearSelection()
        dgvBatch.Rows(0).Selected = True
        BBDisplay.ScrollBars = ScrollBars.None
        ViewSelectedFile()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If dgvBatch.CurrentCell.RowIndex = -1 Then Exit Sub
        If dgvBatch.CurrentCell.RowIndex = dgvBatch.Rows.Count - 1 Then Exit Sub
        dgvBatch.ClearSelection()
        dgvBatch.Rows(dgvBatch.Rows.Count - 1).Selected = True
        BBDisplay.ScrollBars = ScrollBars.None
        ViewSelectedFile()
    End Sub

    Private Sub chkFS_Click(sender As Object, e As EventArgs) Handles chkFS.Click
        My.Settings.SSize = chkFS.Checked
        My.Settings.Save()
    End Sub

    Private Sub chkSCRC_Click(sender As Object, e As EventArgs) Handles chkSCRC.Click
        My.Settings.SCRC = chkSCRC.Checked
        My.Settings.Save()
    End Sub

    Private Sub chkBMT_Click(sender As Object, e As EventArgs) Handles chkBMT.Click
        My.Settings.SScan = chkBMT.Checked
        My.Settings.Save()
    End Sub

    Private Sub DisplayRecog()
        If Foundno = -1 Then Exit Sub
        Dim test() As String
        test = Recogline(Foundno).Split(",")
        rtBBDisplay.SelectionStart = test(0)
        rtBBDisplay.SelectionLength = 1

        rtBBDisplay.SelectionBackColor = Color.Yellow
        rtBBDisplay.SelectionStart = test(2)
        rtBBDisplay.SelectionLength = 1

        rtBBDisplay.SelectionBackColor = Color.Yellow
        rtBBDisplay.SelectionStart = test(4)
        rtBBDisplay.SelectionLength = 1

        rtBBDisplay.SelectionBackColor = Color.Yellow
        rtBBDisplay.SelectionStart = test(6)
        rtBBDisplay.SelectionLength = 1

        rtBBDisplay.SelectionBackColor = Color.Yellow
        rtBBDisplay.SelectionStart = test(8)
        rtBBDisplay.SelectionLength = 1

        rtBBDisplay.SelectionBackColor = Color.Yellow
        rtBBDisplay.SelectionStart = test(10)
        rtBBDisplay.SelectionLength = 1

        rtBBDisplay.SelectionBackColor = Color.Yellow
        rtBBDisplay.SelectionStart = test(12)
        rtBBDisplay.SelectionLength = 1
        rtBBDisplay.SelectionBackColor = Color.Yellow

    End Sub

    Private Sub Button2_Click_2(sender As Object, e As EventArgs) Handles Button2.Click
        fbdDMS.ShowDialog()
        dgvDMS.Rows.Clear()
        Dim dmsfiles() As String
        Dim SO As SearchOption
        If chkPDMS.Checked = True Then SO = SearchOption.AllDirectories
        If chkPDMS.Checked = False Then SO = SearchOption.TopDirectoryOnly
        dmsfiles = Directory.GetFiles(fbdDMS.SelectedPath, "*.dms", SO)
        Dim i As Integer
        For i = 0 To dmsfiles.Length - 1
            dgvDMS.Rows.Add(dmsfiles(i))
        Next
    End Sub

    Private Sub Button8_Click_1(sender As Object, e As EventArgs) Handles Button8.Click
        Dim x As Integer
        Dim resp As String
        Dim fname As String
        Dim adfname As String
        For x = 0 To dgvDMS.Rows.Count - 1
            fname = dgvDMS.Rows(x).Cells(0).Value
            If fname = Nothing Then Exit For
            adfname = fname.ToLower.Replace(".dms", ".adf")
            If File.Exists(fname) = False Then Exit For
            If File.Exists(adfname) Then
                If chkOW.Checked = False Then
                    dgvDMS.Rows(x).Cells(1).Value = "ADF file already exists"
                End If
            End If
            resp = UnDMStoFile(fname, Path.GetDirectoryName(fname))
            If resp.Contains("correctly unpacked") Then
                dgvDMS.Rows(x).Cells(1).Value = "File successfully unpacked"
                If FileSystem.FileLen(adfname) < 901120 Then dgvDMS.Rows(x).Cells(1).Value &= " - ADF File size incorrect (" & FileSystem.FileLen(adfname) & " bytes)"
            Else
                dgvDMS.Rows(x).Cells(1).Value = resp
            End If
            If CheckBox4.Checked = True Then
                Try
                    File.Delete(fname)
                Catch ex As Exception
                    dgvDMS.Rows(x).Cells(1).Value &= " - File delete failed!"
                End Try
            End If
        Next

    End Sub

    Private Sub Button9_Click_1(sender As Object, e As EventArgs) Handles Button9.Click
        dgvDMS.Rows.Clear()
        Dim dmsfiles() As String
        Dim SO As SearchOption
        If chkPDMS.Checked = True Then SO = SearchOption.AllDirectories
        If chkPDMS.Checked = False Then SO = SearchOption.TopDirectoryOnly
        dmsfiles = Directory.GetFiles(fbdDMS.SelectedPath, "*.dms", SO)
        Dim i As Integer
        For i = 0 To dmsfiles.Length - 1
            dgvDMS.Rows.Add(dmsfiles(i))
        Next
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        If Recogtoggle = True Then
            PictureBox1.Image = My.Resources.Off
            Recogtoggle = False
            rtBBDisplay.SelectAll()
            rtBBDisplay.SelectionBackColor = Color.AliceBlue
        Else
            PictureBox1.Image = My.Resources._On
            Recogtoggle = True
            DisplayRecog()
        End If

    End Sub
End Class

