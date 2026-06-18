SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_Sub_Branch_id'
GO

CREATE PROCEDURE spu_ACT_Get_Sub_Branch_id
(
    @sub_branch_id INT OUTPUT,
    @account_id INT=NULL,
    @transdetail_id INT=NULL,
    @period_id INT=NULL,
    @bankaccount_id INT=NULL,
    @party_cnt INT=NULL,
    @Source_id INT=NULL
)

AS

IF @Source_id IS NOT NULL
    select  @sub_branch_id=min(sub_branch_id) 
    from    sub_branch WITH(NOLOCK)
    where   source_id=@source_id
IF @account_id IS NOT NULL
    SELECT  @sub_branch_id=sub_branch_id
    FROM    Account WITH(NOLOCK)
    WHERE   account_id=@account_id
IF @transdetail_id IS NOT NULL
    SELECT  @sub_branch_id=sub_branch_id
    FROM    TransDetail WITH(NOLOCK)
    WHERE   transdetail_id=@transdetail_id
IF @period_id IS NOT NULL
    SELECT  @sub_branch_id=sub_branch_id
    FROM    Period WITH(NOLOCK)
    WHERE   period_id=@period_id
IF @bankaccount_id IS NOT NULL
    SELECT  @sub_branch_id=sub_branch_id
    FROM    BankAccount WITH(NOLOCK)
    WHERE   bankaccount_id=@bankaccount_id
IF @party_cnt IS NOT NULL
    SELECT  @sub_branch_id=sub_branch_id
    FROM    Party WITH(NOLOCK)
    WHERE   party_cnt=@party_cnt
