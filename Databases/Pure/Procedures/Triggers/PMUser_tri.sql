SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropTrigger 'PMUser_tri'
GO
CREATE TRIGGER PMUser_tri
ON PMUser
FOR INSERT AS
BEGIN
SET NOCOUNT ON
    INSERT
    INTO    user_authorities
        (user_id,
         has_write_off_authority,
         write_off_amount,
         has_unrestricted_enquiry,
         has_unrestricted_update,
         display_reinsurance,
         display_claim_reinsurance)
    SELECT
        user_id,
        1,
        0,
        0,
        0,
        1,
        1
    FROM    inserted
    WHERE   user_id NOT IN (SELECT user_id FROM deleted)
SET NOCOUNT OFF
END
GO

