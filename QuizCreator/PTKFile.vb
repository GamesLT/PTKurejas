'
' Created by SharpDevelop.
' User: Administrator
' Date: 2009.03.07
' Time: 11:56
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.

Imports System.IO
Imports Ionic.Zip.ZipFile

Namespace PTK.File	
Public Class PTKFile

	Private _FileName As String = Nothing 	
	Private _Data1 As New Collections.Generic.SortedDictionary(Of String, Image)()
	Private _Data2 As New Collections.Generic.List(Of String)()	
	Private _State As States = States.Idling
	Private WithEvents _tc As TransferControl = TransferControl.Instance			
	
	Public Property FileName As String
		Get
			Return Me._FileName
		End Get
		Set(ByVal Value As String)
			Me._FileName = Value
		End Set
	End Property
	
	Public ReadOnly Property ImageAnswersItems As Collections.Generic.SortedDictionary(Of String, Image)
		Get
			Return Me._Data1
		End Get
	End Property
	
	Public ReadOnly Property OtherAnswersItems As Collections.Generic.List(Of String)
		Get
			Return Me._Data2
		End Get
	End Property		
	
	Public ReadOnly Property State As States
		Get
			Return Me._State			
		End Get
	End Property
	
	Public Enum States As SByte
		Idling = 0
		Working = 1
	End Enum
	
	Public Sub Open(Optional FileName As String = Nothing) 
		Me._State = States.Working
		If filename Is Nothing Or filename.Trim() = "" Then 
			If Me.filename Is Nothing Or Me.filename.Trim() = "" Then 	
				Me._State = States.Idling
				Exit Sub 				
			Else
				Filename = Me.FileName				
			End If			
		Else
			Me.FileName = FileName			
		End If				
		
		Me.ImageAnswersItems.Clear()
		me.OtherAnswersItems.Clear()
		
		Dim zip As Ionic.Zip.ZipFile = Ionic.Zip.ZipFile.Read(filename)  'ZipStorer.Open(filename, System.IO.FileAccess.Read)
		
		Dim Images As New Collections.Generic.SortedDictionary(Of String, Image)		
		Dim fInfo As FileInfo
		Dim jsText As String = ""
		Dim ms As MemoryStream, sr as StreamReader
		
		Me._tc.Begin(Me)		
		
		' Renkame reikalingà turiná ið archyvo		
		'Dim dir As List(Of ZipStorer.ZipFileEntry) = zip.ReadCentralDir()
		For Each entry As Ionic.Zip.ZipEntry In zip.Entries
			fInfo = New FileInfo(entry.FileName)				
			Select Case fInfo.Extension.ToLower()
				Case ".png", ".gif", ".jpeg", ".jpg", ".jpe"
					ms = New MemoryStream()
					entry.Extract(ms)
					ms.Position = 0
					Me._tc.AddImage(entry.filename, Image.FromStream(ms))					
					'tmpFileName = System.IO.Path.GetTempFileName()
'					zip.Extract(entry.FileName, tmpFileName, True)
					'zip.ExtractStoredFile(entry, tmpFileName)					
