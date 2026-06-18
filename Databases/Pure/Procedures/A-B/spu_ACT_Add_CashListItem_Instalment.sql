SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_CashListItem_Instalment'
GO



CREATE PROCEDURE spu_ACT_Add_CashListItem_Instalment
@cashlistitem_id int,
@PFInstalments_id int
AS
INSERT INTO cashlistitem_instalments
(cashlistitem_id,
pfinstalments_id)
VALUES
(@cashlistitem_id,
@pfinstalments_id)
GO
