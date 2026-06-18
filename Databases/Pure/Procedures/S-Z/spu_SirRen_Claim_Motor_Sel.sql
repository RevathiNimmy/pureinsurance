SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Claim_Motor_Sel'
GO


CREATE PROCEDURE spu_SirRen_Claim_Motor_Sel
    @Insurance_File_Cnt int
AS

/* AK 30072001 - To get Claims data for Motor */
BEGIN

    SELECT C.Claim_Id, C.Loss_From_Date, Code=Substring(P.Code, 7, 2),
    AtFault = CASE (Select value
            from claim_user_defined_risk_data U, risk_data_definition D
            where U.Claim_id = C.Claim_Id
            and D.Description = 'At Fault Indicator'
            and U.risk_data_defn_id = D.risk_data_defn_id)
              WHEN '1' THEN 'Y'
              WHEN '0' THEN 'N'
          END,
    BodilyInjury= CASE ( Select value
                 from claim_user_defined_risk_data U, risk_data_definition D
            where U.Claim_id = C.Claim_Id
            and D.Description = 'Bodily Injury Indicator'
            and U.risk_data_defn_id = D.risk_data_defn_id)
              WHEN '1' THEN 'Y'
              WHEN '0' THEN 'N'
          END,
    Driver = ( Select value
                 from claim_user_defined_risk_data U, risk_data_definition D
            where U.Claim_id = C.Claim_Id
            and D.Description = 'Driver'
            and U.risk_data_defn_id = D.risk_data_defn_id)

    From Claim C, Primary_Cause P, Insurance_File I
    Where I.Insurance_File_Cnt = @Insurance_File_Cnt
    AND C.Policy_Id in (Select Insurance_File_Cnt from Insurance_File F
                WHERE F.Insurance_Folder_Cnt = I.Insurance_Folder_cnt)
    And P.Primary_Cause_Id = C.Primary_Cause_Id
END
GO


