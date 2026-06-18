SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'Spu_SIR_Get_BackdatedVersions'
GO

CREATE PROCEDURE Spu_SIR_Get_BackdatedVersions  
@ifile_cnt int  
AS  
BEGIN  
Select insurance_file_cnt ,policy_version,ift.description,cover_start_date,expiry_date,this_premium,
--PN 71176(Jeetendra Kumar)
IsNull(IFS.description,'Live') description 
from insurance_file iFile   
join insurance_file_type IFT on iFile.insurance_file_type_id=IFT.insurance_file_type_id  
left outer join insurance_file_Status IFS on iFile.insurance_file_status_id=IFS.insurance_file_status_id  
where base_insurance_file_cnt=@ifile_cnt  
END  
