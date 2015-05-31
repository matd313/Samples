﻿Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim partDocument As SolidEdgePart.PartDocument = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Bring Solid Edge to the foreground.
            application.Activate()

            ' Get a reference to the active sheetmetal document.
            partDocument = application.GetActiveDocument(Of SolidEdgePart.PartDocument)(False)

            If partDocument IsNot Nothing Then
                Select Case partDocument.ModelingMode
                    Case SolidEdgePart.ModelingModeConstants.seModelingModeOrdered
                        partDocument.ModelingMode = SolidEdgePart.ModelingModeConstants.seModelingModeSynchronous
                        Console.WriteLine("Modeling mode changed to synchronous.")
                    Case SolidEdgePart.ModelingModeConstants.seModelingModeSynchronous
                        partDocument.ModelingMode = SolidEdgePart.ModelingModeConstants.seModelingModeOrdered
                        Console.WriteLine("Modeling mode changed to ordered (traditional).")
                End Select
            Else
                Throw New System.Exception("No active document.")
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
