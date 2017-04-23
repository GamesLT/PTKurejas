Imports System.Collections.Generic
Imports System.Text
Imports System.IO

Public Class ZipStorer

#Region "Public Structure Declaration"

    Public Structure ZipFileEntry
        Public FilenameInZip As String
        Public FileSize As UInteger
        Public FileOffset As UInteger
        Public HeaderSize As UInteger
        Public HeaderOffset As UInteger
        Public Crc32 As UInteger
        Public ModifyTime As DateTime
        Public Comment As String
    End Structure

#End Region

#Region "CRC32 Table"

    Shared CrcTable As UInt32() = New UInt32() _
    {0, 1996959894, 3993919788, 2567524794, 124634137, 1886057615, _
    3915621685, 2657392035, 249268274, 2044508324, 3772115230, 2547177864, _
    162941995, 2125561021, 3887607047, 2428444049, 498536548, 1789927666, _
    4089016648, 2227061214, 450548861, 1843258603, 4107580753, 2211677639, _
    325883990, 1684777152, 4251122042, 2321926636, 335633487, 1661365465, _
    4195302755, 2366115317, 997073096, 1281953886, 3579855332, 2724688242, _
    1006888145, 1258607687, 3524101629, 2768942443, 901097722, 1119000684, _
    3686517206, 2898065728, 853044451, 1172266101, 3705015759, 2882616665, _
    651767980, 1373503546, 3369554304, 3218104598, 565507253, 1454621731, _
    3485111705, 3099436303, 671266974, 1594198024, 3322730930, 2970347812, _
    795835527, 1483230225, 3244367275, 3060149565, 1994146192, 31158534, _
    2563907772, 4023717930, 1907459465, 112637215, 2680153253, 3904427059, _
    2013776290, 251722036, 2517215374, 3775830040, 2137656763, 141376813, _
    2439277719, 3865271297, 1802195444, 476864866, 2238001368, 4066508878, _
    1812370925, 453092731, 2181625025, 4111451223, 1706088902, 314042704, _
    2344532202, 4240017532, 1658658271, 366619977, 2362670323, 4224994405, _
    1303535960, 984961486, 2747007092, 3569037538, 1256170817, 1037604311, _
    2765210733, 3554079995, 1131014506, 879679996, 2909243462, 3663771856, _
    1141124467, 855842277, 2852801631, 3708648649, 1342533948, 654459306, _
    3188396048, 3373015174, 1466479909, 544179635, 3110523913, 3462522015, _
    1591671054, 702138776, 2966460450, 3352799412, 1504918807, 783551873, _
    3082640443, 3233442989, 3988292384, 2596254646, 62317068, 1957810842, _
    3939845945, 2647816111, 81470997, 1943803523, 3814918930, 2489596804, _
    225274430, 2053790376, 3826175755, 2466906013, 167816743, 2097651377, _
    4027552580, 2265490386, 503444072, 1762050814, 4150417245, 2154129355, _
    426522225, 1852507879, 4275313526, 2312317920, 282753626, 1742555852, _
    4189708143, 2394877945, 397917763, 1622183637, 3604390888, 2714866558, _
    953729732, 1340076626, 3518719985, 2797360999, 1068828381, 1219638859, _
    3624741850, 2936675148, 906185462, 1090812512, 3747672003, 2825379669, _
    829329135, 1181335161, 3412177804, 3160834842, 628085408, 1382605366, _
    3423369109, 3138078467, 570562233, 1426400815, 3317316542, 2998733608, _
    733239954, 1555261956, 3268935591, 3050360625, 752459403, 1541320221, _
    2607071920, 3965973030, 1969922972, 40735498, 2617837225, 3943577151, _
    1913087877, 83908371, 2512341634, 3803740692, 2075208622, 213261112, _
    2463272603, 3855990285, 2094854071, 198958881, 2262029012, 4057260610, _
    1759359992, 534414190, 2176718541, 4139329115, 1873836001, 414664567, _
    2282248934, 4279200368, 1711684554, 285281116, 2405801727, 4167216745, _
    1634467795, 376229701, 2685067896, 3608007406, 1308918612, 956543938, _
    2808555105, 3495958263, 1231636301, 1047427035, 2932959818, 3654703836, _
    1088359270, 936918000, 2847714899, 3736837829, 1202900863, 817233897, _
    3183342108, 3401237130, 1404277552, 615818150, 3134207493, 3453421203, _
    1423857449, 601450431, 3009837614, 3294710456, 1567103746, 711928724, _
    3020668471, 3272380065, 1510334235, 755167117}

