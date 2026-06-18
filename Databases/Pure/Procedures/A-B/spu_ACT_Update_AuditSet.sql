SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_AuditSet'
GO


CREATE PROCEDURE spu_ACT_Update_AuditSet
    @auditset_id int,
    @company_id smallint,
    @user_id smallint,
    @posted_date datetime,
    @comment varchar(255),
    @document_id int,
    @auditset_type_id int,
    @approved_date datetime,
    @approved_user_id smallint,
    @rejected tinyint,
    @rejected_user_id smallint,
    @cashlistitem_id int
AS


BEGIN
UPDATE AuditSet
    SET
    company_id=@company_id,
    user_id=@user_id,
    posted_date=@posted_date,
    comment=@comment,
    document_id=@document_id,
    auditset_type_id=@auditset_type_id,
    approved_date=@approved_date,
    approved_user_id=@approved_user_id,
    rejected=@rejected,
    rejected_user_id=@rejected_user_id,
    cashlistitem_id=@cashlistitem_id   
    
WHERE auditset_id = @auditset_id
END
GO