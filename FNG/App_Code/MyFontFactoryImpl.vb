Imports Microsoft.VisualBasic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO

Public Class MyFontFactoryImpl
    Inherits FontFactoryImp
    Private defaultFont As Font

    Public Sub New(fontPath As String, fontsize As Single)
        '"C:\WINDOWS\Fonts\tahoma.ttf"
        Dim tahoma As BaseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED)
        defaultFont = New Font(tahoma, fontsize)
    End Sub
    Public Overrides Function GetFont(fontname As String, encoding As String, embedded As [Boolean], size As Single, style As Integer, color As iTextSharp.text.Color, _
     cached As [Boolean]) As Font
        Return defaultFont
    End Function
End Class

Public Class HtmlToPdf
    Private _html As String
    Public Sub New(html As String, fontPath As String, fontsize As Single)
        If Not (TypeOf FontFactory.FontImp Is MyFontFactoryImpl) Then
            FontFactory.FontImp = New MyFontFactoryImpl(fontPath, fontsize)
        End If
        _html = html
    End Sub

    Public Sub Render(stream As Stream, isLandScape As Boolean)
        Dim sr As New StringReader(_html)
        Dim pdfDoc As New Document()
        If isLandScape = True Then
            pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate())
        End If
        PdfWriter.GetInstance(pdfDoc, stream)
        Dim htmlparser As New html.simpleparser.HTMLWorker(pdfDoc)
        pdfDoc.Open()
        htmlparser.Parse(sr)
        pdfDoc.Close()
    End Sub

    Public Sub Render(response As HttpResponse, fileName As String, Optional isLandScape As Boolean = False)
        response.Clear()
        response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", fileName))
        response.ContentType = "application/octet-stream"

        Render(response.OutputStream, isLandScape)

        response.[End]()
    End Sub
End Class