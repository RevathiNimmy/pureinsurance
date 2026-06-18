Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '





    Public Const ACApp As String = "bPMZipper"

    '***********************************************************************************
    '
    ' IMPORTANT: this project was originally cloned from the bSirZipper project
    ' Created CL170299
    '
    '***********************************************************************************

    ' RDC 26072001 declarations for new GEM2/GIS functionality
    Public Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByVal hpvDest As Integer, ByVal hpvSource As Integer, ByVal cbCopy As Integer)
    Public Declare Function compress Lib "zlib.dll" (ByVal dest As Integer, ByVal destLen As Integer, ByVal src As Integer, ByVal srcLen As Integer) As Integer
    Public Declare Function uncompress Lib "zlib.dll" (ByVal dest As Integer, ByVal destLen As Integer, ByVal src As Integer, ByVal srcLen As Integer) As Integer

    ' for GEM2/GIS
    ' RDC 29102001 ZLIB Compression utility constants START
    ' return codes
    Public Const ZLIB_NOERROR As Integer = 0
    Public Const ZLIB_STREAM_END As Integer = 1
    Public Const ZLIB_NEED_DICT As Integer = 2
    Public Const ZLIB_ERRNO As Integer = -1
    Public Const ZLIB_STREAM_ERROR As Integer = -2
    Public Const ZLIB_DATA_ERROR As Integer = -3
    Public Const ZLIB_MEM_ERROR As Integer = -4
    Public Const ZLIB_BUF_ERROR As Integer = -5
    Public Const ZLIB_VERSION_ERROR As Integer = 6

    ' Compression levels
    Public Const ZLIB_COMPRESS_NONE As Integer = 0
    Public Const ZLIB_COMPRESS_BEST_SPEED As Integer = 1
    Public Const ZLIB_COMPRESS_BEST_SIZE As Integer = 9
    Public Const ZLIB_COMPRESS_DEFAULT As Integer = -1

    ' misc. zlib stuff
    Public Const ZLIB_NULL As Integer = 0
    ' RDC 29102001 END




    Public Sub Main_Renamed()

        ' MessageBox.Show("Error " & Informations.Err().Number & ": " & Informations.Err().Description, "Main", MessageBoxButtons.OK)

    End Sub
End Module