#End Region

#Region "Private Fields"

    'List of files to store
    Private Files As New List(Of ZipFileEntry)
    'Filename of storage file
    Private FileName As String
    'Stream object of storage file
    Private ZipFileStream As FileStream
    'General comment
    Private Comment As String = ""
    'Central dir image
    Private CentralDirImage As Byte() = Nothing
    'Existing files in zip
    Private ExistingFiles As UShort = 0
    'File access for Open method
    Private Access As FileAccess

#End Region

#Region "Public Methods"
    'Private constructor. Avoid direct construction
    Private Sub New()
    End Sub

    'Method to create a new storage
    Public Shared Function Create(ByVal _filename As String, ByVal _comment As String) As ZipStorer
        Dim zip As New ZipStorer

        zip.FileName = _filename
        zip.Comment = _comment
        zip.ZipFileStream = New FileStream(_filename, FileMode.Create, FileAccess.ReadWrite)
        zip.Access = FileAccess.Write

        Return zip
    End Function

    'Method to open an existing storage
    Public Shared Function Open(ByVal _filename As String, ByVal _access As FileAccess) As ZipStorer
        Dim zip As New ZipStorer
        If _access <> System.IO.FileAccess.Read Then
            _access = FileAccess.Write
        End If
        zip.FileName = _filename
        zip.ZipFileStream = New FileStream(_filename, FileMode.Open, _access)
        zip.Access = _access

        If zip.ReadFileInfo Then
            Return zip
        End If

        Throw New System.IO.InvalidDataException
    End Function

    'Add file stream facility method
    Public Sub AddFile(ByVal _pathname As String, ByVal _filenameInZip As String, ByVal _comment As String)
        If Access = FileAccess.Read Then
            Throw New InvalidOperationException("Writing is not allowed")
        End If

        Dim stream As New FileStream(_pathname, FileMode.Open, FileAccess.Read)
        AddStream(_filenameInZip, stream, File.GetLastWriteTime(_pathname), _comment)
        stream.Close()
    End Sub

    'Main method for adding a stream to storage file
    Public Sub AddStream(ByVal _filenameInZip As String, ByVal _source As Stream, ByVal _modTime As DateTime, ByVal _comment As String)
        If Access = FileAccess.Read Then
            Throw New InvalidOperationException("Writing is not allowed")
        End If

        Dim offset As Long = 0
        If Me.Files.Count = 0 Then
            offset = 0
        Else
            Dim last As ZipFileEntry = Me.Files(Me.Files.Count - 1)
            offset = last.HeaderOffset + last.HeaderSize
        End If

        If _comment Is Nothing Then
            _comment = ""
        End If

        'Prepare the fileinfo
        Dim zfe As New ZipFileEntry
        zfe.FilenameInZip = NormalizedFilename(_filenameInZip)
        zfe.Comment = _comment

        'Even though we write the header now, it will have to be rewritten, since we don't know compressed size or crc.
        zfe.Crc32 = 0  'To be updated later
        zfe.HeaderOffset = CInt(Me.ZipFileStream.Position)  'Offset within file of the start of this local record
        zfe.ModifyTime = _modTime

        'Write local header
        WriteLocalHeader(zfe)
        zfe.FileOffset = CInt(Me.ZipFileStream.Position)

        'Write file to zip (store)
        zfe.FileSize = Store(zfe, _source)
        _source.Close()

        Me.UpdateCrcAndSizes(zfe)

        Files.Add(zfe)
    End Sub

    'Updates central directory (if pertinent) and close
    Public Sub Close()
        If Access <> FileAccess.Read Then
            Dim centralOffset As UInteger = CInt(Me.ZipFileStream.Position)
            Dim centralSize As UInteger = 0

            If Me.CentralDirImage IsNot Nothing Then
                Me.ZipFileStream.Write(CentralDirImage, 0, CentralDirImage.Length)
            End If

            For i As Integer = 0 To Files.Count - 1
                Dim pos As Long = Me.ZipFileStream.Position
                Me.WriteCentralDirRecord(Files(i))
                centralSize += CInt(Me.ZipFileStream.Position - pos)
            Next i

            If Me.CentralDirImage IsNot Nothing Then
                Me.WriteEndRecord(centralSize + CInt(CentralDirImage.Length), centralOffset)
            Else
                Me.WriteEndRecord(centralSize, centralOffset)
            End If
        End If

        Me.ZipFileStream.Close()
    End Sub

    'Read all the file records in the central directory
    Public Function ReadCentralDir() As List(Of ZipFileEntry)
        If Me.CentralDirImage Is Nothing Then
            Throw New InvalidOperationException("Central directory currently does not exist")
        End If

        Dim result As New List(Of ZipFileEntry)

        For pointer As Integer = 0 To Me.CentralDirImage.Length - 1
            Dim comprSize As UInteger = BitConverter.ToUInt32(CentralDirImage, pointer + 20)
            Dim filenameSize As UShort = BitConverter.ToUInt16(CentralDirImage, pointer + 28)
            Dim extraSize As UShort = BitConverter.ToUInt16(CentralDirImage, pointer + 30)
            Dim commentSize As UShort = BitConverter.ToUInt16(CentralDirImage, pointer + 32)
            Dim headerOffset As UInteger = BitConverter.ToUInt32(CentralDirImage, pointer + 42)
            Dim headerSize As UInteger = CInt(46 + filenameSize + extraSize + commentSize)

            Dim zfe As New ZipFileEntry
            zfe.FilenameInZip = Encoding.UTF8.GetString(CentralDirImage, pointer + 46, filenameSize)
            zfe.FileOffset = GetFileOffset(headerOffset)
            zfe.FileSize = comprSize
            zfe.HeaderOffset = headerOffset
            zfe.HeaderSize = headerSize
            'zfe.ModifyTime = ;  'Date format can vary
            If commentSize > 0 Then
                zfe.Comment = Encoding.UTF8.GetString(CentralDirImage, pointer + 46 + filenameSize + extraSize, commentSize)
            End If

            result.Add(zfe)
            pointer += (46 + filenameSize + extraSize + commentSize)
        Next pointer

        Return result
    End Function

    'Calculate the file offset by reading the corresponding local header
    Public Function GetFileOffset(ByVal _headerOffset As UInteger) As UInteger
        Dim buffer As Byte() = New Byte(2) {}

        Me.ZipFileStream.Seek(_headerOffset + 26, SeekOrigin.Begin)
        Me.ZipFileStream.Read(buffer, 0, 2)
        Dim filenameSize As UShort = BitConverter.ToUInt16(buffer, 0)
        Me.ZipFileStream.Read(buffer, 0, 2)
        Dim extraSize As UShort = BitConverter.ToUInt16(buffer, 0)

        Return CInt(30 + filenameSize + extraSize + _headerOffset)
    End Function

    'Copy the contents of a stored file into a physical file
    Public Sub ExtractStoredFile(ByVal _zfe As ZipFileEntry, ByVal _filename As String)
        Dim buffer As Byte() = New Byte(32767) {}
        Dim bytesPending As UInteger = _zfe.FileSize

        Dim output As FileStream = New FileStream(_filename, FileMode.Create, FileAccess.Write)
        Me.ZipFileStream.Seek(_zfe.FileOffset, SeekOrigin.Begin)

        'Buffered copy
        While bytesPending > 0
            Dim bytesRead As Integer = Me.ZipFileStream.Read(buffer, 0, CInt(Math.Min(bytesPending, buffer.Length)))
            output.Write(buffer, 0, bytesRead)
            bytesPending -= CInt(bytesRead)
        End While

        output.Close()
    End Sub

