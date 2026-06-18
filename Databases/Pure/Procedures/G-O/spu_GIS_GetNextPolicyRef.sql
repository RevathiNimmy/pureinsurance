SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_GIS_GetNextPolicyRef'
GO

CREATE PROCEDURE spu_GIS_GetNextPolicyRef
            @gis_scheme_id int,
            @warninglimit int,
            @is_warning int OUTPUT,
            @policy_ref varchar(20) OUTPUT
AS
BEGIN

    -- Procedure : spu_GIS_GetNextPolicyRef
    -- Revisions : 22/03/02 - CTAF - Created
    --                   01/07/02 - CTAF - Pad 0's to quote number

    DECLARE @polnonextno int    
    DECLARE @polnoprefix varchar(10)
    DECLARE @polnoendno int
    DECLARE @sPrefix char(10)
    DECLARE @sPolNextNo char(10)

    -- Switch the rowcount off
    SET NOCOUNT ON

    -- Begin a transaction
    BEGIN TRANSACTION GetNextPolicyRef

    -- Get the values, and lock the row for this transaction
    SELECT @polnonextno = gs.[polnonextno],
           @polnoprefix = RTRIM(gs.[polnoprefix]),
           @polnoendno = gs.[polnoendno]
    FROM GIS_Scheme gs
    WITH (ROWLOCK, HOLDLOCK)
    WHERE gs.[gis_scheme_id] = @gis_scheme_id

    -- Do we have a valid policy number?
    IF (@polnonextno IS NULL)
    BEGIN
        -- None used so get the start number
        SELECT @polnonextno = gs.[polnostartno]
        FROM GIS_Scheme gs
        WHERE gs.[gis_scheme_id] = @gis_scheme_id

        -- Is it still null?
        IF (@polnonextno IS NULL)
        BEGIN
            -- Just start at zero
            SELECT @polnonextno = 0

            -- Update the start number so its all nice...
            UPDATE GIS_Scheme
            SET [polnostartno] = @polnonextno
            WHERE [gis_scheme_id] = @gis_scheme_id
        END 
    END


    SELECT @sPolNextNo = CONVERT(varchar(10), @polnonextno)

    -- CJB 131004 PN15751 - If pol no is up to 6 in length then pad with zeros (as before), else don't
    IF LEN(@sPolNextNo) < 7
       BEGIN
          /* CTAF 170702 - Changed to fill to 6 digits - CNIC request */
          SELECT @sPrefix = REPLICATE('0', 6 - LEN(@sPolNextNo))			 
          SELECT @policy_ref = @polnoprefix + RTRIM(@sPrefix) + @sPolNextNo	
       END
    ELSE
       BEGIN
          select @sPrefix = ''
       END
	SELECT @policy_ref = @polnoprefix + RTRIM(@sPrefix) + @sPolNextNo	

    -- Update the record
    UPDATE GIS_Scheme
    SET polnonextno = @polnonextno + 1
    WHERE gis_scheme_id = @gis_scheme_id

    -- Check if we're near the end
    IF (@polnonextno > (@polnoendno - @warninglimit))
    BEGIN
        SELECT @is_warning = 1
    END
    ELSE
    BEGIN
        SELECT @is_warning = 0
    END

    -- End the transaction and release the lock
    COMMIT TRANSACTION GetNextPolicyRef

    -- We want the row count back...
    SET NOCOUNT OFF

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

