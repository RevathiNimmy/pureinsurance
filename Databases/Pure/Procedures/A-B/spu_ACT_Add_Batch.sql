SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Batch'
GO

CREATE PROCEDURE spu_ACT_Add_Batch
    @batch_id int OUTPUT,
    @company_id smallint,
    @batchstatus_id smallint,
    @user_id smallint,
    @batch_ref varchar(25),
    @created_date datetime,
    @authorised_date datetime,
    @accounting_date datetime,
    @comment varchar(60),
    @batch_type_id int,
    @batch_source_id int,
    @xml_object varchar(4000),
--added extra params 14/01/2003 SW
    @exportdate datetime,
    @reexportdate datetime,
    @mediatypeid int,
    @totalamount numeric, 
    @totaltransactions int,
    @importeddate datetime,
    @rejectamount numeric,
    @rejecttransactions As int,
    @closeddate datetime,
    @interfacecode varchar(30),
    @autoclose tinyint
AS

BEGIN
INSERT INTO Batch (
    company_id ,
    batchstatus_id ,
    user_id ,
    batch_ref ,
    created_date ,
    authorised_date ,
    accounting_date ,
    comment ,
    batch_type_id ,
    batch_source_id,
    xml_object,
--- new params (SW)
    export_date,
    reexport_date,
    mediatype_id,
    total_amount, 
    total_transactions,
    imported_date,
    reject_amount,
    reject_transactions,
    closed_date,
    interface_code,
    auto_close)
VALUES (
    @company_id,
    @batchstatus_id,
    @user_id,
    @batch_ref,
    @created_date,
    @authorised_date,
    @accounting_date,
    @comment,
    @batch_type_id,
    @batch_source_id,
    @xml_object,
-- New params (SW)
    @exportdate,
    @reexportdate,
    @mediatypeid,
    @totalamount, 
    @totaltransactions,
    @importeddate,
    @rejectamount,
    @rejecttransactions,
    @closeddate,
    @interfacecode,
    @autoclose)
END

SELECT @batch_id=@@IDENTITY
GO
