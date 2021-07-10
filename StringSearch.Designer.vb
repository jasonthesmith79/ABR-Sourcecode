<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StringSearch
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtString = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtBBName = New System.Windows.Forms.TextBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SaveSS = New System.Windows.Forms.Button()
        Me.cbxKnown = New System.Windows.Forms.ComboBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.chkBoot = New System.Windows.Forms.CheckBox()
        Me.chkDDR = New System.Windows.Forms.CheckBox()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtString
        '
        Me.txtString.Location = New System.Drawing.Point(12, 30)
        Me.txtString.Name = "txtString"
        Me.txtString.Size = New System.Drawing.Size(277, 20)
        Me.txtString.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(88, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Selected search string:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(103, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Bootblock Name:"
        '
        'txtBBName
        '
        Me.txtBBName.Location = New System.Drawing.Point(14, 80)
        Me.txtBBName.Name = "txtBBName"
        Me.txtBBName.Size = New System.Drawing.Size(277, 20)
        Me.txtBBName.TabIndex = 3
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(13, 189)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(278, 21)
        Me.ComboBox1.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(106, 173)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Bootblock Class:"
        '
        'SaveSS
        '
        Me.SaveSS.Location = New System.Drawing.Point(13, 300)
        Me.SaveSS.Name = "SaveSS"
        Me.SaveSS.Size = New System.Drawing.Size(278, 23)
        Me.SaveSS.TabIndex = 6
        Me.SaveSS.Text = "Save"
        Me.SaveSS.UseVisualStyleBackColor = True
        '
        'cbxKnown
        '
        Me.cbxKnown.FormattingEnabled = True
        Me.cbxKnown.Location = New System.Drawing.Point(11, 135)
        Me.cbxKnown.Name = "cbxKnown"
        Me.cbxKnown.Size = New System.Drawing.Size(280, 21)
        Me.cbxKnown.TabIndex = 7
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(91, 106)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(112, 23)
        Me.Button2.TabIndex = 8
        Me.Button2.Text = "^ Copy Name ^"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'chkBoot
        '
        Me.chkBoot.AutoSize = True
        Me.chkBoot.Location = New System.Drawing.Point(29, 229)
        Me.chkBoot.Name = "chkBoot"
        Me.chkBoot.Size = New System.Drawing.Size(68, 17)
        Me.chkBoot.TabIndex = 9
        Me.chkBoot.Text = "Bootable"
        Me.chkBoot.UseVisualStyleBackColor = True
        '
        'chkDDR
        '
        Me.chkDDR.AutoSize = True
        Me.chkDDR.Location = New System.Drawing.Point(153, 229)
        Me.chkDDR.Name = "chkDDR"
        Me.chkDDR.Size = New System.Drawing.Size(119, 17)
        Me.chkDDR.TabIndex = 10
        Me.chkDDR.Text = "Disk Data Required"
        Me.chkDDR.UseVisualStyleBackColor = True
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Items.AddRange(New Object() {"1.2", "1.3", "2.0", "3.0"})
        Me.ComboBox2.Location = New System.Drawing.Point(170, 264)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox2.TabIndex = 11
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(67, 267)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(97, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Kickstart Required:"
        '
        'StringSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(302, 345)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.chkDDR)
        Me.Controls.Add(Me.chkBoot)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.cbxKnown)
        Me.Controls.Add(Me.SaveSS)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.txtBBName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtString)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "StringSearch"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Learn Bootblock by String Search"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtString As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtBBName As TextBox
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents SaveSS As Button
    Friend WithEvents cbxKnown As ComboBox
    Friend WithEvents Button2 As Button
    Friend WithEvents chkBoot As CheckBox
    Friend WithEvents chkDDR As CheckBox
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents Label4 As Label
End Class
