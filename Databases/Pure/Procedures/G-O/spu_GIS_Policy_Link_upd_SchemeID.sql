SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_Policy_Link_upd_SchemeID'
GO

-- Updates the gis_scheme_id in the gis_policy_link record of the @NewInsuranceFileCnt passed in 
-- with the scheme id from the gis_policy_link record of the @OldInsuranceFileCnt passed in

CREATE PROCEDURE spu_GIS_Policy_Link_upd_SchemeID
    @OldInsuranceFileCnt int, 
    @NewInsuranceFileCnt int 
AS
BEGIN

DECLARE @gis_scheme_id INTEGER

--Get the gis_scheme_id from the OLD RISK gis_policy_link record using the old insurance_file_cnt
SELECT @gis_scheme_id = (SELECT gis_scheme_id 
  			   FROM gis_policy_link
 			  WHERE insurance_file_cnt = @OldInsuranceFileCnt)

--Update the gis_scheme_id on the NEW RISK gis_policy_link record using the new insurance_file_cnt
UPDATE gis_policy_link
   SET gis_scheme_id = @gis_scheme_id
 WHERE insurance_file_cnt = @NewInsuranceFileCnt

END
GO