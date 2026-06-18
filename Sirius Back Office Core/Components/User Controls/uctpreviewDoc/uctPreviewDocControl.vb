Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms

Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctPreviewDocControl_NET.uctPreviewDocControl")> _
Partial Public Class uctPreviewDocControl
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Private Const ACClass As String = "uctPreviewDocControl"


    Private Declare Function GetCursorPos Lib "user32" (ByRef lpPoint As POINTAPI) As Integer

    Private Declare Function ScreenToClient Lib "user32" (ByVal hwnd As Integer, ByRef lpPoint As POINTAPI) As Integer
    Private Const SRCCOPY As Integer = &HCC0020 ' (DWORD) dest = source
    Private Declare Function StretchBlt Lib "gdi32" (ByVal hdc As Integer, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hSrcDC As Integer, ByVal xSrc As Integer, ByVal ySrc As Integer, ByVal nSrcWidth As Integer, ByVal nSrcHeight As Integer, ByVal dwRop As Integer) As Integer

    Private m_sFilename As String = ""
    Private m_sImagesPath As String = ""

    Private m_lCurrentPageNo As Integer
    Private m_lMaxPages As Integer

    Private m_lRotateAngle As Integer

    Dim m_lEndX As Integer
    Dim m_lEndY As Integer
    Dim m_lStartX As Integer
    Dim m_lStartY As Integer

    Dim m_lThumbnailsWidth As Integer

    Dim oSiriusDocumentUtility As SiriusDocumentUtility.Document

    Private m_lReturn As Integer

    Private Structure POINTAPI
        Dim x As Integer
        Dim y As Integer
    End Structure

    <Browsable(False)> _
    Public WriteOnly Property Filename() As String
        Set(ByVal Value As String)
            m_sFilename = Value
        End Set
    End Property

    Public Function ProcessInterface() As Integer
        Dim result As Integer = 0
        Dim SiriusDocumentUtility As Object
        Const kMethodName As String = "ProcessInterface"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            pctView.SetBounds(0, 0, pctContainer.ClientRectangle.Width, pctContainer.ClientRectangle.Height)

            m_lRotateAngle = 0

            m_lThumbnailsWidth = CInt(MyBase.ClientRectangle.Width / 5)
            uctPreviewDocControl_Resize(Me, New EventArgs())

            'Create Temp directory
            m_lReturn = bPMDocFunctions.GetClientDirectory(m_sImagesPath, True)

            oSiriusDocumentUtility = Nothing

            If oSiriusDocumentUtility Is Nothing Then
                oSiriusDocumentUtility = New SiriusDocumentUtility.Document()
            End If

            'View First Page
            ViewPage(1)

            VScroll2.Minimum = 1
            VScroll2.Maximum = (m_lMaxPages + VScroll2.LargeChange - 1)


        Catch ex As Exception
            iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)
        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Private Function ViewPage(ByRef lPageNum As Integer) As Integer
        Dim result As Integer = 0
        Dim oFSO As New Object
        Const kMethodName As String = "ViewPage"
        Try

            'oSiriusDocumentUtility = New SiriusDocumentUtility.Document()
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lCurrentPageNo = lPageNum

            'Load Images
            Dim oPicture As Image
            If Not (File.Exists(m_sImagesPath & "\Image" & CStr(m_lCurrentPageNo).Trim() & ".bmp")) Then
                oSiriusDocumentUtility.GetDocumentPageAsImage(m_sFilename, m_sImagesPath & "\Image" & CStr(m_lCurrentPageNo).Trim() & ".bmp", m_lCurrentPageNo, m_lMaxPages)
                Application.DoEvents()
            End If

            oPicture = Image.FromFile(m_sImagesPath & "\Image" & CStr(m_lCurrentPageNo).Trim() & ".bmp")

            If m_lRotateAngle = 90 Or m_lRotateAngle = 180 Then
                pctView.Width = (oPicture.Height) * 0.6
                pctView.Height = (oPicture.Width) * 0.6
            Else
                pctView.Width = (oPicture.Width) * 0.6
                pctView.Height = (oPicture.Height) * 0.6
            End If

            pctContainer_Resize(pctContainer, New EventArgs())
            pctView.Visible = True
            pctView.Image = oPicture
            pctView.ImageLocation = m_sImagesPath & "\Image" & CStr(m_lCurrentPageNo).Trim() & ".bmp"
            pctView.Load()
            pctView.Refresh()

            'Goto Top Of Page
            VScroll1.Value = 0

            ViewThumbnails(m_lCurrentPageNo)


        Catch ex As Exception
            iPMFunc.LogError(ACClass, kMethodName, result, excep:=ex)
        Finally



        End Try
        Return result
    End Function


    Private Function Max(ByVal nr1 As Integer, ByVal nr2 As Integer) As Integer
        If nr1 < nr2 Then
            Return nr2
        Else
            Return nr1
        End If
    End Function

    Private Sub uctPreviewDocControl_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        Try
            pctThumbnails.SetBounds(0, 0, m_lThumbnailsWidth, MyBase.ClientRectangle.Height)

            VScroll2.SetBounds(pctThumbnails.Width, 0, VScroll2.Width, pctThumbnails.Height)

            pctSplitter.SetBounds(pctThumbnails.Width + VScroll2.Width, 0, 10, pctThumbnails.Height)

            pctContainer.SetBounds(pctThumbnails.Width + pctSplitter.Width + VScroll2.Width, 0, MyBase.ClientRectangle.Width - VScroll1.Width - pctThumbnails.Width - pctSplitter.Width - VScroll2.Width, MyBase.ClientRectangle.Height - HScroll1.Height)

            VScroll1.SetBounds(pctContainer.Left + pctContainer.Width, pctContainer.Top, VScroll1.Width, pctContainer.Height)

            HScroll1.SetBounds(m_lThumbnailsWidth + VScroll2.Width + pctSplitter.Width, pctContainer.Top + pctContainer.Height, MyBase.ClientRectangle.Width - m_lThumbnailsWidth - VScroll2.Width - pctSplitter.Width - VScroll1.Width, HScroll1.Height)

            ViewThumbnails(m_lCurrentPageNo)
            MyBase.Refresh()

        Catch exc As System.Exception
            'Developer Guide No 32
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Sub


    Private Sub HScroll1_Change(ByVal newScrollValue As Integer)
        Const kMethodName As String = "HScroll1_Change"
        Try


            pctView.Left = -newScrollValue

        Catch ex As Exception


            iPMFunc.LogError(ACClass, kMethodName, m_lReturn, excep:=ex)
        End Try


    End Sub


    Private Sub pctContainer_DragDrop(ByRef Source As Control, ByRef x As Single, ByRef y As Single)
        If Source.Name = "pctSplitter" Then


            'TODO:MILAN::
            'm_lThumbnailsWidth = CInt(ScaleX(x, vbPixels, vbPixels) + VB6.PixelsToTwipsX(VScroll2.Width) + VB6.PixelsToTwipsX(pctThumbnails.Width))
            m_lThumbnailsWidth = CInt(x + VScroll2.Width + pctThumbnails.Width)
            uctPreviewDocControl_Resize(Me, New EventArgs())
        End If
    End Sub

    Private Sub pctContainer_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles pctContainer.Resize
        Dim NewLargeChange As Integer
        Const kMethodName As String = "pctContainer_Resize"
        Try


            If m_sFilename.Length = 0 Then
                pctView.SetBounds(0, 0, pctContainer.ClientRectangle.Width, pctContainer.ClientRectangle.Height)
            End If
            HScroll1.Enabled = pctView.Width > pctContainer.ClientRectangle.Width
            If HScroll1.Enabled Then
                HScroll1.Maximum = (pctView.Width - pctContainer.ClientRectangle.Width + HScroll1.LargeChange - 1)
                NewLargeChange = Max(CInt((HScroll1.Maximum - (HScroll1.LargeChange + 1)) / 10), 1)
                HScroll1.Maximum = HScroll1.Maximum + NewLargeChange - HScroll1.LargeChange
                HScroll1.LargeChange = NewLargeChange
                HScroll1.SmallChange = Max(CInt((HScroll1.Maximum - (HScroll1.LargeChange + 1)) / 100), 1)
            Else
                HScroll1.Maximum = (0 + HScroll1.LargeChange - 1)
                HScroll1.Value = 0
            End If
            VScroll1.Enabled = pctView.Height > pctContainer.ClientRectangle.Height
            If VScroll1.Enabled Then
                VScroll1.Maximum = (pctView.Height - pctContainer.ClientRectangle.Height + VScroll1.LargeChange - 1)
                NewLargeChange = Max(CInt((VScroll1.Maximum - (VScroll1.LargeChange + 1)) / 10), 1)
                VScroll1.Maximum = VScroll1.Maximum + NewLargeChange - VScroll1.LargeChange
                VScroll1.LargeChange = NewLargeChange
                VScroll1.SmallChange = Max(CInt((VScroll1.Maximum - (VScroll1.LargeChange + 1)) / 100), 1)
            Else
                VScroll1.Maximum = (0 + VScroll1.LargeChange - 1)
                VScroll1.Value = 0
            End If

        Catch ex As Exception


            iPMFunc.LogError(ACClass, kMethodName, m_lReturn, excep:=ex)
        End Try

    End Sub

    Private Sub pctSplitter_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pctSplitter.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        If Button = MouseButtons.Left Then


            'MILAN:TODO::
            'pctSplitter.Drag(vbBeginDrag)
        End If
    End Sub


    Private Sub pctThumbnails_DragDrop(ByRef Source As Control, ByRef x As Single, ByRef y As Single)
        If Source.Name = "pctSplitter" Then


            'TODO:MILAN::
            'm_lThumbnailsWidth = CInt(ScaleX(x, vbPixels, vbPixels))
            m_lThumbnailsWidth = x
            uctPreviewDocControl_Resize(Me, New EventArgs())
        End If
    End Sub

    Private Sub pctThumbnails_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pctThumbnails.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        Dim lNewPageNo As Integer = CInt(((y \ 120)) + (VScroll2.Value))
        If lNewPageNo <= m_lMaxPages Then
            ViewPage(lNewPageNo)
        End If
    End Sub


    Private Sub pctView_DragDrop(ByRef Source As Control, ByRef x As Single, ByRef y As Single)
        If Source.Name = "pctSplitter" Then


            'TODO:MILAN::
            'Dim gr As Graphics
            'gr.ScaleTransform(x, y)
            'm_lThumbnailsWidth = CInt((ScaleX(x, vbPixels, vbPixels) + VScroll2.Width + pctThumbnails.Width)
            m_lThumbnailsWidth = CInt(x + VScroll2.Width + pctThumbnails.Width)
            uctPreviewDocControl_Resize(Me, New EventArgs())
        End If
    End Sub

    Private Sub pctView_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pctView.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        Const kMethodName As String = "pctView_MouseDown"
        Try


            Dim lRet As Integer
            Dim Pt As New POINTAPI
            If Button = MouseButtons.Left Then
                lRet = GetCursorPos(Pt)
                lRet = ScreenToClient(pctView.Handle.ToInt32(), Pt)
                m_lStartX = Pt.x
                m_lStartY = Pt.y
                m_lEndX = m_lStartX
                m_lEndY = m_lStartY
            End If

        Catch ex As Exception


            iPMFunc.LogError(ACClass, kMethodName, m_lReturn, excep:=ex)
        End Try

    End Sub

    Private Sub pctView_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pctView.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        Const kMethodName As String = "pctView_MouseMove"
        Try


            Dim lRet, lMovementX, lMovementY As Integer
            Dim Pt As New POINTAPI

            lRet = GetCursorPos(Pt)
            lRet = ScreenToClient(pctView.Handle.ToInt32(), Pt)

            If Button = MouseButtons.Left Then
                'Get Current Ending Position
                m_lEndX = Pt.x
                m_lEndY = Pt.y

                'Calculate Movement
                lMovementX = m_lEndX - m_lStartX
                lMovementY = m_lEndY - m_lStartY

                If lMovementX <> 0 Or lMovementY <> 0 Then
                    'Movement Occured

                    If VScroll1.Value - lMovementY < 0 Then
                        'User Moved above Page, goto previous page
                        If m_lCurrentPageNo > 1 Then
                            ViewPage(m_lCurrentPageNo - 1)

                            'Goto Bottom of page
                            VScroll1.Value = (VScroll1.Maximum - (VScroll1.LargeChange + 1))

                            'Reset Counters
                            lRet = GetCursorPos(Pt)
                            lRet = ScreenToClient(pctView.Handle.ToInt32(), Pt)
                            m_lStartX = Pt.x
                            m_lEndX = Pt.x
                            m_lStartY = Pt.y
                            m_lEndY = Pt.y
                        Else
                            VScroll1.Value = 0
                        End If
                    ElseIf VScroll1.Value - lMovementY > (VScroll1.Maximum - (VScroll1.LargeChange + 1)) Then
                        'User Moved below Page, goto Next Page
                        If m_lCurrentPageNo < m_lMaxPages Then
                            ViewPage(m_lCurrentPageNo + 1)

                            'Reset Counters
                            lRet = GetCursorPos(Pt)
                            lRet = ScreenToClient(pctView.Handle.ToInt32(), Pt)
                            m_lStartX = Pt.x
                            m_lEndX = Pt.x
                            m_lStartY = Pt.y
                            m_lEndY = Pt.y
                        Else
                            VScroll1.Value = (VScroll1.Maximum - (VScroll1.LargeChange + 1))
                        End If
                    Else
                        'Move on current page
                        If VScroll1.Value - lMovementY < VScroll1.Minimum Then
                            VScroll1.Value = VScroll1.Minimum
                        ElseIf VScroll1.Value - lMovementY > (VScroll1.Maximum - (VScroll1.LargeChange + 1)) Then
                            VScroll1.Value = (VScroll1.Maximum - (VScroll1.LargeChange + 1))
                        Else
                            VScroll1.Value -= lMovementY
                        End If
                        If HScroll1.Value - lMovementX < HScroll1.Minimum Then
                            HScroll1.Value = HScroll1.Minimum
                        ElseIf HScroll1.Value - lMovementX > (HScroll1.Maximum - (HScroll1.LargeChange + 1)) Then
                            HScroll1.Value = (HScroll1.Maximum - (HScroll1.LargeChange + 1))
                        Else
                            HScroll1.Value -= lMovementX
                        End If
                    End If
                End If
            End If

        Catch ex As Exception


            iPMFunc.LogError(ACClass, kMethodName, m_lReturn, excep:=ex)
        End Try



    End Sub


    Private Sub VScroll1_Change(ByVal newScrollValue As Integer)
        Const kMethodName As String = "VScroll1_Change"
        Try


            pctView.Top = -newScrollValue

        Catch ex As Exception


            iPMFunc.LogError(ACClass, kMethodName, m_lReturn, excep:=ex)
        End Try


    End Sub


    Private Sub VScroll2_Change(ByVal newScrollValue As Integer)
        ViewThumbnails(newScrollValue)
    End Sub


    Private Sub VScroll2_DragDrop(ByRef Source As Control, ByRef x As Single, ByRef y As Single)
        If Source.Name = "pctSplitter" Then


            'TODO:MILAN::
            'm_lThumbnailsWidth = CInt(VB6.PixelsToTwipsX(pctThumbnails.Width) - ScaleX(x, vbPixels, vbPixels))
            m_lThumbnailsWidth = CInt(pctThumbnails.Width - x)

            uctPreviewDocControl_Resize(Me, New EventArgs())
        End If
    End Sub

    Private Sub ViewThumbnails(ByRef lFirstPos As Integer)
        Dim DrawWidth, DrawHeight As Integer

        If lFirstPos >= VScroll2.Minimum And lFirstPos <= (VScroll2.Maximum - (VScroll2.LargeChange + 1)) Then
            VScroll2.Value = lFirstPos
        End If

        Dim bFinished As Boolean = False

        'MILAN:TODO::
        'pctThumbnails.Cls()

        Dim lCnt As Integer = 0
        Dim CurrentX As Integer = 0
        Dim CurrentY As Integer = 0

        Dim oPicture As Image
        Dim oFSO As Scripting.FileSystemObject
        Dim xFactor, yFactor As Double
        Do Until bFinished Or CurrentY > pctThumbnails.Height
            If lFirstPos + lCnt <= m_lMaxPages Then

                oFSO = New Scripting.FileSystemObject()

                If m_sFilename.Trim().Length > 0 Then
                    If Not (File.Exists(m_sImagesPath & "\Image" & CStr(lCnt + lFirstPos).Trim() & ".bmp")) Then
                        oSiriusDocumentUtility.GetDocumentPageAsImage(m_sFilename, m_sImagesPath & "\Image" & CStr(lCnt + lFirstPos).Trim() & ".bmp", lCnt + lFirstPos, m_lMaxPages)
                    End If

                    If File.Exists(m_sImagesPath & "\Image" & CStr(lCnt + lFirstPos).Trim() & ".bmp") Then
                        oPicture = Image.FromFile(m_sImagesPath & "\Image" & CStr(lCnt + lFirstPos).Trim() & ".bmp")

                        'DrawHeight = ((oPicture.Height / 10) * Screen.TwipsPerPixelY)
                        'DrawWidth = ((oPicture.Width / 10) * Screen.TwipsPerPixelX)
                        DrawHeight = CInt(oPicture.Height * 0.08)
                        DrawWidth = CInt(oPicture.Width * 0.08)

                        'Set Dimensions on Temporary Image Box (for loading) and clear
                        pctTemp.Width = DrawWidth
                        pctTemp.Height = DrawHeight
                        pctTemp.Load(m_sImagesPath & "\Image" & CStr(lCnt + lFirstPos).Trim() & ".bmp")

                        pctTemp.Image = oPicture
                    End If
                End If

                'Get New Resize Dimensions
                xFactor = pctTemp.Width / 100
                yFactor = pctTemp.Height / 100
                If xFactor > yFactor Then
                    DrawWidth = (CInt(DrawWidth / xFactor))
                    DrawHeight = (CInt(DrawHeight / xFactor))
                Else
                    DrawWidth = (CInt(DrawWidth / yFactor))
                    DrawHeight = (CInt(DrawHeight / yFactor))
                End If

                'Recalculate using Twips
                ''''DrawHeight = CInt(DrawHeight * VB6.TwipsPerPixelY())
                ''''DrawWidth = CInt(DrawWidth * VB6.TwipsPerPixelX())
                'Calculate Current Position
                CurrentX = CInt(((pctThumbnails.ClientRectangle.Width - 20) - DrawWidth) \ 2)
                CurrentY = CInt((lCnt * 120 + 20))
                'pctThumbnails.SizeMode = PictureBoxSizeMode.StretchImage

                'Set Backgroup (highlighted for current page) and border for all pages
                'MIlAN:TODO::
                'If lCnt + lFirstPos = m_lCurrentPageNo Then

                'Copy image to thumbnails (using pixels)
                StretchBlt(pctThumbnails.CreateGraphics().GetHdc().ToInt32(), CInt(CurrentX), CInt(CurrentY), DrawWidth, DrawHeight, pctTemp.CreateGraphics().GetHdc().ToInt32(), 0, 0, CInt(pctTemp.Width), CInt(pctTemp.Height), SRCCOPY)

                'Write Page Number

                pctThumbnails.Refresh()
            Else
                bFinished = True
            End If
            lCnt += 1
        Loop

    End Sub

    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"

        Static bInitialised As Boolean

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Check if this method has already been called.
            If bInitialised Then
                Return result
            Else
                bInitialised = True
            End If

            'Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.Initialise", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



            pctContainer.Scale(3)


            pctView.Scale(3)

            bPMDocFunctions.Username = g_oObjectManager.UserName.Trim()


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_oObjectManager.UserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                oSiriusDocumentUtility = Nothing


            End If
        End If
        Me.disposedValue = True
    End Sub

    Private Sub HScroll1_Scroll(ByVal eventSender As Object, ByVal eventArgs As ScrollEventArgs) Handles HScroll1.Scroll
        Select Case eventArgs.Type
            Case ScrollEventType.EndScroll
                HScroll1_Change(eventArgs.NewValue)
        End Select
    End Sub
    Private Sub VScroll1_Scroll(ByVal eventSender As Object, ByVal eventArgs As ScrollEventArgs) Handles VScroll1.Scroll
        Select Case eventArgs.Type
            Case ScrollEventType.EndScroll
                VScroll1_Change(eventArgs.NewValue)
        End Select
    End Sub
    Private Sub VScroll2_Scroll(ByVal eventSender As Object, ByVal eventArgs As ScrollEventArgs) Handles VScroll2.Scroll
        Select Case eventArgs.Type
            Case ScrollEventType.EndScroll
                VScroll2_Change(eventArgs.NewValue)
        End Select
    End Sub
End Class
