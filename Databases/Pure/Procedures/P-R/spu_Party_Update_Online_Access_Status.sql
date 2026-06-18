SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Party_Update_Online_Access_Status'
GO

CREATE PROCEDURE spu_Party_Update_Online_Access_Status
    @party_cnt int,
    @online_status boolean,
    @online_status_updated datetime, 
    @contact_cnt int OUTPUT

AS
BEGIN

    SELECT TOP 1 @contact_cnt = c.contact_cnt
      FROM party_contact_usage pcu
INNER JOIN contact c ON pcu.contact_cnt = c.contact_cnt
     WHERE pcu.party_cnt = @party_cnt 
       AND c.contact_type_id = 3

    IF ISNULL(@contact_cnt,0) <> 0 OR @online_status = 0
        BEGIN
        IF EXISTS(SELECT party_cnt FROM Party_Net_Data WHERE party_cnt = @party_cnt)
            BEGIN
            UPDATE Party_Net_Data
               SET online_status=@online_status,
                   online_status_updated=@online_status_updated,
                   contact_cnt=@contact_cnt
             WHERE party_cnt = @party_cnt
            END
        ELSE
            BEGIN
            INSERT INTO Party_Net_Data (
                   party_cnt, 
                   password,
                   online_status,
                   online_status_updated,
                   contact_cnt)
           VALUES (@party_cnt,
                   '',
                   @online_status,
                   @online_status_updated,
                   @contact_cnt)
            END
        END
END
GO

