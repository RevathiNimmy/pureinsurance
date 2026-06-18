SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_resv_defn_add'
GO


CREATE PROCEDURE spu_resv_defn_add
    @ReserveTypeID int OUTPUT,
    @Description varchar(50),
    @IncludeInTotal bit,
    @Name varchar(50),
    @IsExcess bit,
    @is_indemnity tinyint=null,
    @is_expense tinyint=null
AS


BEGIN
INSERT INTO Reserve_type(Description, Include_in_Total,Name,Is_Excess,is_indemnity,is_expense)
VALUES (@Description, @IncludeInTotal, @Name, @IsExcess,@is_indemnity,@is_expense)
END
BEGIN
SELECT @ReserveTypeID = @@IDENTITY
END
GO


