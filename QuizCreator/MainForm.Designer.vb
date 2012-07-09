'
' Created by SharpDevelop.
' User: Administrator
' Date: 2009.03.06
' Time: 23:38
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class MainForm
	Inherits System.Windows.Forms.Form
	
	''' <summary>
	''' Designer variable used to keep track of non-visual components.
	''' </summary>
	Private components As System.ComponentModel.IContainer
	
	''' <summary>
	''' Disposes resources used by the form.
	''' </summary>
	''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing Then
			If components IsNot Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub
	
	''' <summary>
	''' This method is required for Windows Forms designer support.
	''' Do not change the method contents inside the source code editor. The Forms designer might
	''' not be able to load this method if it was changed manually.
	''' </summary>
	Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container
		Me.menuStrip1 = New System.Windows.Forms.MenuStrip
		Me.atidarytiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.toolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
		Me.toolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
		Me.peržiūrėtiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
		Me.imageList1 = New System.Windows.Forms.ImageList(Me.components)
		Me.panel1 = New System.Windows.Forms.Panel
		Me.tabControl1 = New System.Windows.Forms.TabControl
		Me.tabPage1 = New System.Windows.Forms.TabPage
		Me.tableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
		Me.dataGridView1 = New System.Windows.Forms.DataGridView
		Me.Image = New System.Windows.Forms.DataGridViewImageColumn
		Me.Answer = New System.Windows.Forms.DataGridViewTextBoxColumn
		Me.panel2 = New System.Windows.Forms.Panel
		Me.button3 = New System.Windows.Forms.Button
		Me.button2 = New System.Windows.Forms.Button
		Me.textBox2 = New System.Windows.Forms.TextBox
		Me.label2 = New System.Windows.Forms.Label
		Me.pictureBox1 = New System.Windows.Forms.PictureBox
		Me.tabPage2 = New System.Windows.Forms.TabPage
		Me.tableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
		Me.listBox1 = New System.Windows.Forms.ListBox
		Me.panel3 = New System.Windows.Forms.Panel
		Me.button1 = New System.Windows.Forms.Button
		Me.label1 = New System.Windows.Forms.Label
		Me.textBox1 = New System.Windows.Forms.TextBox
		Me.webBrowser = New System.Windows.Forms.WebBrowser
		Me.menuStrip1.SuspendLayout
		Me.panel1.SuspendLayout
		Me.tabControl1.SuspendLayout
		Me.tabPage1.SuspendLayout
		Me.tableLayoutPanel2.SuspendLayout
		CType(Me.dataGridView1,System.ComponentModel.ISupportInitialize).BeginInit
		Me.panel2.SuspendLayout
		CType(Me.pictureBox1,System.ComponentModel.ISupportInitialize).BeginInit
		Me.tabPage2.SuspendLayout
		Me.tableLayoutPanel1.SuspendLayout
		Me.panel3.SuspendLayout
		Me.SuspendLayout
		'
		'menuStrip1
		'
		Me.menuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.atidarytiToolStripMenuItem, Me.toolStripMenuItem1, Me.toolStripMenuItem2, Me.peržiūrėtiToolStripMenuItem})
		Me.menuStrip1.Location = New System.Drawing.Point(0, 0)
		Me.menuStrip1.Name = "menuStrip1"
		Me.menuStrip1.Size = New System.Drawing.Size(552, 24)
		Me.menuStrip1.TabIndex = 1
		Me.menuStrip1.Text = "menuStrip1"
		'
		'atidarytiToolStripMenuItem
		'
		Me.atidarytiToolStripMenuItem.Name = "atidarytiToolStripMenuItem"
		Me.atidarytiToolStripMenuItem.Size = New System.Drawing.Size(72, 20)
		Me.atidarytiToolStripMenuItem.Text = "Atidaryti..."
		AddHandler Me.atidarytiToolStripMenuItem.Click, AddressOf Me.AtidarytiToolStripMenuItemClick
		'
		'toolStripMenuItem1
		'
		Me.toolStripMenuItem1.Name = "toolStripMenuItem1"
		Me.toolStripMenuItem1.Size = New System.Drawing.Size(62, 20)
		Me.toolStripMenuItem1.Text = "Įrašyti..."
		AddHandler Me.toolStripMenuItem1.Click, AddressOf Me.ToolStripMenuItem1Click
		'
		'toolStripMenuItem2
		'
		Me.toolStripMenuItem2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
		Me.toolStripMenuItem2.Name = "toolStripMenuItem2"
		Me.toolStripMenuItem2.Size = New System.Drawing.Size(52, 20)
		Me.toolStripMenuItem2.Text = "Apie..."
		AddHandler Me.toolStripMenuItem2.Click, AddressOf Me.ToolStripMenuItem2Click
		'
		'peržiūrėtiToolStripMenuItem
		'
		Me.peržiūrėtiToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.peržiūrėtiToolStripMenuItem.Name = "peržiūrėtiToolStripMenuItem"
		Me.peržiūrėtiToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5
		Me.peržiūrėtiToolStripMenuItem.Size = New System.Drawing.Size(64, 20)
		Me.peržiūrėtiToolStripMenuItem.Text = "Peržiūrėti"
		Me.peržiūrėtiToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay
		AddHandler Me.peržiūrėtiToolStripMenuItem.Click, AddressOf Me.PeržiūrėtiToolStripMenuItemClick
		'
		'imageList1
		'
		Me.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
		Me.imageList1.ImageSize = New System.Drawing.Size(16, 16)
		Me.imageList1.TransparentColor = System.Drawing.Color.Transparent
		'
		'panel1
		'
		Me.panel1.Controls.Add(Me.tabControl1)
		Me.panel1.Controls.Add(Me.webBrowser)
		Me.panel1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.panel1.Location = New System.Drawing.Point(0, 24)
		Me.panel1.Name = "panel1"
		Me.panel1.Size = New System.Drawing.Size(552, 449)
		Me.panel1.TabIndex = 4
		'
		'tabControl1
		'
		Me.tabControl1.Controls.Add(Me.tabPage1)
		Me.tabControl1.Controls.Add(Me.tabPage2)
		Me.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tabControl1.Location = New System.Drawing.Point(0, 0)
		Me.tabControl1.Name = "tabControl1"
		Me.tabControl1.SelectedIndex = 0
		Me.tabControl1.Size = New System.Drawing.Size(552, 449)
		Me.tabControl1.TabIndex = 1
		'
		'tabPage1
		'
		Me.tabPage1.Controls.Add(Me.tableLayoutPanel2)
		Me.tabPage1.Location = New System.Drawing.Point(4, 22)
		Me.tabPage1.Name = "tabPage1"
		Me.tabPage1.Padding = New System.Windows.Forms.Padding(3)
		Me.tabPage1.Size = New System.Drawing.Size(544, 423)
		Me.tabPage1.TabIndex = 0
		Me.tabPage1.Text = "Paveikslėliai-Atsakymai"
		Me.tabPage1.UseVisualStyleBackColor = true
		'
		'tableLayoutPanel2
		'
		Me.tableLayoutPanel2.ColumnCount = 1
		Me.tableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
		Me.tableLayoutPanel2.Controls.Add(Me.dataGridView1, 0, 1)
		Me.tableLayoutPanel2.Controls.Add(Me.panel2, 0, 0)
		Me.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
		Me.tableLayoutPanel2.Name = "tableLayoutPanel2"
		Me.tableLayoutPanel2.RowCount = 2
		Me.tableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80!))
		Me.tableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle)
		Me.tableLayoutPanel2.Size = New System.Drawing.Size(538, 417)
		Me.tableLayoutPanel2.TabIndex = 0
		'
		'dataGridView1
		'
		Me.dataGridView1.AllowUserToAddRows = false
		Me.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.dataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Image, Me.Answer})
		Me.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.dataGridView1.Location = New System.Drawing.Point(3, 83)
		Me.dataGridView1.Name = "dataGridView1"
		Me.dataGridView1.RowTemplate.Height = 66
		Me.dataGridView1.Size = New System.Drawing.Size(534, 331)
		Me.dataGridView1.TabIndex = 3
		AddHandler Me.dataGridView1.CellDoubleClick, AddressOf Me.DataGridView1CellDoubleClick
		AddHandler Me.dataGridView1.CellContentDoubleClick, AddressOf Me.DataGridView1CellContentDoubleClick
		'
		'Image
		'
		Me.Image.HeaderText = "Paveiksliukas"
		Me.Image.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
		Me.Image.Name = "Image"
		Me.Image.Width = 88
		'
		'Answer
		'
		Me.Answer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
		Me.Answer.HeaderText = "Atsakymas"
		Me.Answer.Name = "Answer"
		'
		'panel2
		'
		Me.panel2.Controls.Add(Me.button3)
		Me.panel2.Controls.Add(Me.button2)
		Me.panel2.Controls.Add(Me.textBox2)
		Me.panel2.Controls.Add(Me.label2)
		Me.panel2.Controls.Add(Me.pictureBox1)
		Me.panel2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.panel2.Location = New System.Drawing.Point(3, 3)
		Me.panel2.Name = "panel2"
		Me.panel2.Size = New System.Drawing.Size(534, 74)
		Me.panel2.TabIndex = 1
		'
		'button3
		'
		Me.button3.Location = New System.Drawing.Point(97, 45)
		Me.button3.Name = "button3"
		Me.button3.Size = New System.Drawing.Size(132, 23)
		Me.button3.TabIndex = 6
		Me.button3.Text = "Pasirinkti paveiksliuką..."
		Me.button3.UseVisualStyleBackColor = true
		AddHandler Me.button3.Click, AddressOf Me.Button3Click
		'
		'button2
		'
		Me.button2.Enabled = false
		Me.button2.Location = New System.Drawing.Point(325, 45)
		Me.button2.Name = "button2"
		Me.button2.Size = New System.Drawing.Size(75, 23)
		Me.button2.TabIndex = 5
		Me.button2.Text = "Pridėti"
		Me.button2.UseVisualStyleBackColor = true
		AddHandler Me.button2.Click, AddressOf Me.Button2Click
		'
		'textBox2
		'
		Me.textBox2.AcceptsReturn = true
		Me.textBox2.Location = New System.Drawing.Point(97, 19)
		Me.textBox2.Name = "textBox2"
		Me.textBox2.Size = New System.Drawing.Size(303, 20)
		Me.textBox2.TabIndex = 4
		AddHandler Me.textBox2.TextChanged, AddressOf Me.TextBox2TextChanged
		AddHandler Me.textBox2.KeyUp, AddressOf Me.TextBox2KeyUp
		'
		'label2
		'
		Me.label2.AutoSize = true
		Me.label2.Location = New System.Drawing.Point(97, 3)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(70, 13)
		Me.label2.TabIndex = 3
		Me.label2.Text = "Pavadinimas:"
		'
		'pictureBox1
		'
		Me.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pictureBox1.Location = New System.Drawing.Point(3, 3)
		Me.pictureBox1.Name = "pictureBox1"
		Me.pictureBox1.Size = New System.Drawing.Size(88, 65)
		Me.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.pictureBox1.TabIndex = 2
		Me.pictureBox1.TabStop = false
		AddHandler Me.pictureBox1.Click, AddressOf Me.PictureBox1Click
		'
		'tabPage2
		'
		Me.tabPage2.Controls.Add(Me.tableLayoutPanel1)
		Me.tabPage2.Location = New System.Drawing.Point(4, 22)
		Me.tabPage2.Name = "tabPage2"
		Me.tabPage2.Padding = New System.Windows.Forms.Padding(3)
		Me.tabPage2.Size = New System.Drawing.Size(544, 423)
		Me.tabPage2.TabIndex = 1
		Me.tabPage2.Text = "Papildomi Atsakymai"
		Me.tabPage2.UseVisualStyleBackColor = true
		'
		'tableLayoutPanel1
		'
		Me.tableLayoutPanel1.ColumnCount = 1
		Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
		Me.tableLayoutPanel1.Controls.Add(Me.listBox1, 0, 1)
		Me.tableLayoutPanel1.Controls.Add(Me.panel3, 0, 0)
		Me.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
		Me.tableLayoutPanel1.Name = "tableLayoutPanel1"
		Me.tableLayoutPanel1.RowCount = 2
		Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30!))
		Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
		Me.tableLayoutPanel1.Size = New System.Drawing.Size(538, 417)
		Me.tableLayoutPanel1.TabIndex = 5
		'
		'listBox1
		'
		Me.listBox1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.listBox1.FormattingEnabled = true
		Me.listBox1.IntegralHeight = false
		Me.listBox1.Location = New System.Drawing.Point(3, 33)
		Me.listBox1.Name = "listBox1"
		Me.listBox1.Size = New System.Drawing.Size(534, 400)
		Me.listBox1.TabIndex = 6
		AddHandler Me.listBox1.KeyUp, AddressOf Me.ListBox1KeyUp
		'
		'panel3
		'
		Me.panel3.Controls.Add(Me.button1)
		Me.panel3.Controls.Add(Me.label1)
		Me.panel3.Controls.Add(Me.textBox1)
		Me.panel3.Dock = System.Windows.Forms.DockStyle.Top
		Me.panel3.Location = New System.Drawing.Point(3, 3)
		Me.panel3.Name = "panel3"
		Me.panel3.Size = New System.Drawing.Size(534, 24)
		Me.panel3.TabIndex = 5
		'
		'button1
		'
		Me.button1.Enabled = false
		Me.button1.Location = New System.Drawing.Point(338, 1)
		Me.button1.Name = "button1"
		Me.button1.Size = New System.Drawing.Size(75, 23)
		Me.button1.TabIndex = 2
		Me.button1.Text = "Pridėti"
		Me.button1.UseVisualStyleBackColor = true
		AddHandler Me.button1.Click, AddressOf Me.Button1Click
		'
		'label1
		'
		Me.label1.AutoSize = true
		Me.label1.Location = New System.Drawing.Point(6, 4)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(70, 13)
		Me.label1.TabIndex = 1
		Me.label1.Text = "Pavadinimas:"
		'
		'textBox1
		'
		Me.textBox1.Location = New System.Drawing.Point(80, 1)
		Me.textBox1.Name = "textBox1"
		Me.textBox1.Size = New System.Drawing.Size(252, 20)
		Me.textBox1.TabIndex = 0
		AddHandler Me.textBox1.TextChanged, AddressOf Me.TextBox1TextChanged
		AddHandler Me.textBox1.KeyUp, AddressOf Me.TextBox1KeyUp
		'
		'webBrowser
		'
		Me.webBrowser.AllowWebBrowserDrop = false
		Me.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill
		Me.webBrowser.IsWebBrowserContextMenuEnabled = false
		Me.webBrowser.Location = New System.Drawing.Point(0, 0)
		Me.webBrowser.MinimumSize = New System.Drawing.Size(20, 20)
		Me.webBrowser.Name = "webBrowser"
		Me.webBrowser.ScriptErrorsSuppressed = true
		Me.webBrowser.ScrollBarsEnabled = false
		Me.webBrowser.Size = New System.Drawing.Size(552, 449)
		Me.webBrowser.TabIndex = 4
		Me.webBrowser.Visible = false
		Me.webBrowser.WebBrowserShortcutsEnabled = false
		'
		'MainForm
		'
		Me.AllowDrop = true
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(552, 473)
		Me.Controls.Add(Me.panel1)
		Me.Controls.Add(Me.menuStrip1)
		Me.MinimumSize = New System.Drawing.Size(560, 500)
		Me.Name = "MainForm"
		Me.Text = "PTKūrėjas"
		AddHandler DragDrop, AddressOf Me.MainFormDragDrop
		AddHandler DragEnter, AddressOf Me.MainFormDragOver
		Me.menuStrip1.ResumeLayout(false)
		Me.menuStrip1.PerformLayout
		Me.panel1.ResumeLayout(false)
		Me.tabControl1.ResumeLayout(false)
		Me.tabPage1.ResumeLayout(false)
		Me.tableLayoutPanel2.ResumeLayout(false)
		CType(Me.dataGridView1,System.ComponentModel.ISupportInitialize).EndInit
		Me.panel2.ResumeLayout(false)
		Me.panel2.PerformLayout
		CType(Me.pictureBox1,System.ComponentModel.ISupportInitialize).EndInit
		Me.tabPage2.ResumeLayout(false)
		Me.tableLayoutPanel1.ResumeLayout(false)
		Me.panel3.ResumeLayout(false)
		Me.panel3.PerformLayout
		Me.ResumeLayout(false)
		Me.PerformLayout
	End Sub
	Private panel3 As System.Windows.Forms.Panel
	Private webBrowser As System.Windows.Forms.WebBrowser
	Private peržiūrėtiToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private toolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
	Private atidarytiToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private toolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
	Private imageList1 As System.Windows.Forms.ImageList
	Private Answer As System.Windows.Forms.DataGridViewTextBoxColumn
	Private Image As System.Windows.Forms.DataGridViewImageColumn
	Private dataGridView1 As System.Windows.Forms.DataGridView
	Private menuStrip1 As System.Windows.Forms.MenuStrip
	Private textBox1 As System.Windows.Forms.TextBox
	Private label1 As System.Windows.Forms.Label
	Private button1 As System.Windows.Forms.Button
	Private panel1 As System.Windows.Forms.Panel
	Private listBox1 As System.Windows.Forms.ListBox
	Private tableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
	Private tabPage2 As System.Windows.Forms.TabPage
	Private pictureBox1 As System.Windows.Forms.PictureBox
	Private label2 As System.Windows.Forms.Label
	Private textBox2 As System.Windows.Forms.TextBox
	Private button2 As System.Windows.Forms.Button
	Private button3 As System.Windows.Forms.Button
	Private panel2 As System.Windows.Forms.Panel
	Private tableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
	Private tabPage1 As System.Windows.Forms.TabPage
	Private tabControl1 As System.Windows.Forms.TabControl
End Class
