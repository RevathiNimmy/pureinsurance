EXECUTE DDLDropProcedure spu_Create_Risk_Link
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

-- ED 17072002 - Original
-- PW151102 - add the link to the persistent risk link table too

CREATE PROCEDURE spu_Create_Risk_Link

@Insurance_File_Cnt Int,
@NewInsurance_File_Cnt Int


AS

INSERT INTO insurance_file_risk_link
      (insurance_file_cnt,
       risk_cnt,
       status_flag,
       original_risk_cnt)
SELECT @NewInsurance_File_Cnt  ,
       risk_cnt,
       'U',
       original_risk_cnt
  FROM insurance_file_risk_link
 WHERE insurance_file_cnt =  @Insurance_File_Cnt


INSERT INTO insurance_file_persistent_risk_link
      (insurance_file_cnt,
       risk_cnt,
       status_flag,
       original_risk_cnt)
SELECT @NewInsurance_File_Cnt  ,
       risk_cnt,
       'U',
       original_risk_cnt
  FROM insurance_file_risk_link
 WHERE insurance_file_cnt =  @Insurance_File_Cnt

GO


SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
