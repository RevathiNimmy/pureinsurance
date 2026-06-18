EXECUTE DDLDropProcedure 'spu_ACT_DeleteCashlistitem_Instalments'
GO
CREATE PROCEDURE spu_ACT_DeleteCashlistitem_Instalments
    @pfinstalments_id int
AS
BEGIN
   DELETE FROM cashlistitem_instalments WHERE pfinstalments_id = @pfinstalments_id
   
   UPDATE TransDetail SET PFInstalments_id=NULL WHERE PFInstalments_id=pfinstalments_id
END
Go

