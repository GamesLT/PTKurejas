'
' Created by SharpDevelop.
' User: Administrator
' Date: 2009.03.06
' Time: 23:38
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Public Partial Class MainForm
	Public Sub New()
		' The Me.InitializeComponent call is required for Windows Forms designer support.
		Me.InitializeComponent()
		
		'
		' TODO : Add constructor code after InitializeComponents
		'
	End Sub
	
	Private LastTrueName as String = ""
	
	Sub Button1Click(sender As Object, e As EventArgs)
		If CheckIfNameIsInList(Me.textBox1.Text.Trim()) Then
			messagebox.Show("Toks pavadinimas jau yra sąraše!", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			Exit Sub			
		End If		
		Me.listBox1.Items.Add(Me.textBox1.Text.Trim())
		Me.textBox1.Text = ""
		Me.textBox1.Focus()			
	End Sub
	
	Private Function OpenImageDialog(ByRef RezImage As System.Drawing.Image) as Boolean
		Dim fo As New System.Windows.Forms.OpenFileDialog()
		fo.CheckFileExists = True
		fo.CheckPathExists = True
		fo.Filter = "Visi galimi paveikslėliai (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png|Visi failai (*.*)|*.*"
		If fo.ShowDialog() = DialogResult.OK Then
			Dim Image As System.Drawing.Image = System.Drawing.Image.FromFile(fo.FileName)
			If Image.Width < 440 Then
				If MessageBox.Show("Pasirinkto paveikslėlio plotis yra mažesnis už 440px" + vbcrlf + vbcrlf + "Ar bandysite pasirinkti kitą paveiksliuką?","Klaida", MessageBoxButtons.RetryCancel) = DialogResult.Retry Then
					Return OpenImageDialog(rezimage)								
				End If
				image.Dispose()
				Return False 				
			End If 
			If Image.Height < 220 Then
				If MessageBox.Show("Pasirinkto paveikslėlio aukštis yra mažesnis už 220px" + vbcrlf + vbcrlf + "Ar bandysite pasirinkti kitą paveiksliuką?","Klaida", MessageBoxButtons.RetryCancel) = DialogResult.Retry Then
					Return OpenImageDialog(rezimage)					
				End If
				image.Dispose()
				Return False 			
			End If
			If Image.Width/Image.Height > 5 Then
				If MessageBox.Show("Pasirinkto paveikslėlio kraštinių santykis yra per dideliuose rėžiuose" + vbcrlf + vbcrlf + "Ar bandysite pasirinkti kitą paveiksliuką?","Klaida", MessageBoxButtons.RetryCancel) = DialogResult.Retry Then
					Return OpenImageDialog(rezimage)					
				End If
				image.Dispose()
				Return False 
			End If	
			If Image.Width < Image.Height Then
				If MessageBox.Show("Tinka tik horizontalūs paveiksliukai!" + vbcrlf + vbcrlf + "Ar bandysite pasirinkti kitą paveiksliuką?","Klaida", MessageBoxButtons.RetryCancel) = DialogResult.Retry Then
					Return OpenImageDialog(rezimage)					
				End If
				image.Dispose()
				Return False 
			End If	
			RezImage = Image
			Return True
		End If
		Return False		
	End Function
	
	Sub Button3Click(sender As Object, e As EventArgs)
		if me.OpenImageDialog(Me.pictureBox1.Image ) then			
			Me.button2.Enabled = (Me.textBox2.Text.Trim().Length > 0) And (Me.pictureBox1.Image IsNot Nothing)			
			me.textBox2.Focus()
		End If
	End Sub	
	
	Sub TextBox1TextChanged(sender As Object, e As EventArgs)
		me.button1.Enabled = Me.textBox1.Text.Trim().Length > 0
	End Sub
	
	Sub TextBox2TextChanged(sender As Object, e As EventArgs)
		Me.button2.Enabled = (Me.textBox2.Text.Trim().Length > 0) And (Me.pictureBox1.Image IsNot Nothing)		
	End Sub
		
	Sub Button2Click(sender As Object, e As EventArgs)
		If CheckIfNameIsInList(Me.textBox2.Text.Trim()) Then
			messagebox.Show("Toks pavadinimas jau yra sąraše!", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			Exit Sub			
		End If
		Me.dataGridView1.Rows.Add(Me.pictureBox1.Image, Me.textBox2.Text.Trim())
		Me.pictureBox1.Image = Nothing
		Me.textBox2.Text = ""
		Me.button3.Focus()
	End Sub
	
	Public Function CheckIfNameIsInList(Name As String) As Boolean		
		If Name Is Nothing Then Return False		
		If Name.Trim() = "" Then Return False 		
		For I As Integer = 0 To Me.listBox1.Items.Count - 1			
			If Me.listBox1.Items.Item(i).ToLower().Trim() = Name.ToLower().Trim() Then Return True			
		Next
		For I As Integer = 0 To Me.dataGridView1.Rows.Count - 1
			If Me.dataGridView1.Rows.Item(i).Cells.Item(1).Value.ToString().ToLower().Trim() = Name.ToLower().Trim() Then Return True			
		Next
		Return False		
	End Function	
	
	Sub DataGridView1CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
		On Error Resume Next 		
		'If e.ColumnIndex  = 1 Then
		'	If CheckIfNameIsInList(Me.dataGridView1.Rows.Item(e.RowIndex).Cells.Item(e.ColumnIndex).Value.ToString().trim()) Then
		'		messagebox.Show("Toks pavadinimas jau yra sąraše!", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
		'		Me.dataGridView1.Rows.Item(e.RowIndex).Cells.Item(e.ColumnIndex).Value = Me.LastTrueName				
		'		Exit Sub			
		'	End If		
	'	end if
	End Sub
	
	Sub DataGridView1CellEnter(sender As Object, e As DataGridViewCellEventArgs)
		On Error Resume Next 
		If e.ColumnIndex  = 1 Then
			Me.LastTrueName = Me.dataGridView1.Rows.Item(e.RowIndex).Cells.Item(e.ColumnIndex).Value.ToString()			
		End If
	End Sub
	
	Sub DataGridView1CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
		If e.ColumnIndex = 0 Then 
			If Me.OpenImageDialog(Me.dataGridView1.Rows.Item(e.RowIndex).Cells.Item(e.ColumnIndex).Value) Then
				
			End If
		End If
	End Sub
		
	Sub TextBox2KeyUp(sender As Object, e As KeyEventArgs)		
		If (e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Return) And Me.button2.Enabled Then						
			Call Button2Click(sender, New EventArgs())					
		End If
	End Sub
	
	Sub TextBox1KeyUp(sender As Object, e As KeyEventArgs)
		If (e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Return) And Me.button1.Enabled Then						
			Call Button1Click(sender, New EventArgs())
		End If
	End Sub
	
	Sub PictureBox1Click(sender As Object, e As EventArgs)
		Call Button3Click(sender, e)
	End Sub
	
	Sub ListBox1KeyUp(sender As Object, e As KeyEventArgs)
		If e.KeyCode = keys.Delete Then
			If Me.listBox1.SelectedItem IsNot Nothing Then
				Me.listBox1.Items.Remove(Me.listBox1.SelectedItem)
				Me.listBox1.Focus()
			End If
		End If
	End Sub
		
	Sub ToolStripMenuItem1Click(sender As Object, e As EventArgs)
		Dim sdl As New SaveFileDialog()
		sdl.Filter = "PTKūrėjo failas (*.ptkf)|*.ptkf"
		If sdl.ShowDialog() = DialogResult.OK Then
			Me.Enabled = False
			Dim file As New PTK.File.PTKFile()
			For I As Integer = 0 To Me.listBox1.Items.Count - 1			
				file.OtherAnswersItems.Add(Me.listBox1.Items.Item(i))				
			Next
			For I As Integer = 0 To Me.dataGridView1.Rows.Count - 1
				Try
					file.ImageAnswersItems.Add(Me.dataGridView1.Rows.Item(i).Cells.Item(1).Value, Me.dataGridView1.Rows.Item(i).Cells.Item(0).Value)
				Catch ex As Exception
					Me.Enabled = True 	
					MessageBox.Show("Du vienodi pavadinimai yra paveikslėlių sąraše. Išsaugojimas buvo nutrauktas.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)								
					Exit Sub					
				End Try				
			Next
			file.Save(sdl.FileName)			
			Me.Enabled = True 			
		End If
	End Sub
		
	Sub ToolStripMenuItem2Click(sender As Object, e As EventArgs)
		Dim txt As String = my.VersionInfo.AppName + vbcrlf
			txt			 += "------------------------------------------------------------------------------------------------------------------" + vbcrlf
			txt			 += "Versija: " + my.VersionInfo.Full + vbcrlf			
			txt			 += "Kūrimo metai: " + my.VersionInfo.DevYears + vbcrlf
			txt			 += "Autorius: " + my.VersionInfo.Author + vbcrlf
			txt			 += "Daugiau informacijos: " + my.VersionInfo.MoreInfoURL.ToString()			
		MessageBox.Show( txt, "Apie", MessageBoxButtons.OK, MessageBoxIcon.Information )
	End Sub	
	
	Private Sub LoadFile(FileName As String)		
		Me.Enabled = False
		Dim file As New PTK.File.PTKFile()
		file.Open(FileName)			
		Do
			Application.DoEvents()				
		Loop Until file.State = ptk.File.PTKFile.States.Idling			
		Me.listBox1.Items.Clear()
		Me.dataGridView1.Rows.Clear()				
		For Each Key As String In file.OtherAnswersItems
			Me.listBox1.Items.add(key)
		Next			
		For Each Key As String In file.ImageAnswersItems.Keys
			Me.dataGridView1.Rows.Add(file.ImageAnswersItems.Item(key), key)
		Next				
		Me.dataGridView1.Update()
		Me.dataGridView1.Refresh()
		Me.dataGridView1.AutoResizeColumns()
		Me.Enabled = True
		Me.dataGridView1.Update()
		Me.dataGridView1.Refresh()
		
	End Sub
	
	Sub AtidarytiToolStripMenuItemClick(sender As Object, e As EventArgs)
		Dim odl As New OpenFileDialog()
		odl.Filter = "PTKūrėjo failas (*.ptkf)|*.ptkf"
		If odl.ShowDialog() = DialogResult.OK Then
			Me.LoadFile(odl.FileName)			
		End If	
	End Sub	
	
	Sub PeržiūrėtiToolStripMenuItemClick(sender As Object, e As EventArgs)
		Me.webBrowser.Visible = Not Me.webBrowser.Visible
		Me.tabControl1.Visible = Not Me.webBrowser.Visible
		Me.toolStripMenuItem1.Enabled = Me.tabControl1.Visible
		me.atidarytiToolStripMenuItem.Enabled = Me.tabControl1.Visible
		If Me.webBrowser.Visible Then
			Me.peržiūrėtiToolStripMenuItem.Text = "Redaguoti"
			Dim file As New PTK.File.PTKFile()
			For I As Integer = 0 To Me.listBox1.Items.Count - 1			
				file.OtherAnswersItems.Add(Me.listBox1.Items.Item(i))				
			Next
			For I As Integer = 0 To Me.dataGridView1.Rows.Count - 1
				Try
					file.ImageAnswersItems.Add(Me.dataGridView1.Rows.Item(i).Cells.Item(1).Value, Me.dataGridView1.Rows.Item(i).Cells.Item(0).Value)
				Catch ex As Exception
					Me.Enabled = True 	
					MessageBox.Show("Du vienodi pavadinimai yra paveikslėlių sąraše. Išsaugojimas buvo nutrauktas.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)								
					Exit Sub					
				End Try				
			Next
			Me.webBrowser.DocumentText = file.GeneratePreviewHTML()			
		Else
			Me.peržiūrėtiToolStripMenuItem.Text = "Peržiūrėti"
		End If
	End Sub
		
	Sub MainFormDragOver(sender As Object, e As DragEventArgs)		
		If e.Data.GetDataPresent("FileDrop") Then
			Dim theFiles() As String = CType(e.Data.GetData("FileDrop", True), String())
			Dim nd As Boolean = False 			
			For Each theFile As String In theFiles
				nd = nd Or ((New System.IO.FileInfo(theFile)).Extension.Trim().ToLower() = ".ptkf")
			Next
			If nd Then
				e.Effect = DragDropEffects.Copy
			Else
				e.Effect = DragDropEffects.None
			End If			
		Else
			e.Effect = DragDropEffects.None
		End If		
	End Sub
	
	
	Sub MainFormDragDrop(sender As Object, e As DragEventArgs)
		If e.Data.GetDataPresent("FileDrop") Then
			Dim theFiles() As String = CType(e.Data.GetData("FileDrop", True), String())
			Dim nd As Boolean = False 			
			For Each theFile As String In theFiles
				If (New System.IO.FileInfo(theFile)).Extension.Trim().ToLower() = ".ptkf" Then
					Me.LoadFile(theFile)					
				End If
			Next
		End If
	End Sub
	
	Sub DataGridView1CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
		
	End Sub
End Class
