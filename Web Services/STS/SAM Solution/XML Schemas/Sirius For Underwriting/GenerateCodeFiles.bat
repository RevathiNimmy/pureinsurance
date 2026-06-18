
"C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\xsd" /c /l:vb /n:"SFI.AgentTypes" Agenttypes.xsd basetypes.xsd

del SFI_Agenttypes_basetypes.vb
rename Agenttypes_basetypes.vb SFI_Agenttypes_basetypes.vb
  	
"C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\xsd" /c /l:vb /n:"SFI.AnonymousTypes" Anonymoustypes.xsd basetypes.xsd

del SFI_Anonymoustypes_basetypes.vb
rename Anonymoustypes_basetypes.vb SFI_Anonymoustypes_basetypes.vb

"C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\xsd" /c /l:vb /n:"SFI.MessagingTypes" messagingtypes.xsd basetypes.xsd

del SFI_Messagingtypes_basetypes.vb
rename Messagingtypes_basetypes.vb SFI_Messagingtypes_basetypes.vb


"C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\xsd" /c /l:vb /n:"SFI.SAMForInsurance" SAMForInsurance.xsd basetypesBV1.xsd

del SFI_SAMForInsurance_basetypes.vb
rename SAMForInsurance_basetypesBV1.vb SFI_SAMForInsurance_basetypes.vb

"C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\xsd" /c /l:vb /n:"SFI.SAMForInsuranceV2" SAMForInsuranceBV2.xsd basetypesBV2.xsd

del SFI_SAMForInsuranceV2_basetypes.vb
rename SAMForInsuranceBV2_basetypesBV2.vb SFI_SAMForInsuranceV2_basetypes.vb


Pause

