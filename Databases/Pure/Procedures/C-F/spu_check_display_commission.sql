SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_check_display_commission'
GO


CREATE PROCEDURE spu_check_display_commission
    @insurance_file_cnt INT
AS


DECLARE @business_type_id INT,
    @how_many INT

SELECT  @business_type_id = NULL

SELECT  @business_type_id = business_type_id
FROM    insurance_file
WHERE   insurance_file_cnt = @insurance_file_cnt

IF @business_type_id IS NOT NULL
    IF @business_type_id <> 1
    BEGIN
        SELECT 1
        RETURN
    END

SELECT  @how_many = 0

SELECT  @how_many = COUNT(party_cnt)
FROM    insurance_file_agent
WHERE   insurance_file_cnt = @insurance_file_cnt

IF @how_many = 0
    SELECT 0
ELSE
    SELECT 1
GO


