SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Cheque'
GO


CREATE PROCEDURE spu_ACT_Add_Cheque
    @transdetail_id int,
    @bankaccount_id int,
    @media_ref varchar(100)
AS


BEGIN
IF @media_ref=''
	SELECT @media_ref=NULL

INSERT INTO Cheque (
    transdetail_id,
    bankaccount_id,
    media_ref )
VALUES (
    @transdetail_id,
    @bankaccount_id,
    @media_ref)
END

--BEGIN
--SELECT @cheque_id = @@IDENTITY
--END
GO