'					System.IO.File.Delete(tmpFileName)
				Case ".js"					
					If Path.GetFileName(entry.filename) = "quiz-data.js" Then						
						ms = New MemoryStream()
						entry.Extract(ms)
						ms.Position = 0
						sr = New StreamReader(ms)	
						jsText = sr.ReadToEnd()											
					End If
			End Select    		
		Next

            ' Skaitome kintamøjø reikðmes ið quiz-data.js failo
            Dim wb As New WebBrowser()
            wb.ObjectForScripting = Me._tc
            wb.ScriptErrorsSuppressed = False
            Dim script As String = My.Resources.open_file_script.Replace("jsText", jsText)
            Dim s2 As String = Me.GetBasicHTMLFile("<script type=""text/javascript"">" + script + "</script>")
            Clipboard.SetText(s2)
            wb.DocumentText = s2
        End Sub
	
	Private Function GetQuizJS(ByRef possibleAnswers As Collections.Generic.List(Of String), Optional ImageFileNames As Collections.Specialized.StringCollection = Nothing) As String		
		Dim sk As New StringWriter()
		Dim I As Integer 
		Dim tmpFile As String
		
		If ImageFileNames IsNot Nothing Then
			If ImageFileNames.Count < 1 Then
				ImageFileNames = Nothing				
			End If
		End If
		
		sk.WriteLine("/**")		
		sk.WriteLine(" * @CreatorName:		" + my.VersionInfo.AppName)
		sk.WriteLine(" * @CreatorVersion:	" + my.VersionInfo.Full)
		sk.WriteLine(" * @CreatorUrl:		" + my.VersionInfo.MoreInfoURL.ToString() )
		sk.WriteLine(" * @DateCreation:		" + Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString() )
		sk.WriteLine(" */" )
		sk.WriteLine("" )
            sk.WriteLine("var quiz_photo_data =")
            sk.WriteLine("  ptkurejas_quiz({")
            sk.WriteLine("     ""answers"" : [" )
		I = 0
		For Each Key As String In possibleAnswers
			i += 1
			sk.Write( "		 '" )
			sk.Write( key.Replace("'", "\'") )
			sk.Write( "'" )
			If i < possibleAnswers.Count Then
				sk.Write(",")				
			End If
			sk.WriteLine()
		Next
		sk.WriteLine("	 ] ," )
		sk.WriteLine("	 ""images"" : {" )
		i = 0		
		For Each Key As String In Me.ImageAnswersItems.Keys
			If ImageFileNames Is Nothing Then
				tmpFile = I.ToString() + ".png"
			Else
				tmpFile = ImageFileNames.Item(I)				
			End If			
			sk.Write( "		" )
			sk.Write( possibleAnswers.IndexOf(key) )
			sk.Write( " : '" )
			sk.Write( tmpFile )
			sk.Write( "'"  )
			i += 1
			If i < me.ImageAnswersItems.Count Then
				sk.Write(",")	
			End If	
			sk.WriteLine()
		Next
		sk.WriteLine("	 }")
            sk.WriteLine("  });")
            Return sk.tostring()			
	End Function	
	
	Private Sub ResizeImageIfNeeded(ByRef Image As Image)
		If Image.Width > 440 Then						
			Dim Bitmap As New Bitmap( Convert.ToInt32(440), Convert.ToInt32(Image.Height * (440 / Image.Width)))			
			Using gr As Graphics = Graphics.FromImage(Bitmap)
				gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality
				gr.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy
				gr.DrawImage(image, 0, 0, bitmap.Width, bitmap.Height)					
			End Using
			Image = image.FromHbitmap(Bitmap.GetHbitmap())
		End If
	End Sub

        Private Function GetCoreJS() As String
            Return My.Resources.quiz_core
        End Function

        Private Function GetPossibleAnswersList() As Collections.Generic.List(Of String)
            Dim possibleAnswers As New Collections.Generic.List(Of String)
            For Each Key As String In Me.ImageAnswersItems.Keys
                possibleAnswers.Add(Key)
            Next
            For Each Key As String In Me.OtherAnswersItems
                possibleAnswers.Add(Key)
            Next
            possibleAnswers.Sort()
            Return possibleAnswers
        End Function

        Private Function GetBasicHTMLFile(Optional BodyHTML As String = "", Optional Title as String = "Quiz Test") As String		
		Dim tmpData as String 
		tmpData ="<!DOCTYPE html" + Environment.NewLine
		tmpData +="PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" " + Environment.NewLine
		tmpData +="""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">" + Environment.NewLine
		tmpData +="<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""lt"" lang=""lt"">" + Environment.NewLine
		tmpData +="	<head>" + Environment.NewLine
		tmpData +="		<title>" + Title + "</title>" + Environment.NewLine
		tmpData +="	</head>" + Environment.NewLine
		tmpData +="	<body>" + Environment.NewLine
		tmpData += BodyHTML + Environment.NewLine
		tmpData +="	</body>" + Environment.NewLine
		tmpData +="</html>" + Environment.NewLine
		Return tmpData		
	End Function
	
	Public Function GeneratePreviewHTML() As String						
		'Iðsaugo laikinus paveikslëlius
		Dim tmpFile As String, tmpFiles As New Collections.Specialized.StringCollection()		
		Dim Image as Image, LocPaths() as String, Url as String 
		For Each Key As String In Me.ImageAnswersItems.Keys			
			tmpFile = system.IO.Path.GetTempFileName() ' + ".png"
                LocPaths = tmpFile.Split("\"c)
                Url = "file:///" 
			For Each Path As String In LocPaths
				If path.EndsWith(":") Then
					Url += path.Replace(":", "|")					
				Else
					Url += web.HttpUtility.UrlPathEncode(path)
				End If
				Url += "/"
			Next
			tmpFiles.Add(url.Substring(0, url.Length - 1))			
			Image = Me.ImageAnswersItems.Item(key)			
			Me.ResizeImageIfNeeded(image)			
			Image.Save(tmpFile, System.Drawing.Imaging.ImageFormat.Png)				
		Next
		
		'Sukûriame atsakymø masyvà
		Dim possibleAnswers As Collections.Generic.List(Of String) = Me.GetPossibleAnswersList()
		
		Dim tmpData As String = "<table border=""0"" style=""width: 100%; height: 100%;""><tr><td style=""vertical-align: middle; text-align: center;""><script type=""text/javascript"">" + Environment.NewLine
		tmpData += "var quiz_photo_prv_here = true;" + Environment.NewLine
		tmpData += Me.GetQuizJS(possibleAnswers, tmpFiles) + Environment.NewLine
		tmpData += Me.GetCoreJS() + Environment.NewLine
		tmpData += "</script></td></tr></table>"
		
		Return Me.GetBasicHTMLFile(tmpData)		
	End Function
	
	Public Sub Save(Optional FileName As String = Nothing) 
		Me._State = States.Working
		If filename Is Nothing Or filename.Trim() = "" Then 
			If Me.filename Is Nothing Or Me.filename.Trim() = "" Then 	
				Me._State = States.Idling
				Exit Sub 				
			Else
				Filename = Me.FileName				
			End If			
		Else
			Me.FileName = FileName			
		End If	
				
		If system.IO.File.Exists(filename) Then
			system.IO.File.Delete(filename)			
		End If
		
		Dim zip As Ionic.Zip.ZipFile = new Ionic.Zip.ZipFile(filename)  'ZipStorer.Create(filename, "")
		Dim tmpFile As String, tmpFile2 As String
		Dim Image As Image
		Dim I As Integer
		Dim tmpData as String
		
		'zip.CompressionLevel = CompressionLevel.LEVEL9_BEST_COMPRESSION
		'zip.Password = "MekDrop"
		zip.UseUnicodeAsNecessary = True		
		
		Dim ms As MemoryStream
		
		' Krauname paveiksliukus á archyvà				
		I = 0
		For Each Key As String In Me.ImageAnswersItems.Keys
			tmpFile2 = I.ToString() + ".png"
			tmpFile = system.IO.Path.GetTempFileName()
			Image = Me.ImageAnswersItems.Item(key)			
			Me.ResizeImageIfNeeded(image)			
			ms = New MemoryStream()			
			Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
			ms.Position = 0
			zip.AddFileStream(tmpFile2, "", ms)
			'Image.Save(tmpFile, System.Drawing.Imaging.ImageFormat.Png)
			'ms.Read(buffer, 0, ms.Length)			
			'zip.AddFile(tmpFile, tmpFile2)
			'System.IO.File.Delete(tmpfile)	
			I += 1
		Next
		
		'Sukûriame atsakymø masyvà
		Dim possibleAnswers As Collections.Generic.List(Of String) = Me.GetPossibleAnswersList()		
		
		'Sukûriame atsakymø informaciná failà		
		zip.AddFileFromString("quiz-data.js", "", me.GetQuizJS(possibleAnswers))		
		
		'Pridedame paveikslëliø testo varikliukà (dël viso pikto)
		'Dim wc As New System.Net.WebClient()
		zip.AddFileFromString("quiz-core.js", "", me.GetCoreJS())

            'Pridedame testiná HTML failà
            tmpFile = System.IO.Path.GetTempFileName()
            tmpData = "		<script type=""text/javascript"" src=""quiz-core.js""></script>" + Environment.NewLine
            tmpData += "	<script type=""text/javascript"" src=""quiz-data.js""></script>" + Environment.NewLine
            zip.AddFileFromString("quiz-test.html", "", Me.GetBasicHTMLFile(tmpData))
				
		zip.Save(filename)
		
		Me._State = States.Idling
		
	End Sub	
	
	Private Sub FinishedLoading() Handles _tc.Finished
		Me._state = states.Idling		
	End Sub
	
End Class
End Namespace 