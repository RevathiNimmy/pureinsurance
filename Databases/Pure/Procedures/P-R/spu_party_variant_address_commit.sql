SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_party_variant_address_commit'
GO
/* Select party_variant_address records for a given party (party_cnt) */
CREATE PROCEDURE spu_party_variant_address_commit
    @effective_date DATETIME
AS

DECLARE @address_usage_type_id INT
DECLARE @event_type_id INT

/* Get the code for correspondance address */
SELECT @address_usage_type_id =
    address_usage_type_id FROM address_usage_type
    WHERE code = '3131 XCO'

/* Get the event type for address change */
SELECT @event_type_id = MAX(event_type_id)
    FROM event_type
    WHERE code = 'ADDCHANGE'
    
/* Testing only */
--SELECT @effective_date = DATEADD(d,1,getdate())

/* Update the status on any potential duplicates to 2 */
UPDATE party_variant_address
SET commit_ind = 2
WHERE party_variant_address_cnt  IN
    (SELECT party_variant_address_cnt
         FROM party_address_usage a,
              party_variant_address v
         WHERE a.party_cnt = v.party_cnt
         AND a.address_cnt = v.address_cnt
     AND effective_date <= @effective_date
         AND v.commit_ind = 0)

/* Insert the uncomitted records */

INSERT INTO party_address_usage
	 (party_cnt,address_cnt,description,address_usage_type_id)
    SELECT party_cnt,address_cnt,null,@address_usage_type_id
         FROM party_variant_address
         WHERE commit_ind = 0
         AND effective_date <= @effective_date

/* Create an event */
INSERT INTO event_log 
(party_cnt,insurance_folder_cnt,insurance_file_cnt,claim_cnt,document_cnt,new_address_cnt,old_address_cnt,campaign_id,document_type_id,report_type_id,event_type_id,user_id,event_date,description,old_party_type_id,transaction_export_folder_cnt,event_log_subject_id,account_key) 
SELECT party_cnt,null,null,null,null,address_cnt,original_address_cnt,null,null,null,@event_type_id,user_id,effective_date,'Correspondance Address Change',null,null,null,null
FROM party_variant_address
         WHERE commit_ind = 0
         AND effective_date <= @effective_date

/* Remove the original records */
DELETE FROM party_address_usage
FROM party_address_usage a,
     party_variant_address v
WHERE a.party_cnt = v.party_cnt
AND a.address_cnt = v.original_address_cnt
AND effective_date <= @effective_date
AND v.commit_ind = 0

/*Update status to 1 */
UPDATE party_variant_address
SET commit_ind = 1
WHERE commit_ind = 0
AND effective_date <= @effective_date

GO
