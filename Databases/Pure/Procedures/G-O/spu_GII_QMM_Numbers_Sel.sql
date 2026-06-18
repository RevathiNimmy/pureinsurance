-- AK03082004: spu_GII_QMM_Numbers_Sel.sql
-- Retrieves selected/all GII_QMM_Numbers record(s) from the database

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GII_QMM_Numbers_Sel'
GO

CREATE PROCEDURE spu_GII_QMM_Numbers_Sel
    @ClassOfBusiness char(2),
    @QMInsRef varchar(10)
AS

DECLARE @ByC int
DECLARE @ByI int

-- Set relevant flags for each search criterion provided
IF Len(@ClassOfBusiness) = 2 SELECT @ByC = 1 ELSE SELECT @ByC = 0
IF Len(@QMInsRef) = 3 SELECT @ByI = 1 ELSE SELECT @ByI = 0

-- Retrieve the selected record
SELECT class_of_business,
	   qm_insurer_ref,
	   pol_allowed,
	   cov_allowed
  FROM GII_QMM_Numbers
WHERE (@ByI = 0 AND @ByC = 0) OR
      (@ByI = 1 AND qm_insurer_ref = @QMInsRef AND @ByC = 0) OR
      (@ByC = 1 AND class_of_business = @ClassOfBusiness AND @ByI = 0) OR
      (@ByI = 1 AND qm_insurer_ref = @QMInsRef AND @ByC = 1 
           AND class_of_business = @ClassOfBusiness)

GO
