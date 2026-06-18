SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_resv_defn_upd'
GO


CREATE PROCEDURE spu_resv_defn_upd
    @ReserveTypeID int,
    @Description varchar(50),
    @IncludeInTotal bit,
    @Name varchar(50),
    @IsExcess bit,
    @is_indemnity tinyint=null,
    @is_expense tinyint=null
AS


BEGIN
UPDATE Reserve_type
SET Description=@Description, Include_in_Total=@IncludeInTotal, Name=@Name, Is_Excess=@IsExcess,
Is_Indemnity=@is_indemnity,Is_Expense=@is_expense 
WHERE Reserve_type_id=@ReserveTypeID
END
GO


