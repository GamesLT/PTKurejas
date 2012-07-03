'
' Created by SharpDevelop.
' User: Administrator
' Date: 2009.03.07
' Time: 23:46
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Public Class ParsingSystem
	
	Public ReadOnly Col As New Collections.Generic.List(Of DataType)	
	
	Public Enum DataType as SByte 
		Space = 0
		Quotes1Opening = 1	' '
		Quotes1Closing = 2
		Quotes2Opening = 3  ' "
		Quotes2Closing = 4
		Backslash = 5		' \
		Brackets1Opening = 6' {
		Brackets1Closing = 7' }
		Brackets2Opening = 8' [
		Brackets2Closing = 9' ]
		Number 		  = 10
		TextInQuotes  = 11
		Colon		  = 12 ':
		Comma		  = 13 ',
		Semicolon	  = 14 ';
		NewLine		  = 15 
		Equality	  = 16 '=
		Slash		  = 18 '/
		Star		  = 19 '*
		Comments1	  = 17 '//
		Comments2Opening = 20 '/*
		Comments2Closing = 21 '*/
	End Enum
	
	Public Sub Add(DataType As DataType)
		Me.Col.Add(datatype)
	End Sub
	
	Public Function GetLast() As DataType 
		If Me.Col.Count < 1 Then Return Nothing 		
		Return Me.Col.Item(Me.Col.Count - 1)		
	End Function
	
	Public Sub RemoveLast()
		If Me.Col.Count < 1 Then Exit Sub		
		Me.Col.RemoveAt(Me.Col.Count - 1)		
	End Sub
	
End Class
