SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Document'
GO

--sj 02/08/2002 - Add insurance_file_cnt and reason

CREATE PROCEDURE spu_ACT_Add_Document
    @document_id int OUTPUT,
    @company_id int,
    @postingstatus_id smallint,
    @documenttype_id smallint,
    @auditset_id int,
    @batch_id int,
    @document_ref varchar(25),
    @document_date datetime,
    @created_date datetime,
    @authorised_date datetime,
    @comment varchar(255),
    @write_off_reason_id int,
    @insurance_file_cnt int,
    @reason varchar(255),
    @sub_branch_id int=NULL,
    @claim_id int,
    @terms_of_payment_id int = Null,
    @payment_due_date datetime = Null

AS

-- PWF 02/09/2002 - Two-part check on sub_branch
IF ISNULL(@sub_branch_id, 0) = 0
    SELECT @sub_branch_id = branch_id -- IFIBCR
    FROM   insurance_file
    WHERE  insurance_file_cnt = @insurance_file_cnt

IF ISNULL(@sub_branch_id, 0) = 0
    SELECT @sub_branch_id = 1
  
-- AMB 02/04/2003 - changed document_id to IDENTITY column

-- IF ISNULL(@document_id, 0) = 0
--     SELECT @document_id = ISNULL(MAX(document_id), 0) + 1
--     FROM   Document

/*	IF @documenttype_id = 22 or @documenttype_id = 23
	SELECT @document_date = @created_date
*/

INSERT INTO Document (
--    document_id ,
    company_id ,
    sub_branch_id ,
    postingstatus_id ,
    documenttype_id ,
    auditset_id ,
    batch_id ,
    document_ref ,
    document_date ,
    created_date ,
    authorised_date ,
    comment ,
    write_off_reason_id,
    insurance_file_cnt,
    reason,
    claim_id,
    terms_of_payment_id,
    payment_due_date)
VALUES (
--    @document_id,
    @company_id,
    @sub_branch_id,
    @postingstatus_id,
    @documenttype_id,
    @auditset_id,
    @batch_id,
    @document_ref,
    @document_date,
    @created_date,
    @authorised_date,
    @comment,
    @write_off_reason_id, 
    @insurance_file_cnt,
    @reason,
    @claim_id,
    @terms_of_payment_id,
    @payment_due_date)

-- AMB 02/04/2003 - return the new id
SELECT @document_id = @@IDENTITY

GO


