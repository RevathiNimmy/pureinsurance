EXECUTE DDLDROPPROCEDURE 'spu_SIR_get_insurance_file_Associates'
GO
CREATE PROCEDURE spu_SIR_get_insurance_file_Associates          
	@InsuranceFileKey INTEGER       
AS      
BEGIN      
SELECT Row_number() OVER (ORDER BY IFA.Party_cnt) AS RowID,      
	IFA.Insurance_file_associates_cnt,IFA.Party_cnt,IFA.Insurance_file_cnt,INF.insurance_folder_cnt,      
	IFA.Association_type_id,IFA.date_attached,IFA.date_removed,ISNull(IFA.Is_Deleted,0) AS Is_Deleted ,IFA.Association_detail,      
	INS.insurance_ref,Ptype.code AS PartyTypeCode,Ptype.description AS PartytypeDesc,      
	PT.shortname,PT.Name As PartyName,PT.ShortName As PartyShortName,PT.resolved_name,PT.shortname As PartyTypeCode,       
	At.code AS AssociatestypeCode,At.description AS AssociatesType_name,      
	PAddU.description AS Addressdesc,AUT.code As Address_Usage_Type_Code,ad.address1,Ad.address2,ad.address3,ad.address4,      
	ad.country_id,ad.postal_code,cu.code As Country_Code,
	IFA.is_AddUnConfirmed, IFA.is_DelUnConfirmed    
	FROM Insurance_file_associates IFA            
	INNER JOIN Insurance_File INS ON INS.insurance_file_cnt = IFA.Insurance_file_cnt      
	INNER JOIN Insurance_Folder INF ON INF.insurance_folder_cnt = INS.insurance_folder_cnt      
	INNER JOIN Party Pt ON PT.party_cnt=IFA.Party_cnt      
	INNER JOIN Party_Type Ptype ON PType.party_type_id=Pt.party_type_id      
	LEFT JOIN Association_Type AT ON AT.Association_Type_id=IFA.Association_Type_id      
	LEFT JOIN Party_Address_Usage PAddU ON PAddU.party_cnt=pt.party_cnt AND PAddU.address_usage_type_id=4 --For Only for Correspondance Address      
	LEFT JOIN Address_Usage_Type AUT ON AUT.address_usage_type_id=PAddU.address_usage_type_id      
	LEFT JOIN Address ad ON Ad.address_cnt=PAddU.address_cnt      
	LEFT JOIN Country Cu ON Cu.country_id=Ad.country_id      
      
WHERE ifa.insurance_file_cnt=@InsuranceFileKey          
ORDER BY IFA.date_attached Desc,PT.ShortName

      
END   

GO


