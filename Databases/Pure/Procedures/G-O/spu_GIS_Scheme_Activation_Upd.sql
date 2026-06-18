-- AK15092004: spu_GIS_Scheme_Activation_Upd.sql
-- Updates a record in the GIS_Scheme_Activation table - if record isn't 
-- found a new one is inserted
/*JRD 13/10/2005 PN24189 - Extended to include Mailbox processing*/

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GIS_Scheme_Activation_Upd'
GO

CREATE PROCEDURE spu_GIS_Scheme_Activation_Upd
    @QMInsurerRef varchar(3),
    @ClassOfBusiness varchar(2),
    @SchemeNo smallint,
    @ActivationLevel varchar(2),
    @EffectiveDate datetime,
	@MailBox varchar(4)
AS


-- Check for an existing record
IF EXISTS (SELECT gis_scheme_activation_id
             FROM GIS_Scheme_Activation 
            WHERE qm_insurer_ref = @QMInsurerRef
              AND class_of_business = @ClassOfBusiness
              AND scheme_no = @SchemeNo
			  AND mailbox = @MailBox) BEGIN

    -- Record found, so update it
    UPDATE GIS_Scheme_Activation
       SET activation_level = @ActivationLevel,
           effective_date = @EffectiveDate
     WHERE qm_insurer_ref = @QMInsurerRef
     AND class_of_business = @ClassOfBusiness
     AND scheme_no = @SchemeNo
	 AND mailbox = @MailBox

END ELSE BEGIN

    -- No match found, insert a new record
    INSERT INTO GIS_Scheme_Activation (
                qm_insurer_ref,
                class_of_business,
                scheme_no,
                activation_level,
                effective_date,
				mailbox)
        VALUES (@QMInsurerRef, 
                @ClassOfBusiness,
                @SchemeNo,
                @ActivationLevel,
                @EffectiveDate,
				@MailBox)

END

GO
