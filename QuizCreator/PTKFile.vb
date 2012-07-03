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
		Dim NewLine As String = Environment.NewLine		
		Dim rez as String 
		wb.ObjectForScripting = Me._tc		
		wb.ScriptErrorsSuppressed = True		
		rez = Me.GetBasicHTMLFile("		<script type=""text/javascript"">" & _
						  "         //<!--" + NewLine _
						+ "		  try {" + NewLine _
						+ jsText + NewLine _
						+ "		  for (i=0; i<quiz_photo_data.answers.length; i++) { " + NewLine _
						+ "	          if (!quiz_photo_data.images[i]) { " + NewLine _
						+ "  	   	      window.external.AddAnswer(quiz_photo_data.answers[i]);" + NewLine _
						+ "  	      } else {" + NewLine _
						+ "  	  	      window.external.AddImage(quiz_photo_data.answers[i], quiz_photo_data.images[i]);" + NewLine _
						+ "  	      }" + NewLine _
						+ "		   } " + NewLine _
						+ "		   } catch(err) { " & NewLine _
						+ "           " + NewLine _
						+ "        } " + NewLine _
						+ "        window.external.Finish();" + NewLine _
						+ "         //--></script>" + NewLine)									
		wb.DocumentText = rez
		
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
		sk.WriteLine("var quiz_photo_data =" )
		sk.WriteLine("  {" )
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
		sk.WriteLine("  };")
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
		Using wc As System.Net.WebClient = New System.Net.WebClient()			
			Try
				Return wc.DownloadString(my.VersionInfo.LatestQuizCoreUrl)				
			Catch ex As Exception
				Return web.HttpUtility.UrlDecode("function+quiz_DisplayTimer(interval%2c+message%2c+func%2c+func_chk)+%7b%0d%0a%09if+(!(!func_chk)+%26%26+func_chk())+%7b%0d%0a%09%09func()%3b%0d%0a%09%09return%3b%0d%0a%09%7d%0d%0a%09var+k+%3d+message+%2b+%22%22%3b%0d%0a%09k+%3d+k.replace(%22%25d%22%2c+interval)%3b%0d%0a%09quiz_UpdateView(k)%3b%0d%0a%09interval--%3b%0d%0a%09if+(interval+%3e+-2)+%7b%0d%0a%09%09setTimeout(function()+%7bquiz_DisplayTimer(interval%2c+message%2c+func)%3b%7d%2c+1000)%3b%0d%0a%09%7d+else+%7b%0d%0a%09%09func()%3b%0d%0a%09%7d%0d%0a%7d%0d%0a%0d%0afunction+quiz_showError(text)+%7b%0d%0a%09var+obj+%3d+document.getElementById('photo_quiz')%0d%0a%09obj.innerHTML+%3d+'%3cb%3e'+%2b+unescape('Klaida%253A')+%2b+'%3c%2fb%3e+'+%2b+text%3b%0d%0a%09obj.style.borderWidth+%3d+'0px'%3b%0d%0a%09obj.style.borderStyle+%3d+'none'%3b%0d%0a%09obj.style.backgroundColor+%3d+'transparent'%3b%0d%0a%09obj.style.color+%3d+'auto'%3b%0d%0a%09obj.style.height+%3d+'auto'%3b%0d%0a%7d%0d%0a%0d%0afunction+quiz_UpdateView(content)+%7b%0d%0a%09var+obj+%3d+document.getElementById('photo_quiz')%3b%0d%0a%09obj.innerHTML+%3d+content%3b%0d%0a%7d%0d%0a%0d%0afunction+quiz_addEvent(el%2c+event%2c+myFunction)+%7b%0d%0a%09if+(el.addEventListener)+%7b%0d%0a%09%09el.addEventListener+(event%2cmyFunction%2cfalse)%3b%0d%0a%09%7d+else+if+(el.attachEvent)+%7b%0d%0a%09%09el.attachEvent+(%22on%22+%2b+event%2cmyFunction)%3b%09%09%0d%0a%09%7d+else+%7b%0d%0a%09%09eval(%22el.on%22+%2b+event+%2b+%22+%3d+myFunction%3b%22)%3b%0d%0a%09%7d%0d%0a%7d%0d%0a%0d%0afunction+quiz_showMain()+%7b%0d%0a%09var+gal+%3d+'ai'%3b%0d%0a%09if+((quiz_count+%3e+10+%26%26+quiz_count+%3c%3d+20)+%7c%7c+(quiz_count+%25+10+%3d%3d+0)++)+%7b%0d%0a%09%09gal+%3d+unescape('%2526%2523371%253B')%3b%0d%0a%09%7d+else+if+(+quiz_count+%25+10+%3d%3d+1+)+%7b%0d%0a%09%09gal+%3d+'as'%3b%0d%0a%09%7d+%0d%0a%09var+txt+%3d+'%3cdiv+style%3d%22text-align%3a+left%3b%22%3e%3cb%3eApie%3c%2fb%3e%3cbr+%2f%3e%3cbr+%2f%3e%3cul%3e'%3b%0d%0a%09txt++++%2b%3d+'%3cli%3e'+%2b+unescape('%25u0160io%2520testo%2520metu%252C%2520Jums%2520bus%2520rodomi%2520paveiksl%2526%2523279%253Bliai%2520i%25u0161%2520%2526%2523303%253Bvairi%2526%2523371%253B%2520%25u017Eaidim%2526%2523371%253B.')+%2b++'%3c%2fli%3e'%3b%0d%0a%09txt++++%2b%3d+'%3cli%3e'+%2b+unescape('Jums%2520reik%2526%2523279%253Bs%2520nuspr%2526%2523281%253Bsti%252C%2520i%25u0161%2520kokio%2520%25u017Eaidimo%2520kiekvienas%2520paveiksl%2526%2523279%253Blis.')+%2b++'%3c%2fli%3e'%3b%0d%0a%09txt++++%2b%3d+'%3cli%3e'+%2b+unescape('Atsakius%2520klausim%2526%2523261%253B%2520jo%2520atsakymo%2520koreguoti%2520nebebus%2520galima%2520-%2520tod%2526%2523279%253Bl%2520atid%25u017Eiai%2520pasirinkite%2520atsakymus%2521')+%2b++'%3c%2fli%3e'%3b%0d%0a%09txt++++%2b%3d+'%3cli%3e'+%2b+unescape('%25u0160io%2520testo%2520rezultatai%2520nebus%2520i%25u0161saugoti%2520jokio%2520duomen%2526%2523371%253B%2520baz%2526%2523279%253Bje.')+%2b++'%3c%2fli%3e'%3b%0d%0a%09txt++++%2b%3d+'%3cli%3e'+%2b+unescape('Perkrovus%2520puslap%2526%2523303%253B%2520testas%2520prasid%2526%2523279%253Bs%2520nuo%2520prad%25u017Ei%2526%2523371%253B.')+%2b++'%3c%2fli%3e'%3b%0d%0a%09txt++++%2b%3d+'%3cli%3e'+%2b+unescape('Teste%2520bus%2520i%25u0161%2520viso')+%2b++'+'%2b+quiz_count++%2b'+klausim'+%2b+gal+%2b++'.%3c%2fli%3e'%3b%0d%0a%09txt++++%2b%3d+'%3c%2ful%3e%3c%2fdiv%3e%3cp+style%3d%22text-align%3a+center%3b%22%3e'%3b%0d%0a%09txt++++%2b%3d+'%3cbutton+onclick%3d%22quiz_begin()%3b%22+accesskey%3d%22p%22%3e'+%2b+unescape('Prad%2526%2523279%253Bti')+%2b++'%3c%2fbutton%3e'%3b%0d%0a%09txt++++%2b%3d+'%3c%2fp%3e'%3b%0d%0a%09quiz_UpdateView(txt)%3b%0d%0a%7d%0d%0a%0d%0avar+quiz_Images+%3d+new+Array()%3b%0d%0avar+quiz_n_to_load+%3d+0%3b%0d%0avar+quiz_count+%3d+0%3b%0d%0avar+quiz_options_list+%3d+''%3b%0d%0avar+quiz_cquestion+%3d+0%3b%0d%0avar+quiz_answers_all_list+%3d+new+Array()%3b%0d%0avar+quiz_opacity_timer+%3d+window.setInterval(quiz_opacityMakeBigger%2c+10)%3b%0d%0avar+quiz_opacity+%3d+0%3b%0d%0avar+quiz_marquee_top+%3d+0%3b%0d%0a%0d%0afunction+quiz_QuizDataExists()+%7b%0d%0a%09if+(!quiz_photo_data)+%7b%0d%0a%09%09return+false%3b%0d%0a%09%7d%0d%0a%09return+true%3b%0d%0a%7d%0d%0a%0d%0afunction+quiz_begin()+%7b%0d%0a%09quiz_cquestion+%3d+0%3b%0d%0a%09quiz_answers_all_list+%3d+new+Array()%3b%0d%0a%09quiz_showNext()%3b%0d%0a%7d%0d%0a%0d%0afunction+quiz_nextLoadStep()+%7b%0d%0a%09if+(!quiz_photo_data)+%7b%0d%0a%09%09return+quiz_showError(+unescape('nepavyko%2520atsi%2526%2523371%253Bsti%2520%25u0161iam%2520testui%2520reikaling%2526%2523371%253B%2520duomen%2526%2523371%253B%2520i%25u0161%2520serverio.')+)%3b%09%09%0d%0a%09%7d%0d%0a%0d%0a%09var+list+%3d+new+Array()%3b%0d%0a%09var+llist+%3d+new+Array()%3b%0d%0a%09for+(i%3d0%3bi%3cquiz_photo_data.answers.length%3bi%2b%2b)+%7b%0d%0a%09%09list%5bi%5d+%3d+quiz_photo_data.answers%5bi%5d%3b%0d%0a%09%09llist%5bquiz_photo_data.answers%5bi%5d%5d+%3d+i%3b%0d%0a%09%7d%0d%0a%09list.sort()%3b%0d%0a%0d%0a%09quiz_options_list+%2b%3d+'%3cselect+id%3d%22quiz_answer%22%3e'%3b%0d%0a%09for+(i%3d0%3bi%3clist.length%3bi%2b%2b)+%7b%0d%0a%09%09quiz_options_list+%2b%3d+'%3coption+value%3d%22'+%2b+llist%5blist%5bi%5d%5d+%2b+'%22%3e'%3b%0d%0a%09%09quiz_options_list+%2b%3d+list%5bi%5d%3b%0d%0a%09%09quiz_options_list+%2b%3d+'%3c%2foption%3e'%3b%09%0d%0a%09%7d%0d%0a%09quiz_options_list+%2b%3d+'%3c%2fselect%3e'%3b%0d%0a%0d%0a%09quiz_loadImages()%3b%0d%0a%7d%0d%0a%0d%0a%0d%0a%0d%0afunction+quiz_opacityMakeBigger()+%7b%0d%0a%09var+obj+%3d+document.getElementById('quiz_oda')%3b%0d%0a%09if+(!obj)+return%3b%0d%0a%09if+(!obj.style.opacity)+return%3b%0d%0a%09if+(quiz_opacity+%3e+100)+return%3b%0d%0a%09obj.style.opacity+%3d+(quiz_opacity%2b%3d2)+%2f+100%3b%0d%0a%7d%0d%0a%0d%0afunction+quiz_showNext()+%7b%0d%0a%09quiz_cquestion%2b%2b%3b%0d%0a%09if+(quiz_cquestion+%3e+quiz_count)+%7b%0d%0a%09%09return+quiz_finish()%3b%0d%0a%09%7d%0d%0a%09var+txt+%3d+'%3cdiv+style%3d%22text-align%3a+left%3b%22%3e%3cb%3eKlausimas+'+%2b+quiz_cquestion+%2b+unescape('%2520i%25u0161%2520')+%2b+quiz_count+%2b+'+%3c%2fb%3e%3cdiv+style%3d%22width%3a+440px%3b+height%3a+330px%3b+border-style%3a+solid%3b+border-width%3a+1px%3b+position%3a+relative%3b+left%3a+4px%3b+background-color%3a+black%3b%22%3e'%3b%0d%0a+%09txt++++%2b%3d+'%3cdiv+id%3d%22quiz_oda%22+style%3d%22background-image%3a+url('+%2b+quiz_Images%5bquiz_cquestion-1%5d.src+%2b+')%3b+width%3a+440px%3b+height%3a+330px%3b+background-repeat%3a+no-repeat%3b+background-color%3a+black%3b+background-position%3a+center+center%3b++opacity%3a+0.0%3b%22%3e%3c%2fdiv%3e%3c%2fdiv%3e'%3b%0d%0a%09txt++++%2b%3d+unescape('I%25u0161%2520kurio%2520%25u017Eaidimo%2520matomas%2520vaizdas%2520paveiksl%2526%2523279%253Blyje%253F')+%2b'+%3cbr+%2f%3e'%3b%0d%0a%09txt%09+++%2b%3d+quiz_options_list%3b%0d%0a%09txt%09+++%2b%3d+'%3cbutton+onclick%3d%22quiz_record_answer()%3b%22+accesskey%3d%22enter%22%3eAtsakyti%3c%2fbutton%3e'%3b%0d%0a%09txt++++%2b%3d+'%3c%2fdiv%3e'%3b%0d%0a%09quiz_UpdateView(txt)%3b%0d%0a%09quiz_opacity+%3d+0%3b%0d%0a%7d%0d%0a%0d%0afunction+quiz_record_answer()+%7b%0d%0a%09var+answer_list+%3d+document.getElementById('quiz_answer')%3b%0d%0a%09quiz_answers_all_list%5bquiz_answers_all_list.length%5d+%3d+answer_list.options%5banswer_list.selectedIndex%5d.value%3b%0d%0a%09quiz_showNext()%3b%0d%0a%7d%0d%0a%0d%0afunction+quiz_onLoadImage()+%7b%0d%0a%09quiz_n_to_load--%3b%0d%0a%09quiz_UpdateView(unescape('Kraunasi%2520paveiksl%2526%2523279%253Bliai%2520%2528liko%2520atsi%2526%2523371%253Bsti%2520')+%2b+quiz_n_to_load+%2b+')...')%3b%0d%0a%09if+(quiz_n_to_load+%3c+1)%09%7b%0d%0a%09%09quiz_showMain()%3b%0d%0a%09%7d%0d%0a%7d%0d%0a%0d%0afunction+quiz_loadImages()+%7b%0d%0a%09n_to_load+%3d+0%3b%0d%0a%09for(var+x+in+quiz_photo_data.images)+%7b%0d%0a%09%09if+(!quiz_photo_data.images%5bx%5d)%09continue%3b%0d%0a%09%09quiz_n_to_load%2b%2b%3b%0d%0a%09%7d%0d%0a%09quiz_count+%3d+quiz_n_to_load%3b%0d%0a%09quiz_UpdateView(unescape('Kraunasi%2520paveiksl%2526%2523279%253Bliai%2520%2528liko%2520atsi%2526%2523371%253Bsti%2520')+%2b+quiz_n_to_load+%2b+')...')%3b%0d%0a%09var+i+%3d+0%3b%0d%0a%09for(var+x+in+quiz_photo_data.images)+%7b%0d%0a%09%09if+(!quiz_photo_data.images%5bx%5d)%09continue%3b%0d%0a%09%09quiz_Images%5bi%5d+%3d+new+Image()%3b%0d%0a%09%09quiz_addEvent(quiz_Images%5bi%5d%2c+'load'%2c+quiz_onLoadImage)%3b%0d%0a%09%09quiz_Images%5bi%5d.src+%3d+quiz_photo_data.images%5bx%5d%3b%0d%0a%09%09i%2b%2b%3b%0d%0a%09%7d%0d%0a%7d%0d%0a%0d%0afunction+quiz_gen_rez_row(data)+%7b%0d%0a%09var+tbl+%3d+''%3b%0d%0a%09var+img+%3d+null%3b%0d%0a%09var+ith+%3d+0%3b%0d%0a%09var+per_row+%3d+3%3b%0d%0a%09for+(o%3d0%3bo%3cdata.length%2fper_row%3bo%2b%2b)+%7b%0d%0a%09%09tbl++++%2b%3d+'%3ctr%3e'%3b%0d%0a%09%09for+(i%3do+*+per_row%3bi%3c((data.length%3cper_row)%3fdata.length%3ao+*+per_row+%2b+per_row)%3bi%2b%2b)+%7b%0d%0a%09%09%09if+(!data%5bi%5d)+continue%3b%0d%0a%09%09%09if+(!data%5bi%5d%5b2%5d)+continue%3b%09%0d%0a%09%09%09img+%3d+quiz_Images%5bdata%5bi%5d%5b2%5d%5d%3b%0d%0a%09%09%09ith+%3d+146+%2f+img.width+*+img.height%3b%0d%0a%09%09%09tbl++++%2b%3d+'%3ctd+style%3d%22vertical-align%3a+top%3b+text-align%3a+left%3b+border-width%3a+0px%3b+margin%3a+0px%3b+padding%3a+0px%3b%22%3e'%3b%0d%0a%09%09%09tbl%09+++%2b%3d+'%3cimg+src%3d%22'+%2b+img.src+%2b+'%22+width%3d%22146%22+height%3d%22'+%2b+ith+%2b+'%22+alt%3d%22'+%2b+data%5bi%5d%5b1%5d+%2b+'%22+title%3d%22'+%2b+data%5bi%5d%5b1%5d+%2b+'%22+%2f%3e'%3b%0d%0a%09%09%09tbl++++%2b%3d+'%3c%2ftd%3e'%3b%0d%0a%09%09%7d%0d%0a%09%09tbl++++%2b%3d+'%3c%2ftr%3e'%3b%0d%0a%09%09tbl++++%2b%3d+'%3ctr%3e'%3b%0d%0a%09%09for+(i%3do+*+per_row%3bi%3c((data.length%3cper_row)%3fdata.length%3ao+*+per_row+%2b+per_row)%3bi%2b%2b)+%7b%0d%0a%09%09%09if+(!data%5bi%5d)+continue%3b%0d%0a%09%09%09if+(!data%5bi%5d%5b1%5d)+continue%3b%09%0d%0a%09%09%09tbl+%2b%3d+'%3ctd+style%3d%22vertical-align%3a+top%3b+text-align%3a+center%3b%22%3e'%3b%0d%0a%09%09%09tbl+%2b%3d+'%3ch5+style%3d%22font-weight%3a+normal%3b%22%3e'%3b%0d%0a%09%09%09tbl+%2b%3d+data%5bi%5d%5b1%5d%3b%0d%0a%09%09%09tbl+%2b%3d+'%3c%2fh5%3e'%3b%0d%0a%09%09%09tbl+%2b%3d+'%3c%2ftd%3e'%3b%0d%0a%09%09%7d%0d%0a%09%09tbl++++%2b%3d+'%3c%2ftr%3e'%3b%0d%0a%09%7d%0d%0a%09return+tbl%0d%0a%7d%0d%0a%0d%0afunction+quiz_scroll()+%7b%0d%0a%09var+obj+%3d+document.getElementById('quiz_marquee')%3b%0d%0a%09if+(!obj)+return%3b%0d%0a%09if+(obj.style.position+!%3d+'relative')%09%7b%0d%0a%09%09obj.style.position+%3d+'relative'%3b%0d%0a%09%7d+%0d%0a%09if+((-quiz_marquee_top)+%3e+obj.offsetHeight)+%7b%0d%0a%09%09quiz_marquee_top+%3d+330%3b%0d%0a%09%7d+else+%7b%0d%0a%09%09quiz_marquee_top--%3b%0d%0a%09%7d%0d%0a%09obj.style.top+%3d+quiz_marquee_top%3b%0d%0a%7d%0d%0a%0d%0afunction+quiz_finish()+%7b%0d%0a%09var+good+%3d+new+Array()%3b%0d%0a%09var+bad+%3d+new+Array()%3b%0d%0a%09var+i+%3d+0%3b%0d%0a%09for(var+x+in+quiz_photo_data.images)+%7b%0d%0a%09%09if+(!quiz_photo_data.images%5bx%5d)%09continue%3b%0d%0a%09%09if+(quiz_answers_all_list%5bi%2b%2b%5d+%3d%3d+x)+%7b%0d%0a%09%09%09good%5bgood.length%5d+%3d+%5bquiz_photo_data.images%5bx%5d%2c+quiz_photo_data.answers%5bx%5d%2c+x%5d%3b%0d%0a%09%09%7d+else+%7b%0d%0a%09%09%09bad%5bbad.length%5d+%3d+%5bquiz_photo_data.images%5bx%5d%2c+quiz_photo_data.answers%5bx%5d%2c+x%5d%3b%0d%0a%09%09%7d%0d%0a%09%7d%0d%0a%09var+tbl+%3d+'%3ctable+border%3d%220%22%3e'%3b%0d%0a%09tbl++++%2b%3d+'%3ctr%3e'%3b%0d%0a%09if+(good.length+%3e+0)+%7b%0d%0a%09%09tbl++++%2b%3d+'%3cth+colspan%3d%224%22%3e'%3b%0d%0a%09%09tbl%09+++%2b%3d+'Teisingai+atsakyta+('+%2b+good.length+%2b+')'%3b%0d%0a%09%09tbl++++%2b%3d+'%3c%2fth%3e'%3b%0d%0a%09%09tbl++++%2b%3d+'%3c%2ftr%3e'%3b%0d%0a%09%09tbl++++%2b%3d+quiz_gen_rez_row(good)%3b%0d%0a%09%7d%0d%0a%09if+(bad.length+%3e+0)+%7b%0d%0a%09%09tbl++++%2b%3d+'%3ctr%3e'%3b%0d%0a%09%09tbl++++%2b%3d+'%3cth+colspan%3d%224%22%3e'%3b%0d%0a%09%09tbl%09+++%2b%3d+'Klaidingai+atsakyta+('+%2b+bad.length+%2b+')'%3b%0d%0a%09%09tbl++++%2b%3d+'%3c%2fth%3e'%3b%0d%0a%09%09tbl++++%2b%3d+'%3c%2ftr%3e'%3b%0d%0a%09%09tbl++++%2b%3d+quiz_gen_rez_row(bad)%3b%09%0d%0a%09%7d%0d%0a%09tbl++++%2b%3d+'%3c%2ftable%3e'%3b%0d%0a%09var+results_block+%3d+'%3cdiv+style%3d%22width%3a+448px%3b+height%3a+330px%3b+position%3a+relative%3b+left%3a+0px%3b+overflow%3a+hidden%3b+border-width%3a+1px%3b+border-style%3a+solid%3b+border-color%3a+black%3b+background-color%3a+black%3b%22%3e%3cdiv+id%3d%22quiz_oda%22+style%3d%22background-color%3a+%23F2F2F2%3b+opacity%3a+0%3b%22%3e%3cdiv+id%3d%22quiz_marquee%22%3e'%3b%0d%0a%09results_block+%2b%3d+tbl%3b%0d%0a%09results_block+%2b%3d+'%3c%2fdiv%3e%3c%2fdiv%3e%3c%2fdiv%3e'%3b%0d%0a%09var+txt+%3d+'%3cdiv+style%3d%22text-align%3a+left%3b%22%3e%3cb%3eRezultatai%3a%3c%2fb%3e+'%3b%0d%0a%09if+(bad.length+%3d%3d+0)+%7b%0d%0a%09%09txt+%2b%3d+'Esi+tikras(-a)+guru!!!'%3b%0d%0a%09%7d+else+if+(bad.length%2fquiz_count+%3c+0.14)+%7b%0d%0a%09%09txt+%2b%3d+unescape('Esi%2520tikras%2528-a%2529%2520eksperas%2528-%2526%2523279%253B%2529%2521%2521')%3b%0d%0a%09%7d+else+if+(bad.length%2fquiz_count+%3c+0.28)+%7b%0d%0a%09%09txt+%2b%3d+unescape('Esi%2520beveik%2520ekspertas%2528-%2526%2523279%253B%2529%2521')%3b%0d%0a%09%7d+else+if+(bad.length%2fquiz_count+%3c+0.47)+%7b%0d%0a%09%09txt+%2b%3d+unescape('Esi%2520vidutiniokas%2528-%2526%2523279%253B%2529.')%3b%0d%0a%09%7d+else+if+(bad.length%2fquiz_count+%3c+0.71)+%7b%0d%0a%09%09txt+%2b%3d+unescape('%25u0160ioje%2520srityje%2520esi%2520jau%2520ka%25u017Ekiek%2520pa%25u017Eeng%2526%2523281%253Bs%2528-usi%2529.')%3b%0d%0a%09%7d+else+if+(bad.length%2fquiz_count+%3c+1)+%7b%0d%0a%09%09txt+%2b%3d+unescape('%25u0160ioje%2520srityje%2520esi%2520kol%2520kas%2520naujokas%2528-%2526%2523279%253B%2529.')%3b%0d%0a%09%7d+else+%7b%0d%0a%09%09txt+%2b%3d+unescape('Tu%2520esi%2520tikras%2520Ne%25u017Einiukas%2521')%3b%0d%0a%09%7d%0d%0a%09txt++++%2b%3d+'%3cbr+%2f%3e%3cbr+%2f%3e'+%2b+results_block+%2b+'%3cp%3e%3c%2fdiv%3e'%3b%0d%0a%09quiz_UpdateView(txt)%3b%0d%0a%09quiz_opacity+%3d+0%3b%0d%0a%09setTimeout(function()+%7bwindow.setInterval(quiz_scroll%2c+100)%3b%7d%2c+2121)%3b%0d%0a%7d%0d%0a%0d%0adocument.write('%3cdiv+style%3d%22border-style%3a+solid%3b+border-width%3a+1px%3b+text-align%3a+center%3b+width%3a+450px%3b+vertical-align%3a+middle%3b+padding%3a+5px%3b+background-color%3a+white%3b%22+id%3d%22photo_quiz%22%3eKraunasi...%3c%2fdiv%3e')%3b%0d%0a%0d%0aquiz_DisplayTimer(150%2c+'Duomenys+kraunasi...'%2c+quiz_nextLoadStep%2c+quiz_QuizDataExists)%3b")				
			End Try
		End Using
	End Function
	
	Private Function GetPossibleAnswersList() As Collections.Generic.List(Of String)
		Dim possibleAnswers As New Collections.Generic.List(Of String)
		For Each Key As String In Me.ImageAnswersItems.Keys
			possibleAnswers.Add(key)
		Next
		For Each Key As String In Me.OtherAnswersItems
			possibleAnswers.Add(key)
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
			tmpFile = system.IO.Path.GetTempFileName()' + ".png"
			LocPaths = tmpFile.Split("\")
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
		tmpFile = system.IO.Path.GetTempFileName()		
		tmpData ="		<script type=""text/javascript"" src=""quiz-data.js""></script>" + Environment.NewLine
		tmpData +="		<script type=""text/javascript"" src=""quiz-core.js""></script>" + Environment.NewLine		
		zip.AddFileFromString("quiz-test.html", "", Me.GetBasicHTMLFile(tmpData))
				
		zip.Save(filename)
		
		Me._State = States.Idling
		
	End Sub	
	
	Private Sub FinishedLoading() Handles _tc.Finished
		Me._state = states.Idling		
	End Sub
	
End Class
End Namespace 