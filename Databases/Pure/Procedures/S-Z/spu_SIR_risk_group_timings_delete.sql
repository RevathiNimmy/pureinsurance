SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIR_risk_group_timings_delete'
GO


CREATE PROCEDURE spu_SIR_risk_group_timings_delete
AS


BEGIN
    IF EXISTS (SELECT * FROM Renewal_Settings)
        TRUNCATE TABLE Renewal_Settings
END
GO


