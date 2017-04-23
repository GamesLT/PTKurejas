'
' Created by SharpDevelop.
' User: Administrator
' Date: 2009.03.07
' Time: 21:45
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Namespace My
    <HideModuleName>
    Friend Class MyVersionInfoClass

        Public ReadOnly Property Author As String
            Get
                Return My.Application.Info.CompanyName
            End Get
        End Property

        Public ReadOnly Property MoreInfoURL As Uri
            Get
                Return New Uri("https://github.com/Gameslt/PTKurejas")
            End Get
        End Property

        Public ReadOnly Property DevYearsFrom As UInt16
            Get
                Return 2009
            End Get
        End Property

        Public ReadOnly Property DevYearsTo As UInt16
            Get
                Return 201
            End Get
        End Property

        '#Region Other Functions

        Public ReadOnly Property Full As String
            Get
                Return My.Application.Info.Version.ToString()
            End Get
        End Property

        Public ReadOnly Property DevYears As String
            Get
                If Me.DevYearsFrom <> Me.DevYearsTo Then
                    Return Me.DevYearsFrom.ToString() + "-" + Me.DevYearsTo.ToString()
                End If
                Return Me.DevYearsFrom.ToString()
            End Get
        End Property

        Public ReadOnly Property AppName As String
            Get
                Return MainForm.ActiveForm.Text
            End Get
        End Property

        '#End Region		

    End Class

    ' Register extension in my namespace
    <HideModuleName>
    Friend Module MyVersionInfoModule
        Private instance As New MyVersionInfoClass

        Public ReadOnly Property VersionInfo() As MyVersionInfoClass
            <DebuggerHidden>
            Get
                Return instance
            End Get
        End Property
    End Module
End Namespace
