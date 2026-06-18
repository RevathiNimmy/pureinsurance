-- AK10092004: spu_GII_QMM_Numbers_Del.sql
-- Deletes an existing record from the GII_QMM_Numbers table

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GII_QMM_Numbers_Del'
GO

CREATE PROCEDURE spu_GII_QMM_Numbers_Del
    @ClassOfBusiness char(2),
    @QMInsRef varchar(10)
AS

-- If the record exists...
IF EXISTS (SELECT qm_insurer_ref
             FROM GII_QMM_Numbers 
            WHERE class_of_business = @ClassOfBusiness
              AND qm_insurer_ref = @QMInsRef) BEGIN

    -- ...delete it
    DELETE FROM GII_QMM_Numbers
          WHERE class_of_business = @ClassOfBusiness
            AND qm_insurer_ref = @QMInsRef
END

GO
