﻿

Public Class StringSearch
    Private Sub Dialog3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim x As Integer
        chkBoot.Checked = True
        For x = 0 To Main.CatList.Length - 1
            ComboBox1.Items.Add(Main.CatList(x))
        Next
        Me.Location = New Point(Main.Location.X + 650, Main.Location.Y + 100)
        txtString.Text = Main.rtBBDisplay.SelectedText
        ComboBox2.SelectedText = "1.3"
    End Sub

    'Private Sub Button1_Click(sender As Object, e As EventArgs) Handles SaveSS.Click
    '    Dim newSSClass As String = ""
    '    Dim newssboot As String
    '    Dim newssdr As String
    '    If chkBoot.Checked = True Then newSSBoot = "Y" Else newSSBoot = "N"
    '    If chkDDR.Checked = True Then newSSDR = "Y" Else newSSDR = "N"
    '    Dim x As Integer
    '    For x = 0 To Main.CatList.Length - 1
    '        If ComboBox1.Text = Main.CatList(x) Then
    '            newSSClass = Main.CatAb(x)
    '        End If
    '    Next
    '    Main.SaveSearchBBEntry(txtBBName.Text, newSSClass, txtString.Text, ComboBox2.Text, newssdr, newssboot)
    'End Sub
    Private Sub MatchName() Handles txtBBName.TextChanged
        cbxKnown.Items.Clear()
        Dim i As Integer
        For i = 0 To Main.Bootno - 1
            If InStr(Main.BBDatabase(i).Name.ToUpper, txtBBName.Text.ToUpper) Then
                cbxKnown.Items.Add(Main.BBDatabase(i).Name)
            End If
        Next
        If cbxKnown.Items.Count = Main.Bootno Or cbxKnown.Items.Count = 0 Then
            cbxKnown.Text = "No Similar Bootblocks found"
        ElseIf cbxKnown.Items.Count > 0 Then
            cbxKnown.Text = cbxKnown.Items(0)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        txtBBName.Text = cbxKnown.Text
    End Sub
End Class