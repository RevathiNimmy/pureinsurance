SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Address_MultipleUse'
GO

CREATE PROCEDURE spu_Address_MultipleUse
    @address_cnt int

AS BEGIN
    SELECT ISNULL(COUNT(*),0) 
    FROM Party_Address_Usage
    WHERE Address_cnt = @Address_cnt 
END

GO