#End Region

#Region "Private Methods"
    'Local file header:
    'local file header signature     4 bytes  (0x04034b50)
    'version needed to extract       2 bytes
    'general purpose bit flag        2 bytes
    'compression method              2 bytes
    'last mod file time              2 bytes
    'last mod file date              2 bytes
    'crc-32                          4 bytes
    'compressed size                 4 bytes
    'uncompressed size               4 bytes
    'filename length                 2 bytes
    'extra field length              2 bytes

    'filename (variable size)
    'extra field (variable size)

    Private Sub WriteLocalHeader(ByRef zfe As ZipFileEntry)
        Dim pos As Long = Me.ZipFileStream.Position

        Me.ZipFileStream.Write(New Byte() {80, 75, 3, 4, 20, 0, 0, 0}, 0, 8) 'No extra header
        Me.ZipFileStream.Write(BitConverter.GetBytes(CUShort(0)), 0, 2)  'stored (0)
        Me.ZipFileStream.Write(BitConverter.GetBytes(DosTime(zfe.ModifyTime)), 0, 4) 'zipping date and time
        Me.ZipFileStream.Write(New Byte() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 0, 12) 'unused CRC, un/compressed size, updated later
        Me.ZipFileStream.Write(BitConverter.GetBytes(CUShort(zfe.FilenameInZip.Length)), 0, 2) 'filename length
        Me.ZipFileStream.Write(BitConverter.GetBytes(CUShort(0)), 0, 2) 'extra length

        Me.ZipFileStream.Write(Encoding.UTF8.GetBytes(zfe.FilenameInZip), 0, zfe.FilenameInZip.Length)
        zfe.HeaderSize = CInt(Me.ZipFileStream.Position - pos)
    End Sub

    'Central directory's File header:
    'central file header signature   4 bytes  (0x02014b50)
    'version made by                 2 bytes
    'version needed to extract       2 bytes
    'general purpose bit flag        2 bytes
    'compression method              2 bytes
    'last mod file time              2 bytes
    'last mod file date              2 bytes
    'crc-32                          4 bytes
    'compressed size                 4 bytes
    'uncompressed size               4 bytes
    'filename length                 2 bytes
    'extra field length              2 bytes
    'file comment length             2 bytes
    'disk number start               2 bytes
    'internal file attributes        2 bytes
    'external file attributes        4 bytes
    'relative offset of local header 4 bytes

    'filename (variable size)
    'extra field (variable size)
    'file comment (variable size)

    Private Sub WriteCentralDirRecord(ByVal zfe As ZipFileEntry)
        Me.ZipFileStream.Write(New Byte() {80, 75, 1, 2, 23, 11, 20, 0, 0, 0}, 0, 10)
        Me.ZipFileStream.Write(BitConverter.GetBytes(CUShort(0)), 0, 2)  'zipping method: stored (0)
        Me.ZipFileStream.Write(BitConverter.GetBytes(DosTime(zfe.ModifyTime)), 0, 4)  'zipping date and time
        Me.ZipFileStream.Write(BitConverter.GetBytes(zfe.Crc32), 0, 4) 'file CRC
        Me.ZipFileStream.Write(BitConverter.GetBytes(zfe.FileSize), 0, 4) 'compressed file size
        Me.ZipFileStream.Write(BitConverter.GetBytes(zfe.FileSize), 0, 4) 'uncompressed file size
        Me.ZipFileStream.Write(BitConverter.GetBytes(CUShort(zfe.FilenameInZip.Length)), 0, 2) 'Filename in zip
        Me.ZipFileStream.Write(BitConverter.GetBytes(CUShort(0)), 0, 2) 'extra length
        Me.ZipFileStream.Write(BitConverter.GetBytes(CUShort(zfe.Comment.Length)), 0, 2)

        Me.ZipFileStream.Write(BitConverter.GetBytes(CUShort(0)), 0, 2) 'disk=0
        Me.ZipFileStream.Write(BitConverter.GetBytes(CUShort(0)), 0, 2) 'file type: binary
        Me.ZipFileStream.Write(BitConverter.GetBytes(CUShort(0)), 0, 2) 'Internal file attributes
        Me.ZipFileStream.Write(BitConverter.GetBytes(CUShort(33024)), 0, 2) 'External file attributes (normal/readable)
        Me.ZipFileStream.Write(BitConverter.GetBytes(zfe.HeaderOffset), 0, 4)  'Offset of header

        Me.ZipFileStream.Write(Encoding.UTF8.GetBytes(zfe.FilenameInZip), 0, zfe.FilenameInZip.Length)
        Me.ZipFileStream.Write(Encoding.UTF8.GetBytes(zfe.Comment), 0, zfe.Comment.Length)
    End Sub

    'End of central dir record:
    'end of central dir signature    4 bytes  (0x06054b50)
    'number of this disk             2 bytes
    'number of the disk with the
    'start of the central directory  2 bytes
    'total number of entries in
    'the central dir on this disk    2 bytes
    'total number of entries in
    'the central dir                 2 bytes
    'size of the central directory   4 bytes
    'offset of start of central
    'directory with respect to
    'the starting disk number        4 bytes
    'zipfile comment length          2 bytes
    'zipfile comment (variable size)

    Private Sub WriteEndRecord(ByVal size As UInteger, ByVal offset As UInteger)
        Me.ZipFileStream.Write(New Byte() {80, 75, 5, 6, 0, 0, 0, 0}, 0, 8)
        Me.ZipFileStream.Write(BitConverter.GetBytes(CUShort(Files.Count + ExistingFiles)), 0, 2)
        Me.ZipFileStream.Write(BitConverter.GetBytes(CUShort(Files.Count + ExistingFiles)), 0, 2)
        Me.ZipFileStream.Write(BitConverter.GetBytes(size), 0, 4)
        Me.ZipFileStream.Write(BitConverter.GetBytes(offset), 0, 4)
        Me.ZipFileStream.Write(BitConverter.GetBytes(CUShort(Me.Comment.Length)), 0, 2)
        Me.ZipFileStream.Write(Encoding.UTF8.GetBytes(Me.Comment), 0, Me.Comment.Length)
    End Sub

    'Copies all source file into storage file
    Private Function Store(ByRef zfe As ZipFileEntry, ByVal source As Stream) As UInteger
        Dim buffer As Byte() = New Byte(16383) {}
        Dim bytesRead As Integer
        Dim totalRead As UInteger = 0

        Do
            bytesRead = source.Read(buffer, 0, buffer.Length)
            totalRead += CInt(bytesRead)
            If bytesRead > 0 Then
                Me.ZipFileStream.Write(buffer, 0, bytesRead)
            End If
        Loop While bytesRead = buffer.Length

        Return totalRead
    End Function

    'DOS Date and time:
    'MS-DOS date. The date is a packed value with the following format. Bits Description 
    '    0-4 Day of the month (1–31) 
    '    5-8 Month (1 = January, 2 = February, and so on) 
    '    9-15 Year offset from 1980 (add 1980 to get actual year) 
    'MS-DOS time. The time is a packed value with the following format. Bits Description 
    '    0-4 Second divided by 2 
    '    5-10 Minute (0–59) 
    '    11-15 Hour (0–23 on a 24-hour clock) 

    Private Function DosTime(ByVal dt As DateTime) As UInteger
        Return CInt(((dt.Second / 2) Or (dt.Minute << 5) Or (dt.Hour << 11) Or (dt.Day << 16) Or (dt.Month << 21) Or ((dt.Year - 1980) << 25)))
    End Function

    'CRC32 algorithm
    'The 'magic number' for the CRC is 0xdebb20e3.  
    'The proper CRC pre and post conditioning
    'is used, meaning that the CRC register is
    'pre-conditioned with all ones (a starting value
    'of 0xffffffff) and the value is post-conditioned by
    'taking the one's complement of the CRC residual.
    'If bit 3 of the general purpose flag is set, this
    'field is set to zero in the local header and the correct
    'value is put in the data descriptor and in the central
    'directory.

    Private Sub UpdateCrcAndSizes(ByRef zfe As ZipFileEntry)
        Dim lastPos As Long = Me.ZipFileStream.Position  'remember position
        Const MagicCRT As UInteger = 4294967295
        Const UOne As UInteger = 1

        zfe.Crc32 = 0 Xor 4294967295
        Me.ZipFileStream.Position = zfe.FileOffset
        For i As UInteger = 0 To zfe.FileSize - UOne
            Dim b As Byte = CByte(Me.ZipFileStream.ReadByte)
            zfe.Crc32 = ZipStorer.CrcTable((zfe.Crc32 Xor b) And 255) Xor (zfe.Crc32 >> 8)
        Next i
        zfe.Crc32 = zfe.Crc32 Xor MagicCRT

        Me.ZipFileStream.Position = zfe.HeaderOffset + 14
        Me.ZipFileStream.Write(BitConverter.GetBytes(zfe.Crc32), 0, 4)  'Update CRC
        Me.ZipFileStream.Write(BitConverter.GetBytes(zfe.FileSize), 0, 4)  'Compressed size
        Me.ZipFileStream.Write(BitConverter.GetBytes(zfe.FileSize), 0, 4)  'Uncompressed size

        Me.ZipFileStream.Position = lastPos  'restore position
    End Sub

    'Replaces backslashes with slashes to store in zip header
    Private Function NormalizedFilename(ByVal _filename As String) As String
        Dim filename As String = _filename.Replace("\"c, "/"c)

        Dim pos As Integer = filename.IndexOf(":"c)
        If pos >= 0 Then
            filename = filename.Remove(0, pos + 1)
        End If

        Return filename.Trim("/"c)
    End Function

    'Read entire directory, by reading local headers
    Private Function ReadFileInfo() As Boolean
        Dim sig As Byte() = New Byte(3) {}
        Dim header As Byte() = New Byte(41) {}
        Dim signature As UInteger
        Dim filenameSize As UShort
        Dim extraSize As UShort
        Dim commentSize As UShort
        Dim comprSize As UInteger
        Const One As UShort = 1

        Try
            Me.ZipFileStream.Seek(0, SeekOrigin.Begin)

            Do
                If (Me.ZipFileStream.Read(sig, 0, 4) < 4) Then
                    Exit Do  'end of file
                End If

                signature = BitConverter.ToUInt32(sig, 0)

                If signature = 67324752 Then  'Local header
                    If Me.ZipFileStream.Read(header, 0, 26) < 26 Then
                        Return False 'error
                    End If

                    comprSize = BitConverter.ToUInt32(header, 14)
                    filenameSize = BitConverter.ToUInt16(header, 22)
                    extraSize = BitConverter.ToUInt16(header, 24)

                    'Just skip the record and file contents to reach the end-of-central-dir entry
                    Me.ZipFileStream.Seek(comprSize + filenameSize + extraSize, SeekOrigin.Current)

                    ExistingFiles += One
                ElseIf signature = 33639248 Then  'Central dir header
                    If Me.ZipFileStream.Read(header, 0, 42) < 42 Then
                        Return False 'error
                    End If

                    filenameSize = BitConverter.ToUInt16(header, 24)
                    extraSize = BitConverter.ToUInt16(header, 26)
                    commentSize = BitConverter.ToUInt16(header, 28)

                    'Just skip the record to reach the end-of-central-dir entry
                    Me.ZipFileStream.Seek(filenameSize + extraSize + commentSize, SeekOrigin.Current)
                    ElseIf signature = 101010256 Then 'end of central dir
                    If Me.ZipFileStream.Read(header, 0, 18) < 18 Then
                        Return False 'error
                    End If

                    Dim centralSize As Integer = BitConverter.ToInt32(header, 8)
                    Dim centralDirOffset As UInteger = BitConverter.ToUInt32(header, 12)

                    'Copy entire central directory to a memory buffer
                    Me.CentralDirImage = New Byte(centralSize - 1) {}
                    Me.ZipFileStream.Seek(centralDirOffset, SeekOrigin.Begin)
                    Me.ZipFileStream.Read(Me.CentralDirImage, 0, centralSize)

                    'Leave the pointer at the begining of central dir, to append new files
                    Me.ZipFileStream.Seek(centralDirOffset, SeekOrigin.Begin)

                    Exit Do
                End If
            Loop

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

End Class