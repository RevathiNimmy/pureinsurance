
Imports System.Drawing

Namespace Drawing
    ''' <summary>
    ''' A class for deriving border colours for colours other than
    ''' the system control colour using luminance adjustment
    ''' techniques.  Requires the vbAccelerator <see cref="HLSRGB"/>
    ''' class.
    ''' </summary>
    Public Class CustomBorderColor
        ' For black we can't create a darker colour, and for
        ' white we can't create a lighter one.  Therefore we
        ' need to play with the colours a little bit to try
        ' and achieve an equivalent effect.
        Private Shared BlackDarkDark As Color = Color.FromArgb(32, 32, 32)
        Private Shared BlackDark As Color = Color.FromArgb(48, 48, 48)
        Private Shared BlackLight As Color = Color.FromArgb(128, 128, 128)
        Private Shared BlackLightLight As Color = Color.FromArgb(192, 192, 192)
        Private Shared WhiteDarkDark As Color = Color.FromArgb(96, 96, 96)
        Private Shared WhiteDark As Color = Color.FromArgb(160, 160, 160)
        Private Shared WhiteLight As Color = Color.FromArgb(230, 230, 230)
        Private Shared WhiteLightLight As Color = Color.FromArgb(250, 250, 250)



        Private Sub New()
        End Sub

        ''' <summary>
        ''' Returns the grey intensity of a colour using the ITU
        ''' grey-scale standard.
        ''' </summary>
        ''' <param name="color">The <see cref="System.Drawing.Color"/> to get
        ''' the grey scale value for.</param>
        ''' <returns>A value between 0 and 255 holding the grey scale amount.
        ''' This can be used as the r, g and b arguments to the
        ''' <c>Color</c> class to construct the grey scale colour.</returns>
        Public Shared Function GreyScale(color As Color) As Integer
            Return (222 * color.R + 707 * color.G + 71 * color.B) \ 1000
        End Function

        ''' <summary>
        ''' Draw a border on the specified <see cref="System.Drawing.Graphics"/>
        ''' object.
        ''' </summary>
        ''' <param name="gfx">The <see cref="System.Drawing.Graphics"/> object
        ''' to draw onto.</param>
        ''' <param name="rect">The <see cref="System.Drawing.Rectangle"/> boundary
        ''' for the border.</param>
        ''' <param name="color">The <see cref="System.Drawing.Color"/> of the 
        ''' object.  This is used to determine the border colours.</param>
        ''' <param name="thin">Whether to draw a thin border or not.</param>
        ''' <param name="pressed">Whether the border should be drawn pressed
        ''' or raised.</param>
        Public Shared Sub DrawBorder(gfx As Graphics, rect As Rectangle, color As Color, thin As Boolean, pressed As Boolean)
            Dim darkPen As New Pen(CustomBorderColor.ColorDark(color), 1)
            Dim lightPen As New Pen(CustomBorderColor.ColorLight(color), 1)
            If thin Then
                If pressed Then
                    gfx.DrawLine(darkPen, rect.Left, rect.Bottom - 2, rect.Left, rect.Top)
                    gfx.DrawLine(darkPen, rect.Left, rect.Top, rect.Right - 2, rect.Top)
                    gfx.DrawLine(lightPen, rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom - 1)
                    gfx.DrawLine(lightPen, rect.Right - 1, rect.Bottom - 1, rect.Left, rect.Bottom - 1)
                Else
                    gfx.DrawLine(lightPen, rect.Left, rect.Bottom - 2, rect.Left, rect.Top)
                    gfx.DrawLine(lightPen, rect.Left, rect.Top, rect.Right - 1, rect.Top)
                    gfx.DrawLine(darkPen, rect.Right - 1, rect.Top + 1, rect.Right - 1, rect.Bottom - 1)
                    gfx.DrawLine(darkPen, rect.Right - 1, rect.Bottom - 1, rect.Left, rect.Bottom - 1)
                End If
            Else
                Dim lightLightPen As New Pen(CustomBorderColor.ColorLightLight(color), 1)
                Dim darkDarkPen As New Pen(CustomBorderColor.ColorDarkDark(color), 1)
                If pressed Then
                    gfx.DrawLine(darkDarkPen, rect.Left, rect.Bottom - 1, rect.Left, rect.Top)
                    gfx.DrawLine(darkDarkPen, rect.Left, rect.Top, rect.Right - 1, rect.Top)
                    gfx.DrawLine(lightLightPen, rect.Right - 1, rect.Top + 1, rect.Right - 1, rect.Bottom - 1)
                    gfx.DrawLine(lightLightPen, rect.Right - 1, rect.Bottom - 1, rect.Left + 1, rect.Bottom - 1)
                    gfx.DrawLine(darkPen, rect.Left + 1, rect.Bottom - 2, rect.Left + 1, rect.Top + 1)
                    gfx.DrawLine(darkPen, rect.Left + 1, rect.Top + 1, rect.Right - 2, rect.Top + 1)
                Else
                    gfx.DrawLine(lightLightPen, rect.Left, rect.Bottom - 1, rect.Left, rect.Top)
                    gfx.DrawLine(lightLightPen, rect.Left, rect.Top, rect.Right - 1, rect.Top)
                    gfx.DrawLine(darkPen, rect.Right - 1, rect.Top + 1, rect.Right - 1, rect.Bottom - 1)
                    gfx.DrawLine(darkPen, rect.Right - 1, rect.Bottom - 1, rect.Left + 1, rect.Bottom - 1)
                    gfx.DrawLine(lightPen, rect.Left + 1, rect.Bottom - 2, rect.Left + 1, rect.Top + 1)
                    gfx.DrawLine(lightPen, rect.Left + 1, rect.Top + 1, rect.Right - 2, rect.Top + 1)
                End If
                darkDarkPen.Dispose()
                lightLightPen.Dispose()
            End If
            darkPen.Dispose()
            lightPen.Dispose()
        End Sub

        ''' <summary>
        ''' Returns the equivalent of the <see cref="System.Drawing.KnownColor.ControlDarkDark"/> colour for a specified colour.
        ''' </summary>
        ''' <param name="color__1">The <see cref="System.Drawing.Color"/> to get the darker shadow shade for.</param>
        ''' <returns>The darker shadow <see cref="System.Drawing.Color"/> for the specified colour.</returns>
        ''' <remarks></remarks>
        Public Shared Function ColorDarkDark(color__1 As Color) As Color
            If color__1.Equals(Color.FromKnownColor(KnownColor.Control)) Then
                Return Color.FromKnownColor(KnownColor.ControlDarkDark)
            ElseIf color__1.Equals(Color.Black) Then
                Return Color.FromArgb(color__1.A, CustomBorderColor.BlackDarkDark)
            ElseIf color__1.Equals(Color.White) Then
                Return Color.FromArgb(color__1.A, CustomBorderColor.WhiteDarkDark)
            Else
                Dim grey As Integer = GreyScale(color__1)
                Dim hls As New HLSRGB(color__1.R, color__1.G, color__1.B)
                If grey > 250 Then
                    hls.Luminance = (CustomBorderColor.WhiteDarkDark.R / 255.0F)
                    Return Color.FromArgb(color__1.A, hls.Color)
                ElseIf grey < 64 Then
                    hls.Luminance = (CustomBorderColor.BlackDarkDark.R / 255.0F)
                    Return Color.FromArgb(color__1.A, hls.Color)
                Else
                    hls.DarkenColor(0.5F)
                    Return Color.FromArgb(color__1.A, hls.Color)

                End If
            End If
        End Function

        ''' <summary>
        ''' Returns the equivalent of the <see cref="System.Drawing.KnownColor.ControlDark"/> colour for a specified colour.
        ''' </summary>
        ''' <param name="color__1">The <see cref="System.Drawing.Color"/> to get the shadow shade for.</param>
        ''' <returns>The shadow <see cref="System.Drawing.Color"/> for the specified colour.</returns>
        ''' <remarks></remarks>
        Public Shared Function ColorDark(color__1 As Color) As Color
            If color__1.Equals(Color.FromKnownColor(KnownColor.Control)) Then
                Return Color.FromKnownColor(KnownColor.ControlDark)
            ElseIf color__1.Equals(Color.Black) Then
                Return Color.FromArgb(color__1.A, CustomBorderColor.BlackDark)
            ElseIf color__1.Equals(Color.White) Then
                Return Color.FromArgb(color__1.A, CustomBorderColor.WhiteDark)
            Else
                Dim grey As Integer = GreyScale(color__1)
                Dim hls As New HLSRGB(color__1.R, color__1.G, color__1.B)
                If grey > 250 Then
                    hls.Luminance = (CustomBorderColor.WhiteDark.R / 255.0F)
                    Return Color.FromArgb(color__1.A, hls.Color)
                ElseIf grey < 64 Then
                    hls.Luminance = (CustomBorderColor.BlackDark.R / 255.0F)
                    Return Color.FromArgb(color__1.A, hls.Color)
                Else
                    hls.DarkenColor(0.7F)
                    Return Color.FromArgb(color__1.A, hls.Color)
                End If
            End If
        End Function

        ''' <summary>
        ''' Returns the equivalent of the <see cref="System.Drawing.KnownColor.ControlLight"/> colour for a specified colour.
        ''' </summary>
        ''' <param name="color__1">The <see cref="System.Drawing.Color"/> to get the highlight shade for.</param>
        ''' <returns>The highlight <see cref="System.Drawing.Color"/> for the specified colour.</returns>
        Public Shared Function ColorLight(color__1 As Color) As Color
            If color__1.Equals(Color.FromKnownColor(KnownColor.Control)) Then
                Return Color.FromKnownColor(KnownColor.ControlLight)
            ElseIf color__1.Equals(Color.Black) Then
                Return Color.FromArgb(color__1.A, CustomBorderColor.BlackLight)
            ElseIf color__1.Equals(Color.White) Then
                Return Color.FromArgb(color__1.A, CustomBorderColor.WhiteLight)
            Else
                Dim grey As Integer = GreyScale(color__1)
                Dim hls As New HLSRGB(color__1.R, color__1.G, color__1.B)
                If grey > 250 Then
                    hls.Luminance = (CustomBorderColor.WhiteLight.R / 255.0F)
                    Return Color.FromArgb(color__1.A, hls.Color)
                ElseIf grey < 64 Then
                    hls.Luminance = (CustomBorderColor.BlackLight.R / 255.0F)
                    Return Color.FromArgb(color__1.A, hls.Color)
                Else
                    hls.LightenColor(0.1F)
                    Return Color.FromArgb(color__1.A, hls.Color)
                End If
            End If
        End Function

        ''' <summary>
        ''' Returns the equivalent of the <see cref="System.Drawing.KnownColor.ControlLightLight"/> colour for a specified colour.
        ''' </summary>
        ''' <param name="color__1">The <see cref="System.Drawing.Color"/> to get the lightest highlight shade for.</param>
        ''' <returns>The lightest highlight <see cref="System.Drawing.Color"/> for the specified colour.</returns>
        Public Shared Function ColorLightLight(color__1 As Color) As Color
            If color__1.Equals(Color.FromKnownColor(KnownColor.Control)) Then
                Return Color.FromKnownColor(KnownColor.ControlLightLight)
            ElseIf color__1.Equals(Color.Black) Then
                Return Color.FromArgb(color__1.A, CustomBorderColor.BlackLightLight)
            ElseIf color__1.Equals(Color.White) Then
                Return Color.FromArgb(color__1.A, CustomBorderColor.WhiteLightLight)
            Else
                Dim grey As Integer = GreyScale(color__1)
                Dim hls As New HLSRGB(color__1.R, color__1.G, color__1.B)
                If grey > 250 Then
                    hls.Luminance = (CustomBorderColor.WhiteLightLight.R / 255.0F)
                    Return Color.FromArgb(color__1.A, hls.Color)
                ElseIf grey < 64 Then
                    hls.Luminance = (CustomBorderColor.BlackLightLight.R / 255.0F)
                    Return Color.FromArgb(color__1.A, hls.Color)
                Else
                    hls.LightenColor(0.5F)
                    Return Color.FromArgb(color__1.A, hls.Color)
                End If
            End If
        End Function

    End Class
End Namespace