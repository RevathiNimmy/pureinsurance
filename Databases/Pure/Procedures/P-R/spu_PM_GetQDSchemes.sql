EXECUTE DDLDropProcedure 'spu_PM_GetQDSchemes'
GO

CREATE PROCEDURE spu_PM_GetQDSchemes
    
        @source_id int

AS
BEGIN
    SELECT 
        S.CompanyNo, 
        S.SchemeNo, 
        S.SchemeVersion, 
        S.SchemeName,
        S.NoOfInstallments, 
        RF.DepositReq, 
        RF.DepositPC,
        0 as MontlyPayment,
        RF.arrangementfee,
        RF.Min1,
        RF.Max1,
        RF.Rate1,
        RF.Min2,
        RF.Max2,
        RF.Rate2,
        RF.Min3,
        RF.Max3,
        RF.Rate3,
        RF.Min4,
        RF.Max4,
        RF.Rate4,
        RF.Min5,
        RF.Max5,
        RF.Rate5
    FROM PFScheme S
    INNER JOIN PFRF RF
    ON RF.CompanyNo = S.CompanyNo
    AND RF.SchemeNo = S.SchemeNo
    AND RF.SchemeVersion = S.SchemeVersion
    AND RF.StartDate <= GetDate()
    AND RF.EndDate >= GetDate()

    LEFT OUTER JOIN PFSchemeSource PFS 
    ON PFS.CompanyNo = S.CompanyNo
    AND PFS.SchemeNo = S.SchemeNo
    AND PFS.SchemeVersion = S.SchemeVersion
    WHERE PFS.source_id = @source_id
    OR
    NOT EXISTS
        (SELECT CompanyNo FROM PFSchemeSource WHERE  
         CompanyNo = S.CompanyNo
         AND SchemeNo = S.SchemeNo
         AND SchemeVersion = S.SchemeVersion)

END
GO