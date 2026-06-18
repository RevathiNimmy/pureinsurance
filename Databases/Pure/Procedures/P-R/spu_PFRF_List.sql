EXECUTE DDLDropProcedure 'spu_PFRF_List'
GO

CREATE PROCEDURE spu_PFRF_List
    @CompanyNo int,
    @SchemeNo int,
    @SchemeVersion int

AS BEGIN

    SELECT pfrf_id, startdate, enddate, Mnemonic,ProductFamily, f.description AS Frequency
    FROM PFRF p, pfFrequency f
    WHERE p.pfFrequency_id = f.pfFrequency_id
    AND CompanyNo = @CompanyNo
    AND SchemeNo = @SchemeNo
    AND SchemeVersion = @SchemeVersion

END
GO