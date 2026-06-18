-- AK15092004: spu_GIS_Scheme_Activation_Sel.sql
-- Retrieves selected/all GIS_Scheme_Activation record(s) from the database
/*JRD 13/10/2005 PN24189 - Extended to include Mailbox processing*/

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GIS_Scheme_Activation_Sel'
GO

CREATE PROCEDURE spu_GIS_Scheme_Activation_Sel
    @QMInsurerRef varchar(3),
    @ClassOfBusiness varchar(2),
    @SchemeNo smallint,
    @MailBox varchar(4)
AS

DECLARE @ByI int
DECLARE @ByC int
DECLARE @ByS int
DECLARE @ByM int

-- Set relevant flags for each search criterion provided
IF Len(@QMInsurerRef) = 3 SELECT @ByI = 1 ELSE SELECT @ByI = 0
IF Len(@ClassOfBusiness) = 2 SELECT @ByC = 1 ELSE SELECT @ByC = 0
IF @SchemeNo > 0 SELECT @ByS = 1 ELSE SELECT @ByS = 0
IF Len(@MailBox) = 4 SELECT @ByM = 1 ELSE SELECT @ByM = 0

-- Run the select query
SELECT gis_scheme_activation_id,
       qm_insurer_ref,
       class_of_business,
       scheme_no,
       activation_level,
       effective_date,
       MailBox
  FROM GIS_Scheme_Activation
WHERE (@ByI = 0 AND @ByC = 0 AND @ByS = 0 AND @ByM = 0) OR
      (@ByI = 1 AND qm_insurer_ref = @QMInsurerRef AND @ByC = 0 AND @ByS = 0 AND @ByM = 0) OR
      (@ByC = 1 AND class_of_business = @ClassOfBusiness AND @ByI = 0 AND @ByS = 0 AND @ByM = 0) OR
      (@ByI = 1 AND qm_insurer_ref = @QMInsurerRef AND @ByC = 1 AND class_of_business = @ClassOfBusiness AND @ByS = 0 AND @ByM = 0) OR
      (@ByI = 1 AND qm_insurer_ref = @QMInsurerRef AND @ByC = 1 AND class_of_business = @ClassOfBusiness AND @ByS = 1 AND scheme_no = @SchemeNo AND @ByM = 0) OR
      (@ByI = 1 AND qm_insurer_ref = @QMInsurerRef AND @ByC = 1 AND class_of_business = @ClassOfBusiness AND @ByS = 1 AND scheme_no = @SchemeNo AND @ByM = 1 AND MailBox = @MailBox)

GO