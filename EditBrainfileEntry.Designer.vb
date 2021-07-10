<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EditBrainfileEntry
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.grpEBrain = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNote = New System.Windows.Forms.TextBox()
        Me.lbKS = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.cmdECancel = New System.Windows.Forms.Button()
        Me.cmdESave = New System.Windows.Forms.Button()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.txtEName = New System.Windows.Forms.TextBox()
        Me.lblNum = New System.Windows.Forms.Label()
        Me.grpEBrain.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpEBrain
        '
        Me.grpEBrain.Controls.Add(Me.lblNum)
        Me.grpEBrain.Controls.Add(Me.Label3)
        Me.grpEBrain.Controls.Add(Me.txtNote)
        Me.grpEBrain.Controls.Add(Me.lbKS)
        Me.grpEBrain.Controls.Add(Me.Label2)
        Me.grpEBrain.Controls.Add(Me.CheckBox2)
        Me.grpEBrain.Controls.Add(Me.CheckBox1)
        Me.grpEBrain.Controls.Add(Me.Button1)
        Me.grpEBrain.Controls.Add(Me.Label1)
        Me.grpEBrain.Controls.Add(Me.ComboBox1)
        Me.grpEBrain.Controls.Add(Me.cmdECancel)
        Me.grpEBrain.Controls.Add(Me.cmdESave)
        Me.grpEBrain.Controls.Add(Me.Label40)
        Me.grpEBrain.Controls.Add(Me.txtEName)
        Me.grpEBrain.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.grpEBrain.Location = New System.Drawing.Point(12, 12)
        Me.grpEBrain.Name = "grpEBrain"
        Me.grpEBrain.Size = New System.Drawing.Size(470, 354)
        Me.grpEBrain.TabIndex = 112
        Me.grpEBrain.TabStop = False
        Me.grpEBrain.Text = "Edit Brainfile Entry"
        Me.grpEBrain.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 113)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(197, 13)
        Me.Label3.TabIndex = 145
        Me.Label3.Text = "Bootblock notes: (e.g. Clone or Hack of)"
        '
        'txtNote
        '
        Me.txtNote.Location = New System.Drawing.Point(12, 132)
        Me.txtNote.Name = "txtNote"
        Me.txtNote.Size = New System.Drawing.Size(439, 20)
        Me.txtNote.TabIndex = 144
        '
        'lbKS
        '
        Me.lbKS.FormattingEnabled = True
        Me.lbKS.Items.AddRange(New Object() {"1.2", "1.3", "2.0", "3.0"})
        Me.lbKS.Location = New System.Drawing.Point(273, 248)
        Me.lbKS.Name = "lbKS"
        Me.lbKS.Size = New System.Drawing.Size(121, 21)
        Me.lbKS.TabIndex = 143
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(285, 232)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 13)
        Me.Label2.TabIndex = 142
        Me.Label2.Text = "Kickstart Required"
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(123, 248)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(110, 17)
        Me.CheckBox2.TabIndex = 140
        Me.CheckBox2.Text = "Needs Disk Data "
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(22, 248)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(68, 17)
        Me.CheckBox1.TabIndex = 139
        Me.CheckBox1.Text = "Bootable"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(256, 47)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(194, 23)
        Me.Button1.TabIndex = 138
        Me.Button1.Text = "Delete entry"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 175)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 13)
        Me.Label1.TabIndex = 137
        Me.Label1.Text = "Bootblock Classification:"
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(12, 194)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(439, 21)
        Me.ComboBox1.Sorted = True
        Me.ComboBox1.TabIndex = 36
        '
        'cmdECancel
        '
        Me.cmdECancel.Location = New System.Drawing.Point(239, 290)
        Me.cmdECancel.Name = "cmdECancel"
        Me.cmdECancel.Size = New System.Drawing.Size(211, 34)
        Me.cmdECancel.TabIndex = 35
        Me.cmdECancel.Text = "Cancel"
        Me.cmdECancel.UseVisualStyleBackColor = False
        '
        'cmdESave
        '
        Me.cmdESave.Location = New System.Drawing.Point(11, 290)
        Me.cmdESave.Name = "cmdESave"
        Me.cmdESave.Size = New System.Drawing.Size(222, 34)
        Me.cmdESave.TabIndex = 34
        Me.cmdESave.Text = "Save Changes"
        Me.cmdESave.UseVisualStyleBackColor = False
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Location = New System.Drawing.Point(8, 55)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(38, 13)
        Me.Label40.TabIndex = 32
        Me.Label40.Text = "Name:"
        '
        'txtEName
        '
        Me.txtEName.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEName.Location = New System.Drawing.Point(11, 76)
        Me.txtEName.Name = "txtEName"
        Me.txtEName.Size = New System.Drawing.Size(439, 22)
        Me.txtEName.TabIndex = 31
        '
        'lblNum
        '
        Me.lblNum.AutoSize = True
        Me.lblNum.Location = New System.Drawing.Point(9, 30)
        Me.lblNum.Name = "lblNum"
        Me.lblNum.Size = New System.Drawing.Size(78, 13)
        Me.lblNum.TabIndex = 146
        Me.lblNum.Text = "Bootblock No: "
        '
        'EditBrainfileEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(495, 394)
        Me.Controls.Add(Me.grpEBrain)
        Me.Name = "EditBrainfileEntry"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit Brainfile Entry"
        Me.grpEBrain.ResumeLayout(False)
        Me.grpEBrain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpEBrain As GroupBox
    Friend WithEvents cmdECancel As Button
    Friend WithEvents cmdESave As Button
    Friend WithEvents Label40 As Label
    Friend WithEvents txtEName As TextBox
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents CheckBox2 As CheckBox
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents lbKS As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtNote As TextBox
    Friend WithEvents lblNum As Label
End Class
