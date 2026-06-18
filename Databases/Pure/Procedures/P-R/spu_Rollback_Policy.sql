SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Rollback_Policy'
GO

CREATE PROCEDURE spu_Rollback_Policy    
    @product_id int ,
    @renewal_status_type_id int ,
    @insurance_holder_cnt int ,
    @old_insurance_file_cnt int ,
    @lead_agent_cnt int ,
    @created_by_id smallint ,
    @date_created datetime ,
    @critical_date datetime ,
    @new_insurance_file_cnt int,
    @is_invite_printed tinyint,
    @BrokerXferStatusTypeID int

AS

BEGIN

--Update insurance_file to reset to renewal quote

UPDATE Insurance_File set insurance_file_type_id = 3, insurance_file_status_id = NULL where insurance_file_cnt = @new_insurance_file_cnt

UPDATE Insurance_File set insurance_file_status_id = 3 where insurance_file_cnt = @old_insurance_file_cnt 

-- insert values in Renewal_Status

INSERT INTO Renewal_Status (
    product_id,
    renewal_status_type_id,
    insurance_holder_cnt,
    insurance_file_cnt,
    lead_agent_cnt,
    created_by_id,
    date_created,
    critical_date,
    renewal_insurance_file_cnt,
    is_invite_printed,
    broker_xfer_status_type_id)
VALUES (
    @product_id,
    @renewal_status_type_id,
    @insurance_holder_cnt,
    @old_insurance_file_cnt,
    @lead_agent_cnt,
    @created_by_id,
    @date_created,
    @critical_date,
    @new_insurance_file_cnt,
    @is_invite_printed,
    @BrokerXferStatusTypeID)



DECLARE @renewal_count int
DECLARE @insurance_folder_cnt int

Set @insurance_folder_cnt = 0

-- GET INSURANCE FOLDER COUNT
IF @insurance_folder_cnt = 0
    SELECT  @insurance_folder_cnt = insurance_folder_cnt
    FROM    insurance_file
    WHERE   insurance_file_cnt = @old_insurance_file_cnt

-- GET RENEWAL COUNT
SELECT  @renewal_count = renewal_count
FROM    insurance_folder
WHERE   insurance_folder_cnt = @insurance_folder_cnt

-- DECREMENT RENEWAL COUNT
IF @renewal_count IS NULL
    SELECT @renewal_count = 0

SELECT @renewal_count = @renewal_count - 1

-- UPDATE INSURANCE FOLDER WITH NEW RENEWAL COUNT
UPDATE  insurance_folder
SET     renewal_count = @renewal_count
WHERE   insurance_folder_cnt = @insurance_folder_cnt

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO