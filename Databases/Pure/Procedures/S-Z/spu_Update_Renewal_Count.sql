SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Update_Renewal_Count'
GO


CREATE PROCEDURE spu_Update_Renewal_Count
    @insurance_file_cnt int,
    @insurance_folder_cnt int
AS

/********************************************************************************************************
-- Desc : increment renewal count in insurance folder
-- Hist : 09/07/2001 TN - Created
**********************************************************************************************************/
DECLARE @renewal_count int

-- GET INSURANCE FOLDER COUNT
IF @insurance_folder_cnt = 0
    SELECT  @insurance_folder_cnt = insurance_folder_cnt
    FROM    insurance_file
    WHERE   insurance_file_cnt = @insurance_file_cnt

-- GET RENEWAL COUNT
SELECT  @renewal_count = renewal_count
FROM    insurance_folder
WHERE   insurance_folder_cnt = @insurance_folder_cnt

-- INCREMENT RENEWAL COUNT
IF @renewal_count IS NULL
    SELECT @renewal_count = 0

SELECT @renewal_count = @renewal_count + 1

-- UPDATE INSURANCE FOLDER WITH NEW RENEWAL COUNT
UPDATE  insurance_folder
SET     renewal_count = @renewal_count
WHERE   insurance_folder_cnt = @insurance_folder_cnt
GO


