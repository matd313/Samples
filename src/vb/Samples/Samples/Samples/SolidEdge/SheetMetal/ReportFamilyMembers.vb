﻿Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge.SheetMetal
	''' <summary>
	''' Reports family members of the active sheetmetal.
	''' </summary>
	Friend Class ReportFamilyMembers
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim sheetMetalDocument As SolidEdgePart.SheetMetalDocument = Nothing
			Dim familyMembers As SolidEdgePart.FamilyMembers = Nothing
			Dim familyMember As SolidEdgePart.FamilyMember = Nothing
			Dim round As SolidEdgePart.Round = Nothing
			Dim userDefinedPattern As SolidEdgePart.UserDefinedPattern = Nothing
			Dim dimension As SolidEdgeFrameworkSupport.Dimension = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect(True)

				' Make sure user can see the GUI.
				application.Visible = True

				' Bring Solid Edge to the foreground.
				application.Activate()

				' Get a reference to the active part document.
				sheetMetalDocument = application.TryActiveDocumentAs(Of SolidEdgePart.SheetMetalDocument)()

				If sheetMetalDocument IsNot Nothing Then
					' Get a reference to the FamilyMembers collection.
					familyMembers = sheetMetalDocument.FamilyMembers

					' Interate through the family members.
					For i As Integer = 1 To familyMembers.Count
						' Get the FamilyMember at current index.
						familyMember = familyMembers.Item(i)

						Console.WriteLine(familyMember.Name)

						' Determine FamilyMember MovePrecedence.
						Select Case familyMember.MovePrecedence
							Case SolidEdgePart.MovePrecedenceConstants.igModelMovePredecence
							Case SolidEdgePart.MovePrecedenceConstants.igSelectSetMovePrecedence
						End Select

						' Warning: Accessing certain LiveRule[...] properties may throw an exception.
						'Console.WriteLine("igConcentricLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igConcentricLiveRule]);
						'Console.WriteLine("igCoplanarAxesAboutXLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igCoplanarAxesAboutXLiveRule]);
						'Console.WriteLine("igCoplanarAxesAboutYLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igCoplanarAxesAboutYLiveRule]);
						'Console.WriteLine("igCoplanarAxesAboutZLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igCoplanarAxesAboutZLiveRule]);
						'Console.WriteLine("igCoplanarAxesLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igCoplanarAxesLiveRule]);
						'Console.WriteLine("igCoplanarLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igCoplanarLiveRule]);
						'Console.WriteLine("igMaintainRadiusLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igMaintainRadiusLiveRule]);
						'Console.WriteLine("igOrthoLockingLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igOrthoLockingLiveRule]);
						'Console.WriteLine("igParallelLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igParallelLiveRule]);
						'Console.WriteLine("igPerpendicularLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igPerpendicularLiveRule]);
						'Console.WriteLine("igSymmetricLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igSymmetricLiveRule]);
						'Console.WriteLine("igSymmetricXYLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igSymmetricXYLiveRule]);
						'Console.WriteLine("igSymmetricYZLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igSymmetricYZLiveRule]);
						'Console.WriteLine("igSymmetricZXLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igSymmetricZXLiveRule]);
						'Console.WriteLine("igTangentEdgeLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igTangentEdgeLiveRule]);
						'Console.WriteLine("igTangentTouchingLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igTangentTouchingLiveRule]);
						'Console.WriteLine("igThicknessChainLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igThicknessChainLiveRule]);

						' Interate through the suppressed features of the current family member.
						For j As Integer = 1 To familyMember.SuppressedFeatureCount
							Dim suppressedFeature As Object = familyMember.SuppressedFeature(j)

							' Use ReflectionHelper class to get the feature type.
							Dim featureType As SolidEdgePart.FeatureTypeConstants = ReflectionHelper.GetPartFeatureType(suppressedFeature)

							Select Case featureType
								Case SolidEdgePart.FeatureTypeConstants.igRoundFeatureObject
									round = DirectCast(suppressedFeature, SolidEdgePart.Round)
								Case SolidEdgePart.FeatureTypeConstants.igUserDefinedPatternFeatureObject
									userDefinedPattern = DirectCast(suppressedFeature, SolidEdgePart.UserDefinedPattern)
							End Select
						Next j

						' Interate through the variables of the current family member.
						For j As Integer = 1 To familyMember.VariableCount
							Dim variable As Object = familyMember.Variable(j)

							' Use ReflectionHelper class to get the object type.
							Dim objectType As SolidEdgeFramework.ObjectType = ReflectionHelper.GetObjectType(variable)

							Select Case objectType
								Case SolidEdgeFramework.ObjectType.igDimension
									dimension = DirectCast(variable, SolidEdgeFrameworkSupport.Dimension)
							End Select
						Next j
					Next i
				Else
					Throw New System.Exception(Resources.NoActiveSheetMetalDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace