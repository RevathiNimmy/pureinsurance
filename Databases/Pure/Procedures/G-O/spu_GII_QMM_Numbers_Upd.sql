-- AK10092004: spu_GII_QMM_Numbers_Upd.sql
-- Updates a record in the GII_QMM_Numbers table - if record isn't found a new
-- one is inserted

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GII_QMM_Numbers_Upd'
GO

CREATE PROCEDURE spu_GII_QMM_Numbers_Upd
    @ClassOfBusiness char(2),
    @QMInsRef varchar(10),
    @PolAllowed tinyint,
    @CovAllowed tinyint
AS

-- Check for an existing record
IF EXISTS (SELECT qm_insurer_ref
             FROM GII_QMM_Numbers 
            WHERE class_of_business = @ClassOfBusiness
              AND qm_insurer_ref = @QMInsRef) BEGIN

    -- Record found, so update it
    UPDATE GII_QMM_Numbers
       SET pol_allowed = @PolAllowed,
           cov_allowed = @CovAllowed
     WHERE class_of_business = @ClassOfBusiness
       AND qm_insurer_ref = @QMInsRef

END ELSE BEGIN

    -- No match found, insert a new record
    INSERT INTO GII_QMM_Numbers (
                qm_insurer_ref,
                class_of_business,
                pol_allowed,
                cov_allowed)
        VALUES (@QMInsRef, 
                @ClassOfBusiness,
                @PolAllowed,
                @CovAllowed)

END

GO
