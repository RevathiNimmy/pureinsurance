
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Get_All_Policy_Transaction_Dates'
GO

--Start( Sriram )PN 56443
CREATE PROCEDURE spu_Get_All_Policy_Transaction_Dates 
@insurance_file_cnt int
AS
Declare
@LastTransDescription varchar(255),
@Insurance_Folder_Cnt int

SELECT @Insurance_Folder_Cnt = Insurance_folder_cnt 
	FROM 	Insurance_File 
	Where Insurance_File_Cnt = @insurance_file_cnt

SELECT @LastTransDescription=last_trans_description 
	FROM insurance_file_system
	 where insurance_file_cnt=@insurance_file_cnt 

if @LastTransDescription='Renewals' 
	BEGIN
	
		SELECT expiry_date FROM insurance_file ifile
			INNER JOIN insurance_file_system ifsys  on
			 ifile.insurance_file_cnt = ifsys.insurance_file_cnt
			WHERE insurance_folder_cnt = @Insurance_Folder_Cnt AND insurance_file_type_id IN (2,5,8,9) and last_trans_description<>'' 
			ORDER BY cover_start_date DESC

	END
ELSE IF @LastTransDescription='' 
	BEGIN
		SELECT expiry_date FROM insurance_file ifile
			INNER JOIN insurance_file_system ifsys  on
			 ifile.insurance_file_cnt = ifsys.insurance_file_cnt
			WHERE insurance_folder_cnt = @Insurance_Folder_Cnt AND insurance_file_type_id IN (2,5,8,9) and last_trans_description<>'Renewals' 
			ORDER BY cover_start_date DESC
	END
--End( Sriram) PN 56443



SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO