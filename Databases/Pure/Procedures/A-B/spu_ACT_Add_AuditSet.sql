SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_AuditSet'
GO


CREATE PROCEDURE spu_ACT_Add_AuditSet
    @auditset_id int OUTPUT,
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
INSERT INTO AuditSet (
    company_id ,
    user_id ,
    posted_date ,
    comment ,
    document_id ,
    auditset_type_id ,
    approved_date ,
    approved_user_id ,
    rejected ,
    rejected_user_id ,
    cashlistitem_id
 )
VALUES (
    @company_id,
    @user_id,
    @posted_date,
    @comment,
    @document_id,
    @auditset_type_id,
    @approved_date,
    @approved_user_id,
    @rejected,
    @rejected_user_id,
    @cashlistitem_id
)
END
SELECT @auditset_id=@@IDENTITY
GO