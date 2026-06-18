SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_Get_NextOrionDocRef'
GO


CREATE PROCEDURE spu_SIR_Get_NextOrionDocRef
    @next_number int output,
    @document_prefix char(10)
AS

-- Get the next number and increment in one go
UPDATE Next_Orion_Doc_Ref
    SET  @next_number = next_number,
          next_number = next_number + 1
    WHERE prefix = @document_prefix

-- Quick check in case this is a first run
IF @next_number IS NULL
BEGIN
    DECLARE @max_orion_ref varchar(20)

    -- This is a bit yakky but gives us the best chance of avoiding 
    -- duplicates when dealing with exisiting data.
    SELECT  @max_orion_ref = MAX(document_ref)
    FROM    Document
    WHERE   document_ref like @document_prefix +'%'
    AND     LEN(LTRIM(RTRIM(document_ref))) - LEN(@document_prefix) = 6

    -- Check result
    IF @max_orion_ref IS NULL
        SELECT @next_number = 100001
    ELSE
        SELECT @next_number = SUBSTRING(@max_orion_ref, LEN(@document_prefix) + 1, 6) + 1

    -- Insert base number
    INSERT  Next_Orion_Doc_Ref
    VALUES (@document_prefix, @next_number + 1)
END

GO

