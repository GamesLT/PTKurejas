'
' Created by SharpDevelop.
' User: Administrator
' Date: 2009.03.07
' Time: 21:45
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Namespace My
	<HideModuleName> _
	Friend Class MyVersionInfoClass
		
		Public ReadOnly Property Minor As UInteger
			Get
				Return 1				
			End Get
		End Property
		
		Public ReadOnly Property Revision As UInteger
			Get
                Return 1
            End Get
		End Property
		
		Public ReadOnly Property Major As UInteger
			Get
				Return 0
			End Get
		End Property
		
		Public ReadOnly Property Author As String
			Get
                Return "Raimondas Rimkevièius (aka MekDrop) <github@mekdrop.name>"
            End Get
		End Property
		
		Public ReadOnly Property MoreInfoURL As Uri
			Get
                Return New Uri("https://github.com/MekDrop/PTKurejas")
            End Get
		End Property
		
		Public ReadOnly Property DevYearsFrom As UInt16
			Get
				Return 2009				
			End Get
		End Property

        Public ReadOnly Property DevYearsTo As UInt16
            Get
                Return 2016
            End Get
        End Property

        '#Region Other Functions

        Public ReadOnly Property Full As String
			Get
				Return Me.Major.ToString() + "."  + Me.Minor.ToString() + "." + Me.Revision.ToString()				
			End Get
		End Property	
		
		Public ReadOnly Property DevYears As String
			Get
				If Me.DevYearsFrom <> Me.DevYearsTo Then
					Return Me.DevYearsFrom.ToString() + "-"  + Me.DevYearsTo.ToString()
				End If
				Return Me.DevYearsFrom.ToString()
			End Get
		End Property
		
		Public ReadOnly Property AppName As String
			Get
				Return Mainform.ActiveForm.Text
			End Get
		End Property
		
		'#End Region		
		
	End Class
	
	' Register extension in my namespace
	<HideModuleName> _
	Friend Module MyVersionInfoModule
		Private instance As New MyVersionInfoClass
		
		Public ReadOnly Property VersionInfo() As MyVersionInfoClass
			<DebuggerHidden> _
			Get
				Return instance
			End Get
		End Property
	End Module
End Namespace
