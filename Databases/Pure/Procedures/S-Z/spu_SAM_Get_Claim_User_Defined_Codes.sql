SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Claim_User_Defined_Codes'
GO

CREATE PROCEDURE spu_SAM_Get_Claim_User_Defined_Codes

@claim_id int, 
@source_id int, 
@user_defined_field_A_code varchar(20) OUTPUT, 
@user_defined_field_B_Code varchar(20) OUTPUT, 
@user_defined_field_C_Code varchar(20) OUTPUT,
@user_defined_field_D_Code varchar(20) OUTPUT,
@user_defined_field_E_Code varchar(20) OUTPUT

AS

SELECT @user_defined_field_A_Code = code
FROM gis_user_def_detail 
WHERE gis_user_def_detail_id in (
				SELECT user_defined_field_A 
				FROM claim 
				WHERE claim_id = @claim_id)

SELECT @user_defined_field_B_Code = code
FROM gis_user_def_detail 
WHERE gis_user_def_detail_id in (
				SELECT user_defined_field_B 
				FROM claim 
				WHERE claim_id = @claim_id)

SELECT @user_defined_field_C_Code = code
FROM gis_user_def_detail 
WHERE gis_user_def_detail_id in (
				SELECT user_defined_field_C 
				FROM claim 
				WHERE claim_id = @claim_id)


SELECT @user_defined_field_D_Code = code
FROM gis_user_def_detail 
WHERE gis_user_def_detail_id in (
				SELECT user_defined_field_D 
				FROM claim 
				WHERE claim_id = @claim_id)


SELECT @user_defined_field_E_Code = code
FROM gis_user_def_detail 
WHERE gis_user_def_detail_id in (
				SELECT user_defined_field_E
				FROM claim 
				WHERE claim_id = @claim_id)



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
