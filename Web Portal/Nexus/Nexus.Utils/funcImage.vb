Imports System.Drawing

Public Module funcImage

    Sub ScaleFactorCalculations(ByRef imgHeight As Integer, ByRef imgWidth As Integer, _
                                ByRef currentImage As Drawing.Image, ByVal maxHeight As Int16, _
                                ByVal maxWidth As Int16)

        imgHeight = currentImage.Height
        imgWidth = currentImage.Width

        Dim scaleFactor As Double

        If imgWidth > maxWidth Or imgHeight > maxHeight Then
            If (maxHeight / imgHeight) > (maxWidth / imgWidth) Then
                scaleFactor = maxHeight / imgHeight
            Else
                scaleFactor = maxWidth / imgWidth
            End If
        End If

        If imgWidth > maxWidth Then
            scaleFactor = maxWidth / imgWidth
            imgWidth *= scaleFactor
            imgHeight *= scaleFactor
        End If

        If imgHeight > maxHeight Then
            imgWidth /= scaleFactor
            imgHeight /= scaleFactor
            scaleFactor = maxHeight / imgHeight
            imgWidth *= scaleFactor
            imgHeight *= scaleFactor
        End If

        'Zero width or height runs out of memory and is clearly impossible to display, so minimum of 1x1 pixel
        If imgHeight < 1 Then imgHeight = 1
        If imgWidth < 1 Then imgWidth = 1

    End Sub

    Public Sub CreateThumbnail(ByVal pWidth As Integer, ByVal pHeight As Integer, _
                                ByVal pImageFileName As String, ByVal pProportioal As Boolean, _
                                ByRef pStream As IO.Stream)

        'Get the image
        Dim bmpFullSize As Drawing.Bitmap
        bmpFullSize = New Drawing.Bitmap(pImageFileName)

        'Workout scale factor and return the proportional height/width
        If pProportioal = True Then
            funcImage.ScaleFactorCalculations(pHeight, pWidth, bmpFullSize, pHeight, pWidth)
        End If

        'Create white background, to impose thumbnail on, this will fix any transparancies
        Dim bmpThumbnail As Drawing.Bitmap = New Drawing.Bitmap(pWidth, pHeight)
        Dim oGraphics As Drawing.Graphics = Drawing.Graphics.FromImage(bmpThumbnail)

        oGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
        oGraphics.FillRectangle(Drawing.Brushes.White, 0, 0, pWidth, pHeight)
        oGraphics.DrawImage(bmpFullSize, 0, 0, pWidth, pHeight)
        oGraphics.Save()
        oGraphics.Dispose()

        bmpFullSize.Dispose()

        'Output to stream as jpeg
        bmpThumbnail.Save(pStream, Imaging.ImageFormat.Jpeg)
        bmpThumbnail.Dispose()

    End Sub

End Module
